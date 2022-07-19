using RandomSkunk.Results.Unsafe;

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
    /// <param name="getNoneError">An optional function that creates the <see cref="Error"/> that is returned if this is a
    ///     <c>None</c> result; otherwise this parameter is ignored. If <see langword="null"/> (and applicable), a function that
    ///     returns an error with message "Not Found" and error code <see cref="ErrorCodes.NotFound"/> is used instead.</param>
    /// <returns>The error of the <c>non-Success</c> result.</returns>
    /// <exception cref="InvalidStateException">If this is a <c>Success</c> result.</exception>
    Error GetNonSuccessError(Func<Error>? getNoneError = null);
}
