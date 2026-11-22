using Masar.Models;

namespace Masar.ViewModels.Client;

public class ProposalShowViewModel
{
    public Proposal Proposal { get; set; } = null!;

    public ServiceRequest Request { get; set; } = null!;

    public Expert Expert { get; set; } = null!;

    public User User { get; set; } = null!;

    public int Reviews { get; set; }

    public List<Portfolio> Related { get; set; } = new();

    public DateTime SentAt { get; set; }
}

public class ProposalTotalsViewModel
{
    public Proposal Proposal { get; set; } = null!;

    public ServiceRequest Request { get; set; } = null!;

    public Expert Expert { get; set; } = null!;

    public User User { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal Commission { get; set; }

    public decimal Vat { get; set; }

    public decimal Total { get; set; }

    public decimal Payout { get; set; }
}

public class ProposalAcceptViewModel : ProposalTotalsViewModel
{
    public int Others { get; set; }
}

public class ProposalPayViewModel : ProposalTotalsViewModel
{
    public bool Card { get; set; }
}
