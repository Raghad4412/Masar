using Masar.Models;
using Masar.Support.Dto;
using Masar.Support.Services;

namespace Masar.ViewModels.Client;

public class ActiveFilter
{
    public string Key { get; set; } = string.Empty;

    public string Label { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;
}

public class Suggestion
{
    public string Label { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;
}

public class Heading
{
    public string Title { get; set; } = string.Empty;

    public string Crumb { get; set; } = string.Empty;
}

public class ExpertsIndexViewModel
{
    public List<Expert> Experts { get; set; } = new();

    public int Total { get; set; }

    public int Page { get; set; }

    public int Pages { get; set; }

    public int PerPage { get; set; }

    public List<ActiveFilter> Filters { get; set; } = new();

    public StatsService Stats { get; set; } = null!;

    public bool ConsultantsOnly { get; set; }

    public List<Suggestion> Suggestions { get; set; } = new();

    public Heading Heading { get; set; } = new();

    public List<string> Specializations { get; set; } = new();
}

public class ExpertShowViewModel
{
    public Expert Expert { get; set; } = null!;

    public User User { get; set; } = null!;

    public string Tab { get; set; } = "overview";

    public List<Review> Reviews { get; set; } = new();

    public int WithComments { get; set; }

    public Dictionary<int, DistributionRow> Distribution { get; set; } = new();

    public List<Portfolio> Portfolio { get; set; } = new();

    public List<ConsultationSession> Slots { get; set; } = new();

    public int Bookings { get; set; }

    public int? ReplyHours { get; set; }

    public List<VerificationDocument> ApprovedDocuments { get; set; } = new();

    public bool TopRated { get; set; }

    public StatsService Stats { get; set; } = null!;
}

public class ServiceCard
{
    public string Icon { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public decimal From { get; set; }

    public int Experts { get; set; }

    public List<ServiceType> Packages { get; set; } = new();
}

public class ServiceHubViewModel
{
    public Dictionary<string, ServiceCard> Cards { get; set; } = new();

    public decimal IdeaPrice { get; set; }
}
