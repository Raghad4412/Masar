using System.ComponentModel.DataAnnotations;
using Masar.Models;
using Masar.Support;
using Masar.Support.Rules;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels.Client;

public class IdeaRequestForm : IPrepareForValidation, IValidatableObject
{
    [FromForm(Name = "Budget")]
    [Required(ErrorMessage = "Choose your budget.")]
    [OneOf("budgets", ErrorMessage = "Choose your budget.")]
    public string Budget { get; set; } = string.Empty;

    [FromForm(Name = "City")]
    [Required(ErrorMessage = "Choose your city.")]
    [OneOf("cities", ErrorMessage = "Choose your city.")]
    public string City { get; set; } = string.Empty;

    [FromForm(Name = "interests")]
    public List<string> Interests { get; set; } = new();

    public void Prepare()
    {
        Interests = (Interests ?? new List<string>())
            .Select(i => (i ?? string.Empty).Trim())
            .Where(i => i != string.Empty)
            .Distinct()
            .ToList();
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Interests.Count < 1)
        {
            yield return new ValidationResult("Choose at least one interest.", new[] { nameof(Interests) });
        }
        if (Interests.Count > 6)
        {
            yield return new ValidationResult("Choose up to six interests.", new[] { nameof(Interests) });
        }
    }
}

public class IdeasIndexViewModel
{
    public List<BusinessIdea> Ideas { get; set; } = new();

    public Dictionary<string, List<BusinessIdea>> Packages { get; set; } = new();

    public decimal Price { get; set; }

    public decimal Total { get; set; }
}

public class IdeaCreateViewModel
{
    public Models.Client Client { get; set; } = null!;

    public User User { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal Commission { get; set; }

    public decimal Vat { get; set; }

    public decimal Total { get; set; }
}

public class IdeaPayViewModel
{
    public BusinessIdea Idea { get; set; } = null!;

    public bool Card { get; set; }

    public decimal Price { get; set; }

    public decimal Commission { get; set; }

    public decimal Vat { get; set; }

    public decimal Total { get; set; }
}

public class IdeaShowViewModel
{
    public BusinessIdea Idea { get; set; } = null!;

    public List<BusinessIdea> Package { get; set; } = new();

    public Payment? Payment { get; set; }

    public BusinessIdea? Chosen { get; set; }

    public Models.Client Client { get; set; } = null!;
}
