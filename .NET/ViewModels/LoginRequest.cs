using System.ComponentModel.DataAnnotations;
using Masar.Support;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class LoginRequest : IPrepareForValidation
{
    [FromForm(Name = "Email")]
    [Required(ErrorMessage = "Enter a valid email address, for example name@example.com.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address, for example name@example.com.")]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [FromForm(Name = "Password")]
    [Required(ErrorMessage = "Enter your password.")]
    public string Password { get; set; } = string.Empty;

    [FromForm(Name = "remember")]
    public bool Remember { get; set; }

    public void Prepare()
    {
        Email = (Email ?? string.Empty).Trim().ToLowerInvariant();
    }
}
