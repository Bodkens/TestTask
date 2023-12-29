using DataModels;
using FirmREST;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FirmContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IValidator<Construction>, ConstructionValidator>();
builder.Services.AddScoped<IValidator<Project>, ProjectValidator>();
builder.Services.AddScoped<IValidator<Organization>, OrganizationValidator>();
builder.Services.AddScoped<IConstructionPriceService, ConstructionPriceService>();
builder.AddFluentValidationEndpointFilter();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var root = app.MapGroup("/").AddFluentValidationFilter();

root.MapGet("/constructions", async (FirmContext firmContext) => await firmContext.Constructions.ToListAsync())
    .WithName("GetAllConstructions")
    .WithOpenApi();

root.MapGet("/constructions/{id}", async (int id, FirmContext firmContext) =>
    {
        var construction = await firmContext.Constructions.FindAsync(id);

        return construction is null ? Results.NotFound() : Results.Ok(construction);
        
        
    })
    .WithName("GetConstructionsByID")
    .WithOpenApi();
root.MapGet("/construction/compare_expected/{id}",
        async (IConstructionPriceService priceService, int id, FirmContext firmContext) =>
        {
            var constructionTask = firmContext.Constructions.FindAsync(id);
            var projectPrices = (from project in firmContext.Projects.Include(p => p.Organization).ToList()
                where project.ConstructionID == id
                select project.Price).ToList();

            var construction = await constructionTask;

            return construction is null ? Results.NotFound() : Results.Ok(priceService.CompareExpectedAndProjectPrice(construction.ExpectedPrice, projectPrices));
        }
        
    )
    .WithName("CompareExpectedById")
    .WithOpenApi();
root.MapPost("/constructions", async (Construction construction, FirmContext firmContext) =>
    {
        
        firmContext.Constructions.Add(construction);
        await firmContext.SaveChangesAsync();

        return Results.Created($"/construction/{construction.ID}", construction);

    })
    .WithName("AddConstruction")
    .WithOpenApi();

root.MapPut("/constructions/{id}", async (IValidator<Construction> validator,int id, Construction newConstruction, FirmContext firmContext) =>
    {
        var oldConstruction = await firmContext.Constructions.FindAsync(id);

        if (oldConstruction is null) return Results.NotFound();

        oldConstruction.Name = newConstruction.Name;
        oldConstruction.ExpectedPrice = newConstruction.ExpectedPrice;
        oldConstruction.FinishDate = newConstruction.FinishDate;
        oldConstruction.StartDate = newConstruction.StartDate;
        oldConstruction.FinishDate = newConstruction.FinishDate;
        
        await firmContext.SaveChangesAsync();

        return Results.NoContent();

    })
    .WithName("EditConstruction")
    .WithOpenApi();

root.MapDelete("/constructions/{id}", async (int id, FirmContext firmContext) =>
{
    if (await firmContext.Constructions.FindAsync(id) is Construction construction)
    {
        firmContext.Constructions.Remove(construction);
        await firmContext.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
})
    .WithName("DeleteConstruction")
    .WithOpenApi();



root.MapGet("/projects", async (FirmContext firmContext) => await firmContext.Projects.Include(p => p.Construction).Include(p => p.Organization).ToListAsync())
    .WithName("GetAllProjects")
    .WithOpenApi();
root.MapGet("/projects/{id}", async (int id, FirmContext firmContext) =>
        {
           var project =  await firmContext.Projects.Include(p => p.Construction).Include(p => p.Organization)
                .FirstOrDefaultAsync(p => p.ID == id);

           return project is null ? Results.NotFound() : Results.Ok(project);
        }
        
    )
    .WithName("GetProjectsByID")
    .WithOpenApi();

root.MapGet("/projects/by_construction/{id}",
    async (IConstructionPriceService priceService, int id, FirmContext firmContext) =>
    {
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;
        
        
        List<Dictionary<string, object>> storage = new List<Dictionary<string, object>>();

        var projects = from project in firmContext.Projects.Include(p => p.Organization).ToList()
            where project.ConstructionID == id
            select project;

        foreach (var project in projects)
        {
            var pricesTask = priceService.GetConstructionPriceAsync(id, token);
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                ["ProjectID"] = project.ID,
                ["ProjectLocation"] = project.Location,
                ["ProjectPrice"] = project.Price,
                ["OrganizationID"] = project.OrganizationID,
                ["OrganizationName"] = project.Organization.Name,
                ["OrganizationType"] = project.Organization.Type
            };
            var prices = await pricesTask;
            dictionary["ConstructionProjectsPrice"] = prices.ProjectsPrice;
            dictionary["ConstructionExpectedPrice"] = prices.ExprectedPrice;
            storage.Add(dictionary);
        }
        return Results.Ok(storage);
    }
        
    )
    .WithName("GetProjectsByConstructionID")
    .WithOpenApi();



root.MapPost("/projects", async (Project project, FirmContext firmContext) =>
    {
        firmContext.Projects.Add(project);
        await firmContext.SaveChangesAsync();

        return Results.Created($"/construction/{project.ID}", project);

    })
    .WithName("AddProject")
    .WithOpenApi();

root.MapPut("/projects/{id}", async (int id, Project newProject, FirmContext firmContext) =>
    {
        var oldProject = await firmContext.Projects.FindAsync(id);

        if (oldProject is null) return Results.NotFound();

        oldProject.Location = newProject.Location;
        oldProject.Construction = newProject.Construction;
        oldProject.ConstructionID = newProject.ConstructionID;
        oldProject.Organization= newProject.Organization;
        oldProject.OrganizationID = newProject.OrganizationID;
        oldProject.Price = newProject.Price;
        await firmContext.SaveChangesAsync();

        return Results.NoContent();

    })
    .WithName("EditProject")
    .WithOpenApi();

root.MapDelete("/projects/{id}", async (int id, FirmContext firmContext) =>
    {
        if (await firmContext.Projects.FindAsync(id) is Project project)
        {
            firmContext.Projects.Remove(project);
            await firmContext.SaveChangesAsync();
            return Results.NoContent();
        }

        return Results.NotFound();
    })
    .WithName("DeleteProject")
    .WithOpenApi();



root.MapGet("/organizations", async (FirmContext firmContext) => await firmContext.Organizations.ToListAsync())
    .WithName("GetAllOrganizations")
    .WithOpenApi();

root.MapGet("/organizations/{id}", async (int id, FirmContext firmContext) =>
    {
        var organization = await firmContext.Organizations.FindAsync(id);

        return organization is null ? Results.NotFound() : Results.Ok(organization);
        
        
    })
    .WithName("GetOrganizationsByID")
    .WithOpenApi();

root.MapPost("/organizations", async (Organization organization, FirmContext firmContext) =>
    {
        firmContext.Organizations.Add(organization);
        await firmContext.SaveChangesAsync();

        return Results.Created($"/construction/{organization.ID}", organization);

    })
    .WithName("AddOrganization")
    .WithOpenApi();

root.MapPut("/organizations/{id}", async (int id, Organization newOrganization, FirmContext firmContext) =>
    {
        var oldOrganization = await firmContext.Organizations.FindAsync(id);

        if (oldOrganization is null) return Results.NotFound();

        oldOrganization.Type = newOrganization.Type;
        oldOrganization.CountryCode = newOrganization.CountryCode;
        oldOrganization.VATNumber = newOrganization.VATNumber;
        oldOrganization.Name= newOrganization.Name;
        await firmContext.SaveChangesAsync();

        return Results.NoContent();

    })
    .WithName("EditOrganization")
    .WithOpenApi();

root.MapDelete("/organizations/{id}", async (int id, FirmContext firmContext) =>
    {
        if (await firmContext.Organizations.FindAsync(id) is Organization organization)
        {
            firmContext.Organizations.Remove(organization);
            await firmContext.SaveChangesAsync();
            return Results.NoContent();
        }

        return Results.NotFound();
    })
    .WithName("DeleteOrganization")
    .WithOpenApi();


app.Run();