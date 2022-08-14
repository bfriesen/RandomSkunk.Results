namespace RandomSkunk.Results;

/// <summary>
/// Defines a common interface for all result types.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets a value indicating whether this is a <c>Success</c> result.
    /// </summary>
    /// <returns><see langword="true"/> if this is a <c>Success</c> result; otherwise, <see langword="false"/>.</returns>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets the error of the <c>non-Success</c> result, or throws an <see cref="InvalidStateException"/> if this is a
    /// <c>Success</c> result.
    /// </summary>
    /// <returns>The error of the <c>non-Success</c> result.</returns>
    /// <exception cref="InvalidStateException">If this is a <c>Success</c> result.</exception>
    Error GetNonSuccessError();
}
