namespace RandomSkunk.Results;

/// <summary>
/// Defines an interface for results that have a value.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public interface IResult<T> : IResult
{
    /// <summary>
    /// Gets the value from the <c>Success</c> result.
    /// </summary>
    /// <returns>If this is a <c>Success</c> result, its value; otherwise throws an <see cref="InvalidStateException"/>.
    ///     </returns>
    /// <exception cref="InvalidStateException">If the result is not a <c>Success</c> result.</exception>
    T Value { get; }
}
