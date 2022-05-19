using static RandomSkunk.Results.Delegates;

namespace RandomSkunk.Results.Http;

/// <summary>
/// Defines extension methods for <see cref="HttpClient"/> that return result values.
/// </summary>
public static class HttpClientExtensions
{
    private static Func<HttpRequestException, Error> _defaultGetHttpError =
        ex => Error.FromException(ex, "The HTTP request failed.", 500);

    private static Func<TaskCanceledException, Error> _defaultGetTimeoutError =
        ex => Error.FromException(ex, "The HTTP request timed out.", 504);

    /// <summary>
    /// Gets or sets the default value for <c>Func&lt;HttpRequestException, Error&gt; getHttpError</c> parameters.
    /// </summary>
    public static Func<HttpRequestException, Error> DefaultGetHttpError
    {
        get => _defaultGetHttpError;
        set => _defaultGetHttpError = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the default value for <c>Func&lt;TaskCanceledException, Error&gt; getTimeoutError</c> parameters.
    /// </summary>
    public static Func<TaskCanceledException, Error> DefaultGetTimeoutError
    {
        get => _defaultGetTimeoutError;
        set => _defaultGetTimeoutError = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Sends a DELETE request to the specified Uri with a cancellation token as an asynchronous operation. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="source">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="getHttpError">
    /// An optional function that maps a caught <see cref="HttpRequestException"/> to a <c>Fail</c> result's error. If
    /// <see langword="null"/>, the exception is mapped by invoking <see cref="DefaultGetHttpError"/>.
    /// </param>
    /// <param name="getTimeoutError">
    /// An optional function that maps a caught <see cref="TaskCanceledException"/> to a <c>Fail</c> result's error. If
    /// <see langword="null"/>, the exception is mapped by invoking <see cref="DefaultGetTimeoutError"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task<Result> TryDelete(
        this HttpClient source,
        string? requestUri,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        var responseResult = await AsyncFunc(() => source.DeleteAsync(requestUri, cancellationToken))
            .ToResultAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        try
        {
            return await responseResult.CrossMapAsync(response => response.GetResultAsync(cancellationToken));
        }
        finally
        {
            responseResult.OnSuccess(response => response.Dispose());
        }
    }

    /// <summary>
    /// Sends a DELETE request to the specified Uri with a cancellation token as an asynchronous operation. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="source">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result> TryDelete(
        this HttpClient source,
        string? requestUri,
        CancellationToken cancellationToken = default) =>
        source.TryDelete(requestUri, null, null, cancellationToken);

    /// <summary>
    /// Sends a GET request to the specified Uri and gets the value that results from deserializing the response body as JSON in
    /// an asynchronous operation. A <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="getHttpError">
    /// An optional function that maps a caught <see cref="HttpRequestException"/> to a <c>Fail</c> result's error. If
    /// <see langword="null"/>, the exception is mapped by invoking <see cref="DefaultGetHttpError"/>.
    /// </param>
    /// <param name="getTimeoutError">
    /// An optional function that maps a caught <see cref="TaskCanceledException"/> to a <c>Fail</c> result's error. If
    /// <see langword="null"/>, the exception is mapped by invoking <see cref="DefaultGetTimeoutError"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task<Maybe<TValue>> TryGetFromJsonAsync<TValue>(
        this HttpClient source,
        string? requestUri,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var responseResult = await AsyncFunc(() => source.GetAsync(requestUri, cancellationToken))
            .ToResultAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        try
        {
            return await responseResult.CrossMapAsync(response => response.ReadMaybeFromJsonAsync<TValue>(options, cancellationToken));
        }
        finally
        {
            responseResult.OnSuccess(response => response.Dispose());
        }
    }

    /// <summary>
    /// Sends a GET request to the specified Uri and gets the value that results from deserializing the response body as JSON in
    /// an asynchronous operation. A <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Maybe<TValue>> TryGetFromJsonAsync<TValue>(
        this HttpClient source,
        string? requestUri,
        CancellationToken cancellationToken = default) =>
        source.TryGetFromJsonAsync<TValue>(requestUri, null, null, null, cancellationToken);

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER

    /// <summary>
    /// Sends a PATCH request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="getHttpError">
    /// An optional function that maps a caught <see cref="HttpRequestException"/> to a <c>Fail</c> result's error. If
    /// <see langword="null"/>, the exception is mapped by invoking <see cref="DefaultGetHttpError"/>.
    /// </param>
    /// <param name="getTimeoutError">
    /// An optional function that maps a caught <see cref="TaskCanceledException"/> to a <c>Fail</c> result's error. If
    /// <see langword="null"/>, the exception is mapped by invoking <see cref="DefaultGetTimeoutError"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task<Result> TryPatchAsJsonAsync<TValue>(
        this HttpClient source,
        string? requestUri,
        TValue value,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var content = JsonContent.Create(value, mediaType: null, options);

        var responseResult = await AsyncFunc(() => source.PatchAsync(requestUri, content, cancellationToken))
            .ToResultAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        try
        {
            return await responseResult.CrossMapAsync(response => response.GetResultAsync(options, cancellationToken));
        }
        finally
        {
            responseResult.OnSuccess(response => response.Dispose());
        }
    }

    /// <summary>
    /// Sends a PATCH request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result> TryPatchAsJsonAsync<TValue>(
        this HttpClient source,
        string? requestUri,
        TValue value,
        CancellationToken cancellationToken = default) =>
        source.TryPatchAsJsonAsync(requestUri, value, null, null, null, cancellationToken);

#endif

    /// <summary>
    /// Sends a POST request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="getHttpError">
    /// An optional function that maps a caught <see cref="HttpRequestException"/> to a <c>Fail</c> result's error. If
    /// <see langword="null"/>, the exception is mapped by invoking <see cref="DefaultGetHttpError"/>.
    /// </param>
    /// <param name="getTimeoutError">
    /// An optional function that maps a caught <see cref="TaskCanceledException"/> to a <c>Fail</c> result's error. If
    /// <see langword="null"/>, the exception is mapped by invoking <see cref="DefaultGetTimeoutError"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task<Result> TryPostAsJsonAsync<TValue>(
        this HttpClient source,
        string? requestUri,
        TValue value,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var content = JsonContent.Create(value, mediaType: null, options);

        var responseResult = await AsyncFunc(() => source.PostAsync(requestUri, content, cancellationToken))
            .ToResultAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        try
        {
            return await responseResult.CrossMapAsync(response => response.GetResultAsync(options, cancellationToken));
        }
        finally
        {
            responseResult.OnSuccess(response => response.Dispose());
        }
    }

    /// <summary>
    /// Sends a POST request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result> TryPostAsJsonAsync<TValue>(
        this HttpClient source,
        string? requestUri,
        TValue value,
        CancellationToken cancellationToken = default) =>
        source.TryPostAsJsonAsync(requestUri, value, null, null, null, cancellationToken);

    /// <summary>
    /// Sends a PUT request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="getHttpError">
    /// An optional function that maps a caught <see cref="HttpRequestException"/> to a <c>Fail</c> result's error. If
    /// <see langword="null"/>, the exception is mapped by invoking <see cref="DefaultGetHttpError"/>.
    /// </param>
    /// <param name="getTimeoutError">
    /// An optional function that maps a caught <see cref="TaskCanceledException"/> to a <c>Fail</c> result's error. If
    /// <see langword="null"/>, the exception is mapped by invoking <see cref="DefaultGetTimeoutError"/>.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during deserialization. The default options are those specified by
    /// <see cref="JsonSerializerDefaults.Web"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task<Result> TryPutAsJsonAsync<TValue>(
        this HttpClient source,
        string? requestUri,
        TValue value,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var content = JsonContent.Create(value, mediaType: null, options);

        var responseResult = await AsyncFunc(() => source.PutAsync(requestUri, content, cancellationToken))
            .ToResultAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        try
        {
            return await responseResult.CrossMapAsync(response => response.GetResultAsync(options, cancellationToken));
        }
        finally
        {
            responseResult.OnSuccess(response => response.Dispose());
        }
    }

    /// <summary>
    /// Sends a PUT request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result> TryPutAsJsonAsync<TValue>(
        this HttpClient source,
        string? requestUri,
        TValue value,
        CancellationToken cancellationToken = default) =>
        source.TryPutAsJsonAsync(requestUri, value, null, null, null, cancellationToken);

    /// <summary>
    /// Send an HTTP request as an asynchronous operation. A <see cref="Result{T}"/> value is returned, representing the result of
    /// the overall operation.
    /// </summary>
    /// <param name="source">The HTTP client used to send the request.</param>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="getHttpError">
    /// An optional function that maps a caught <see cref="HttpRequestException"/> to a <c>Fail</c> result's error. If
    /// <see langword="null"/>, the exception is mapped by invoking <see cref="DefaultGetHttpError"/>.
    /// </param>
    /// <param name="getTimeoutError">
    /// An optional function that maps a caught <see cref="TaskCanceledException"/> to a <c>Fail</c> result's error. If
    /// <see langword="null"/>, the exception is mapped by invoking <see cref="DefaultGetTimeoutError"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task<Result<HttpResponseMessage>> TrySendAsync(
        this HttpClient source,
        HttpRequestMessage request,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        var responseResult = await AsyncFunc(() => source.SendAsync(request, cancellationToken))
            .ToResultAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        return responseResult;
    }

    /// <summary>
    /// Send an HTTP request as an asynchronous operation. A <see cref="Result{T}"/> value is returned, representing the result of
    /// the overall operation.
    /// </summary>
    /// <param name="source">The HTTP client used to send the request.</param>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TrySendAsync(
        this HttpClient source,
        HttpRequestMessage request,
        CancellationToken cancellationToken = default) =>
        source.TrySendAsync(request, null, null, cancellationToken);
}
