using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels;

public class MessageRequest
{
    [FromForm(Name = "Message_Text")]
    [Required(ErrorMessage = "Write a message before you send it.")]
    [MaxLength(2000, ErrorMessage = "Keep your message under 2,000 characters.")]
    public string MessageText { get; set; } = string.Empty;
}
