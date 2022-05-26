using static RandomSkunk.Results.Exceptions;
using static RandomSkunk.Results.MaybeType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Or</c> methods.
/// </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Returns the current result if it is a <c>Some</c> result; otherwise, returns a new
    /// <c>Some</c> result with the specified fallback value.
    /// </summary>
    /// <param name="fallbackValue">The fallback value if the result is not <c>Some</c>.</param>
    /// <returns>A <c>Some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public Maybe<T> Or([DisallowNull] T fallbackValue)
    {
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return _type == Some ? this : Maybe<T>.Create.Some(fallbackValue);
    }

    /// <summary>
    /// Returns the current result if it is a <c>Some</c> result; otherwise, returns a new
    /// <c>Some</c> result with its value from evaluating the <paramref name="getFallbackValue"/>
    /// function.
    /// </summary>
    /// <param name="getFallbackValue">
    /// A function that returns the fallback value if the result is not <c>Some</c>.
    /// </param>
    /// <returns>A <c>Some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public Maybe<T> Or(Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        if (_type == Some)
            return this;

        var fallbackValue = getFallbackValue()
             ?? throw FunctionMustNotReturnNull(nameof(getFallbackValue));

        return Maybe<T>.Create.Some(fallbackValue);
    }
}
