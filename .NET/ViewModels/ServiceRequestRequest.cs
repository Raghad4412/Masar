using System.ComponentModel.DataAnnotations;
using Masar.Data;
using Masar.Models;
using Masar.Support;
using Masar.Support.Rules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class ServiceRequestRequest : IValidatableObject
{
    [FromForm(Name = "Service_Type_ID")]
    public int? ServiceTypeId { get; set; }

    [FromForm(Name = "field")]
    public string? Field { get; set; }

    [FromForm(Name = "Title")]
    [Required(ErrorMessage = "Give your request a short title.")]
    [MinLength(5, ErrorMessage = "Give your request a short title.")]
    [MaxLength(100, ErrorMessage = "Keep the title under 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [FromForm(Name = "Request_Description")]
    [Required(ErrorMessage = "Describe the work in at least 30 characters.")]
    [MinLength(30, ErrorMessage = "Describe the work in at least 30 characters.")]
    [MaxLength(2000, ErrorMessage = "Keep the description under 2,000 characters.")]
    public string RequestDescription { get; set; } = string.Empty;

    [FromForm(Name = "budget_min")]
    public int? BudgetMin { get; set; }

    [FromForm(Name = "budget_max")]
    public int? BudgetMax { get; set; }

    [FromForm(Name = "Deadline")]
    public DateTime? Deadline { get; set; }

    [FromForm(Name = "files")]
    [MaxLength(5, ErrorMessage = "Attach up to five files.")]
    [FileRules("jpg,jpeg,png,pdf,zip", 25600, ErrorMessage = "Attach an image, a PDF or a ZIP file. Each file must be 25 MB or smaller.")]
    public List<IFormFile> Files { get; set; } = new();

    public ServiceType? ResolvedType { get; set; }

    public static DateTime Earliest()
    {
        return Clock.Now.StartOfDay().AddDays(Config.Masar.RequestLeadDays);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        var db = (AppDbContext?)context.GetService(typeof(AppDbContext));
        var type = ServiceTypeId == null || db == null ? null : db.ServiceTypes.Find(ServiceTypeId.Value);
        ResolvedType = type;

        var typeMessage = "Choose a service type before you submit.";
        if (ServiceTypeId == null || type == null || (!string.IsNullOrEmpty(Field) && type.TypeName != Field))
        {
            yield return new ValidationResult(typeMessage, new[] { nameof(ServiceTypeId) });
        }

        var minPrice = type?.MinPrice ?? 0m;
        var minMessage = "The lowest budget starts at " + Helpers.Sar(minPrice, 0) + " SAR for this package.";
        if (BudgetMin == null || BudgetMin < Math.Max(1, (int)minPrice))
        {
            yield return new ValidationResult(minMessage, new[] { nameof(BudgetMin) });
        }

        if (BudgetMax == null || BudgetMin == null || BudgetMax <= BudgetMin)
        {
            yield return new ValidationResult("The highest budget must be more than the lowest.", new[] { nameof(BudgetMax) });
        }
        else if (BudgetMax > 1000000)
        {
            yield return new ValidationResult("The highest budget on Masar is 1,000,000 SAR.", new[] { nameof(BudgetMax) });
        }

        var earliest = Earliest();
        if (Deadline == null || Deadline.Value.Date < earliest.Date)
        {
            yield return new ValidationResult("Choose a date from " + earliest.Format("j M Y") + " on.", new[] { nameof(Deadline) });
        }
    }

    public static string Banner(int count)
    {
        var words = new Dictionary<int, string>
        {
            [1] = "One", [2] = "Two", [3] = "Three", [4] = "Four", [5] = "Five", [6] = "Six", [7] = "Seven",
        };
        var number = words.TryGetValue(count, out var word) ? word : count.ToString();

        return number + " " + (count == 1 ? "field needs" : "fields need")
            + " your attention before the request can be sent to the experts.";
    }
}
