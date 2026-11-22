using Masar.Models;
using Masar.Support.Dto;
using Masar.Support.Services;

namespace Masar.ViewModels.Client;

public class ProfileViewModel
{
    public Models.Client Client { get; set; } = null!;

    public User User { get; set; } = null!;

    public bool HasOpenWork { get; set; }
}

public class ServiceWorkspaceViewModel
{
    public ServiceRequest Request { get; set; } = null!;

    public Proposal? Proposal { get; set; }

    public Expert? Expert { get; set; }

    public User? User { get; set; }

    public Payment? Payment { get; set; }

    public List<StoredFile> Delivered { get; set; } = new();

    public List<StoredFile> References { get; set; } = new();

    public List<Message> Messages { get; set; } = new();

    public List<TimelineStep> Timeline { get; set; } = new();

    public int Progress { get; set; }

    public int DoneSteps { get; set; }

    public Review? Review { get; set; }

    public int Reviews { get; set; }
}

public class RequestsIndexViewModel
{
    public Support.Pager<ServiceRequest> Pager { get; set; } = null!;

    public List<ServiceRequest> Requests { get; set; } = new();

    public List<string> Popular { get; set; } = new();
}

public class RequestCreateViewModel
{
    public string Field { get; set; } = string.Empty;

    public List<ServiceType> Packages { get; set; } = new();

    public DateTime Earliest { get; set; }

    public ServiceRequest? Editing { get; set; }
}

public class RequestPublishedViewModel
{
    public ServiceRequest Request { get; set; } = null!;

    public int Experts { get; set; }

    public int? ReplyHours { get; set; }
}

public class RequestShowViewModel
{
    public ServiceRequest Request { get; set; } = null!;

    public List<Proposal> Proposals { get; set; } = new();

    public List<Proposal> AllProposals { get; set; } = new();

    public string? Sort { get; set; }

    public long DaysLeft { get; set; }

    public List<StoredFile> Files { get; set; } = new();

    public List<Expert> Suggested { get; set; } = new();

    public int Experts { get; set; }

    public StatsService Stats { get; set; } = null!;

    public Proposal? BestValue { get; set; }
}

public class RequestCompareViewModel
{
    public ServiceRequest Request { get; set; } = null!;

    public List<Proposal> Proposals { get; set; } = new();

    public Proposal? BestValue { get; set; }

    public StatsService Stats { get; set; } = null!;
}

public class ReviewCreateViewModel
{
    public string Kind { get; set; } = string.Empty;

    public object Target { get; set; } = null!;

    public string Reference { get; set; } = string.Empty;

    public Expert? Expert { get; set; }

    public User? User { get; set; }

    public string Caption { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public string Back { get; set; } = string.Empty;

    public string StatusKind { get; set; } = string.Empty;

    public string? StatusValue { get; set; }
}

public class ReviewDoneViewModel
{
    public Review Review { get; set; } = null!;

    public Expert? Expert { get; set; }

    public User? User { get; set; }

    public string Chip { get; set; } = string.Empty;

    public string BookAgain { get; set; } = string.Empty;
}
