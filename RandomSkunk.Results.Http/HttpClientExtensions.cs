using System.Diagnostics;

namespace RandomSkunk.Results.Http;

/// <summary>
/// Defines extension methods for <see cref="HttpClient"/> that return result values.
/// </summary>
public static class HttpClientExtensions
{
    private static readonly Func<HttpRequestException, Error> _defaultGetHttpError = GetHttpError;
    private static readonly Func<TaskCanceledException, Error> _defaultGetTimeoutError = GetTimeoutError;

    /// <summary>
    /// Sends a DELETE request to the specified Uri with a cancellation token as an asynchronous operation. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="getHttpError">An optional function that maps a caught <see cref="HttpRequestException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.BadGateway"/>.
    ///     </para>
    /// </param>
    /// <param name="getTimeoutError">An optional function that maps a caught <see cref="TaskCanceledException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.GatewayTimeout"/>.
    ///     </para>
    /// </param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryDeleteAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default) =>
        TryCatch<HttpRequestException, TaskCanceledException>.AsResult(
            () => sourceHttpClient.DeleteAsync(requestUri, cancellationToken),
            getHttpError ?? _defaultGetHttpError,
            getTimeoutError ?? _defaultGetTimeoutError);

    /// <summary>
    /// Sends a GET request to the specified Uri and gets the value that results from deserializing the response body as JSON in
    /// an asynchronous operation. A <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="getHttpError">An optional function that maps a caught <see cref="HttpRequestException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.BadGateway"/>.
    ///     </para>
    /// </param>
    /// <param name="getTimeoutError">An optional function that maps a caught <see cref="TaskCanceledException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.GatewayTimeout"/>.
    ///     </para>
    /// </param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryGetAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default) =>
        TryCatch<HttpRequestException, TaskCanceledException>.AsResult(
            () => sourceHttpClient.GetAsync(requestUri, cancellationToken),
            getHttpError ?? _defaultGetHttpError,
            getTimeoutError ?? _defaultGetTimeoutError);

    /// <summary>
    /// Sends a GET request to the specified Uri and gets the value that results from deserializing the response body as JSON in
    /// an asynchronous operation. A <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="getHttpError">An optional function that maps a caught <see cref="HttpRequestException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.BadGateway"/>.
    ///     </para>
    /// </param>
    /// <param name="getTimeoutError">An optional function that maps a caught <see cref="TaskCanceledException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.GatewayTimeout"/>.
    ///     </para>
    /// </param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
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

#if !NETSTANDARD2_0

    /// <summary>
    /// Sends a PATCH request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="getHttpError">An optional function that maps a caught <see cref="HttpRequestException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.BadGateway"/>.
    ///     </para>
    /// </param>
    /// <param name="getTimeoutError">An optional function that maps a caught <see cref="TaskCanceledException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.GatewayTimeout"/>.
    ///     </para>
    /// </param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPatchAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        HttpContent? content,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default) =>
        TryCatch<HttpRequestException, TaskCanceledException>.AsResult(
            () => sourceHttpClient.PatchAsync(requestUri, content!, cancellationToken),
            getHttpError ?? _defaultGetHttpError,
            getTimeoutError ?? _defaultGetTimeoutError);

    /// <summary>
    /// Sends a PATCH request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="getHttpError">An optional function that maps a caught <see cref="HttpRequestException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.BadGateway"/>.
    ///     </para>
    /// </param>
    /// <param name="getTimeoutError">An optional function that maps a caught <see cref="TaskCanceledException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.GatewayTimeout"/>.
    ///     </para>
    /// </param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
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
    /// <param name="getHttpError">An optional function that maps a caught <see cref="HttpRequestException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.BadGateway"/>.
    ///     </para>
    /// </param>
    /// <param name="getTimeoutError">An optional function that maps a caught <see cref="TaskCanceledException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.GatewayTimeout"/>.
    ///     </para>
    /// </param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPostAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        HttpContent? content,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default) =>
        TryCatch<HttpRequestException, TaskCanceledException>.AsResult(
            () => sourceHttpClient.PostAsync(requestUri, content!, cancellationToken),
            getHttpError ?? _defaultGetHttpError,
            getTimeoutError ?? _defaultGetTimeoutError);

    /// <summary>
    /// Sends a POST request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="getHttpError">An optional function that maps a caught <see cref="HttpRequestException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.BadGateway"/>.
    ///     </para>
    /// </param>
    /// <param name="getTimeoutError">An optional function that maps a caught <see cref="TaskCanceledException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.GatewayTimeout"/>.
    ///     </para>
    /// </param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
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
    /// <param name="getHttpError">An optional function that maps a caught <see cref="HttpRequestException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.BadGateway"/>.
    ///     </para>
    /// </param>
    /// <param name="getTimeoutError">An optional function that maps a caught <see cref="TaskCanceledException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.GatewayTimeout"/>.
    ///     </para>
    /// </param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPutAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        HttpContent? content,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default) =>
        TryCatch<HttpRequestException, TaskCanceledException>.AsResult(
            () => sourceHttpClient.PutAsync(requestUri, content!, cancellationToken),
            getHttpError ?? _defaultGetHttpError,
            getTimeoutError ?? _defaultGetTimeoutError);

    /// <summary>
    /// Sends a PUT request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="getHttpError">An optional function that maps a caught <see cref="HttpRequestException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.BadGateway"/>.
    ///     </para>
    /// </param>
    /// <param name="getTimeoutError">An optional function that maps a caught <see cref="TaskCanceledException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.GatewayTimeout"/>.
    ///     </para>
    /// </param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
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
    /// Send an HTTP request as an asynchronous operation. A <see cref="Result{T}"/> value is returned, representing the result
    /// of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="getHttpError">An optional function that maps a caught <see cref="HttpRequestException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.BadGateway"/>.
    ///     </para>
    /// </param>
    /// <param name="getTimeoutError">An optional function that maps a caught <see cref="TaskCanceledException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.GatewayTimeout"/>.
    ///     </para>
    /// </param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TrySendAsync(
        this HttpClient sourceHttpClient,
        HttpRequestMessage request,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default) =>
        TryCatch<HttpRequestException, TaskCanceledException>.AsResult(
            () => sourceHttpClient.SendAsync(request, cancellationToken),
            getHttpError ?? _defaultGetHttpError,
            getTimeoutError ?? _defaultGetTimeoutError);

#if NET5_0_OR_GREATER
    /// <summary>
    /// Sends a GET request to the specified Uri and return the response body as a byte array in an asynchronous operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="getHttpError">An optional function that maps a caught <see cref="HttpRequestException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.BadGateway"/>.
    ///     </para>
    /// </param>
    /// <param name="getTimeoutError">An optional function that maps a caught <see cref="TaskCanceledException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.GatewayTimeout"/>.
    ///     </para>
    /// </param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<byte[]>> TryGetByteArrayAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default) =>
        TryCatch<HttpRequestException, TaskCanceledException>.AsResult(
            () => sourceHttpClient.GetByteArrayAsync(requestUri, cancellationToken),
            getHttpError ?? _defaultGetHttpError,
            getTimeoutError ?? _defaultGetTimeoutError);
#else
    // The different signature for lower targets is because before .NET 5, HttpClient.GetByteArrayAsync didn't have
    // an overload with a cancellation token, and it didn't throw a TaskCanceledException due to request timeout.

    /// <summary>
    /// Sends a GET request to the specified Uri and return the response body as a byte array in an asynchronous operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="getHttpError">An optional function that maps a caught <see cref="HttpRequestException"/> to the returned
    ///     <c>Fail</c> result's error.
    ///     <para>
    ///     When <see langword="null"/> or not provided, the error is created with the <see cref="Error.FromException"/> method
    ///     and assigned error code <see cref="ErrorCodes.BadGateway"/>.
    ///     </para>
    /// </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<byte[]>> TryGetByteArrayAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        Func<HttpRequestException, Error>? getHttpError = null) =>
        TryCatch<HttpRequestException>.AsResult(
            () => sourceHttpClient.GetByteArrayAsync(requestUri),
            getHttpError ?? _defaultGetHttpError);
#endif

    [StackTraceHidden]
    private static Error GetHttpError(HttpRequestException ex) =>
        Error.FromException(ex, "The HTTP request failed.", ErrorCodes.BadGateway);

    [StackTraceHidden]
    private static Error GetTimeoutError(TaskCanceledException ex) =>
        Error.FromException(ex, "The HTTP request timed out.", ErrorCodes.GatewayTimeout);
}
