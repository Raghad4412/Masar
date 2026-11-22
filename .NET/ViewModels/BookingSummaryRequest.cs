using System.ComponentModel.DataAnnotations;
using Masar.Support.Rules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class BookingSummaryRequest
{
    [FromForm(Name = "Session_Topic")]
    [Required(ErrorMessage = "This field is required.")]
    [MinLength(10, ErrorMessage = "Give the expert a little more detail: at least 10 characters.")]
    [MaxLength(200, ErrorMessage = "Keep the topic under 200 characters.")]
    public string SessionTopic { get; set; } = string.Empty;

    [FromForm(Name = "notes")]
    [MaxLength(1000, ErrorMessage = "Keep your notes under 1,000 characters.")]
    public string? Notes { get; set; }

    [FromForm(Name = "files")]
    [MaxLength(5, ErrorMessage = "Attach up to five files.")]
    [FileRules("pdf,doc,docx,jpg,jpeg,png,zip", 10240, ErrorMessage = "Attach a PDF, Word document, image or ZIP file. Each file must be 10 MB or smaller.")]
    public List<IFormFile> Files { get; set; } = new();
}
