using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Masar.Models;

[Table("Business_Service")]
public class BusinessService
{
    [Key]
    [Column("Business_Service_ID")]
    public int BusinessServiceId { get; set; }

    [Column("Service_Type_ID")]
    public int ServiceTypeId { get; set; }

    [Column("Business_Idea_ID")]
    public int BusinessIdeaId { get; set; }

    [Column("Description")]
    public string Description { get; set; } = string.Empty;

    [Column("Estimated_Cost")]
    public decimal EstimatedCost { get; set; }

    public ServiceType? Type { get; set; }

    public BusinessIdea? Idea { get; set; }
}
