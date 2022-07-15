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
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
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
    public static Task<Result<HttpResponseMessage>> TryDeleteAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default) =>
        Delegates.AsyncFunc(() => sourceHttpClient.DeleteAsync(requestUri, cancellationToken))
            .TryInvokeAsResultAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

    /// <summary>
    /// Sends a GET request to the specified Uri and gets the value that results from deserializing the response body as JSON in
    /// an asynchronous operation. A <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
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
    public static Task<Result<HttpResponseMessage>> TryGetAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default) =>
        Delegates.AsyncFunc(() => sourceHttpClient.GetAsync(requestUri, cancellationToken))
            .TryInvokeAsResultAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

    /// <summary>
    /// Sends a GET request to the specified Uri and gets the value that results from deserializing the response body as JSON in
    /// an asynchronous operation. A <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
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
        this HttpClient sourceHttpClient,
        string? requestUri,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var responseResult = await sourceHttpClient.TryGetAsync(requestUri, getHttpError, getTimeoutError, cancellationToken).ConfigureAwait(false);
        var returnResult = await responseResult.ReadMaybeFromJsonAsync<TValue>(options, cancellationToken).ConfigureAwait(false);
        responseResult.OnSuccess(response => response.Dispose());
        return returnResult;
    }

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER

    /// <summary>
    /// Sends a PATCH request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
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
    public static Task<Result<HttpResponseMessage>> TryPatchAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        HttpContent? content,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default) =>
        Delegates.AsyncFunc(() => sourceHttpClient.PatchAsync(requestUri, content, cancellationToken))
            .TryInvokeAsResultAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

    /// <summary>
    /// Sends a PATCH request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
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
    public static Task<Result<HttpResponseMessage>> TryPatchAsJsonAsync<TValue>(
        this HttpClient sourceHttpClient,
        string? requestUri,
        TValue value,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var content = JsonContent.Create(value, mediaType: null, options);
        return sourceHttpClient.TryPatchAsync(requestUri, content, getHttpError, getTimeoutError, cancellationToken);
    }

#endif

    /// <summary>
    /// Sends a POST request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
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
    public static Task<Result<HttpResponseMessage>> TryPostAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        HttpContent? content,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default) =>
        Delegates.AsyncFunc(() => sourceHttpClient.PostAsync(requestUri, content, cancellationToken))
            .TryInvokeAsResultAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

    /// <summary>
    /// Sends a POST request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
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
    public static Task<Result<HttpResponseMessage>> TryPostAsJsonAsync<TValue>(
        this HttpClient sourceHttpClient,
        string? requestUri,
        TValue value,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var content = JsonContent.Create(value, mediaType: null, options);
        return sourceHttpClient.TryPostAsync(requestUri, content, getHttpError, getTimeoutError, cancellationToken);
    }

    /// <summary>
    /// Sends a PUT request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
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
    public static Task<Result<HttpResponseMessage>> TryPutAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        HttpContent? content,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default) =>
        Delegates.AsyncFunc(() => sourceHttpClient.PutAsync(requestUri, content, cancellationToken))
            .TryInvokeAsResultAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

    /// <summary>
    /// Sends a PUT request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
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
    public static Task<Result<HttpResponseMessage>> TryPutAsJsonAsync<TValue>(
        this HttpClient sourceHttpClient,
        string? requestUri,
        TValue value,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var content = JsonContent.Create(value, mediaType: null, options);
        return sourceHttpClient.TryPutAsync(requestUri, content, getHttpError, getTimeoutError, cancellationToken);
    }

    /// <summary>
    /// Send an HTTP request as an asynchronous operation. A <see cref="Result{T}"/> value is returned, representing the result of
    /// the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
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
    public static Task<Result<HttpResponseMessage>> TrySendAsync(
        this HttpClient sourceHttpClient,
        HttpRequestMessage request,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default) =>
        Delegates.AsyncFunc(() => sourceHttpClient.SendAsync(request, cancellationToken))
            .TryInvokeAsResultAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);
}
