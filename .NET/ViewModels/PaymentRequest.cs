using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Masar.Support;
using Masar.Support.Dto;
using Masar.Support.Services.Gateway;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class PaymentRequest : IValidatableObject
{
    [FromForm(Name = "Payment_Method")]
    [Required(ErrorMessage = "Choose how to pay.")]
    [RegularExpression("^(mada|Apple Pay|Credit card)$", ErrorMessage = "Choose how to pay.")]
    public string PaymentMethod { get; set; } = string.Empty;

    [FromForm(Name = "card_number")]
    public string? CardNumber { get; set; }

    [FromForm(Name = "card_expiry")]
    public string? CardExpiry { get; set; }

    [FromForm(Name = "card_cvc")]
    public string? CardCvc { get; set; }

    [FromForm(Name = "card_name")]
    public string? CardName { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (PaymentMethod != "Credit card")
        {
            yield break;
        }

        if (string.IsNullOrWhiteSpace(CardNumber))
        {
            yield return new ValidationResult("Enter the 16 digits of your card.", new[] { nameof(CardNumber) });
        }
        else if (!FakeGateway.SixteenDigits(Regex.Replace(CardNumber, @"\D", string.Empty)))
        {
            yield return new ValidationResult("Enter the 16 digits of your card.", new[] { nameof(CardNumber) });
        }

        if (string.IsNullOrWhiteSpace(CardExpiry))
        {
            yield return new ValidationResult("Enter the expiry date as MM / YY.", new[] { nameof(CardExpiry) });
        }
        else
        {
            var m = Regex.Match(CardExpiry, @"^(0[1-9]|1[0-2])\s*/\s*(\d{2})$");
            if (!m.Success)
            {
                yield return new ValidationResult("Enter the expiry date as MM / YY.", new[] { nameof(CardExpiry) });
            }
            else
            {
                var end = new DateTime(2000 + int.Parse(m.Groups[2].Value), int.Parse(m.Groups[1].Value), 1).EndOfMonth();
                if (end.IsPast())
                {
                    yield return new ValidationResult("This card has expired. Use another card.", new[] { nameof(CardExpiry) });
                }
            }
        }

        if (string.IsNullOrWhiteSpace(CardCvc))
        {
            yield return new ValidationResult("Enter the security code from the back of the card.", new[] { nameof(CardCvc) });
        }
        else if (!Regex.IsMatch(CardCvc, @"^\d{3,4}$"))
        {
            yield return new ValidationResult("Enter the 3 or 4 digit security code.", new[] { nameof(CardCvc) });
        }

        if (string.IsNullOrWhiteSpace(CardName) || !Regex.IsMatch(CardName, @"^[\p{L}\s\-'.]+$") || CardName.Length > 60)
        {
            yield return new ValidationResult("Enter the name printed on the card.", new[] { nameof(CardName) });
        }
    }

    public CardDetails Card()
    {
        return new CardDetails
        {
            CardNumber = CardNumber,
            CardExpiry = CardExpiry,
            CardCvc = CardCvc,
            CardName = CardName,
        };
    }
}
