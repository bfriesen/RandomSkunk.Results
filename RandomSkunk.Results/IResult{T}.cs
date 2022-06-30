using RandomSkunk.Results.Unsafe;

namespace RandomSkunk.Results;

/// <summary>
/// Defines an interface for results that have a value.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public interface IResult<T> : IResult
{
    /// <summary>
    /// Gets the value of the <c>Success</c> result, or throws an <see cref="InvalidStateException"/> if this is not a
    /// <c>Success</c> result.
    /// </summary>
    /// <returns>The value of the <c>Success</c> result.</returns>
    /// <exception cref="InvalidStateException">If this is not a <c>Success</c> result.</exception>
    new T GetSuccessValue();
}
