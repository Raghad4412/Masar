using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class ReviewRequest
{
    [FromForm(Name = "Rating")]
    [Required(ErrorMessage = "Choose a rating.")]
    [Range(1, 5, ErrorMessage = "Choose a rating.")]
    public int? Rating { get; set; }

    [FromForm(Name = "Comment")]
    [MaxLength(1000, ErrorMessage = "Keep your review under 1,000 characters.")]
    public string? Comment { get; set; }
}
