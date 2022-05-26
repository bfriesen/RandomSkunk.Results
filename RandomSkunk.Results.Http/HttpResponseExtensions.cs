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
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response.</returns>
    public static async Task<Result> GetResultAsync(
        this HttpResponseMessage source,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        if (source.IsSuccessStatusCode)
            return Result.Create.Success();

        options ??= _defaultOptions;
        var problemDetails = await ReadProblemDetails(source, options, cancellationToken).ConfigureAwait(false);
        var error = GetErrorFromProblemDetails(problemDetails, options);
        return Result.Create.Fail(error);
    }

    /// <summary>
    /// Gets a <see cref="Result"/> value representing the HTTP response.
    /// </summary>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response.</returns>
    public static Task<Result> GetResultAsync(
        this HttpResponseMessage source,
        CancellationToken cancellationToken = default) =>
        source.GetResultAsync(null, cancellationToken);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response content.</returns>
    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(
        this HttpResponseMessage source,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        options ??= _defaultOptions;

        if (source.IsSuccessStatusCode)
            return await ReadValue<T>(source.Content, options, cancellationToken).ConfigureAwait(false);

        var problemDetails = await ReadProblemDetails(source, options, cancellationToken).ConfigureAwait(false);
        var error = GetErrorFromProblemDetails(problemDetails, options);
        return Result<T>.Create.Fail(error);
    }

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response content.</returns>
    public static Task<Result<T>> ReadResultFromJsonAsync<T>(
        this HttpResponseMessage source,
        CancellationToken cancellationToken = default) =>
        source.ReadResultFromJsonAsync<T>(null, cancellationToken);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The maybe result representing the response content.</returns>
    public static async Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this HttpResponseMessage source,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        options ??= _defaultOptions;

        if (source.IsSuccessStatusCode)
            return await ReadMaybeValue<T>(source.Content, options, cancellationToken).ConfigureAwait(false);

        var problemDetails = await ReadProblemDetails(source, options, cancellationToken).ConfigureAwait(false);
        var error = GetErrorFromProblemDetails(problemDetails, options);
        return Maybe<T>.Create.Fail(error);
    }

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The maybe result representing the response content.</returns>
    public static Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this HttpResponseMessage source,
        CancellationToken cancellationToken = default) =>
        source.ReadMaybeFromJsonAsync<T>(null, cancellationToken);

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage"/> has a success status code; otherwise, returns
    /// a <c>Fail</c> result.
    /// </summary>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>A result representing the response.</returns>
    public static Task<Result> EnsureSuccessStatusCodeAsync(
        this Result<HttpResponseMessage> source,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        source.CrossMapAsync(response => response.GetResultAsync(options, cancellationToken));

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage"/> has a success status code; otherwise, returns
    /// a <c>Fail</c> result.
    /// </summary>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>A result representing the response.</returns>
    public static Task<Result> EnsureSuccessStatusCodeAsync(
        this Result<HttpResponseMessage> source,
        CancellationToken cancellationToken = default) =>
        source.EnsureSuccessStatusCodeAsync(null, cancellationToken);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response content.</returns>
    public static Task<Result<T>> ReadResultFromJsonAsync<T>(
        this Result<HttpResponseMessage> source,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        source.FlatMapAsync(response => response.ReadResultFromJsonAsync<T>(options, cancellationToken));

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response content.</returns>
    public static Task<Result<T>> ReadResultFromJsonAsync<T>(
        this Result<HttpResponseMessage> source,
        CancellationToken cancellationToken = default) =>
        source.ReadResultFromJsonAsync<T>(null, cancellationToken);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The maybe result representing the response content.</returns>
    public static Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this Result<HttpResponseMessage> source,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        source.CrossMapAsync(response => response.ReadMaybeFromJsonAsync<T>(options, cancellationToken));

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The maybe result representing the response content.</returns>
    public static Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this Result<HttpResponseMessage> source,
        CancellationToken cancellationToken = default) =>
        source.ReadMaybeFromJsonAsync<T>(null, cancellationToken);

    /// <summary>
    /// Gets a <see cref="Result"/> value representing the HTTP response.
    /// </summary>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response.</returns>
    public static async Task<Result> GetResultAsync(
        this Task<HttpResponseMessage> source,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        await (await source.ConfigureAwait(false)).GetResultAsync(options, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Gets a <see cref="Result"/> value representing the HTTP response.
    /// </summary>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response.</returns>
    public static async Task<Result> GetResultAsync(
        this Task<HttpResponseMessage> source,
        CancellationToken cancellationToken = default) =>
        await (await source.ConfigureAwait(false)).GetResultAsync(cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response content.</returns>
    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(
        this Task<HttpResponseMessage> source,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        await (await source.ConfigureAwait(false)).ReadResultFromJsonAsync<T>(options, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response content.</returns>
    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(
        this Task<HttpResponseMessage> source,
        CancellationToken cancellationToken = default) =>
        await (await source.ConfigureAwait(false)).ReadResultFromJsonAsync<T>(cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The maybe result representing the response content.</returns>
    public static async Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this Task<HttpResponseMessage> source,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        await (await source.ConfigureAwait(false)).ReadMaybeFromJsonAsync<T>(options, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The maybe result representing the response content.</returns>
    public static async Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this Task<HttpResponseMessage> source,
        CancellationToken cancellationToken = default) =>
        await (await source.ConfigureAwait(false)).ReadMaybeFromJsonAsync<T>(cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage"/> has a success status code; otherwise, returns
    /// a <c>Fail</c> result.
    /// </summary>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>A result representing the response.</returns>
    public static async Task<Result> EnsureSuccessStatusCodeAsync(
        this Task<Result<HttpResponseMessage>> source,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        await (await source.ConfigureAwait(false)).EnsureSuccessStatusCodeAsync(options, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Returns a <c>Success</c> result if the <see cref="HttpResponseMessage"/> has a success status code; otherwise, returns
    /// a <c>Fail</c> result.
    /// </summary>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>A result representing the response.</returns>
    public static async Task<Result> EnsureSuccessStatusCodeAsync(
        this Task<Result<HttpResponseMessage>> source,
        CancellationToken cancellationToken = default) =>
        await (await source.ConfigureAwait(false)).EnsureSuccessStatusCodeAsync(cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response content.</returns>
    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(
        this Task<Result<HttpResponseMessage>> source,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        await (await source.ConfigureAwait(false)).ReadResultFromJsonAsync<T>(options, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Result{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response content.</returns>
    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(
        this Task<Result<HttpResponseMessage>> source,
        CancellationToken cancellationToken = default) =>
        await (await source.ConfigureAwait(false)).ReadResultFromJsonAsync<T>(cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The maybe result representing the response content.</returns>
    public static async Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this Task<Result<HttpResponseMessage>> source,
        JsonSerializerOptions? options,
        CancellationToken cancellationToken = default) =>
        await (await source.ConfigureAwait(false)).ReadMaybeFromJsonAsync<T>(options, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Reads the HTTP content and returns a <see cref="Maybe{T}"/> value representing the result from deserializing the content
    /// as JSON in an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The maybe result representing the response content.</returns>
    public static async Task<Maybe<T>> ReadMaybeFromJsonAsync<T>(
        this Task<Result<HttpResponseMessage>> source,
        CancellationToken cancellationToken = default) =>
        await (await source.ConfigureAwait(false)).ReadMaybeFromJsonAsync<T>(cancellationToken).ConfigureAwait(false);

    private static async Task<Result<T>> ReadValue<T>(
        HttpContent content,
        JsonSerializerOptions options,
        CancellationToken cancellationToken)
    {
        try
        {
            var value = await content.ReadFromJsonAsync<T>(options,  cancellationToken).ConfigureAwait(false);

            if (value is null)
                return Result<T>.Create.Fail("Response content was the literal string \"null\".");

            return Result<T>.Create.Success(value);
        }
        catch (Exception ex)
        {
            return Result<T>.Create.Fail(ex, "Error reading value from response content.");
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
                return Maybe<T>.Create.None();

            return Maybe<T>.Create.Some(value);
        }
        catch (Exception ex)
        {
            return Maybe<T>.Create.Fail(ex, "Error reading value from response content.");
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

    private static ExtendedError GetErrorFromProblemDetails(ProblemDetails problemDetails, JsonSerializerOptions options)
    {
        string? stackTrace = null;
        string? identifier = null;
        ExtendedError? innerError = null;
        Dictionary<string, object>? extensions = null;

        foreach (var extension in problemDetails.Extensions)
        {
            if (extension.Value is null)
                continue;

            switch (extension.Key)
            {
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

        return new ExtendedError(problemDetails.Detail, problemDetails.Title, extensions)
        {
            StackTrace = stackTrace,
            ErrorCode = problemDetails.Status,
            Identifier = identifier,
            InnerError = innerError,
        };
    }

    private static ExtendedError? TryDeserializeError(JsonElement element, JsonSerializerOptions options)
    {
        try
        {
            return JsonSerializer.Deserialize<ExtendedError>(element, options);
        }
        catch
        {
            return null;
        }
    }
}
