using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Masar.Models;

[Table("Consultation_Type")]
public class ConsultationType
{
    [Key]
    [Column("Consultation_Type_ID")]
    public int ConsultationTypeId { get; set; }

    [Column("Type_Name")]
    public string TypeName { get; set; } = string.Empty;

    [Column("Description")]
    public string? Description { get; set; }

    [Column("Base_Price")]
    public decimal BasePrice { get; set; }

    public ICollection<ConsultationSession> Sessions { get; set; } = new List<ConsultationSession>();

    [NotMapped]
    public int SessionsCount { get; set; }
}
