using System.ComponentModel.DataAnnotations;
using Masar.Models;
using Masar.Support.Rules;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class ChangePasswordRequest : IValidatableObject
{
    [FromForm(Name = "current_password")]
    [Required(ErrorMessage = "Enter your current password.")]
    public string CurrentPassword { get; set; } = string.Empty;

    [FromForm(Name = "Password")]
    [Required(ErrorMessage = "Enter a new password.")]
    [MaxLength(64)]
    [StrongPassword]
    public string Password { get; set; } = string.Empty;

    [FromForm(Name = "Password_confirmation")]
    public string? PasswordConfirmation { get; set; }

    public User? Owner { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Password != PasswordConfirmation)
        {
            yield return new ValidationResult("The passwords do not match.", new[] { nameof(Password) });
        }

        if (Owner != null)
        {
            var lower = (Password ?? string.Empty).ToLowerInvariant();
            foreach (var name in new[] { Owner.FirstName, Owner.LastName })
            {
                var n = name.Trim().ToLowerInvariant();
                if (n.Length >= 3 && lower.Contains(n))
                {
                    yield return new ValidationResult("Your password must not contain your name.", new[] { nameof(Password) });
                    break;
                }
            }

            if (!string.IsNullOrEmpty(CurrentPassword) && !BCrypt.Net.BCrypt.Verify(CurrentPassword, Owner.Password))
            {
                yield return new ValidationResult("Your current password is not correct.", new[] { nameof(CurrentPassword) });
            }
        }
    }
}
