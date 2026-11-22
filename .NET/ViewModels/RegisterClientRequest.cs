using System.ComponentModel.DataAnnotations;
using Masar.Data;
using Masar.Support;
using Masar.Support.Rules;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class RegisterClientRequest : IPrepareForValidation, IValidatableObject
{
    [FromForm(Name = "first_name")]
    [Required(ErrorMessage = "Enter your first name.")]
    [MaxLength(50)]
    [PersonName(ErrorMessage = "Enter your first name.")]
    public string FirstName { get; set; } = string.Empty;

    [FromForm(Name = "last_name")]
    [Required(ErrorMessage = "Enter your last name.")]
    [MaxLength(50)]
    [PersonName(ErrorMessage = "Enter your last name.")]
    public string LastName { get; set; } = string.Empty;

    [FromForm(Name = "Email")]
    [Required(ErrorMessage = "Enter a valid email address, for example name@example.com.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address, for example name@example.com.")]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [FromForm(Name = "Phone")]
    [Required(ErrorMessage = "Enter a Saudi mobile number: +966 5X XXX XXXX or 05X XXX XXXX.")]
    [MasarPhone]
    public string Phone { get; set; } = string.Empty;

    [FromForm(Name = "City")]
    [Required(ErrorMessage = "Choose your city.")]
    [OneOf("cities", ErrorMessage = "Choose your city.")]
    public string City { get; set; } = string.Empty;

    [FromForm(Name = "Budget")]
    [Required(ErrorMessage = "Choose your budget.")]
    [OneOf("budgets", ErrorMessage = "Choose your budget.")]
    public string Budget { get; set; } = string.Empty;

    [FromForm(Name = "interests")]
    public List<string> Interests { get; set; } = new();

    [FromForm(Name = "Password")]
    [Required(ErrorMessage = "Enter a password.")]
    [MaxLength(64)]
    [StrongPassword]
    [NotContainName("FirstName", "LastName")]
    public string Password { get; set; } = string.Empty;

    [FromForm(Name = "Password_confirmation")]
    public string? PasswordConfirmation { get; set; }

    [FromForm(Name = "terms")]
    [Accepted(ErrorMessage = "Accept the Terms of use and the Privacy policy to continue.")]
    public string? Terms { get; set; }

    public void Prepare()
    {
        Interests = (Interests ?? new List<string>())
            .Select(i => (i ?? string.Empty).Trim())
            .Where(i => i != string.Empty)
            .Distinct()
            .ToList();
        Email = (Email ?? string.Empty).Trim().ToLowerInvariant();
        Phone = MasarPhoneAttribute.Normalise(Phone) ?? string.Empty;
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
        if (Interests.Any(i => i.Length > 30))
        {
            yield return new ValidationResult("Choose up to six interests.", new[] { nameof(Interests) });
        }
        if (Password != PasswordConfirmation)
        {
            yield return new ValidationResult("The passwords do not match.", new[] { nameof(Password) });
        }

        var db = (AppDbContext?)context.GetService(typeof(AppDbContext));
        if (db != null)
        {
            if (db.Users.Any(u => u.Email == Email))
            {
                yield return new ValidationResult("An account with this email already exists.", new[] { nameof(Email) });
            }
            if (db.Users.Any(u => u.Phone == Phone))
            {
                yield return new ValidationResult("This phone number is already registered.", new[] { nameof(Phone) });
            }
        }
    }
}
