using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Masar.Support;

namespace Masar.Models;

[Table("Consultation_Session")]
public class ConsultationSession
{
    [Key]
    [Column("Consultation_Session_ID")]
    public int ConsultationSessionId { get; set; }

    [Column("Consultation_Type_ID")]
    public int ConsultationTypeId { get; set; }

    [Column("Expert_ID")]
    public int ExpertId { get; set; }

    [Column("Client_ID")]
    public int? ClientId { get; set; }

    [Column("Price")]
    public decimal Price { get; set; }

    [Column("Session_Date", TypeName = "date")]
    public DateTime SessionDate { get; set; }

    [Column("Start_Time", TypeName = "time(0)")]
    public TimeSpan StartTime { get; set; }

    [Column("End_Time", TypeName = "time(0)")]
    public TimeSpan EndTime { get; set; }

    [Column("Session_Status")]
    public string SessionStatus { get; set; } = "Available";

    [Column("Session_Topic")]
    public string? SessionTopic { get; set; }

    public ConsultationType? Type { get; set; }

    public Expert? Expert { get; set; }

    public Client? Client { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public ICollection<Message> Messages { get; set; } = new List<Message>();

    public Review? Review { get; set; }

    [NotMapped]
    public string Reference => "MSR-C-" + StartsAt().ToString("yyyy") + "-" + ConsultationSessionId.ToString().PadLeft(5, '0');

    public DateTime StartsAt()
    {
        return SessionDate.Date + StartTime;
    }

    public DateTime EndsAt()
    {
        return SessionDate.Date + EndTime;
    }

    public string TimeString(string column)
    {
        var value = column == "End_Time" ? EndTime : StartTime;
        return value.ToString(@"hh\:mm\:ss");
    }

    [NotMapped]
    public string SlotLabel => TimeString("Start_Time").Substring(0, 5);

    public Payment? LivePayment()
    {
        var all = Payments.OrderByDescending(p => p.PaymentId).ToList();
        return all.FirstOrDefault(p => p.PaymentStatus != "Failed") ?? all.FirstOrDefault();
    }

    public bool IsLive()
    {
        return SessionStatus == "Held" || SessionStatus == "Scheduled";
    }
}
