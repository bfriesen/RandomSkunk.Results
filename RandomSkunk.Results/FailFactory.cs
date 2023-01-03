namespace RandomSkunk.Results;

/// <summary>
/// A factory object that create <c>Fail</c> results.
/// </summary>
/// <typeparam name="TResult">The type of <c>Fail</c> result that the factory creates: either <see cref="Result"/>,
///     <see cref="Result{T}"/>, or <see cref="Maybe{T}"/>.</typeparam>
public abstract class FailFactory<TResult>
{
    internal FailFactory()
    {
    }

    /// <summary>
    /// Creates a <c>Fail</c> result with the specified error.
    /// </summary>
    /// <param name="error">An error that describes the failure. If <see langword="null"/>, a default error is used.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract TResult Error(Error error);
}
