using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Masar.Support;

namespace Masar.Models;

[Table("Service_Request")]
public class ServiceRequest
{
    [Key]
    [Column("Service_Request_ID")]
    public int ServiceRequestId { get; set; }

    [Column("Client_ID")]
    public int ClientId { get; set; }

    [Column("Service_Type_ID")]
    public int ServiceTypeId { get; set; }

    [Column("Request_Description")]
    public string RequestDescription { get; set; } = string.Empty;

    [Column("Budget_Range")]
    public string BudgetRange { get; set; } = string.Empty;

    [Column("Deadline", TypeName = "date")]
    public DateTime Deadline { get; set; }

    [Column("Status")]
    public string Status { get; set; } = "Open";

    [Column("Agreed_Price")]
    public decimal? AgreedPrice { get; set; }

    [Column("Delivery_Date", TypeName = "date")]
    public DateTime? DeliveryDate { get; set; }

    [Column("Title")]
    public string Title { get; set; } = string.Empty;

    public Client? Client { get; set; }

    public ServiceType? Type { get; set; }

    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public ICollection<Message> Messages { get; set; } = new List<Message>();

    public Review? Review { get; set; }

    public Proposal? AcceptedProposal()
    {
        return Proposals.FirstOrDefault(p => p.ProposalStatus == "Accepted");
    }

    [NotMapped]
    public string Reference
    {
        get
        {
            var prefix = Status == "In Progress" || Status == "Completed" ? "MSR-S-" : "MSR-R-";
            return prefix + Year() + "-" + ServiceRequestId.ToString().PadLeft(5, '0');
        }
    }

    [NotMapped]
    public string RequestReference => "MSR-R-" + Year() + "-" + ServiceRequestId.ToString().PadLeft(5, '0');

    private string Year()
    {
        var first = Payments.OrderBy(p => p.PaymentDate).Select(p => (DateTime?)p.PaymentDate).FirstOrDefault();
        if (first != null)
        {
            return first.Value.ToString("yyyy");
        }
        return Deadline != default ? Deadline.ToString("yyyy") : Clock.Now.ToString("yyyy");
    }

    public int? BudgetMin()
    {
        var parts = (BudgetRange ?? string.Empty).Split('-');
        return parts.Length > 0 ? Helpers.ToInt(parts[0]) : null;
    }

    public int? BudgetMax()
    {
        var parts = (BudgetRange ?? string.Empty).Split('-');
        return parts.Length > 1 ? Helpers.ToInt(parts[1]) : null;
    }

    [NotMapped]
    public string BudgetDisplay => Helpers.NumberFormat(BudgetMin() ?? 0) + " - " + Helpers.NumberFormat(BudgetMax() ?? 0) + " SAR";

    public string FieldName()
    {
        return Type?.TypeName ?? string.Empty;
    }
}
