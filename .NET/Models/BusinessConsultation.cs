using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Masar.Models;

[Table("Business_Consultation")]
public class BusinessConsultation
{
    [Key]
    [Column("Business_Consultation_ID")]
    public int BusinessConsultationId { get; set; }

    [Column("Consultation_Type_ID")]
    public int ConsultationTypeId { get; set; }

    [Column("Business_Idea_ID")]
    public int BusinessIdeaId { get; set; }

    [Column("Description")]
    public string Description { get; set; } = string.Empty;

    [Column("Estimated_Cost")]
    public decimal EstimatedCost { get; set; }

    public ConsultationType? Type { get; set; }

    public BusinessIdea? Idea { get; set; }
}
