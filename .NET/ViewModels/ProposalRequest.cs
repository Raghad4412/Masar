using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class ProposalRequest : IValidatableObject
{
    [FromForm(Name = "Proposal_Price")]
    public decimal? ProposalPrice { get; set; }

    [FromForm(Name = "Delivery_Time")]
    public int? DeliveryTime { get; set; }

    [FromForm(Name = "Scope_OF_Work")]
    [Required(ErrorMessage = "Describe the scope in at least 50 characters.")]
    [MinLength(50, ErrorMessage = "Describe the scope in at least 50 characters.")]
    [MaxLength(1500, ErrorMessage = "Keep the scope under 1,500 characters.")]
    public string ScopeOfWork { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (ProposalPrice == null)
        {
            yield return new ValidationResult("Enter your price in SAR.", new[] { nameof(ProposalPrice) });
        }
        else if (ProposalPrice < 100 || ProposalPrice > 1000000)
        {
            yield return new ValidationResult("Enter a price between 100 and 1,000,000 SAR.", new[] { nameof(ProposalPrice) });
        }

        if (DeliveryTime == null)
        {
            yield return new ValidationResult("Enter the delivery time in days.", new[] { nameof(DeliveryTime) });
        }
        else if (DeliveryTime < 1 || DeliveryTime > 180)
        {
            yield return new ValidationResult("Enter a delivery time between 1 and 180 days.", new[] { nameof(DeliveryTime) });
        }
    }
}
