namespace RandomSkunk.Results.Http;

/// <summary>
/// Define extension methods for the <see cref="HttpResponseMessage"/> class.
/// </summary>
public static class HttpResponseExtensions
{
    /// <summary>
    /// Reads the HTTP response as a <see cref="Result"/>.
    /// </summary>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result"/>.
    /// </param>
    /// <returns>The result representing the response.</returns>
    public static async Task<Result> ReadResultFromJsonAsync(this HttpResponseMessage source)
    {
        if (source.IsSuccessStatusCode)
            return Result.Create.Success();

        var problemDetails = await ReadProblemDetails(source.Content);

        if (problemDetails.IsSuccess)
        {
            var error = GetErrorFromProblemDetails(problemDetails.Value);
            return Result.Create.Fail(error);
        }

        return Result.Create.Fail(problemDetails.Error);
    }

    /// <summary>
    /// Reads the HTTP response as a <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="Result{T}"/>.
    /// </param>
    /// <returns>The result representing the response.</returns>
    public static async Task<Result<T>> ReadResultFromJsonAsync<T>(this HttpResponseMessage source)
    {
        if (source.IsSuccessStatusCode)
            return await ReadValue<T>(source.Content);

        var problemDetails = await ReadProblemDetails(source.Content);

        if (problemDetails.IsSuccess)
        {
            var error = GetErrorFromProblemDetails(problemDetails.Value);
            return Result<T>.Create.Fail(error);
        }

        return Result<T>.Create.Fail(problemDetails.Error);
    }

    /// <summary>
    /// Reads the HTTP response as a <see cref="MaybeResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="source">
    /// The <see cref="HttpResponseMessage"/> to convert to a <see cref="MaybeResult{T}"/>.
    /// </param>
    /// <returns>The maybe result representing the response.</returns>
    public static async Task<MaybeResult<T>> ReadMaybeResultFromJsonAsync<T>(this HttpResponseMessage source)
    {
        if (source.IsSuccessStatusCode)
            return await ReadMaybeValue<T>(source.Content);

        var problemDetails = await ReadProblemDetails(source.Content);

        if (problemDetails.IsSuccess)
        {
            var error = GetErrorFromProblemDetails(problemDetails.Value);
            return MaybeResult<T>.Create.Fail(error);
        }

        return MaybeResult<T>.Create.Fail(problemDetails.Error);
    }

    private static async Task<Result<T>> ReadValue<T>(HttpContent content)
    {
        try
        {
            var value = await content.ReadFromJsonAsync<T>();

            if (value is null)
                return Result<T>.Create.Fail("Response content was the literal string \"null\".");

            return Result<T>.Create.Success(value);
        }
        catch (Exception ex)
        {
            return Result<T>.Create.Fail(ex, "Error reading value from response content");
        }
    }

    private static async Task<MaybeResult<T>> ReadMaybeValue<T>(HttpContent content)
    {
        try
        {
            var value = await content.ReadFromJsonAsync<T>();

            if (value is null)
                return MaybeResult<T>.Create.None();

            return MaybeResult<T>.Create.Some(value);
        }
        catch (Exception ex)
        {
            return MaybeResult<T>.Create.Fail(ex, "Error reading value from response content");
        }
    }

    private static async Task<Result<ProblemDetails>> ReadProblemDetails(HttpContent content)
    {
        try
        {
            var problemDetails = await content.ReadFromJsonAsync<ProblemDetails>();

            if (problemDetails is null)
                return Result<ProblemDetails>.Create.Fail("Response content was the literal string \"null\".");

            return Result<ProblemDetails>.Create.Success(problemDetails);
        }
        catch (Exception ex)
        {
            return Result<ProblemDetails>.Create.Fail(ex, "Error reading problem details from response content.");
        }
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

        return new Error(message.ToString(), stackTrace, problemDetails.Status, identifier, errorType);
    }
}
