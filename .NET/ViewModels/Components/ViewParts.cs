using Masar.Models;

namespace Masar.ViewModels.Components;

public class TabLink
{
    public string Label { get; set; } = string.Empty;

    public int Count { get; set; }

    public string Url { get; set; } = string.Empty;

    public bool On { get; set; }
}

public class HeadCell
{
    public string Label { get; set; } = string.Empty;

    public bool Right { get; set; }

    public HeadCell()
    {
    }

    public HeadCell(string label)
    {
        Label = label;
    }
}

public class DayPart
{
    public string Icon { get; set; } = string.Empty;

    public List<ConsultationSession> Slots { get; set; } = new();
}
