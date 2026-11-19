using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Masar.Models;

[Table("Client")]
public class Client
{
    [Key]
    [Column("Client_ID")]
    public int ClientId { get; set; }

    [Column("User_ID")]
    public int UserId { get; set; }

    [Column("Budget")]
    public string? Budget { get; set; }

    [Column("Interests")]
    public string? Interests { get; set; }

    public User? User { get; set; }

    public ICollection<ConsultationSession> Sessions { get; set; } = new List<ConsultationSession>();

    public ICollection<ServiceRequest> Requests { get; set; } = new List<ServiceRequest>();

    public ICollection<BusinessIdea> Ideas { get; set; } = new List<BusinessIdea>();

    [NotMapped]
    public List<string> InterestList
    {
        get
        {
            if (string.IsNullOrEmpty(Interests))
            {
                return new List<string>();
            }
            return Interests.Split(',').Select(i => i.Trim()).Where(i => i != string.Empty).ToList();
        }
    }
}
