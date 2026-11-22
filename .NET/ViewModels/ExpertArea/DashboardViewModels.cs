using Masar.Models;
using Masar.Support.Dto;

namespace Masar.ViewModels.ExpertArea;

public class Sparklines
{
    public List<double> Earnings { get; set; } = new();

    public List<double> Services { get; set; } = new();

    public List<double> Win { get; set; } = new();

    public List<double> Rating { get; set; } = new();
}

public class ExpertDashboardViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public User User { get; set; } = null!;

    public decimal EarningsThisMonth { get; set; }

    public double BilledThisMonth { get; set; }

    public Dictionary<string, decimal> EarningsByMonth { get; set; } = new();

    public List<ServiceRequest> ActiveServices { get; set; } = new();

    public decimal AgreedTotal { get; set; }

    public WinRate Win { get; set; } = new();

    public decimal? Rating { get; set; }

    public int ReviewCount { get; set; }

    public int FiveStarShare { get; set; }

    public ProposalOutcomes Outcomes { get; set; } = new();

    public List<ConsultationSession> Upcoming { get; set; } = new();

    public List<ActivityEvent> Activity { get; set; } = new();

    public int OpenRequests { get; set; }

    public decimal ReceivedThisYear { get; set; }

    public decimal BilledThisYear { get; set; }

    public decimal CommissionThisYear { get; set; }

    public DateTime NextPayout { get; set; }

    public Sparklines Sparklines { get; set; } = new();
}

public class ExpertEarningsViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public User User { get; set; } = null!;

    public decimal Available { get; set; }

    public decimal InEscrow { get; set; }

    public decimal ReleasedYear { get; set; }

    public decimal BilledYear { get; set; }

    public decimal CommissionYear { get; set; }

    public BilledByMonth ByMonth { get; set; } = new();

    public Dictionary<string, decimal> ByField { get; set; } = new();

    public List<PayoutRow> Payouts { get; set; } = new();

    public DateTime NextPayout { get; set; }
}

public class ExpertPerformanceViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public User User { get; set; } = null!;

    public List<Review> Reviews { get; set; } = new();

    public Dictionary<int, DistributionRow> Distribution { get; set; } = new();

    public RatingSeries RatingByMonth { get; set; } = new();

    public int? ReplyHours { get; set; }

    public int Speed { get; set; }

    public WinRate Win { get; set; } = new();

    public int Completed { get; set; }

    public int OnTime { get; set; }

    public int RepeatClients { get; set; }
}

public class ExpertReviewsViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public User User { get; set; } = null!;

    public Support.Pager<Review> Pager { get; set; } = null!;

    public List<Review> Reviews { get; set; } = new();

    public Dictionary<int, DistributionRow> Distribution { get; set; } = new();
}

public class OpenRequestTiles
{
    public int Open { get; set; }

    public int Sent { get; set; }

    public int Accepted { get; set; }

    public int Win { get; set; }
}

public class ExpertRequestsViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public Support.Pager<ServiceRequest> Pager { get; set; } = null!;

    public List<ServiceRequest> Rows { get; set; } = new();

    public List<int> Sent { get; set; } = new();

    public Dictionary<int, Proposal> ProposalByRequest { get; set; } = new();

    public List<string> Fields { get; set; } = new();

    public string? Field { get; set; }

    public string? Sort { get; set; }

    public OpenRequestTiles Tiles { get; set; } = new();
}

public class ExpertMessagesViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public List<ExpertThread> Threads { get; set; } = new();
}
