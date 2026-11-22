using Masar.Models;
using Masar.Support.Dto;

namespace Masar.ViewModels.Auth;

public class LoginViewModel
{
    public int VerifiedExperts { get; set; }

    public int BookingsDone { get; set; }

    public double AvgRating { get; set; }
}

public class RegisterClientViewModel
{
    public int SpecializationCount { get; set; }

    public int VerifiedExperts { get; set; }

    public int? FirstReplyHours { get; set; }

    public List<string> Cities { get; set; } = new();

    public List<string> Budgets { get; set; } = new();

    public List<string> Interests { get; set; } = new();
}

public class RegisterExpertViewModel
{
    public int BookingsDone { get; set; }

    public List<string> Cities { get; set; } = new();

    public List<string> Specializations { get; set; } = new();

    public Dictionary<string, decimal> MinPrices { get; set; } = new();
}

public class SentViewModel
{
    public string Email { get; set; } = string.Empty;

    public string? Link { get; set; }
}

public class ResetViewModel
{
    public User User { get; set; } = null!;

    public string? H { get; set; }
}

public class DocumentsViewModel
{
    public Expert Expert { get; set; } = null!;

    public List<VerificationRow> Rows { get; set; } = new();

    public Dictionary<string, string> Types { get; set; } = new();
}

public class UnderReviewViewModel
{
    public Expert Expert { get; set; } = null!;

    public List<VerificationDocument> Documents { get; set; } = new();

    public DateTime Submitted { get; set; }

    public DateTime Expected { get; set; }

    public bool InReview { get; set; }
}
