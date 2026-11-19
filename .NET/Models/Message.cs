using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Masar.Models;

[Table("Message")]
public class Message
{
    [Key]
    [Column("Message_ID")]
    public int MessageId { get; set; }

    [Column("Service_Request_ID")]
    public int? ServiceRequestId { get; set; }

    [Column("Consultation_Session_ID")]
    public int? ConsultationSessionId { get; set; }

    [Column("Message_Text")]
    public string MessageText { get; set; } = string.Empty;

    [Column("Sent_Date")]
    public DateTime SentDate { get; set; }

    [Column("Sender_User_ID")]
    public int SenderUserId { get; set; }

    public User? Sender { get; set; }

    public ServiceRequest? Request { get; set; }

    public ConsultationSession? Session { get; set; }
}
