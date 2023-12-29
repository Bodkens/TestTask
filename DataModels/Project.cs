namespace DataModels;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
public class Project {

    public int ID {get; set;}
    
    public int ConstructionID { get; set; }
    public Construction Construction { get; set; }
    
    public int OrganizationID { get; set; }
    public Organization Organization { get; set; }
    
    public string Location {get; set;}
    public decimal Price {get; set;}
    
}

public class ProjectValidator : AbstractValidator<Project>
{
    public ProjectValidator()
    {
        RuleFor(p => p.Price).PrecisionScale(19,8, true).NotEmpty();
    }
}