using System.Text.RegularExpressions;

namespace DataModels;
using FluentValidation;

public enum Type{
    Internal = 0,
    External = 1
}
public class Organization{

    public int ID {get; set;}

    public Type Type {get; set;}
    
    public string CountryCode {get; set;}
    public string VATNumber {get; set;}
    public string Name {get; set;}
}

public class OrganizationValidator : AbstractValidator<Organization>
{
    public OrganizationValidator()
    {
        RuleFor(p => p.Type).NotNull();
        RuleFor(p => p.CountryCode).MaximumLength(16).NotEmpty();
        RuleFor(p => p.VATNumber).Must((p, txt) => Regex.IsMatch(txt, @$"({p.CountryCode})[0-9]{{8,10}}$")).MaximumLength(16).WithMessage("Please specify correct VAT Number").NotEmpty();
        RuleFor(p => p.Name).MaximumLength(255).NotEmpty();
    }
    
    
    
}