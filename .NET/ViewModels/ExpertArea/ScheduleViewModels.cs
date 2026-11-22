using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Masar.Models;
using Masar.Support;
using Masar.Support.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Masar.ViewModels.ExpertArea;

public class AvailabilityRequest : IValidatableObject
{
    [FromForm(Name = "Session_Date")]
    public DateTime? SessionDate { get; set; }

    [FromForm(Name = "Start_Time")]
    public string? StartTime { get; set; }

    [FromForm(Name = "repeat_weeks")]
    public int? RepeatWeeks { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (SessionDate == null)
        {
            yield return new ValidationResult("Choose the date of the slot.", new[] { nameof(SessionDate) });
        }
        else if (SessionDate.Value.Date < Clock.Today)
        {
            yield return new ValidationResult("Choose today or a later date.", new[] { nameof(SessionDate) });
        }

        if (string.IsNullOrEmpty(StartTime))
        {
            yield return new ValidationResult("Choose the start time.", new[] { nameof(StartTime) });
        }
        else if (!DateTime.TryParseExact(StartTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
        {
            yield return new ValidationResult("Enter the start time as HH:MM.", new[] { nameof(StartTime) });
        }
        else if (parsed.Minute != 0 && parsed.Minute != 30)
        {
            yield return new ValidationResult("Start a slot on the hour or the half hour.", new[] { nameof(StartTime) });
        }
        else if (parsed.Hour < 8 || parsed.Hour > 22 || (parsed.Hour == 22 && parsed.Minute > 0))
        {
            yield return new ValidationResult("Publish slots between 08:00 and 22:00.", new[] { nameof(StartTime) });
        }

        if (RepeatWeeks != null && (RepeatWeeks < 1 || RepeatWeeks > 8))
        {
            yield return new ValidationResult("Repeat the slot for 1 to 8 weeks.", new[] { nameof(RepeatWeeks) });
        }
    }
}

public class AvailabilityViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public Dictionary<string, WeekDay> Days { get; set; } = new();

    public List<string> Hours { get; set; } = new();

    public DateTime From { get; set; }

    public DateTime PrevWeek { get; set; }

    public DateTime NextWeek { get; set; }

    public bool IsConsultant { get; set; }

    public int Free { get; set; }
}

public class ScheduleViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public Dictionary<string, WeekDay> Days { get; set; } = new();

    public List<string> Hours { get; set; } = new();

    public DateTime From { get; set; }

    public DateTime PrevWeek { get; set; }

    public DateTime NextWeek { get; set; }

    public double HoursBooked { get; set; }

    public int CallsThisWeek { get; set; }

    public int Free { get; set; }

    public int MonthSessions { get; set; }

    public HeatmapData Heatmap { get; set; } = new();

    public List<ConsultationSession> Today { get; set; } = new();

    public List<ServiceRequest> Services { get; set; } = new();
}

public class ExpertSessionViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public ConsultationSession Session { get; set; } = null!;

    public User? Client { get; set; }

    public ConsultationType? Type { get; set; }

    public Payment? Payment { get; set; }

    public List<Message> Messages { get; set; } = new();

    public List<TimelineStep> Timeline { get; set; } = new();

    public List<StoredFile> Files { get; set; } = new();

    public bool CanComplete { get; set; }

    public bool CanCancel { get; set; }
}

public class ProposalSentViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public ServiceRequest Request { get; set; } = null!;

    public Proposal Proposal { get; set; } = null!;

    public int Others { get; set; }
}

public class ProposalFormViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public ServiceRequest Request { get; set; } = null!;

    public Proposal? Proposal { get; set; }

    public int Others { get; set; }

    public int AverageDays { get; set; }

    public decimal Price { get; set; }

    public decimal Commission { get; set; }

    public decimal Payout { get; set; }
}

public class ProposalCounts
{
    public int All { get; set; }

    public int Waiting { get; set; }

    public int Accepted { get; set; }

    public int Rejected { get; set; }

    public int this[string key] => key switch
    {
        "all" => All,
        "Waiting" => Waiting,
        "Accepted" => Accepted,
        "Rejected" => Rejected,
        _ => 0,
    };
}

public class ExpertProposalsViewModel : ShellViewModel
{
    public Expert Expert { get; set; } = null!;

    public Support.Pager<Proposal> Pager { get; set; } = null!;

    public List<Proposal> Rows { get; set; } = new();

    public List<Proposal> All { get; set; } = new();

    public string Tab { get; set; } = "all";

    public ProposalCounts Counts { get; set; } = new();

    public decimal Billed { get; set; }
}
