using static RandomSkunk.Results.Exceptions;
using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Or</c> methods.
/// </content>
public partial struct Result<T>
{
    /// <summary>
    /// Returns the current result if it is a <c>Success</c> result; otherwise, returns a
    /// new <c>Success</c> result with the specified fallback value.
    /// </summary>
    /// <param name="fallbackValue">The fallback value if the result is not <c>Success</c>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public Result<T> Or([DisallowNull] T fallbackValue)
    {
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return _type == Success ? this : Result<T>.Create.Success(fallbackValue);
    }

    /// <summary>
    /// Returns the current result if it is a <c>Success</c> result; otherwise, returns a
    /// new <c>Success</c> result with the specified fallback value.
    /// </summary>
    /// <param name="getFallbackValue">
    /// A function that returns the fallback value if the result is not <c>Success</c>.
    /// </param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public Result<T> Or(Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        if (_type == Success)
            return this;

        var fallbackValue = getFallbackValue()
             ?? throw FunctionMustNotReturnNull(nameof(getFallbackValue));

        return Result<T>.Create.Success(fallbackValue);
    }
}
