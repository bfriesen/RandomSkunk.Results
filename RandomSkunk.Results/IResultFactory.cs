namespace RandomSkunk.Results;

/// <summary>
/// Defines methods for creating instances of <see cref="Result"/>.
/// </summary>
public interface IResultFactory
{
    /// <summary>
    /// Creates a <c>Success</c> result.
    /// </summary>
    /// <returns>A <c>Success</c> result.</returns>
    Result Success();

    /// <summary>
    /// Creates a <c>Fail</c> result with the specified error.
    /// </summary>
    /// <param name="error">
    /// An error that describes the failure. If <see langword="null"/>, a default error is
    /// used.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    Result Fail(Error? error = null);
}
