namespace RandomSkunk.Results.Http;

/// <summary>
/// Define extension methods for the <see cref="HttpResponseMessage"/> class.
/// </summary>
public static class HttpResponseExtensions
{
    private const int _defaultErrorCode = ErrorCodes.BadGateway;

    private static readonly JsonSerializerOptions _defaultOptions = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage.IsSuccessStatusCode"/> property for the HTTP
    /// response is true; otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to verify that it has a success status code.</param>
    /// <param name="getNonSuccessResponseError">A function that creates the error for a non-successful HTTP response.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>A result representing the response.</returns>
    public static async Task<Result<HttpResponseMessage>> TryEnsureSuccessStatusCode(
        this HttpResponseMessage sourceResponse,
        Func<Error, HttpStatusCode, Error> getNonSuccessResponseError,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (getNonSuccessResponseError is null) throw new ArgumentNullException(nameof(getNonSuccessResponseError));

        if (sourceResponse.IsSuccessStatusCode)
            return sourceResponse;

        options ??= _defaultOptions;

        var error = await ReadError(sourceResponse, getNonSuccessResponseError, options, cancellationToken).ConfigureAwait(ContinueOnCapturedContext);
        return error;
    }

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage.IsSuccessStatusCode"/> property for the HTTP
    /// response is true; otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to verify that it has a success status code.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>A result representing the response.</returns>
    public static Task<Result<HttpResponseMessage>> TryEnsureSuccessStatusCode(
        this HttpResponseMessage sourceResponse,
        JsonSerializerOptions? options = null,
        string? errorIdentifier = null,
        int errorCode = _defaultErrorCode,
        CancellationToken cancellationToken = default) =>
        sourceResponse.TryEnsureSuccessStatusCode(
            (problemDetailsError, statusCode) => GetNonSuccessResponseError(problemDetailsError, statusCode, errorCode, errorIdentifier),
            options,
            cancellationToken);

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage.IsSuccessStatusCode"/> property for the HTTP
    /// response is true; otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to verify that it has a success status code.</param>
    /// <param name="getNonSuccessResponseError">A function that creates the error for a non-successful HTTP response.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>A result representing the response.</returns>
    public static async Task<Result<HttpResponseMessage>> TryEnsureSuccessStatusCode(
        this Task<HttpResponseMessage> sourceResponse,
        Func<Error, HttpStatusCode, Error> getNonSuccessResponseError,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(ContinueOnCapturedContext)).TryEnsureSuccessStatusCode(getNonSuccessResponseError, options, cancellationToken).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage.IsSuccessStatusCode"/> property for the HTTP
    /// response is true; otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to verify that it has a success status code.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>A result representing the response.</returns>
    public static Task<Result<HttpResponseMessage>> TryEnsureSuccessStatusCode(
        this Task<HttpResponseMessage> sourceResponse,
        JsonSerializerOptions? options = null,
        string? errorIdentifier = null,
        int errorCode = _defaultErrorCode,
        CancellationToken cancellationToken = default) =>
        sourceResponse.TryEnsureSuccessStatusCode(
            (problemDetailsError, statusCode) => GetNonSuccessResponseError(problemDetailsError, statusCode, errorCode, errorIdentifier),
            options,
            cancellationToken);

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage.IsSuccessStatusCode"/> property for the HTTP
    /// response is true; otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to verify that it has a success status code.</param>
    /// <param name="getNonSuccessResponseError">A function that creates the error for a non-successful HTTP response.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>A result representing the response.</returns>
    public static Task<Result<HttpResponseMessage>> TryEnsureSuccessStatusCode(
        this Result<HttpResponseMessage> sourceResponse,
        Func<Error, HttpStatusCode, Error> getNonSuccessResponseError,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (getNonSuccessResponseError is null) throw new ArgumentNullException(nameof(getNonSuccessResponseError));

        return sourceResponse.SelectMany(response => response.TryEnsureSuccessStatusCode(getNonSuccessResponseError, options, cancellationToken));
    }

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage.IsSuccessStatusCode"/> property for the HTTP
    /// response is true; otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to verify that it has a success status code.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>A result representing the response.</returns>
    public static Task<Result<HttpResponseMessage>> TryEnsureSuccessStatusCode(
        this Result<HttpResponseMessage> sourceResponse,
        JsonSerializerOptions? options = null,
        string? errorIdentifier = null,
        int errorCode = _defaultErrorCode,
        CancellationToken cancellationToken = default) =>
        sourceResponse.TryEnsureSuccessStatusCode(
            (problemDetailsError, statusCode) => GetNonSuccessResponseError(problemDetailsError, statusCode, errorCode, errorIdentifier),
            options,
            cancellationToken);

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage.IsSuccessStatusCode"/> property for the HTTP
    /// response is true; otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to verify that it has a success status code.</param>
    /// <param name="getNonSuccessResponseError">A function that creates the error for a non-successful HTTP response.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>A result representing the response.</returns>
    public static async Task<Result<HttpResponseMessage>> TryEnsureSuccessStatusCode(
        this Task<Result<HttpResponseMessage>> sourceResponse,
        Func<Error, HttpStatusCode, Error> getNonSuccessResponseError,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(ContinueOnCapturedContext)).TryEnsureSuccessStatusCode(getNonSuccessResponseError, options, cancellationToken).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage.IsSuccessStatusCode"/> property for the HTTP
    /// response is true; otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to verify that it has a success status code.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>A result representing the response.</returns>
    public static Task<Result<HttpResponseMessage>> TryEnsureSuccessStatusCode(
        this Task<Result<HttpResponseMessage>> sourceResponse,
        JsonSerializerOptions? options = null,
        string? errorIdentifier = null,
        int errorCode = _defaultErrorCode,
        CancellationToken cancellationToken = default) =>
        sourceResponse.TryEnsureSuccessStatusCode(
            (problemDetailsError, statusCode) => GetNonSuccessResponseError(problemDetailsError, statusCode, errorCode, errorIdentifier),
            options,
            cancellationToken);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to read from in order to get the
    ///     <see cref="Result{T}"/>.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="getNonSuccessResponseError">A function that creates the error for a non-successful HTTP response.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static async Task<Result<T>> TryReadFromJsonAsync<T>(
        this HttpResponseMessage sourceResponse,
        Func<Exception, Error> exceptionHandler,
        Func<Error, HttpStatusCode, Error> getNonSuccessResponseError,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (sourceResponse is null) throw new ArgumentNullException(nameof(sourceResponse));
        if (exceptionHandler is null) throw new ArgumentNullException(nameof(exceptionHandler));
        if (getNonSuccessResponseError is null) throw new ArgumentNullException(nameof(getNonSuccessResponseError));

        options ??= _defaultOptions;

        if (sourceResponse.IsSuccessStatusCode)
        {
            return await TryCatch.AsResult(
                () => sourceResponse.Content.ReadFromJsonAsync<T>(options, cancellationToken),
                exceptionHandler);
        }

        var error = await ReadError(sourceResponse, getNonSuccessResponseError, options, cancellationToken).ConfigureAwait(ContinueOnCapturedContext);
        return error;
    }

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to read from in order to get the
    ///     <see cref="Result{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static Task<Result<T>> TryReadFromJsonAsync<T>(
        this HttpResponseMessage sourceResponse,
        JsonSerializerOptions? options = null,
        string? errorIdentifier = null,
        int errorCode = _defaultErrorCode,
        CancellationToken cancellationToken = default) =>
        sourceResponse.TryReadFromJsonAsync<T>(
            ex => GetReadFromJsonError(ex, typeof(T), errorCode, errorIdentifier),
            (problemDetailsError, statusCode) => GetNonSuccessResponseError(problemDetailsError, statusCode, errorCode, errorIdentifier),
            options,
            cancellationToken);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to read from in order to get the
    ///     <see cref="Result{T}"/>.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="getNonSuccessResponseError">A function that creates the error for a non-successful HTTP response.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static Task<Result<T>> TryReadFromJsonAsync<T>(
        this Result<HttpResponseMessage> sourceResponse,
        Func<Exception, Error> exceptionHandler,
        Func<Error, HttpStatusCode, Error> getNonSuccessResponseError,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (exceptionHandler is null) throw new ArgumentNullException(nameof(exceptionHandler));
        if (getNonSuccessResponseError is null) throw new ArgumentNullException(nameof(getNonSuccessResponseError));

        return sourceResponse.SelectMany(response => response.TryReadFromJsonAsync<T>(exceptionHandler, getNonSuccessResponseError, options, cancellationToken));
    }

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to read from in order to get the
    ///     <see cref="Result{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static Task<Result<T>> TryReadFromJsonAsync<T>(
        this Result<HttpResponseMessage> sourceResponse,
        JsonSerializerOptions? options = null,
        string? errorIdentifier = null,
        int errorCode = _defaultErrorCode,
        CancellationToken cancellationToken = default) =>
        sourceResponse.TryReadFromJsonAsync<T>(
            ex => GetReadFromJsonError(ex, typeof(T), errorCode, errorIdentifier),
            (problemDetailsError, statusCode) => GetNonSuccessResponseError(problemDetailsError, statusCode, errorCode, errorIdentifier),
            options,
            cancellationToken);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to read from in order to get the
    ///     <see cref="Result{T}"/>.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="getNonSuccessResponseError">A function that creates the error for a non-successful HTTP response.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static async Task<Result<T>> TryReadFromJsonAsync<T>(
        this Task<HttpResponseMessage> sourceResponse,
        Func<Exception, Error> exceptionHandler,
        Func<Error, HttpStatusCode, Error> getNonSuccessResponseError,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(ContinueOnCapturedContext)).TryReadFromJsonAsync<T>(exceptionHandler, getNonSuccessResponseError, options, cancellationToken).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to read from in order to get the
    ///     <see cref="Result{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static Task<Result<T>> TryReadFromJsonAsync<T>(
        this Task<HttpResponseMessage> sourceResponse,
        JsonSerializerOptions? options = null,
        string? errorIdentifier = null,
        int errorCode = _defaultErrorCode,
        CancellationToken cancellationToken = default) =>
        sourceResponse.TryReadFromJsonAsync<T>(
            ex => GetReadFromJsonError(ex, typeof(T), errorCode, errorIdentifier),
            (problemDetailsError, statusCode) => GetNonSuccessResponseError(problemDetailsError, statusCode, errorCode, errorIdentifier),
            options,
            cancellationToken);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to read from in order to get the
    ///     <see cref="Result{T}"/>.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="getNonSuccessResponseError">A function that creates the error for a non-successful HTTP response.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static async Task<Result<T>> TryReadFromJsonAsync<T>(
        this Task<Result<HttpResponseMessage>> sourceResponse,
        Func<Exception, Error> exceptionHandler,
        Func<Error, HttpStatusCode, Error> getNonSuccessResponseError,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(ContinueOnCapturedContext)).TryReadFromJsonAsync<T>(exceptionHandler, getNonSuccessResponseError, options, cancellationToken).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to read from in order to get the
    ///     <see cref="Result{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static Task<Result<T>> TryReadFromJsonAsync<T>(
        this Task<Result<HttpResponseMessage>> sourceResponse,
        JsonSerializerOptions? options = null,
        string? errorIdentifier = null,
        int errorCode = _defaultErrorCode,
        CancellationToken cancellationToken = default) =>
        sourceResponse.TryReadFromJsonAsync<T>(
            ex => GetReadFromJsonError(ex, typeof(T), errorCode, errorIdentifier),
            (problemDetailsError, statusCode) => GetNonSuccessResponseError(problemDetailsError, statusCode, errorCode, errorIdentifier),
            options,
            cancellationToken);

    internal static Error GetReadFromJsonError(Exception ex, Type type, int errorCode, string? identifier) =>
        Error.FromException(ex, $"Unable to read JSON content as type '{type.FullName}'.", errorCode, identifier);

    internal static Error GetNonSuccessResponseError(Error problemDetailsError, HttpStatusCode statusCode, int errorCode, string? identifier) =>
        new()
        {
            Message = $"Response status code does not indicate success: {(int)statusCode} ({Format.AsSentenceCase(statusCode.ToString())}). See InnerError for details.",
            ErrorCode = errorCode,
            Identifier = identifier,
            InnerError = problemDetailsError,
        };

    private static async Task<ProblemDetails> ReadProblemDetails(
        HttpResponseMessage response,
        JsonSerializerOptions options,
        CancellationToken cancellationToken)
    {
        try
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(options, cancellationToken).ConfigureAwait(ContinueOnCapturedContext);

            if (problemDetails is not null)
            {
                problemDetails.Title ??= GetTitle(response);
                return problemDetails;
            }
        }
        catch
        {
        }

        return new ProblemDetails
        {
            Title = GetTitle(response),
        };

        static string? GetTitle(HttpResponseMessage response) =>
            !string.IsNullOrWhiteSpace(response.ReasonPhrase)
                ? response.ReasonPhrase
                : Enum.IsDefined(typeof(HttpStatusCode), response.StatusCode)
                    ? response.StatusCode.ToString()
                    : null;
    }

    private static async Task<Error> ReadError(
        HttpResponseMessage response,
        Func<Error, HttpStatusCode, Error> getNonSuccessResponseError,
        JsonSerializerOptions options,
        CancellationToken cancellationToken)
    {
        var problemDetails = await ReadProblemDetails(response, options, cancellationToken).ConfigureAwait(ContinueOnCapturedContext);

        int? errorCode = null;
        string? identifier = null;
        Error? innerError = null;

        Dictionary<string, object>? extensions = new(StringComparer.Ordinal);

        foreach (var extension in problemDetails.Extensions)
        {
            if (extension.Value is null)
                continue;

            switch (extension.Key)
            {
                case "errorCode":
                    if (int.TryParse(extension.Value.ToString(), out var errorCodeValue))
                        errorCode = errorCodeValue;
                    break;
                case "errorIdentifier":
                    identifier = extension.Value.ToString();
                    break;
                case "errorInnerError":
                    if (extension.Value is JsonElement element)
                        innerError = TryDeserializeError(element, options);
                    break;
                default:
                    extensions[extension.Key] = extension.Value;
                    break;
            }
        }

        extensions["responseStatusCode"] = (int)response.StatusCode;

        if (problemDetails.Status != null)
            extensions["problemDetailsStatus"] = problemDetails.Status;

        if (problemDetails.Type != null)
            extensions["problemDetailsType"] = problemDetails.Type;

        if (problemDetails.Instance != null)
            extensions["problemDetailsInstance"] = problemDetails.Instance;

        var problemDetailsError = new Error
        {
            Message = problemDetails.Detail!,
            Title = problemDetails.Title!,
            Extensions = extensions!,
            ErrorCode = errorCode ?? problemDetails.Status ?? (int)response.StatusCode,
            Identifier = identifier,
            InnerError = innerError,
        };

        return getNonSuccessResponseError(problemDetailsError, response.StatusCode);
    }

    private static Error? TryDeserializeError(JsonElement element, JsonSerializerOptions options)
    {
        try
        {
            return JsonSerializer.Deserialize<Error>(element, options);
        }
        catch
        {
            return null;
        }
    }
}
