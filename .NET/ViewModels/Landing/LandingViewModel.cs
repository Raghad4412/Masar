using Masar.Models;
using Masar.Support.Dto;
using Masar.Support.Services;

namespace Masar.ViewModels.Landing;

public class SpecializationMeta
{
    public string Icon { get; set; } = string.Empty;

    public string Unit { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
}

public class LandingViewModel
{
    public List<TickerEvent> Ticker { get; set; } = new();

    public Expert? HeroExpert { get; set; }

    public List<ConsultationSession> HeroSlots { get; set; } = new();

    public int HeroReviews { get; set; }

    public int VerifiedExperts { get; set; }

    public int CompletedBookings { get; set; }

    public decimal MinConsultation { get; set; }

    public decimal MinService { get; set; }

    public Dictionary<string, SpecializationCard> SpecCards { get; set; } = new();

    public Dictionary<string, SpecializationMeta> SpecMeta { get; set; } = new();

    public int IdeaFounders { get; set; }

    public List<Expert> TopExperts { get; set; } = new();

    public List<Review> Stories { get; set; } = new();

    public StatsService Stats { get; set; } = null!;

    public List<Expert> AvatarPool { get; set; } = new();
}
