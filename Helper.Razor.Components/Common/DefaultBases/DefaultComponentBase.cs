using Helper.Razor.Components.Common.Services;
using Microsoft.AspNetCore.Components;

namespace Helper.Razor.Components.Common.DefaultBases;

public abstract class DefaultComponentBase : ComponentBase, IDefaultBase
{
    public virtual string ComponentId {get; init; }
    public virtual string Tooltip { get; set; } = string.Empty;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    protected DefaultComponentBase()
    {
        ComponentId = IdGenerator.GenerateId();
    }
}
