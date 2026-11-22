using Masar.Models;
using Masar.Support.Dto;
using Masar.Support.Services;

namespace Masar.ViewModels.Client;

public class ChooseViewModel
{
    public Expert Expert { get; set; } = null!;

    public User User { get; set; } = null!;

    public Dictionary<string, WeekDay> Week { get; set; } = new();

    public List<string> Times { get; set; } = new();

    public DateTime From { get; set; }

    public DateTime PrevWeek { get; set; }

    public DateTime NextWeek { get; set; }

    public ConsultationSession? Selected { get; set; }

    public int? ReplyHours { get; set; }

    public int Reviews { get; set; }

    public SlotService SlotService { get; set; } = null!;

    public string? TypeName { get; set; }
}

public class BookingViewModel
{
    public ConsultationSession Session { get; set; } = null!;

    public Expert? Expert { get; set; }

    public User? User { get; set; }

    public ConsultationType? Type { get; set; }

    public Payment? Payment { get; set; }

    public decimal Price { get; set; }

    public decimal Commission { get; set; }

    public decimal Vat { get; set; }

    public decimal Total { get; set; }

    public DateTime FreeUntil { get; set; }

    public int? ReplyHours { get; set; }

    public int Reviews { get; set; }
}

public class BookingSummaryViewModel : BookingViewModel
{
    public int Step { get; set; }
}

public class BookingPayViewModel : BookingViewModel
{
    public int Step { get; set; }

    public bool Card { get; set; }

    public bool Declined { get; set; }

    public long HoldSeconds { get; set; }
}

public class BookingShowViewModel : BookingViewModel
{
    public List<Message> Messages { get; set; } = new();

    public List<TimelineStep> Timeline { get; set; } = new();

    public bool Joinable { get; set; }

    public List<StoredFile> Files { get; set; } = new();

    public bool CanCancel { get; set; }

    public bool CancelIsFree { get; set; }

    public Review? Review { get; set; }
}

public class StatusRef
{
    public string Kind { get; set; } = string.Empty;

    public string? Value { get; set; }
}

public class BookingRow
{
    public string Kind { get; set; } = string.Empty;

    public ConsultationSession? Session { get; set; }

    public ServiceRequest? Request { get; set; }

    public string Reference { get; set; } = string.Empty;

    public string? Title { get; set; }

    public User? Who { get; set; }

    public string Subtitle { get; set; } = string.Empty;

    public DateTime When { get; set; }

    public StatusRef Status { get; set; } = new();

    public string Group { get; set; } = string.Empty;

    public decimal? Amount { get; set; }

    public dynamic Model => (object?)Session ?? Request;
}

public class BookingTiles
{
    public int Upcoming { get; set; }

    public int Progress { get; set; }

    public int Completed { get; set; }

    public decimal Spent { get; set; }
}

public class BookingsIndexViewModel
{
    public List<ConsultationSession> Sessions { get; set; } = new();

    public List<ServiceRequest> Requests { get; set; } = new();

    public string Tab { get; set; } = "all";

    public Support.Pager<BookingRow> Pager { get; set; } = null!;

    public List<BookingRow> Rows { get; set; } = new();

    public BookingTiles Tiles { get; set; } = new();

    public BookingService Bookings { get; set; } = null!;
}
