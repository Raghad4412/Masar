using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Masar.Models;

[Table("Review")]
public class Review
{
    [Key]
    [Column("Review_ID")]
    public int ReviewId { get; set; }

    [Column("User_ID")]
    public int UserId { get; set; }

    [Column("Service_Request_ID")]
    public int? ServiceRequestId { get; set; }

    [Column("Consultation_Session_ID")]
    public int? ConsultationSessionId { get; set; }

    [Column("Rating")]
    public int Rating { get; set; }

    [Column("Comment")]
    public string? Comment { get; set; }

    [Column("Review_Date")]
    public DateTime ReviewDate { get; set; }

    [Column("Is_Visible")]
    public bool IsVisible { get; set; } = true;

    public User? User { get; set; }

    public ServiceRequest? Request { get; set; }

    public ConsultationSession? Session { get; set; }

    public static IQueryable<Review> Visible(IQueryable<Review> query)
    {
        return query.Where(r => r.IsVisible);
    }

    public Expert? Expert()
    {
        if (ConsultationSessionId != null)
        {
            return Session?.Expert;
        }
        return Request?.AcceptedProposal()?.Expert;
    }
}
