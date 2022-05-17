using static RandomSkunk.Results.Delegates;

namespace RandomSkunk.Results.Http;

public static class HttpClientExtensions
{
    private static Func<HttpRequestException, Error> _defaultGetHttpError =
        ex => Error.FromException(ex, "The HTTP request failed.", 500);

    private static Func<TaskCanceledException, Error> _defaultGetTimeoutError =
        ex => Error.FromException(ex, "The HTTP request timed out.", 504);

    public static Func<HttpRequestException, Error> DefaultGetHttpError
    {
        get => _defaultGetHttpError;
        set => _defaultGetHttpError = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static Func<TaskCanceledException, Error> DefaultGetTimeoutError
    {
        get => _defaultGetTimeoutError;
        set => _defaultGetTimeoutError = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Send a DELETE request to the specified Uri with a cancellation token as an asynchronous operation. The response content
    /// is then asynchronously read and deserialized as an object of type <typeparamref name="T"/>. A <see cref="Maybe{T}"/>
    /// value is returned, representing the result of the operation.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client to make the request with.</param>
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
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> TryDeleteAsync<T>(
        this HttpClient source,
        string requestUri,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var responseResult = await AsyncFunc(() => source.DeleteAsync(requestUri, cancellationToken))
            .ToMaybeAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        return await responseResult.FlatMapAsync(value => value.ReadMaybeFromJsonAsync<T>(cancellationToken));
    }

    /// <summary>
    /// Send a DELETE request to the specified Uri with a cancellation token as an asynchronous operation. The response content
    /// is then asynchronously read and deserialized as an object of type <typeparamref name="T"/>. A <see cref="Maybe{T}"/>
    /// value is returned, representing the result of the operation.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Task<Maybe<T>> TryDeleteAsync<T>(
        this HttpClient source,
        string requestUri,
        CancellationToken cancellationToken) =>
        source.TryDeleteAsync<T>(requestUri, null, null, cancellationToken);

    /// <summary>
    /// Send a DELETE request to the specified Uri with a cancellation token as an asynchronous operation. The response content
    /// is not read. A <see cref="Result"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <param name="source">The HTTP client to make the request with.</param>
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
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Result> TryDeleteAsync(
        this HttpClient source,
        string requestUri,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var responseMaybe = await AsyncFunc(() => source.DeleteAsync(requestUri, cancellationToken))
            .ToMaybeAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        return await responseMaybe.CrossMapAsync(value => value.ReadResultFromJsonAsync(cancellationToken));
    }

    /// <summary>
    /// Send a DELETE request to the specified Uri with a cancellation token as an asynchronous operation. The response content
    /// is not read. A <see cref="Result"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Task<Result> TryDeleteAsync(
        this HttpClient source,
        string requestUri,
        CancellationToken cancellationToken) =>
        source.TryDeleteAsync(requestUri, null, null, cancellationToken);

    /// <summary>
    /// Send a GET request to the specified Uri with a cancellation token as an asynchronous operation. The response content
    /// is then asynchronously read and deserialized as an object of type <typeparamref name="T"/>. A <see cref="Maybe{T}"/>
    /// value is returned, representing the result of the operation.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client to make the request with.</param>
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
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> TryGetAsync<T>(
        this HttpClient source,
        string requestUri,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var responseResult = await AsyncFunc(() => source.GetAsync(requestUri, cancellationToken))
            .ToMaybeAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        return await responseResult.FlatMapAsync(value => value.ReadMaybeFromJsonAsync<T>(cancellationToken));
    }

    /// <summary>
    /// Send a GET request to the specified Uri with a cancellation token as an asynchronous operation. The response content
    /// is then asynchronously read and deserialized as an object of type <typeparamref name="T"/>. A <see cref="Maybe{T}"/>
    /// value is returned, representing the result of the operation.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Task<Maybe<T>> TryGetAsync<T>(
        this HttpClient source,
        string requestUri,
        CancellationToken cancellationToken) =>
        source.TryGetAsync<T>(requestUri, null, null, cancellationToken);

    /// <summary>
    /// Send a GET request to the specified Uri with a cancellation token as an asynchronous operation. The response content
    /// is not read. A <see cref="Result"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <param name="source">The HTTP client to make the request with.</param>
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
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Result> TryGetAsync(
        this HttpClient source,
        string requestUri,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var responseMaybe = await AsyncFunc(() => source.GetAsync(requestUri, cancellationToken))
            .ToMaybeAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        return await responseMaybe.CrossMapAsync(value => value.ReadResultFromJsonAsync(cancellationToken));
    }

    /// <summary>
    /// Send a GET request to the specified Uri with a cancellation token as an asynchronous operation. The response content
    /// is not read. A <see cref="Result"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Task<Result> TryGetAsync(
        this HttpClient source,
        string requestUri,
        CancellationToken cancellationToken) =>
        source.TryGetAsync(requestUri, null, null, cancellationToken);

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER

    /// <summary>
    /// Sends a PATCH request with a cancellation token to a Uri represented as a string as an asynchronous operation. The
    /// response content is then asynchronously read and deserialized as an object of type <typeparamref name="T"/>. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="httpContent">The HTTP request content sent to the server.</param>
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
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> TryPatchAsync<T>(
        this HttpClient source,
        string? requestUri,
        HttpContent? httpContent,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var responseResult = await AsyncFunc(() => source.PatchAsync(requestUri, httpContent, cancellationToken))
            .ToMaybeAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        return await responseResult.FlatMapAsync(value => value.ReadMaybeFromJsonAsync<T>(cancellationToken));
    }

    /// <summary>
    /// Sends a PATCH request with a cancellation token to a Uri represented as a string as an asynchronous operation. The
    /// response content is then asynchronously read and deserialized as an object of type <typeparamref name="T"/>. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="httpContent">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Task<Maybe<T>> TryPatchAsync<T>(
        this HttpClient source,
        string? requestUri,
        HttpContent? httpContent,
        CancellationToken cancellationToken) =>
        source.TryPatchAsync<T>(requestUri, httpContent, null, null, cancellationToken);

    /// <summary>
    /// Sends a PATCH request with a cancellation token to a Uri represented as a string as an asynchronous operation. The
    /// response content is not read. A <see cref="Result"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="httpContent">The HTTP request content sent to the server.</param>
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
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Result> TryPatchAsync(
        this HttpClient source,
        string? requestUri,
        HttpContent? httpContent,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var responseMaybe = await AsyncFunc(() => source.PatchAsync(requestUri, httpContent, cancellationToken))
            .ToMaybeAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        return await responseMaybe.CrossMapAsync(value => value.ReadResultFromJsonAsync(cancellationToken));
    }

    /// <summary>
    /// Sends a PATCH request with a cancellation token to a Uri represented as a string as an asynchronous operation. The
    /// response content is not read. A <see cref="Result"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="httpContent">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Task<Result> TryPatchAsync(
        this HttpClient source,
        string? requestUri,
        HttpContent? httpContent,
        CancellationToken cancellationToken) =>
        source.TryPatchAsync(requestUri, httpContent, null, null, cancellationToken);
#endif

    /// <summary>
    /// Sends a POST request with a cancellation token to a Uri represented as a string as an asynchronous operation. The
    /// response content is then asynchronously read and deserialized as an object of type <typeparamref name="T"/>. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="httpContent">The HTTP request content sent to the server.</param>
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
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> TryPostAsync<T>(
        this HttpClient source,
        string? requestUri,
        HttpContent? httpContent,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var responseResult = await AsyncFunc(() => source.PostAsync(requestUri, httpContent, cancellationToken))
            .ToMaybeAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        return await responseResult.FlatMapAsync(value => value.ReadMaybeFromJsonAsync<T>(cancellationToken));
    }

    /// <summary>
    /// Sends a POST request with a cancellation token to a Uri represented as a string as an asynchronous operation. The
    /// response content is then asynchronously read and deserialized as an object of type <typeparamref name="T"/>. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="httpContent">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Task<Maybe<T>> TryPostAsync<T>(
        this HttpClient source,
        string? requestUri,
        HttpContent? httpContent,
        CancellationToken cancellationToken) =>
        source.TryPostAsync<T>(requestUri, httpContent, null, null, cancellationToken);

    /// <summary>
    /// Sends a POST request with a cancellation token to a Uri represented as a string as an asynchronous operation. The
    /// response content is not read. A <see cref="Result"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="httpContent">The HTTP request content sent to the server.</param>
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
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Result> TryPostAsync(
        this HttpClient source,
        string? requestUri,
        HttpContent? httpContent,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var responseMaybe = await AsyncFunc(() => source.PostAsync(requestUri, httpContent, cancellationToken))
            .ToMaybeAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        return await responseMaybe.CrossMapAsync(value => value.ReadResultFromJsonAsync(cancellationToken));
    }

    /// <summary>
    /// Sends a POST request with a cancellation token to a Uri represented as a string as an asynchronous operation. The
    /// response content is not read. A <see cref="Result"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="httpContent">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Task<Result> TryPostAsync(
        this HttpClient source,
        string? requestUri,
        HttpContent? httpContent,
        CancellationToken cancellationToken) =>
        source.TryPostAsync(requestUri, httpContent, null, null, cancellationToken);

    /// <summary>
    /// Sends a PUT request with a cancellation token to a Uri represented as a string as an asynchronous operation. The
    /// response content is then asynchronously read and deserialized as an object of type <typeparamref name="T"/>. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="httpContent">The HTTP request content sent to the server.</param>
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
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> TryPutAsync<T>(
        this HttpClient source,
        string? requestUri,
        HttpContent? httpContent,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var responseMaybe = await AsyncFunc(() => source.PutAsync(requestUri, httpContent, cancellationToken))
            .ToMaybeAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        return await responseMaybe.FlatMapAsync(value => value.ReadMaybeFromJsonAsync<T>(cancellationToken));
    }

    /// <summary>
    /// Sends a PUT request with a cancellation token to a Uri represented as a string as an asynchronous operation. The
    /// response content is then asynchronously read and deserialized as an object of type <typeparamref name="T"/>. A
    /// <see cref="Maybe{T}"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="httpContent">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Task<Maybe<T>> TryPutAsync<T>(
        this HttpClient source,
        string? requestUri,
        HttpContent? httpContent,
        CancellationToken cancellationToken) =>
        source.TryPutAsync<T>(requestUri, httpContent, null, null, cancellationToken);

    /// <summary>
    /// Sends a PUT request with a cancellation token to a Uri represented as a string as an asynchronous operation. The
    /// response content is not read. A <see cref="Result"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="httpContent">The HTTP request content sent to the server.</param>
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
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Result> TryPutAsync(
        this HttpClient source,
        string? requestUri,
        HttpContent? httpContent,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var responseResult = await AsyncFunc(() => source.PutAsync(requestUri, httpContent, cancellationToken))
            .ToMaybeAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        return await responseResult.CrossMapAsync(value => value.ReadResultFromJsonAsync(cancellationToken));
    }

    /// <summary>
    /// Sends a PUT request with a cancellation token to a Uri represented as a string as an asynchronous operation. The
    /// response content is not read. A <see cref="Result"/> value is returned, representing the result of the operation.
    /// </summary>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="httpContent">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Task<Result> TryPutAsync(
        this HttpClient source,
        string? requestUri,
        HttpContent? httpContent,
        CancellationToken cancellationToken) =>
        source.TryPutAsync(requestUri, httpContent, null, null, cancellationToken);

    /// <summary>
    /// Send an HTTP request as an asynchronous operation. The response content is then asynchronously read and deserialized as
    /// an object of type <typeparamref name="T"/>. A <see cref="Maybe{T}"/> value is returned, representing the result of the
    /// operation.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client to make the request with.</param>
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
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> TrySendAsync<T>(
        this HttpClient source,
        HttpRequestMessage request,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (request is null) throw new ArgumentNullException(nameof(request));

        var responseMaybe = await AsyncFunc(() => source.SendAsync(request, cancellationToken))
            .ToMaybeAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        return await responseMaybe.FlatMapAsync(value => value.ReadMaybeFromJsonAsync<T>(cancellationToken));
    }

    /// <summary>
    /// Send an HTTP request as an asynchronous operation. The response content is then asynchronously read and deserialized as
    /// an object of type <typeparamref name="T"/>. A <see cref="Maybe{T}"/> value is returned, representing the result of the
    /// operation.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Task<Maybe<T>> TrySendAsync<T>(
        this HttpClient source,
        HttpRequestMessage request,
        CancellationToken cancellationToken) =>
        source.TrySendAsync<T>(request, null, null, cancellationToken);

    /// <summary>
    /// Send an HTTP request as an asynchronous operation. The response content is not read. A <see cref="Result"/> value is
    /// returned, representing the result of the operation.
    /// </summary>
    /// <param name="source">The HTTP client to make the request with.</param>
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
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static async Task<Result> TrySendAsync(
        this HttpClient source,
        HttpRequestMessage request,
        Func<HttpRequestException, Error>? getHttpError = null,
        Func<TaskCanceledException, Error>? getTimeoutError = null,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (request is null) throw new ArgumentNullException(nameof(request));

        var responseResult = await AsyncFunc(() => source.SendAsync(request, cancellationToken))
            .ToMaybeAsync(getHttpError ?? _defaultGetHttpError, getTimeoutError ?? _defaultGetTimeoutError);

        return await responseResult.CrossMapAsync(value => value.ReadResultFromJsonAsync(cancellationToken));
    }

    /// <summary>
    /// Send an HTTP request as an asynchronous operation. The response content is not read. A <see cref="Result"/> value is
    /// returned, representing the result of the operation.
    /// </summary>
    /// <param name="source">The HTTP client to make the request with.</param>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A task that represents the asynchronous delete operation, which wraps the result of the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Task<Result> TrySendAsync(
        this HttpClient source,
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return source.TrySendAsync(request, null, null, cancellationToken);
    }
}
