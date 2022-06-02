namespace RandomSkunk.Results;

/// <summary>
/// Defines methods for creating <c>Fail</c> results of type <see cref="Result"/>.
/// </summary>
public class ResultFactory
{
    internal ResultFactory()
    {
    }

    /// <summary>
    /// Creates a <c>Fail</c> result with the specified error.
    /// </summary>
    /// <param name="error">
    /// An error that describes the failure. If <see langword="null"/>, a default error is used.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public Result Error(Error error) => Result.Fail(error);
}
