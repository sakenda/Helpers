using Helper.Razor.Components.Common.Services;
using Microsoft.AspNetCore.Components;

namespace Helper.Razor.Components.Common;

public abstract class DefaultComponentBase : ComponentBase
{
    public virtual string ComponentId {get; init; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    protected DefaultComponentBase()
    {
        ComponentId = IdGenerator.GenerateId();
    }
}
