namespace RandomSkunk.Results.Http;

/// <summary>
/// Define extension methods for the <see cref="HttpResponseMessage"/> class.
/// </summary>
public static class HttpResponseExtensions
{
    private static readonly JsonSerializerOptions _defaultOptions = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Gets a <see cref="Result"/> value representing the HTTP response.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response.</returns>
    public static async Task<Result> GetResultAsync(
        this HttpResponseMessage sourceResponse,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default)
    {
        if (sourceResponse is null) throw new ArgumentNullException(nameof(sourceResponse));

        if (sourceResponse.IsSuccessStatusCode)
            return Result.Success();

        options ??= _defaultOptions;
        var problemDetails = await ReadProblemDetails(sourceResponse, options, cancellationToken).ConfigureAwait(false);
        var error = GetErrorFromProblemDetails(problemDetails, options);
        return Result.Fail(error);
    }

    /// <summary>
    /// Gets a <see cref="Result"/> value representing the HTTP response.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response.</returns>
    public static Task<Result> GetResultAsync(
        this HttpResponseMessage sourceResponse,
        CancellationToken cancellationToken = default) =>
        sourceResponse.GetResultAsync(null, cancellationToken);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(
        this HttpResponseMessage sourceResponse,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default)
    {
        if (sourceResponse is null) throw new ArgumentNullException(nameof(sourceResponse));
        options ??= _defaultOptions;

        if (sourceResponse.IsSuccessStatusCode)
            return await ReadValue<T>(sourceResponse.Content, options, cancellationToken).ConfigureAwait(false);

        var problemDetails = await ReadProblemDetails(sourceResponse, options, cancellationToken).ConfigureAwait(false);
        var error = GetErrorFromProblemDetails(problemDetails, options);
        return Result<T>.Fail(error);
    }

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static Task<Result<T>> ReadResultFromJsonAsync<T>(
        this HttpResponseMessage sourceResponse,
        CancellationToken cancellationToken = default) =>
        sourceResponse.ReadResultFromJsonAsync<T>(null, cancellationToken);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The maybe result representing the response content.</returns>
    public static async Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this HttpResponseMessage sourceResponse,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default)
    {
        if (sourceResponse is null) throw new ArgumentNullException(nameof(sourceResponse));
        options ??= _defaultOptions;

        if (sourceResponse.IsSuccessStatusCode)
            return await ReadMaybeValue<T>(sourceResponse.Content, options, cancellationToken).ConfigureAwait(false);

        var problemDetails = await ReadProblemDetails(sourceResponse, options, cancellationToken).ConfigureAwait(false);
        var error = GetErrorFromProblemDetails(problemDetails, options);
        return Maybe<T>.Fail(error);
    }

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The maybe result representing the response content.</returns>
    public static Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this HttpResponseMessage sourceResponse,
        CancellationToken cancellationToken = default) =>
        sourceResponse.ReadMaybeFromJsonAsync<T>(null, cancellationToken);

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage"/> has a success status code; otherwise, returns a
    /// <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>A result representing the response.</returns>
    public static Task<Result> EnsureSuccessStatusCodeAsync(
        this Result<HttpResponseMessage> sourceResponse,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        sourceResponse.FlatMapAsync(async response =>
        {
            var result = await response.GetResultAsync(options, cancellationToken).ConfigureAwait(false);
            response.Dispose();
            return result;
        });

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage"/> has a success status code; otherwise, returns a
    /// <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>A result representing the response.</returns>
    public static Task<Result> EnsureSuccessStatusCodeAsync(
        this Result<HttpResponseMessage> sourceResponse,
        CancellationToken cancellationToken = default) =>
        sourceResponse.EnsureSuccessStatusCodeAsync(null, cancellationToken);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static Task<Result<T>> ReadResultFromJsonAsync<T>(
        this Result<HttpResponseMessage> sourceResponse,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        sourceResponse.FlatMapAsync(response => response.ReadResultFromJsonAsync<T>(options, cancellationToken));

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static Task<Result<T>> ReadResultFromJsonAsync<T>(
        this Result<HttpResponseMessage> sourceResponse,
        CancellationToken cancellationToken = default) =>
        sourceResponse.ReadResultFromJsonAsync<T>(null, cancellationToken);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The maybe result representing the response content.</returns>
    public static Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this Result<HttpResponseMessage> sourceResponse,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        sourceResponse.FlatMapAsync(response => response.ReadMaybeFromJsonAsync<T>(options, cancellationToken));

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The maybe result representing the response content.</returns>
    public static Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this Result<HttpResponseMessage> sourceResponse,
        CancellationToken cancellationToken = default) =>
        sourceResponse.ReadMaybeFromJsonAsync<T>(null, cancellationToken);

    /// <summary>
    /// Gets a <see cref="Result"/> value representing the HTTP response.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response.</returns>
    public static async Task<Result> GetResultAsync(
        this Task<HttpResponseMessage> sourceResponse,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(false)).GetResultAsync(options, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Gets a <see cref="Result"/> value representing the HTTP response.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response.</returns>
    public static async Task<Result> GetResultAsync(
        this Task<HttpResponseMessage> sourceResponse,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(false)).GetResultAsync(cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(
        this Task<HttpResponseMessage> sourceResponse,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(false)).ReadResultFromJsonAsync<T>(options, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(
        this Task<HttpResponseMessage> sourceResponse,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(false)).ReadResultFromJsonAsync<T>(cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The maybe result representing the response content.</returns>
    public static async Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this Task<HttpResponseMessage> sourceResponse,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(false)).ReadMaybeFromJsonAsync<T>(options, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The maybe result representing the response content.</returns>
    public static async Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this Task<HttpResponseMessage> sourceResponse,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(false)).ReadMaybeFromJsonAsync<T>(cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage"/> has a success status code; otherwise, returns a
    /// <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>A result representing the response.</returns>
    public static async Task<Result> EnsureSuccessStatusCodeAsync(
        this Task<Result<HttpResponseMessage>> sourceResponse,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(false)).EnsureSuccessStatusCodeAsync(options, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage"/> has a success status code; otherwise, returns a
    /// <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>A result representing the response.</returns>
    public static async Task<Result> EnsureSuccessStatusCodeAsync(
        this Task<Result<HttpResponseMessage>> sourceResponse,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(false)).EnsureSuccessStatusCodeAsync(cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(
        this Task<Result<HttpResponseMessage>> sourceResponse,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(false)).ReadResultFromJsonAsync<T>(options, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The result representing the response content.</returns>
    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(
        this Task<Result<HttpResponseMessage>> sourceResponse,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(false)).ReadResultFromJsonAsync<T>(cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The maybe result representing the response content.</returns>
    public static async Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this Task<Result<HttpResponseMessage>> sourceResponse,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(false)).ReadMaybeFromJsonAsync<T>(options, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="sourceResponse">The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The maybe result representing the response content.</returns>
    public static async Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this Task<Result<HttpResponseMessage>> sourceResponse,
        CancellationToken cancellationToken = default) =>
        await (await sourceResponse.ConfigureAwait(false)).ReadMaybeFromJsonAsync<T>(cancellationToken).ConfigureAwait(false);

    private static async Task<Result<T>> ReadValue<T>(
        HttpContent content,
        JsonSerializerOptions options,
        CancellationToken cancellationToken)
    {
        try
        {
            var value = await content.ReadFromJsonAsync<T>(options,  cancellationToken).ConfigureAwait(false);

            if (value is null)
                return Result<T>.Fail("Response content was the literal string \"null\".");

            return value.ToResult();
        }
        catch (Exception ex)
        {
            return Result<T>.Fail(ex, "Error reading value from response content.");
        }
    }

    private static async Task<Maybe<T>> ReadMaybeValue<T>(
        HttpContent content,
        JsonSerializerOptions options,
        CancellationToken cancellationToken)
    {
        try
        {
            var value = await content.ReadFromJsonAsync<T>(options, cancellationToken).ConfigureAwait(false);

            if (value is null)
                return Maybe<T>.None();

            return value.ToMaybe();
        }
        catch (Exception ex)
        {
            return Maybe<T>.Fail(ex, "Error reading value from response content.");
        }
    }

    private static async Task<ProblemDetails> ReadProblemDetails(
        HttpResponseMessage response,
        JsonSerializerOptions options,
        CancellationToken cancellationToken)
    {
        try
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(options, cancellationToken).ConfigureAwait(false);

            if (problemDetails is not null)
                return problemDetails;
        }
        catch
        {
        }

        return new ProblemDetails
        {
            Status = (int)response.StatusCode,
            Title =
                !string.IsNullOrWhiteSpace(response.ReasonPhrase)
                    ? response.ReasonPhrase
                    : Enum.IsDefined(typeof(HttpStatusCode), response.StatusCode)
                        ? response.StatusCode.ToString()
                        : null,
        };
    }

    private static Error GetErrorFromProblemDetails(ProblemDetails problemDetails, JsonSerializerOptions options)
    {
        int? errorCode = null;
        string? stackTrace = null;
        string? identifier = null;
        Error? innerError = null;
        Dictionary<string, object>? extensions = null;

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
                case "errorStackTrace":
                    stackTrace = extension.Value.ToString();
                    break;
                case "errorIdentifier":
                    identifier = extension.Value.ToString();
                    break;
                case "errorInnerError":
                    if (extension.Value is JsonElement element)
                        innerError = TryDeserializeError(element, options);
                    break;
                default:
                    extensions ??= new Dictionary<string, object>(StringComparer.Ordinal);
                    extensions[extension.Key] = extension.Value;
                    break;
            }
        }

        if (problemDetails.Type != null)
        {
            extensions ??= new Dictionary<string, object>(StringComparer.Ordinal);
            extensions["problemDetailsType"] = problemDetails.Type;
        }

        if (problemDetails.Instance != null)
        {
            extensions ??= new Dictionary<string, object>(StringComparer.Ordinal);
            extensions["problemDetailsInstance"] = problemDetails.Instance;
        }

        return new Error(problemDetails.Detail, problemDetails.Title, false, extensions)
        {
            StackTrace = stackTrace,
            ErrorCode = errorCode,
            Identifier = identifier,
            InnerError = innerError,
        };
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
