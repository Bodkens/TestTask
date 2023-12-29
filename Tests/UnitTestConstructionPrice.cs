namespace Tests;
using Xunit;
using FluentAssertions;
using FirmREST;
public class UnitTestConstructionPrice
{
    [Fact]
    public void TestGetConstructionPriceExpectedBigger()
    {
        List<Decimal> projectPrices =
        [
            1500.0m,
            2000.0m,
            3500m
        ];

        decimal expectedPrice = 8000m;

        ConstructionPriceService service = new ConstructionPriceService();

        service.CompareExpectedAndProjectPrice(expectedPrice, projectPrices).Should().Be(1);
    }
    [Fact]
    public void TestGetConstructionPriceExpectedEqual()
    {
        List<Decimal> projectPrices =
        [
            2500.0m,
            2000.0m,
            3500m
        ];

        decimal expectedPrice = 8000m;

        ConstructionPriceService service = new ConstructionPriceService();

        service.CompareExpectedAndProjectPrice(expectedPrice, projectPrices).Should().Be(0);
    }
    [Fact]
    public void TestGetConstructionPriceExpectedLess()
    {
        List<Decimal> projectPrices =
        [
            4500.0m,
            2000.0m,
            4500m
        ];

        decimal expectedPrice = 8000m;

        ConstructionPriceService service = new ConstructionPriceService();

        service.CompareExpectedAndProjectPrice(expectedPrice, projectPrices).Should().Be(-1);
    }
}