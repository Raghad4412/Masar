using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Masar.Support;

namespace Masar.Models;

[Table("Business_Idea")]
public class BusinessIdea
{
    [Key]
    [Column("Business_Idea_ID")]
    public int BusinessIdeaId { get; set; }

    [Column("Idea_Name")]
    public string IdeaName { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Column("status")]
    public string Status { get; set; } = "Requested";

    [Column("Expiration_Date", TypeName = "date")]
    public DateTime? ExpirationDate { get; set; }

    [Column("Client_ID")]
    public int ClientId { get; set; }

    public Client? Client { get; set; }

    public ICollection<BusinessService> Services { get; set; } = new List<BusinessService>();

    public ICollection<BusinessConsultation> Consultations { get; set; } = new List<BusinessConsultation>();

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [NotMapped]
    public string Reference
    {
        get
        {
            var year = ExpirationDate != null ? ExpirationDate.Value.ToString("yyyy") : Clock.Now.ToString("yyyy");
            return "MSR-B-" + year + "-" + BusinessIdeaId.ToString().PadLeft(5, '0');
        }
    }

    public decimal EstimatedCost()
    {
        return Services.Sum(s => s.EstimatedCost) + Consultations.Sum(c => c.EstimatedCost);
    }
}
