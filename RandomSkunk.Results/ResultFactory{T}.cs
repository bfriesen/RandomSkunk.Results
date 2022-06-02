namespace RandomSkunk.Results;

/// <summary>
/// Defines methods for creating <c>Fail</c> results of type <see cref="Result{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public class ResultFactory<T>
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
    public Result<T> Error(Error error) => Result<T>.Fail(error);
}
