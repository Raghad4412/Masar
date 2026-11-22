using System.ComponentModel.DataAnnotations;
using Masar.Data;
using Masar.Support;
using Masar.Support.Rules;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels.Admin;

public class VerificationDecisionRequest : IValidatableObject
{
    [FromForm(Name = "status")]
    [Required(ErrorMessage = "Choose approve or reject.")]
    [RegularExpression("^(Approved|Rejected)$", ErrorMessage = "Choose approve or reject.")]
    public string Status { get; set; } = string.Empty;

    [FromForm(Name = "reason")]
    [MaxLength(300)]
    public string? Reason { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Status == "Rejected" && (Reason ?? string.Empty).Trim().Length < 10)
        {
            yield return new ValidationResult("A rejection must name the reason, in at least 10 characters.", new[] { nameof(Reason) });
        }
    }
}

public class ConsultationTypeRequest : IValidatableObject
{
    [FromForm(Name = "Type_Name")]
    [Required(ErrorMessage = "Enter the name of the consultation type.")]
    [MaxLength(60)]
    public string TypeName { get; set; } = string.Empty;

    [FromForm(Name = "Description")]
    [MaxLength(300)]
    public string? Description { get; set; }

    [FromForm(Name = "Base_Price")]
    public decimal? BasePrice { get; set; }

    public int? IgnoreId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (BasePrice == null)
        {
            yield return new ValidationResult("Enter the base price in SAR.", new[] { nameof(BasePrice) });
        }
        else if (BasePrice <= 0)
        {
            yield return new ValidationResult("The base price must be more than 0 SAR.", new[] { nameof(BasePrice) });
        }

        var db = (AppDbContext?)context.GetService(typeof(AppDbContext));
        if (db != null && db.ConsultationTypes.Any(t => t.TypeName == TypeName && t.ConsultationTypeId != (IgnoreId ?? 0)))
        {
            yield return new ValidationResult("A consultation type with this name already exists.", new[] { nameof(TypeName) });
        }
    }
}

public class ServiceTypeRequest : IValidatableObject
{
    [FromForm(Name = "Type_Name")]
    [Required(ErrorMessage = "Enter the name of the service field.")]
    [MaxLength(60)]
    public string TypeName { get; set; } = string.Empty;

    [FromForm(Name = "Service_Packge")]
    [Required(ErrorMessage = "Choose Standard, Professional or Premium.")]
    [RegularExpression("^(Standard|Professional|Premium)$", ErrorMessage = "Choose Standard, Professional or Premium.")]
    public string ServicePackge { get; set; } = string.Empty;

    [FromForm(Name = "Description")]
    [Required(ErrorMessage = "List what the package includes, one bullet per line.")]
    [MaxLength(400)]
    public string Description { get; set; } = string.Empty;

    [FromForm(Name = "Min_Price")]
    public decimal? MinPrice { get; set; }

    public int? IgnoreId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (MinPrice == null)
        {
            yield return new ValidationResult("Enter the minimum price in SAR.", new[] { nameof(MinPrice) });
        }
        else if (MinPrice <= 0)
        {
            yield return new ValidationResult("The minimum price must be more than 0 SAR.", new[] { nameof(MinPrice) });
        }

        var db = (AppDbContext?)context.GetService(typeof(AppDbContext));
        if (db != null && db.ServiceTypes.Any(t => t.TypeName == TypeName && t.ServicePackge == ServicePackge && t.ServiceTypeId != (IgnoreId ?? 0)))
        {
            yield return new ValidationResult(
                "This field already has a " + ServicePackge.ToLowerInvariant() + " package.",
                new[] { nameof(TypeName) });
        }
    }
}

public class IdeaServiceInput
{
    public int? ServiceTypeId { get; set; }

    public string? Description { get; set; }

    public decimal? EstimatedCost { get; set; }
}

public class IdeaConsultationInput
{
    public int? ConsultationTypeId { get; set; }

    public string? Description { get; set; }

    public decimal? EstimatedCost { get; set; }
}

public class IdeaInput
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public List<IdeaServiceInput> Services { get; set; } = new();

    public List<IdeaConsultationInput> Consultations { get; set; } = new();
}

public class DeliverIdeasRequest : IValidatableObject
{
    [FromForm(Name = "ideas")]
    public List<IdeaInput> Ideas { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        var db = (AppDbContext?)context.GetService(typeof(AppDbContext));

        if (Ideas.Count != 3)
        {
            yield return new ValidationResult("A package is exactly three ideas.", new[] { "ideas" });
            yield break;
        }

        for (var i = 0; i < Ideas.Count; i++)
        {
            var idea = Ideas[i];

            if (string.IsNullOrWhiteSpace(idea.Name))
            {
                yield return new ValidationResult("Give every idea a name.", new[] { "ideas." + i + ".name" });
            }
            if (string.IsNullOrWhiteSpace(idea.Description))
            {
                yield return new ValidationResult("Describe every idea for the client.", new[] { "ideas." + i + ".description" });
            }

            for (var s = 0; s < idea.Services.Count; s++)
            {
                var line = idea.Services[s];
                if (line.EstimatedCost != null && line.EstimatedCost <= 0)
                {
                    yield return new ValidationResult("An estimated cost must be more than 0 SAR.", new[] { "ideas." + i + ".services." + s + ".Estimated_Cost" });
                }
                if (line.ServiceTypeId != null && db != null && !db.ServiceTypes.Any(t => t.ServiceTypeId == line.ServiceTypeId))
                {
                    yield return new ValidationResult("The selected service type is not valid.", new[] { "ideas." + i + ".services." + s + ".Service_Type_ID" });
                }
            }

            for (var c = 0; c < idea.Consultations.Count; c++)
            {
                var line = idea.Consultations[c];
                if (line.EstimatedCost != null && line.EstimatedCost <= 0)
                {
                    yield return new ValidationResult("An estimated cost must be more than 0 SAR.", new[] { "ideas." + i + ".consultations." + c + ".Estimated_Cost" });
                }
                if (line.ConsultationTypeId != null && db != null && !db.ConsultationTypes.Any(t => t.ConsultationTypeId == line.ConsultationTypeId))
                {
                    yield return new ValidationResult("The selected consultation type is not valid.", new[] { "ideas." + i + ".consultations." + c + ".Consultation_Type_ID" });
                }
            }
        }
    }
}
