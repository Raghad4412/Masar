using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Masar.Support;

namespace Masar.Models;

[Table("Verification_Document")]
public class VerificationDocument
{
    [Key]
    [Column("Document_ID")]
    public int DocumentId { get; set; }

    [Column("Expert_ID")]
    public int ExpertId { get; set; }

    [Column("Admin_ID")]
    public int? AdminId { get; set; }

    [Column("Document_Type")]
    public string DocumentType { get; set; } = string.Empty;

    [Column("Status")]
    public string Status { get; set; } = "Uploaded";

    [Column("Submitted_Date")]
    public DateTime SubmittedDate { get; set; }

    [Column("File_Path")]
    public string FilePath { get; set; } = string.Empty;

    public Expert? Expert { get; set; }

    public Admin? Admin { get; set; }

    public bool FileExists()
    {
        return Img.Exists(FilePath);
    }

    public string? FileUrl()
    {
        return Img.Url(FilePath);
    }

    public bool IsRequired()
    {
        return Expert.RequiredDocuments.Contains(DocumentType);
    }
}
