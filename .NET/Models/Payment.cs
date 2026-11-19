using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Masar.Support;

namespace Masar.Models;

[Table("Payment")]
public class Payment
{
    [Key]
    [Column("Payment_ID")]
    public int PaymentId { get; set; }

    [Column("Service_Request_ID")]
    public int? ServiceRequestId { get; set; }

    [Column("Consultation_Session_ID")]
    public int? ConsultationSessionId { get; set; }

    [Column("Business_Idea_ID")]
    public int? BusinessIdeaId { get; set; }

    [Column("Amount")]
    public decimal Amount { get; set; }

    [Column("Payment_Method")]
    public string PaymentMethod { get; set; } = string.Empty;

    [Column("Payment_Date")]
    public DateTime PaymentDate { get; set; }

    [Column("Payment_Status")]
    public string PaymentStatus { get; set; } = "Pending";

    public ServiceRequest? Request { get; set; }

    public ConsultationSession? Session { get; set; }

    public BusinessIdea? Idea { get; set; }

    [NotMapped]
    public string Reference => "PAY-" + PaymentDate.ToString("yyyy") + "-" + PaymentId.ToString().PadLeft(6, '0');

    [NotMapped]
    public string InvoiceNumber => "INV-" + PaymentDate.ToString("yyyy") + "-" + PaymentId.ToString().PadLeft(6, '0');

    public decimal Price()
    {
        var factor = 1 + Config.Masar.Commission + Config.Masar.Vat;
        return Helpers.Round(Amount / factor, 2);
    }

    public decimal Commission()
    {
        return Helpers.Round(Price() * Config.Masar.Commission, 2);
    }

    public decimal Vat()
    {
        return Helpers.Round(Price() * Config.Masar.Vat, 2);
    }

    public decimal Subtotal()
    {
        return Helpers.Round(Price() + Commission(), 2);
    }

    public decimal ExpertPayout()
    {
        return Helpers.Round(Price() * (1 - Config.Masar.Commission), 2);
    }

    public bool HasInvoice()
    {
        return PaymentStatus == "Paid" || PaymentStatus == "Released";
    }

    public string TargetLabel()
    {
        if (ConsultationSessionId != null)
        {
            return "Consultation session";
        }
        if (ServiceRequestId != null)
        {
            return "Service request";
        }
        return "Business Idea Package";
    }
}
