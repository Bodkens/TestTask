using DataModels;
using Microsoft.EntityFrameworkCore;

namespace FirmREST;

public class ConstructionPriceService : IConstructionPriceService
{

    private FirmContext context;
    
    public ConstructionPriceService()
    {
        
    }

    public void Test()
    {
        Console.WriteLine("Test branch");
    }

    public ConstructionPriceService(FirmContext context)
    {
        this.context = context;
    }
    
    public async Task<(decimal ExprectedPrice, decimal ProjectsPrice)> GetConstructionPriceAsync(int constructionId, CancellationToken token)
    {
        var findConstructionTask = context.Constructions.FindAsync(constructionId).AsTask();
        
        var projectsPrice =  (
            from project in context.Projects
            where project.ConstructionID == constructionId
            select project.Price).ToListAsync(token);
        
        var construction = await findConstructionTask;
        
        if (construction is null) return (-1.0m, -1.0m);

        return (construction.ExpectedPrice, (await projectsPrice).DefaultIfEmpty(0).Sum());
    }
    
     

    public int CompareExpectedAndProjectPrice(decimal expectedPrice, IList<decimal> projectsPrice)
    {
        decimal sum = projectsPrice.Sum();
        if (expectedPrice == sum) return 0;
        return expectedPrice > sum  ? 1 : -1;

    }
}