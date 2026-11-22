using Masar.Models;
using Masar.Support.Dto;

namespace Masar.ViewModels.Client;

public class MessageThread
{
    public string Kind { get; set; } = string.Empty;

    public ConsultationSession? Session { get; set; }

    public ServiceRequest? Request { get; set; }

    public User? Who { get; set; }

    public string? Title { get; set; }

    public string Url { get; set; } = string.Empty;

    public DateTime? Last { get; set; }

    public int Count { get; set; }

    public dynamic Model => (object?)Session ?? Request;
}

public class MessagesViewModel
{
    public List<MessageThread> Threads { get; set; } = new();
}

public class NotificationsViewModel : ShellViewModel
{
    public List<NotificationEvent> Items { get; set; } = new();
}

public class ProjectTiles
{
    public int Upcoming { get; set; }

    public int Progress { get; set; }

    public int Completed { get; set; }

    public decimal Spent { get; set; }
}

public class ProjectsViewModel
{
    public List<ConsultationSession> Sessions { get; set; } = new();

    public List<ServiceRequest> Requests { get; set; } = new();

    public List<BusinessIdea> Ideas { get; set; } = new();

    public ProjectTiles Tiles { get; set; } = new();
}

public class InvoiceViewModel
{
    public InvoiceData Invoice { get; set; } = null!;

    public Payment Payment { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    public string? Reference { get; set; }

    public string ExpertName { get; set; } = string.Empty;

    public DateTime ServiceDate { get; set; }

    public User Client { get; set; } = null!;
}
