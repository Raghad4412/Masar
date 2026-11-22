using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class ForgotPasswordRequest
{
    [FromForm(Name = "Email")]
    [Required(ErrorMessage = "Enter a valid email address, for example name@example.com.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address, for example name@example.com.")]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;
}
