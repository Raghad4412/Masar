using Masar.Models;
using Masar.Support.Dto;

namespace Masar.ViewModels.ExpertArea;

public class ServiceTiles
{
    public int Active { get; set; }

    public decimal Value { get; set; }

    public int DueThisWeek { get; set; }

    public decimal InEscrow { get; set; }

    public int Completed { get; set; }

    public int OnTime { get; set; }
}

public class ServicePayload : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public ServiceRequest Request { get; set; } = null!;

    public Proposal? Proposal { get; set; }

    public User? Client { get; set; }

    public Payment? Payment { get; set; }

    public decimal Released { get; set; }

    public decimal Escrow { get; set; }

    public List<StoredFile> Delivered { get; set; } = new();

    public List<StoredFile> References { get; set; } = new();

    public List<TimelineStep> Timeline { get; set; } = new();

    public int Progress { get; set; }

    public int DoneSteps { get; set; }

    public List<Message> Messages { get; set; } = new();
}

public class ExpertServiceShowViewModel : ServicePayload
{
}

public class ExpertServicesViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public List<ServiceRequest> Active { get; set; } = new();

    public ServiceRequest? Current { get; set; }

    public ServicePayload? CurrentPayload { get; set; }

    public List<ServiceRequest> Rows { get; set; } = new();

    public string Tab { get; set; } = "active";

    public ServiceTiles Tiles { get; set; } = new();
}

public class ExpertProfileViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public User User { get; set; } = null!;

    public int Strength { get; set; }

    public Dictionary<string, bool> Items { get; set; } = new();

    public List<Portfolio> Portfolio { get; set; } = new();

    public List<VerificationRow> Documents { get; set; } = new();

    public int Reviews { get; set; }

    public int Completed { get; set; }

    public int? ReplyHours { get; set; }

    public bool TopRated { get; set; }

    public DateTime? ApprovedSince { get; set; }
}

public class ExpertSettingsViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public User User { get; set; } = null!;

    public decimal MinPrice { get; set; }

    public List<Portfolio> Portfolio { get; set; } = new();

    public bool HasOpenWork { get; set; }
}
