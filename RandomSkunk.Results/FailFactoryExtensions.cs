namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result factories.
/// </summary>
public static class FailFactoryExtensions
{
    /// <summary>
    /// Creates a <c>Fail</c> result with a generated stack trace.
    /// </summary>
    /// <typeparam name="TResult">The type of <c>Fail</c> result to create, either <see cref="Result"/>, <see cref="Result{T}"/>,
    ///     or <see cref="Maybe{T}"/>.</typeparam>
    /// <param name="failWith">The source factory.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="errorType">The optional type of the error. If <see langword="null"/>, then the <see cref="MemberInfo.Name"/>
    ///     of the <see cref="Type"/> of the current instance is used instead.</param>
    /// <param name="innerError">The optional error that is the cause of the current error.</param>
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

    /// <summary>
    /// Creates a <c>Fail</c> result with error code <see cref="ErrorCodes.BadRequest"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of <c>Fail</c> result to create, either <see cref="Result"/>, <see cref="Result{T}"/>,
    ///     or <see cref="Maybe{T}"/>.</typeparam>
    /// <param name="failWith">The source factory.</param>
    /// <param name="errorMessage">The error message. If <see langword="null"/> or not provided, a message indicating that the
    ///     operation could not be process due to a client error is used instead.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static TResult BadRequest<TResult>(
        this FailFactory<TResult> failWith,
        string errorMessage = Errors.BadRequestMessage,
        string? errorIdentifier = null)
    {
        if (failWith is null) throw new ArgumentNullException(nameof(failWith));

        return failWith.Error(Errors.BadRequest(errorMessage, errorIdentifier));
    }

    /// <summary>
    /// Creates a <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of <c>Fail</c> result to create, either <see cref="Result"/>, <see cref="Result{T}"/>,
    ///     or <see cref="Maybe{T}"/>.</typeparam>
    /// <param name="failWith">The source factory.</param>
    /// <param name="errorMessage">The error message. If <see langword="null"/> or not provided, a message indicating that the
    ///     target resource could not be found is used instead.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static TResult NotFound<TResult>(
        this FailFactory<TResult> failWith,
        string errorMessage = Errors.NotFoundMessage,
        string? errorIdentifier = null)
    {
        if (failWith is null) throw new ArgumentNullException(nameof(failWith));

        return failWith.Error(Errors.NotFound(errorMessage, errorIdentifier));
    }

    /// <summary>
    /// Creates a <c>Fail</c> result with error code <see cref="ErrorCodes.Gone"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of <c>Fail</c> result to create, either <see cref="Result"/>, <see cref="Result{T}"/>,
    ///     or <see cref="Maybe{T}"/>.</typeparam>
    /// <param name="failWith">The source factory.</param>
    /// <param name="errorMessage">The error message. If <see langword="null"/> or not provided, a message indicating that the
    ///     target resource is no longer available is used instead.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static TResult Gone<TResult>(
        this FailFactory<TResult> failWith,
        string errorMessage = Errors.GoneMessage,
        string? errorIdentifier = null)
    {
        if (failWith is null) throw new ArgumentNullException(nameof(failWith));

        return failWith.Error(Errors.Gone(errorMessage, errorIdentifier));
    }

    /// <summary>
    /// Creates a <c>Fail</c> result with error code <see cref="ErrorCodes.InternalServerError"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of <c>Fail</c> result to create, either <see cref="Result"/>, <see cref="Result{T}"/>,
    ///     or <see cref="Maybe{T}"/>.</typeparam>
    /// <param name="failWith">The source factory.</param>
    /// <param name="errorMessage">The error message. If <see langword="null"/> or not provided, a message indicating that an
    ///     unexpected condition prevented the operation from completing successfully is used instead.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static TResult InternalServerError<TResult>(
        this FailFactory<TResult> failWith,
        string errorMessage = Errors.InternalServerErrorMessage,
        string? errorIdentifier = null)
    {
        if (failWith is null) throw new ArgumentNullException(nameof(failWith));

        return failWith.Error(Errors.InternalServerError(errorMessage, errorIdentifier));
    }

    /// <summary>
    /// Creates a <c>Fail</c> result with error code <see cref="ErrorCodes.NotImplemented"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of <c>Fail</c> result to create, either <see cref="Result"/>, <see cref="Result{T}"/>,
    ///     or <see cref="Maybe{T}"/>.</typeparam>
    /// <param name="failWith">The source factory.</param>
    /// <param name="errorMessage">The error message. If <see langword="null"/> or not provided, a message indicating that the
    ///     operation is not implemented is used instead.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static TResult NotImplemented<TResult>(
        this FailFactory<TResult> failWith,
        string errorMessage = Errors.NotImplementedMessage,
        string? errorIdentifier = null)
    {
        if (failWith is null) throw new ArgumentNullException(nameof(failWith));

        return failWith.Error(Errors.NotImplemented(errorMessage, errorIdentifier));
    }

    /// <summary>
    /// Creates a <c>Fail</c> result with error code <see cref="ErrorCodes.BadGateway"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of <c>Fail</c> result to create, either <see cref="Result"/>, <see cref="Result{T}"/>,
    ///     or <see cref="Maybe{T}"/>.</typeparam>
    /// <param name="failWith">The source factory.</param>
    /// <param name="errorMessage">The error message. If <see langword="null"/> or not provided, a message indicating that the
    ///     response from the upstream service was invalid is used instead.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static TResult BadGateway<TResult>(
        this FailFactory<TResult> failWith,
        string errorMessage = Errors.BadGatewayMessage,
        string? errorIdentifier = null)
    {
        if (failWith is null) throw new ArgumentNullException(nameof(failWith));

        return failWith.Error(Errors.BadGateway(errorMessage, errorIdentifier));
    }

    /// <summary>
    /// Creates a <c>Fail</c> result with error code <see cref="ErrorCodes.GatewayTimeout"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of <c>Fail</c> result to create, either <see cref="Result"/>, <see cref="Result{T}"/>,
    ///     or <see cref="Maybe{T}"/>.</typeparam>
    /// <param name="failWith">The source factory.</param>
    /// <param name="errorMessage">The error message. If <see langword="null"/> or not provided, a message indicating that the
    ///     operation from the upstream service timed out is used instead.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static TResult GatewayTimeout<TResult>(
        this FailFactory<TResult> failWith,
        string errorMessage = Errors.GatewayTimeoutMessage,
        string? errorIdentifier = null)
    {
        if (failWith is null) throw new ArgumentNullException(nameof(failWith));

        return failWith.Error(Errors.GatewayTimeout(errorMessage, errorIdentifier));
    }
}
