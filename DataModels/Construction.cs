namespace DataModels;
using FluentValidation;
public class Construction
{
    public int ID {get; set;}
    
    public string Name {get; set;}
    public DateTime StartDate {get; set;}
    public DateTime FinishDate {get; set;}
    public decimal ExpectedPrice {get; set;}
}

public class ConstructionValidator : AbstractValidator<Construction>
{
    public ConstructionValidator()
    {
        RuleFor(p => p.Name).MaximumLength(255).NotEmpty();
        RuleFor(p => p.StartDate).NotEmpty();
        RuleFor(p => p.FinishDate).NotEmpty();
        RuleFor(p => p.ExpectedPrice).PrecisionScale(19, 8, true);
    }
}