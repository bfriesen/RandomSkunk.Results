namespace RandomSkunk.Results.Http;

/// <summary>
/// Defines extension methods for <see cref="HttpClient"/> that return result values.
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Sends a DELETE request to the specified Uri with a cancellation token as an asynchronous operation. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryDeleteAsync(
        this HttpClient sourceHttpClient,
        Func<Exception, Error> exceptionHandler,
        string? requestUri,
        CancellationToken cancellationToken = default) =>
        TryCatch.AsResult(
            () => sourceHttpClient.DeleteAsync(requestUri, cancellationToken),
            exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

    /// <summary>
    /// Sends a DELETE request to the specified Uri with a cancellation token as an asynchronous operation. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryDeleteAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        string? errorIdentifier = null,
        int errorCode = ErrorCodes.BadGateway,
        CancellationToken cancellationToken = default) =>
        sourceHttpClient.TryDeleteAsync(ex => GetHttpError(ex, errorCode, errorIdentifier), requestUri, cancellationToken);

    /// <summary>
    /// Sends a GET request to the specified Uri and gets the value that results from deserializing the response body as JSON in
    /// an asynchronous operation. A <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryGetAsync(
        this HttpClient sourceHttpClient,
        Func<Exception, Error> exceptionHandler,
        string? requestUri,
        CancellationToken cancellationToken = default) =>
        TryCatch.AsResult(
            () => sourceHttpClient.GetAsync(requestUri, cancellationToken),
            exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

    /// <summary>
    /// Sends a GET request to the specified Uri and gets the value that results from deserializing the response body as JSON in
    /// an asynchronous operation. A <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryGetAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        string? errorIdentifier = null,
        int errorCode = ErrorCodes.BadGateway,
        CancellationToken cancellationToken = default) =>
        sourceHttpClient.TryGetAsync(ex => GetHttpError(ex, errorCode, errorIdentifier), requestUri, cancellationToken);

    /// <summary>
    /// Sends a GET request to the specified Uri and gets the value that results from deserializing the response body as JSON in
    /// an asynchronous operation. A <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="getExceptionHandler">A function that maps an exception caught when making the GET request to the returned
    ///     <c>Fail</c> result's error.</param>
    /// <param name="readFromJsonExceptionHandler">A function that maps an exception caught when reading from the response's JSON
    ///     content to the returned <c>Fail</c> result's error.</param>
    /// <param name="getNonSuccessResponseError">A function that creates the error for a non-successful HTTP response.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<TValue>> TryGetFromJsonAsync<TValue>(
        this HttpClient sourceHttpClient,
        Func<Exception, Error> getExceptionHandler,
        Func<Exception, Error> readFromJsonExceptionHandler,
        Func<Error, HttpStatusCode, Error> getNonSuccessResponseError,
        string? requestUri,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (sourceHttpClient is null) throw new ArgumentNullException(nameof(sourceHttpClient));
        if (getExceptionHandler is null) throw new ArgumentNullException(nameof(getExceptionHandler));
        if (readFromJsonExceptionHandler is null) throw new ArgumentNullException(nameof(readFromJsonExceptionHandler));
        if (getNonSuccessResponseError is null) throw new ArgumentNullException(nameof(getNonSuccessResponseError));

        return sourceHttpClient
            .TryGetAsync(getExceptionHandler, requestUri, cancellationToken)
            .SelectMany(response =>
                response.TryReadFromJsonAsync<TValue>(readFromJsonExceptionHandler, getNonSuccessResponseError, options, cancellationToken)
                    .Finally(_ => response.Dispose()));
    }

    /// <summary>
    /// Sends a GET request to the specified Uri and gets the value that results from deserializing the response body as JSON in
    /// an asynchronous operation. A <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<TValue>> TryGetFromJsonAsync<TValue>(
        this HttpClient sourceHttpClient,
        string? requestUri,
        JsonSerializerOptions? options = null,
        string? errorIdentifier = null,
        int errorCode = ErrorCodes.BadGateway,
        CancellationToken cancellationToken = default) =>
        sourceHttpClient.TryGetFromJsonAsync<TValue>(
            ex => GetHttpError(ex, errorCode, errorIdentifier),
            ex => HttpResponseExtensions.GetReadFromJsonError(ex, typeof(TValue), errorCode, errorIdentifier),
            (problemDetailsError, statusCode) => HttpResponseExtensions.GetNonSuccessResponseError(problemDetailsError, statusCode, errorCode, errorIdentifier),
            requestUri,
            options,
            cancellationToken);

#if !NETSTANDARD2_0

    /// <summary>
    /// Sends a PATCH request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPatchAsync(
        this HttpClient sourceHttpClient,
        Func<Exception, Error> exceptionHandler,
        string? requestUri,
        HttpContent? content,
        CancellationToken cancellationToken = default) =>
        TryCatch.AsResult(
            () => sourceHttpClient.PatchAsync(requestUri, content!, cancellationToken),
            exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

    /// <summary>
    /// Sends a PATCH request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPatchAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        HttpContent? content,
        string? errorIdentifier = null,
        int errorCode = ErrorCodes.BadGateway,
        CancellationToken cancellationToken = default) =>
        sourceHttpClient.TryPatchAsync(ex => GetHttpError(ex, errorCode, errorIdentifier), requestUri, content, cancellationToken);

    /// <summary>
    /// Sends a PATCH request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPatchAsJsonAsync<TValue>(
        this HttpClient sourceHttpClient,
        Func<Exception, Error> exceptionHandler,
        string? requestUri,
        TValue value,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var content = JsonContent.Create(value, mediaType: null, options);
        return sourceHttpClient.TryPatchAsync(exceptionHandler, requestUri, content, cancellationToken);
    }

    /// <summary>
    /// Sends a PATCH request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPatchAsJsonAsync<TValue>(
        this HttpClient sourceHttpClient,
        string? requestUri,
        TValue value,
        JsonSerializerOptions? options = null,
        string? errorIdentifier = null,
        int errorCode = ErrorCodes.BadGateway,
        CancellationToken cancellationToken = default) =>
        sourceHttpClient.TryPatchAsJsonAsync(ex => GetHttpError(ex, errorCode, errorIdentifier), requestUri, value, options, cancellationToken);

#endif

    /// <summary>
    /// Sends a POST request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPostAsync(
        this HttpClient sourceHttpClient,
        Func<Exception, Error> exceptionHandler,
        string? requestUri,
        HttpContent? content,
        CancellationToken cancellationToken = default) =>
        TryCatch.AsResult(
            () => sourceHttpClient.PostAsync(requestUri, content!, cancellationToken),
            exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

    /// <summary>
    /// Sends a POST request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPostAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        HttpContent? content,
        string? errorIdentifier = null,
        int errorCode = ErrorCodes.BadGateway,
        CancellationToken cancellationToken = default) =>
        sourceHttpClient.TryPostAsync(ex => GetHttpError(ex, errorCode, errorIdentifier), requestUri, content, cancellationToken);

    /// <summary>
    /// Sends a POST request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPostAsJsonAsync<TValue>(
        this HttpClient sourceHttpClient,
        Func<Exception, Error> exceptionHandler,
        string? requestUri,
        TValue value,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var content = JsonContent.Create(value, mediaType: null, options);
        return sourceHttpClient.TryPostAsync(exceptionHandler, requestUri, content, cancellationToken);
    }

    /// <summary>
    /// Sends a POST request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPostAsJsonAsync<TValue>(
        this HttpClient sourceHttpClient,
        string? requestUri,
        TValue value,
        JsonSerializerOptions? options = null,
        string? errorIdentifier = null,
        int errorCode = ErrorCodes.BadGateway,
        CancellationToken cancellationToken = default) =>
        sourceHttpClient.TryPostAsJsonAsync(ex => GetHttpError(ex, errorCode, errorIdentifier), requestUri, value, options, cancellationToken);

    /// <summary>
    /// Sends a PUT request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPutAsync(
        this HttpClient sourceHttpClient,
        Func<Exception, Error> exceptionHandler,
        string? requestUri,
        HttpContent? content,
        CancellationToken cancellationToken = default) =>
        TryCatch.AsResult(
            () => sourceHttpClient.PutAsync(requestUri, content!, cancellationToken),
            exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

    /// <summary>
    /// Sends a PUT request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPutAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        HttpContent? content,
        string? errorIdentifier = null,
        int errorCode = ErrorCodes.BadGateway,
        CancellationToken cancellationToken = default) =>
        sourceHttpClient.TryPutAsync(ex => GetHttpError(ex, errorCode, errorIdentifier), requestUri, content, cancellationToken);

    /// <summary>
    /// Sends a PUT request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPutAsJsonAsync<TValue>(
        this HttpClient sourceHttpClient,
        Func<Exception, Error> exceptionHandler,
        string? requestUri,
        TValue value,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var content = JsonContent.Create(value, mediaType: null, options);
        return sourceHttpClient.TryPutAsync(exceptionHandler, requestUri, content, cancellationToken);
    }

    /// <summary>
    /// Sends a PUT request to the specified Uri containing the value serialized as JSON in the request body. A
    /// <see cref="Result{T}"/> value is returned, representing the result of the overall operation.
    /// </summary>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by
    ///     <see cref="JsonSerializerDefaults.Web"/>.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TryPutAsJsonAsync<TValue>(
        this HttpClient sourceHttpClient,
        string? requestUri,
        TValue value,
        JsonSerializerOptions? options = null,
        string? errorIdentifier = null,
        int errorCode = ErrorCodes.BadGateway,
        CancellationToken cancellationToken = default) =>
        sourceHttpClient.TryPutAsJsonAsync(ex => GetHttpError(ex, errorCode, errorIdentifier), requestUri, value, options, cancellationToken);

    /// <summary>
    /// Send an HTTP request as an asynchronous operation. A <see cref="Result{T}"/> value is returned, representing the result
    /// of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TrySendAsync(
        this HttpClient sourceHttpClient,
        Func<Exception, Error> exceptionHandler,
        HttpRequestMessage request,
        CancellationToken cancellationToken = default) =>
        TryCatch.AsResult(
            () => sourceHttpClient.SendAsync(request, cancellationToken),
            exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

    /// <summary>
    /// Send an HTTP request as an asynchronous operation. A <see cref="Result{T}"/> value is returned, representing the result
    /// of the overall operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<HttpResponseMessage>> TrySendAsync(
        this HttpClient sourceHttpClient,
        HttpRequestMessage request,
        string? errorIdentifier = null,
        int errorCode = ErrorCodes.BadGateway,
        CancellationToken cancellationToken = default) =>
        sourceHttpClient.TrySendAsync(ex => GetHttpError(ex, errorCode, errorIdentifier), request, cancellationToken);

#if NET5_0_OR_GREATER
    /// <summary>
    /// Sends a GET request to the specified Uri and return the response body as a byte array in an asynchronous operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<byte[]>> TryGetByteArrayAsync(
        this HttpClient sourceHttpClient,
        Func<Exception, Error> exceptionHandler,
        string? requestUri,
        CancellationToken cancellationToken = default) =>
        TryCatch.AsResult(
            () => sourceHttpClient.GetByteArrayAsync(requestUri, cancellationToken),
            exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

    /// <summary>
    /// Sends a GET request to the specified Uri and return the response body as a byte array in an asynchronous operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
    ///     cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<byte[]>> TryGetByteArrayAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        string? errorIdentifier = null,
        int errorCode = ErrorCodes.BadGateway,
        CancellationToken cancellationToken = default) =>
        sourceHttpClient.TryGetByteArrayAsync(ex => GetHttpError(ex, errorCode, errorIdentifier), requestUri, cancellationToken);
#else
    // The different signature for lower targets is because before .NET 5, HttpClient.GetByteArrayAsync didn't have
    // an overload with a cancellation token, and it didn't throw a TaskCanceledException due to request timeout.

    /// <summary>
    /// Sends a GET request to the specified Uri and return the response body as a byte array in an asynchronous operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="exceptionHandler">A function that maps a caught exception to the returned <c>Fail</c> result's error.
    ///     </param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<byte[]>> TryGetByteArrayAsync(
        this HttpClient sourceHttpClient,
        Func<Exception, Error> exceptionHandler,
        string? requestUri) =>
        TryCatch.AsResult(
            () => sourceHttpClient.GetByteArrayAsync(requestUri),
            exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler)));

    /// <summary>
    /// Sends a GET request to the specified Uri and return the response body as a byte array in an asynchronous operation.
    /// </summary>
    /// <param name="sourceHttpClient">The HTTP client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="errorIdentifier">The identifier of the error created if the operation is not successful.</param>
    /// <param name="errorCode">The error code of the error created if the operation is not successful.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<Result<byte[]>> TryGetByteArrayAsync(
        this HttpClient sourceHttpClient,
        string? requestUri,
        string? errorIdentifier = null,
        int errorCode = ErrorCodes.BadGateway) =>
        sourceHttpClient.TryGetByteArrayAsync(ex => GetHttpError(ex, errorCode, errorIdentifier), requestUri);
#endif

    private static Error GetHttpError(Exception ex, int errorCode, string? identifier) =>
        Error.FromException(ex, "The HTTP request failed. See InnerError for details.", errorCode, identifier);
}
