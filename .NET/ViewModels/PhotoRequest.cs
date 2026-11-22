using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class PhotoRequest : IValidatableObject
{
    [FromForm(Name = "photo")]
    public IFormFile? Photo { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Photo == null)
        {
            yield return new ValidationResult("Choose a picture to upload.", new[] { nameof(Photo) });
            yield break;
        }

        var extension = Path.GetExtension(Photo.FileName).TrimStart('.').ToLowerInvariant();
        if (!new[] { "jpg", "jpeg", "png", "gif", "bmp", "webp" }.Contains(extension) || !Photo.ContentType.StartsWith("image/"))
        {
            yield return new ValidationResult("Upload a JPG or PNG picture.", new[] { nameof(Photo) });
        }
        else if (Photo.Length > 2048 * 1024)
        {
            yield return new ValidationResult("The picture must be 2 MB or smaller.", new[] { nameof(Photo) });
        }
    }
}
