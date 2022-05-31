namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>FromValue</c> extension method.
/// </content>
public static partial class ResultFactoryExtensions
{
    /// <summary>
    /// Creates a maybe from the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="value">The value. Can be <see langword="null"/>.</param>
    /// <returns>
    /// A <c>Some</c> result if <paramref name="value"/> is not null; otherwise, a <c>None</c>
    /// result.
    /// </returns>
    public static Maybe<T> FromValue<T>(
        this IMaybeFactory<T> source,
        T? value) =>
        value is not null
            ? source.Some(value)
            : source.None();
}
