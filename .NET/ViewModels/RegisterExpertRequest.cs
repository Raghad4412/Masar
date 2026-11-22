using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Masar.Data;
using Masar.Support;
using Masar.Support.Rules;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class RegisterExpertRequest : IPrepareForValidation, IValidatableObject
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

    [FromForm(Name = "Specialization")]
    [Required(ErrorMessage = "Choose your specialization.")]
    [OneOf("specializations", ErrorMessage = "Choose your specialization.")]
    public string Specialization { get; set; } = string.Empty;

    [FromForm(Name = "Experience_Years")]
    [Required(ErrorMessage = "Enter your years of experience.")]
    [Range(0, 50, ErrorMessage = "Enter your years of experience between 0 and 50.")]
    public int? ExperienceYears { get; set; }

    [FromForm(Name = "Session_Price")]
    [Required(ErrorMessage = "Set a price of at least 100 SAR.")]
    public decimal? SessionPrice { get; set; }

    [FromForm(Name = "Bio")]
    [Required(ErrorMessage = "Shown on your public profile. 60 characters minimum.")]
    [MinLength(60, ErrorMessage = "Shown on your public profile. 60 characters minimum.")]
    [MaxLength(1000)]
    public string Bio { get; set; } = string.Empty;

    [FromForm(Name = "Portfolio_Link")]
    [MaxLength(200)]
    [Url(ErrorMessage = "Enter a valid link, for example behance.net/yourname.")]
    public string? PortfolioLink { get; set; }

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
        var link = (PortfolioLink ?? string.Empty).Trim();
        if (link != string.Empty && !Regex.IsMatch(link, "^https?://", RegexOptions.IgnoreCase))
        {
            link = "https://" + link;
        }

        Email = (Email ?? string.Empty).Trim().ToLowerInvariant();
        Phone = MasarPhoneAttribute.Normalise(Phone) ?? string.Empty;
        PortfolioLink = link == string.Empty ? null : link;
    }

    public decimal MinPrice(AppDbContext db)
    {
        var typeName = Config.Masar.Consulting.TryGetValue(Specialization ?? string.Empty, out var name) ? name : null;
        if (typeName != null)
        {
            return db.ConsultationTypes.Where(t => t.TypeName == typeName).Select(t => t.BasePrice).FirstOrDefault();
        }

        return Config.Masar.ServiceExpertMinPrice;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        var db = (AppDbContext?)context.GetService(typeof(AppDbContext));
        var minPrice = db != null ? MinPrice(db) : Config.Masar.ServiceExpertMinPrice;
        var priceMessage = "Set a price of at least " + Helpers.Sar(minPrice, 0) + " SAR.";

        if (SessionPrice == null || SessionPrice < minPrice)
        {
            yield return new ValidationResult(priceMessage, new[] { nameof(SessionPrice) });
        }
        else if (SessionPrice > 20000)
        {
            yield return new ValidationResult("The highest price on Masar is 20,000 SAR.", new[] { nameof(SessionPrice) });
        }

        if (Password != PasswordConfirmation)
        {
            yield return new ValidationResult("The passwords do not match.", new[] { nameof(Password) });
        }

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
