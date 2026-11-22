using System.ComponentModel.DataAnnotations;
using Masar.Data;
using Masar.Support;
using Masar.Support.Rules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels.ExpertArea;

public class DeliverFilesRequest
{
    [FromForm(Name = "files")]
    [Required(ErrorMessage = "Choose the files you want to deliver.")]
    [MaxLength(5, ErrorMessage = "Upload up to five files at a time.")]
    [FileRules("jpg,jpeg,png,pdf,zip,doc,docx", 25600, ErrorMessage = "Upload an image, a PDF, a Word document or a ZIP file. Each file must be 25 MB or smaller.")]
    public List<IFormFile> Files { get; set; } = new();
}

public class ExpertProfileRequest : IValidatableObject
{
    [FromForm(Name = "Bio")]
    [Required(ErrorMessage = "Shown on your public profile. 60 characters minimum.")]
    [MinLength(60, ErrorMessage = "Shown on your public profile. 60 characters minimum.")]
    [MaxLength(1000)]
    public string Bio { get; set; } = string.Empty;

    [FromForm(Name = "Experience_Years")]
    [Required(ErrorMessage = "Enter your years of experience.")]
    [Range(0, 50, ErrorMessage = "Enter your years of experience between 0 and 50.")]
    public int? ExperienceYears { get; set; }

    [FromForm(Name = "Session_Price")]
    public decimal? SessionPrice { get; set; }

    [FromForm(Name = "Portfolio_Link")]
    [MaxLength(200)]
    [Url(ErrorMessage = "Enter a valid link, for example behance.net/yourname.")]
    public string? PortfolioLink { get; set; }

    [FromForm(Name = "City")]
    [Required(ErrorMessage = "Choose your city.")]
    [OneOf("cities", ErrorMessage = "Choose your city.")]
    public string City { get; set; } = string.Empty;

    public decimal MinPrice { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        var message = "Set a price of at least " + Helpers.Sar(MinPrice, 0) + " SAR.";

        if (SessionPrice == null || SessionPrice < MinPrice)
        {
            yield return new ValidationResult(message, new[] { nameof(SessionPrice) });
        }
        else if (SessionPrice > 20000)
        {
            yield return new ValidationResult("The highest price on Masar is 20,000 SAR.", new[] { nameof(SessionPrice) });
        }
    }
}

public class PortfolioRequest : IValidatableObject
{
    [FromForm(Name = "Title")]
    [Required(ErrorMessage = "Give this piece of work a title.")]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [FromForm(Name = "description")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [FromForm(Name = "Image")]
    public IFormFile? Image { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Image == null)
        {
            yield return new ValidationResult("Choose a picture for this item.", new[] { nameof(Image) });
            yield break;
        }

        var extension = Path.GetExtension(Image.FileName).TrimStart('.').ToLowerInvariant();
        if (!new[] { "jpg", "jpeg", "png", "gif", "bmp", "webp" }.Contains(extension) || !Image.ContentType.StartsWith("image/"))
        {
            yield return new ValidationResult("Upload a JPG or PNG picture.", new[] { nameof(Image) });
        }
        else if (Image.Length > 5120 * 1024)
        {
            yield return new ValidationResult("The picture must be 5 MB or smaller.", new[] { nameof(Image) });
        }
    }
}

public class ExpertSettingsRequest : IPrepareForValidation, IValidatableObject
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

    [FromForm(Name = "current_password")]
    public string? CurrentPassword { get; set; }

    [FromForm(Name = "Password")]
    [MaxLength(64)]
    public string? Password { get; set; }

    [FromForm(Name = "Password_confirmation")]
    public string? PasswordConfirmation { get; set; }

    public int OwnerId { get; set; }

    public void Prepare()
    {
        Email = (Email ?? string.Empty).Trim().ToLowerInvariant();
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (!string.IsNullOrEmpty(Password))
        {
            if (!new StrongPasswordAttribute().IsValid(Password))
            {
                yield return new ValidationResult("Use at least 8 characters with a capital letter, a small letter and a number.", new[] { nameof(Password) });
            }
            if (Password != PasswordConfirmation)
            {
                yield return new ValidationResult("The passwords do not match.", new[] { nameof(Password) });
            }
        }

        var db = (AppDbContext?)context.GetService(typeof(AppDbContext));
        if (db != null)
        {
            if (db.Users.Any(u => u.Email == Email && u.UserId != OwnerId))
            {
                yield return new ValidationResult("An account with this email already exists.", new[] { nameof(Email) });
            }

            var phone = MasarPhoneAttribute.Normalise(Phone);
            if (db.Users.Any(u => u.Phone == phone && u.UserId != OwnerId))
            {
                yield return new ValidationResult("This phone number is already registered.", new[] { nameof(Phone) });
            }
        }
    }
}
