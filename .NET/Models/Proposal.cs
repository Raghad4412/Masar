using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Masar.Support;

namespace Masar.Models;

[Table("Proposal")]
public class Proposal
{
    [Key]
    [Column("Proposal_ID")]
    public int ProposalId { get; set; }

    [Column("Service_request_ID")]
    public int ServiceRequestId { get; set; }

    [Column("Expert_ID")]
    public int ExpertId { get; set; }

    [Column("Proposal_Price")]
    public decimal ProposalPrice { get; set; }

    [Column("Delivery_Time")]
    public int DeliveryTime { get; set; }

    [Column("Scope_OF_Work")]
    public string ScopeOfWork { get; set; } = string.Empty;

    [Column("Proposal_Status")]
    public string ProposalStatus { get; set; } = "Waiting";

    [Column("Expiration_Time")]
    public DateTime ExpirationTime { get; set; }

    public ServiceRequest? Request { get; set; }

    public Expert? Expert { get; set; }

    [NotMapped]
    public List<string> ScopeLines
    {
        get
        {
            var text = ScopeOfWork ?? string.Empty;
            var lines = Regex.Split(text, @"\r\n|\r|\n").Select(l => l.Trim()).Where(l => l != string.Empty).ToList();
            if (lines.Count > 1)
            {
                return lines;
            }
            var parts = text.Split(',').Select(l => l.Trim()).Where(l => l != string.Empty).ToList();
            return parts.Count > 1 ? parts : new List<string> { text };
        }
    }

    public decimal ExpertEarns()
    {
        return Helpers.Round(ProposalPrice * (1 - Config.Masar.Commission), 2);
    }

    public bool IsNew()
    {
        var sentAt = ExpirationTime.AddDays(-Config.Masar.ProposalValidityDays);
        return sentAt >= Clock.Now.AddDays(-1);
    }
}
