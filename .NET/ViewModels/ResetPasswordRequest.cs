using System.ComponentModel.DataAnnotations;
using Masar.Models;
using Masar.Support;
using Masar.Support.Rules;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class ResetPasswordRequest : IValidatableObject
{
    [FromForm(Name = "Password")]
    [Required(ErrorMessage = "Enter a password.")]
    [MaxLength(64)]
    [StrongPassword]
    public string Password { get; set; } = string.Empty;

    [FromForm(Name = "Password_confirmation")]
    public string? PasswordConfirmation { get; set; }

    public User? Target { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Password != PasswordConfirmation)
        {
            yield return new ValidationResult("The passwords do not match.", new[] { nameof(Password) });
        }

        if (Target != null)
        {
            var names = new[] { Target.FirstName, Target.LastName };
            var lower = (Password ?? string.Empty).ToLowerInvariant();
            if (names.Any(n => n.Trim().Length >= 3 && lower.Contains(n.Trim().ToLowerInvariant())))
            {
                yield return new ValidationResult("Your password must not contain your name.", new[] { nameof(Password) });
            }

            if (BCrypt.Net.BCrypt.Verify(Password ?? string.Empty, Target.Password))
            {
                yield return new ValidationResult("Choose a password you have not used on Masar before.", new[] { nameof(Password) });
            }
        }
    }
}
