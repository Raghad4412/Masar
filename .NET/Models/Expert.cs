using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Masar.Support;

namespace Masar.Models;

[Table("Expert")]
public class Expert
{
    public static readonly string[] RequiredDocuments =
    {
        "Professional certificate",
        "Commercial registration",
        "Professional licence",
    };

    [Key]
    [Column("Expert_ID")]
    public int ExpertId { get; set; }

    [Column("User_ID")]
    public int UserId { get; set; }

    [Column("Specialization")]
    public string Specialization { get; set; } = string.Empty;

    [Column("Experience_Years")]
    public int ExperienceYears { get; set; }

    [Column("Bio")]
    public string Bio { get; set; } = string.Empty;

    [Column("Rating")]
    public decimal? Rating { get; set; }

    [Column("Session_Price")]
    public decimal SessionPrice { get; set; }

    [Column("Portfolio_Link")]
    public string? PortfolioLink { get; set; }

    public User? User { get; set; }

    public ICollection<VerificationDocument> Documents { get; set; } = new List<VerificationDocument>();

    public ICollection<Portfolio> Portfolio { get; set; } = new List<Portfolio>();

    public ICollection<ConsultationSession> Sessions { get; set; } = new List<ConsultationSession>();

    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();

    public bool IsConsultant()
    {
        return Config.Masar.Consulting.ContainsKey(Specialization);
    }

    public string? ConsultationTypeName()
    {
        return Config.Masar.Consulting.TryGetValue(Specialization, out var name) ? name : null;
    }

    [NotMapped]
    public string Title
    {
        get
        {
            return IsConsultant()
                ? Specialization.Replace(" Consulting", " Consultant").Replace("Financial Guidance", "Financial Adviser")
                : Specialization + " Expert";
        }
    }

    public bool IsVerified()
    {
        return User != null && User.Status == "Active";
    }

    public List<string> ServiceFields()
    {
        var fields = new List<string>();
        foreach (var pair in Config.Masar.Fields)
        {
            if (pair.Value.Contains(Specialization))
            {
                fields.Add(pair.Key);
            }
        }
        return fields;
    }
}
