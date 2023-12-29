namespace FirmREST;
using DataModels;
public interface IConstructionPriceService
{
    Task<(decimal ExprectedPrice, decimal ProjectsPrice)> GetConstructionPriceAsync(int constructionId, CancellationToken token);
    int CompareExpectedAndProjectPrice(decimal expectedPrice, IList<decimal> projectsPrice);
}