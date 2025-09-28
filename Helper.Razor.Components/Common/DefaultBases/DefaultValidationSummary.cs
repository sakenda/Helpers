using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Helper.Razor.Components.Common.DefaultBases;

public class DefaultValidationSummary : ValidationSummary
{
    [CascadingParameter] EditContext EditContext { get; set; } = default!;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        // As an optimization, only evaluate the messages enumerable once, and
        // only produce the enclosing <ul> if there's at least one message
        var validationMessages = Model is null ?
            EditContext.GetValidationMessages() :
            EditContext.GetValidationMessages(new FieldIdentifier(Model, string.Empty));

        var first = true;
        foreach (var error in validationMessages)
        {
            if (first)
            {
                first = false;

                builder.OpenElement(0, "ol");
                builder.AddAttribute(1, "style", "color: purple;");
                builder.AddMultipleAttributes(2, AdditionalAttributes);
            }

            builder.OpenElement(3, "li");
            builder.AddAttribute(4, "style", "color: purple;");
            builder.AddContent(5, error);
            builder.CloseElement();
        }

        if (!first)
        {
            // We have at least one validation message.
            builder.CloseElement();
        }
    }
}
