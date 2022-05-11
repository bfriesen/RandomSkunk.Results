namespace RandomSkunk.Results.Http;

/// <summary>
/// Define extension methods for the <see cref="HttpResponseMessage"/> class.
/// </summary>
public static class HttpResponseExtensions
{
    private static readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Reads the HTTP response as a <see cref="Result"/>.
    /// </summary>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response.</returns>
    public static async Task<Result> ReadResultFromJsonAsync(
        this HttpResponseMessage source,
        CancellationToken cancellationToken = default)
    {
        if (source.IsSuccessStatusCode)
            return Result.Create.Success();

        var problemDetails = await ReadProblemDetails(source, cancellationToken);
        var error = GetErrorFromProblemDetails(problemDetails);
        return Result.Create.Fail(error);
    }

    /// <summary>
    /// Reads the HTTP response as a <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The result representing the response.</returns>
    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(
        this HttpResponseMessage source,
        CancellationToken cancellationToken = default)
    {
        if (source.IsSuccessStatusCode)
            return await ReadValue<T>(source.Content, cancellationToken);

        var problemDetails = await ReadProblemDetails(source, cancellationToken);
        var error = GetErrorFromProblemDetails(problemDetails);
        return Result<T>.Create.Fail(error);
    }

    /// <summary>
    /// Reads the HTTP response as a <see cref="Maybe{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the returned result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Maybe{T}"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of
    /// cancellation.
    /// </param>
    /// <returns>The maybe result representing the response.</returns>
    public static async Task<Maybe<T>> ReadMaybeResultFromJsonAsync<T>(
        this HttpResponseMessage source,
        CancellationToken cancellationToken = default)
    {
        if (source.IsSuccessStatusCode)
            return await ReadMaybeValue<T>(source.Content, cancellationToken);

        var problemDetails = await ReadProblemDetails(source, cancellationToken);
        var error = GetErrorFromProblemDetails(problemDetails);
        return Maybe<T>.Create.Fail(error);
    }

    private static async Task<Result<T>> ReadValue<T>(
        HttpContent content,
        CancellationToken cancellationToken)
    {
        try
        {
            var value = await content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);

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
        CancellationToken cancellationToken)
    {
        try
        {
            var value = await content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);

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
        CancellationToken cancellationToken)
    {
        try
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: cancellationToken);

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

    private static Error GetErrorFromProblemDetails(ProblemDetails problemDetails)
    {
        var message = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(problemDetails.Title))
            message.Append(problemDetails.Title);

        if (!string.IsNullOrWhiteSpace(problemDetails.Detail))
        {
            if (message.Length > 0)
                message.Append(": ");
            message.Append(problemDetails.Detail);
        }

        if (message.Length == 0)
            message.Append(Error.DefaultMessage);

        string? stackTrace = null;
        if (problemDetails.Extensions.TryGetValue("errorStackTrace", out var obj) && obj is not null)
            stackTrace = obj as string;

        string? identifier = null;
        if (problemDetails.Extensions.TryGetValue("errorIdentifier", out obj) && obj is not null)
            identifier = obj as string;

        string? errorType = null;
        if (problemDetails.Extensions.TryGetValue("errorType", out obj) && obj is not null)
            errorType = obj as string;

        Error? innerError = null;
        if (problemDetails.Extensions.TryGetValue("errorInnerError", out obj) && obj is JsonElement element)
        {
            try
            {
                innerError = JsonSerializer.Deserialize<Error>(element, _options);
            }
            catch
            {
            }
        }

        return new Error(message.ToString(), stackTrace, problemDetails.Status, identifier, errorType, innerError);
    }
}
