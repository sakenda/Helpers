using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Helpers.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an argument of an enumeration type is either null or not a defined value
/// of the enumeration.
/// </summary>
/// <remarks>This exception is typically used to validate that an enum argument is both non-null and within the
/// range of valid values defined by the enumeration. It provides a static helper method, <see
/// cref="ThrowIfOutOfRange{TEnum}(TEnum?, string?)"/>, to perform this validation.</remarks>
public class ArgumentEnumException : SystemException
{
    /// <summary>
    /// Throws an exception if the specified enum value is null or not defined in the corresponding enumeration type.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type to validate. Must be a value type that derives from <see cref="Enum"/>.</typeparam>
    /// <param name="argument">The enum value to validate. Cannot be <see langword="null"/>.</param>
    /// <param name="paramName">The name of the parameter being validated. Automatically provided by the compiler if not explicitly specified.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="argument"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="argument"/> is not a defined value of the <typeparamref name="TEnum"/> enumeration.</exception>
    public static void ThrowIfOutOfRange<TEnum>([NotNull] TEnum? argument, [CallerArgumentExpression("argument")] string? paramName = null) where TEnum : struct, Enum
    {
        if (argument is null)
            throw new ArgumentNullException(paramName, "Argument cannot be null.");

        if (!Enum.IsDefined(typeof(TEnum), argument))
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value '{argument}' is not defined in enum '{typeof(TEnum).Name}'.");
    }

}
