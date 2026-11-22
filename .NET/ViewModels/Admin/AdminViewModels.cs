using Masar.Models;
using Masar.Support.Dto;

namespace Masar.ViewModels.Admin;

public class AdminShell : ShellViewModel
{
    public AdminShell()
    {
        Panel = "admin";
    }
}

public class AdminSparklines
{
    public List<double> Clients { get; set; } = new();

    public List<double> Experts { get; set; } = new();

    public List<double> Requests { get; set; } = new();

    public List<double> Revenue { get; set; } = new();
}

public class AdminOverviewViewModel : AdminShell
{
    public int Clients { get; set; }

    public int NewClients { get; set; }

    public int Experts { get; set; }

    public int Waiting { get; set; }

    public int RequestsThisMonth { get; set; }

    public int ThreeProposalShare { get; set; }

    public decimal Revenue { get; set; }

    public decimal GrossThisMonth { get; set; }

    public Dictionary<string, double> PaidByMonth { get; set; } = new();

    public decimal PaidThisYear { get; set; }

    public decimal AverageService { get; set; }

    public decimal AverageSession { get; set; }

    public decimal Escrow { get; set; }

    public Dictionary<string, int> ByField { get; set; } = new();

    public JoinsByMonth Joins { get; set; } = new();

    public Dictionary<string, double> Health { get; set; } = new();

    public List<AdminQueueItem> Queue { get; set; } = new();

    public AdminSparklines Sparklines { get; set; } = new();
}

public class AdminSettingsViewModel : AdminShell
{
    public decimal Collected { get; set; }

    public decimal VatCollected { get; set; }
}

public class UserCounts
{
    public int All { get; set; }

    public int Client { get; set; }

    public int Expert { get; set; }

    public int Admin { get; set; }

    public int Blocked { get; set; }
}

public class AdminUsersViewModel : AdminShell
{
    public Support.Pager<User> Pager { get; set; } = null!;

    public List<User> Users { get; set; } = new();

    public string? Role { get; set; }

    public string? Status { get; set; }

    public UserCounts Counts { get; set; } = new();

    public Dictionary<int, int> OpenWork { get; set; } = new();
}

public class DocPill
{
    public string Label { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}

public class VerificationQueueRow
{
    public Expert Expert { get; set; } = null!;

    public List<VerificationDocument> Documents { get; set; } = new();

    public List<string> Missing { get; set; } = new();

    public string State { get; set; } = string.Empty;

    public DateTime? Submitted { get; set; }

    public List<DocPill> Pills { get; set; } = new();
}

public class QueueCounts
{
    public int All { get; set; }

    public int Waiting { get; set; }

    public int Review { get; set; }

    public int Needs { get; set; }

    public int this[string key] => key switch
    {
        "all" => All,
        "waiting" => Waiting,
        "review" => Review,
        "needs" => Needs,
        _ => 0,
    };
}

public class AdminVerificationViewModel : AdminShell
{
    public Support.Pager<VerificationQueueRow> Pager { get; set; } = null!;

    public List<VerificationQueueRow> Rows { get; set; } = new();

    public string Tab { get; set; } = "all";

    public QueueCounts Counts { get; set; } = new();

    public string? Spec { get; set; }

    public DocumentDecisions Decisions { get; set; } = new();

    public Dictionary<string, int> RejectReasons { get; set; } = new();

    public int ApprovedThisMonth { get; set; }
}

public class AdminApplicationViewModel : AdminShell
{
    public Expert Expert { get; set; } = null!;

    public User User { get; set; } = null!;

    public List<VerificationRow> Documents { get; set; } = new();

    public List<Portfolio> Portfolio { get; set; } = new();

    public int Strength { get; set; }

    public bool ReadyForReview { get; set; }
}

public class AdminIdeasViewModel : AdminShell
{
    public Support.Pager<BusinessIdea> Pager { get; set; } = null!;

    public List<BusinessIdea> Rows { get; set; } = new();

    public string? Status { get; set; }

    public Dictionary<string, int> Counts { get; set; } = new();
}

public class AdminIdeaShowViewModel : AdminShell
{
    public BusinessIdea Idea { get; set; } = null!;

    public List<BusinessIdea> Package { get; set; } = new();

    public List<ServiceType> ServiceTypes { get; set; } = new();

    public List<ConsultationType> ConsultationTypes { get; set; } = new();

    public Payment? Payment { get; set; }
}

public class AdminRequestsViewModel : AdminShell
{
    public Support.Pager<ServiceRequest> Pager { get; set; } = null!;

    public List<ServiceRequest> Rows { get; set; } = new();

    public string? Status { get; set; }

    public Dictionary<string, int> Counts { get; set; } = new();
}

public class AdminServicesViewModel : AdminShell
{
    public Support.Pager<ConsultationSession> Pager { get; set; } = null!;

    public List<ConsultationSession> Rows { get; set; } = new();

    public string? Status { get; set; }

    public Dictionary<string, int> Counts { get; set; } = new();
}

public class AdminPaymentsViewModel : AdminShell
{
    public Support.Pager<Payment> Pager { get; set; } = null!;

    public List<Payment> Rows { get; set; } = new();

    public string? Status { get; set; }

    public List<string> Statuses { get; set; } = new();

    public Dictionary<string, int> Counts { get; set; } = new();

    public decimal Held { get; set; }

    public decimal Released { get; set; }

    public decimal Commission { get; set; }

    public List<int> Releasable { get; set; } = new();
}

public class ReviewCounts
{
    public int All { get; set; }

    public int Shown { get; set; }

    public int Hidden { get; set; }
}

public class AdminReviewsViewModel : AdminShell
{
    public Support.Pager<Review> Pager { get; set; } = null!;

    public List<Review> Rows { get; set; } = new();

    public string? Visible { get; set; }

    public ReviewCounts Counts { get; set; } = new();
}

public class TopExpertRow
{
    public Expert Expert { get; set; } = null!;

    public int Reviews { get; set; }

    public int Completed { get; set; }

    public decimal Earned { get; set; }
}

public class ReportsViewModel : AdminShell
{
    public string Period { get; set; } = "month";

    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public decimal Gross { get; set; }

    public decimal Commission { get; set; }

    public decimal Vat { get; set; }

    public decimal Payouts { get; set; }

    public int SessionCount { get; set; }

    public int CompletedSessions { get; set; }

    public int RequestCount { get; set; }

    public int CompletedRequests { get; set; }

    public int Proposals { get; set; }

    public double? Satisfaction { get; set; }

    public int ReviewCount { get; set; }

    public int FiveStar { get; set; }

    public Dictionary<string, double> PaidByMonth { get; set; } = new();

    public Dictionary<string, int> ByField { get; set; } = new();

    public List<TopExpertRow> TopExperts { get; set; } = new();

    public Dictionary<string, double> Health { get; set; } = new();
}

public class ActivityKind
{
    public string Label { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;
}

public class ActivityLogEvent
{
    public DateTime At { get; set; }

    public string Kind { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public string Tone { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
}

public class ActivityViewModel : AdminShell
{
    public Support.Pager<ActivityLogEvent> Pager { get; set; } = null!;

    public Dictionary<string, ActivityKind> Kinds { get; set; } = new();

    public Dictionary<string, int> Counts { get; set; } = new();

    public string Kind { get; set; } = string.Empty;

    public string Q { get; set; } = string.Empty;

    public DateTime? Since { get; set; }

    public DateTime? Until { get; set; }

    public int Total { get; set; }

    public int Matched { get; set; }
}

public class SpecializationsViewModel : AdminShell
{
    public List<ConsultationType> ConsultationTypes { get; set; } = new();

    public List<ServiceType> ServiceTypes { get; set; } = new();

    public Dictionary<string, int> ExpertCounts { get; set; } = new();

    public List<string> Fields { get; set; } = new();
}
