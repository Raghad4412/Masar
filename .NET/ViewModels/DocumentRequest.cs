using System.ComponentModel.DataAnnotations;
using Masar.Data;
using Masar.Models;
using Masar.Support.Rules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class DocumentRequest : IValidatableObject
{
    public static readonly Dictionary<string, string> Types = new()
    {
        ["certificate"] = "Professional certificate",
        ["registration"] = "Commercial registration",
        ["licence"] = "Professional licence",
        ["portfolio"] = "Portfolio of previous work",
    };

    [FromForm(Name = "action")]
    public string? Action { get; set; }

    [FromForm(Name = "documents.certificate")]
    [FileRules("pdf,jpg,jpeg,png", 10240, ErrorMessage = "Upload a PDF, JPG or PNG file.")]
    public IFormFile? Certificate { get; set; }

    [FromForm(Name = "documents.registration")]
    [FileRules("pdf,jpg,jpeg,png", 10240, ErrorMessage = "Upload a PDF, JPG or PNG file.")]
    public IFormFile? Registration { get; set; }

    [FromForm(Name = "documents.licence")]
    [FileRules("pdf,jpg,jpeg,png", 10240, ErrorMessage = "Upload a PDF, JPG or PNG file.")]
    public IFormFile? Licence { get; set; }

    [FromForm(Name = "documents.portfolio")]
    [FileRules("pdf,jpg,jpeg,png", 10240, ErrorMessage = "Upload a PDF, JPG or PNG file.")]
    public IFormFile? Portfolio { get; set; }

    public Expert? Owner { get; set; }

    public bool Submitting()
    {
        return Action != "later";
    }

    public IFormFile? File(string key)
    {
        return key switch
        {
            "certificate" => Certificate,
            "registration" => Registration,
            "licence" => Licence,
            "portfolio" => Portfolio,
            _ => null,
        };
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        var db = (AppDbContext?)context.GetService(typeof(AppDbContext));

        foreach (var pair in Types)
        {
            if (pair.Key == "portfolio")
            {
                continue;
            }

            var alreadyHave = db != null && Owner != null
                && db.VerificationDocuments.Any(d => d.ExpertId == Owner.ExpertId && d.DocumentType == pair.Value);
            var required = Submitting() && !alreadyHave;

            if (required && File(pair.Key) == null)
            {
                yield return new ValidationResult(
                    "Upload your " + Support.Helpers.Lcfirst(pair.Value) + " to continue.",
                    new[] { pair.Key switch { "certificate" => nameof(Certificate), "registration" => nameof(Registration), _ => nameof(Licence) } });
            }
        }
    }
}
