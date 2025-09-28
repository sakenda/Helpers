using Helper.Razor.Components.Common.Services;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;

namespace Helper.Razor.Components.Common.DefaultBases;

public class DefaultInputBase<TValue> : InputBase<TValue>, IDefaultBase
{
    public virtual string ComponentId { get; init; }
    public virtual string Tooltip { get; set; } = string.Empty;

    protected DefaultInputBase()
    {
        ComponentId = IdGenerator.GenerateId();
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = default;
        validationErrorMessage = null;

        if (typeof(TValue) == typeof(string))
        {
            result = (TValue)(object?)(value ?? string.Empty)!;
            return true;
        }

        if (typeof(TValue) == typeof(int))
        {
            if (int.TryParse(value, out var parsedValue))
            {
                result = (TValue)(object)parsedValue;
                return true;
            }
            else
            {
                validationErrorMessage = $"The {FieldIdentifier.FieldName} field must be a number.";
                return false;
            }
        }
        if (typeof(TValue) == typeof(double))
        {
            if (double.TryParse(value, out var parsedValue))
            {
                result = (TValue)(object)parsedValue;
                return true;
            }
            else
            {
                validationErrorMessage = $"The {FieldIdentifier.FieldName} field must be a number.";
                return false;
            }
        }
        if (typeof(TValue) == typeof(decimal))
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                result = (TValue)(object)parsedValue;
                return true;
            }
            else
            {
                validationErrorMessage = $"The {FieldIdentifier.FieldName} field must be a number.";
                return false;
            }
        }
        if (typeof(TValue) == typeof(DateTime))
        {
            if (DateTime.TryParse(value, out var parsedValue))
            {
                result = (TValue)(object)parsedValue;
                return true;
            }
            else
            {
                validationErrorMessage = $"The {FieldIdentifier.FieldName} field must be a date.";
                return false;
            }
        }
        if (typeof(TValue) == typeof(bool))
        {
            if (bool.TryParse(value, out var parsedValue))
            {
                result = (TValue)(object)parsedValue;
                return true;
            }
            else
            {
                validationErrorMessage = $"The {FieldIdentifier.FieldName} field must be true or false.";
                return false;
            }
        }
        if (typeof(TValue).IsEnum)
        {
            try
            {
                result = (TValue)Enum.Parse(typeof(TValue), (value ?? string.Empty)!, true);
                return true;
            }
            catch
            {
                validationErrorMessage = $"The {FieldIdentifier.FieldName} field is not valid.";
                return false;
            }
        }
        else
        {
            throw new InvalidOperationException($"The type '{typeof(TValue)}' is not supported.");
        }
    }

}
