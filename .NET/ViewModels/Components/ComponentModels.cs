using Masar.Models;
using Masar.Support;
using Masar.Support.Services;
using Microsoft.AspNetCore.Html;

namespace Masar.ViewModels.Components;

public class AdminTableModel
{
    public string Title { get; set; } = string.Empty;

    public string Note { get; set; } = "";

    public List<TabLink> Tabs { get; set; } = new();

    public List<HeadCell> Head { get; set; } = new();

    public dynamic Pager { get; set; }

    public string Unit { get; set; } = "row";

    public IHtmlContent? Action { get; set; }

    public IHtmlContent? Filter { get; set; }

    public IHtmlContent? Slot { get; set; }
}

public class AvatarModel
{
    public User? User { get; set; }

    public string? Photo { get; set; }

    public string? Name { get; set; }

    public string Box { get; set; } = "ava";

    public string? Fallback { get; set; }

    public string Alt { get; set; } = "";
}

public class CheckModel
{
    public string Name { get; set; } = string.Empty;

    public object Value { get; set; } = 1;

    public bool Checked { get; set; } = false;

    public bool Required { get; set; } = false;

    public string Wrap { get; set; } = "ms-check d-flex gap-2";

    public AttributeBag Attrs { get; set; } = new();

    public IHtmlContent? Slot { get; set; }
}

public class ChoiceModel
{
    public string Name { get; set; } = string.Empty;

    public object Value { get; set; } = string.Empty;

    public string Box { get; set; } = "pay";

    public bool Checked { get; set; } = false;

    public bool Required { get; set; } = false;

    public bool Disabled { get; set; } = false;

    public string Extra { get; set; } = "";

    public IHtmlContent? Slot { get; set; }
}

public class ExpertCardModel
{
    public Expert Expert { get; set; } = null!;

    public StatsService? Stats { get; set; }

    public bool TopRated { get; set; } = false;
}

public class FieldModel
{
    public string Name { get; set; } = string.Empty;

    public string? Label { get; set; }

    public string Type { get; set; } = "text";

    public object? Value { get; set; }

    public string? Placeholder { get; set; }

    public string? Help { get; set; }

    public bool Required { get; set; } = false;

    public string? Icon { get; set; }

    public bool Area { get; set; } = false;

    public int? Rows { get; set; }

    public string? Counter { get; set; }

    public object? Options { get; set; }

    public string? Blank { get; set; }

    public string? Prefix { get; set; }

    public string? Suffix { get; set; }

    public string Fg { get; set; } = "m-fg";

    public bool Req { get; set; } = false;

    public string ErrIcon { get; set; } = "bi-exclamation-circle";

    public string? InputStyle { get; set; }

    public string Bag { get; set; } = "default";

    public AttributeBag Attrs { get; set; } = new();

    public IHtmlContent? Slot { get; set; }
}

public class PagerModel
{
    public dynamic Pager { get; set; }

    public string Unit { get; set; } = "row";
}

public class StarsModel
{
    public object Rating { get; set; } = 0;

    public string? Size { get; set; } = "13px";

    public string Class { get; set; } = "stars";

    public bool Bare { get; set; } = false;
}

public class StatusModel
{
    public string Kind { get; set; } = string.Empty;

    public string? Value { get; set; }
}
