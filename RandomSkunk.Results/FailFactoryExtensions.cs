namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result factories.
/// </summary>
public static partial class FailFactoryExtensions
{
    /// <summary>
    /// Creates a <c>Fail</c> result with a generated stack trace.
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of <c>Fail</c> result to create, either <see cref="Result"/>, <see cref="Result{T}"/>, or
    /// <see cref="Maybe{T}"/>.
    /// </typeparam>
    /// <param name="failWith">The source factory.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="errorType">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <param name="innerError">
    /// The optional error that is the cause of the current error.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static TResult Error<TResult>(
        this FailFactory<TResult> failWith,
        string errorMessage,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null)
    {
        if (failWith is null) throw new ArgumentNullException(nameof(failWith));

        return failWith.Error(new Error(errorMessage, errorType, setStackTrace: true)
        {
            ErrorCode = errorCode,
            Identifier = errorIdentifier,
            InnerError = innerError,
        });
    }
}
