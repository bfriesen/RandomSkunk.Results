namespace RandomSkunk.Results;

/// <content> Defines the <c>Else</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Converts this <see cref="Result{T}"/> to an equivalent <see cref="Maybe{T}"/>: if this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned; if this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned; a <c>None</c> result is never returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public static async Task<Maybe<T>> AsMaybe<T>(this Task<Result<T>> sourceResult) =>
        (await sourceResult.ConfigureAwait(false)).AsMaybe();

    /// <summary>
    /// Converts this <see cref="Maybe{T}"/> to an equivalent <see cref="Result{T}"/>: if this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned; if this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned; if this is a <c>None</c> result, then a <c>Fail</c> result with a "Not Found" error (error
    /// code: <see cref="ErrorCodes.NotFound"/>) is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNoneGetError">An optional function that maps a <c>None</c> result to the returned <c>Fail</c> result's
    ///     error.
    ///     <list type="bullet">
    ///         <item>
    ///             This function is evaluated <em>only</em> if this is a <c>None</c> result.
    ///         </item>
    ///         <item>
    ///             When <see langword="null"/> or not provided, an error with error code <see cref="ErrorCodes.NotFound"/> and
    ///             message similar to "Not Found" will be used instead.
    ///         </item>
    ///         <item>
    ///             When provided, the function should not return <see langword="null"/>. If it does, an error with error code
    ///             <see cref="ErrorCodes.BadRequest"/> and a message similar to "Function parameter should not return null" will
    ///             be used instead.
    ///         </item>
    ///     </list>
    /// </param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public static async Task<Result<T>> AsResult<T>(
        this Task<Maybe<T>> sourceResult,
        Func<Error>? onNoneGetError = null) =>
        (await sourceResult.ConfigureAwait(false)).AsResult(onNoneGetError);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> FlatMap<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> FlatMapAsync<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error. A <c>None</c> result is never returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> FlatMap<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error. A <c>None</c> result is never returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> FlatMapAsync<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <param name="onNoneGetError">An optional function that maps a <c>None</c> result to the returned <c>Fail</c> result's
    ///     error.
    ///     <list type="bullet">
    ///         <item>
    ///             This function is evaluated <em>only</em> if this is a <c>None</c> result.
    ///         </item>
    ///         <item>
    ///             When <see langword="null"/> or not provided, an error with error code <see cref="ErrorCodes.NotFound"/> and
    ///             message similar to "Not Found" will be used instead.
    ///         </item>
    ///         <item>
    ///             When provided, the function should not return <see langword="null"/>. If it does, an error with error code
    ///             <see cref="ErrorCodes.BadRequest"/> and a message similar to "Function parameter should not return null" will
    ///             be used instead.
    ///         </item>
    ///     </list>
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> FlatMap<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Result> onSuccessSelector,
        Func<Error>? onNoneGetError = null) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector, onNoneGetError);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <param name="onNoneGetError">An optional function that maps a <c>None</c> result to the returned <c>Fail</c> result's
    ///     error.
    ///     <list type="bullet">
    ///         <item>
    ///             This function is evaluated <em>only</em> if this is a <c>None</c> result.
    ///         </item>
    ///         <item>
    ///             When <see langword="null"/> or not provided, an error with error code <see cref="ErrorCodes.NotFound"/> and
    ///             message similar to "Not Found" will be used instead.
    ///         </item>
    ///         <item>
    ///             When provided, the function should not return <see langword="null"/>. If it does, an error with error code
    ///             <see cref="ErrorCodes.BadRequest"/> and a message similar to "Function parameter should not return null" will
    ///             be used instead.
    ///         </item>
    ///     </list>
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> FlatMapAsync<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Result>> onSuccessSelector,
        Func<Error>? onNoneGetError = null) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector, onNoneGetError).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <param name="onNoneGetError">An optional function that maps a <c>None</c> result to the returned <c>Fail</c> result's
    ///     error.
    ///     <list type="bullet">
    ///         <item>
    ///             This function is evaluated <em>only</em> if this is a <c>None</c> result.
    ///         </item>
    ///         <item>
    ///             When <see langword="null"/> or not provided, an error with error code <see cref="ErrorCodes.NotFound"/> and
    ///             message similar to "Not Found" will be used instead.
    ///         </item>
    ///         <item>
    ///             When provided, the function should not return <see langword="null"/>. If it does, an error with error code
    ///             <see cref="ErrorCodes.BadRequest"/> and a message similar to "Function parameter should not return null" will
    ///             be used instead.
    ///         </item>
    ///     </list>
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> FlatMap<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Result<TReturn>> onSuccessSelector,
        Func<Error>? onNoneGetError = null) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector, onNoneGetError);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <param name="onNoneGetError">An optional function that maps a <c>None</c> result to the returned <c>Fail</c> result's
    ///     error.
    ///     <list type="bullet">
    ///         <item>
    ///             This function is evaluated <em>only</em> if this is a <c>None</c> result.
    ///         </item>
    ///         <item>
    ///             When <see langword="null"/> or not provided, an error with error code <see cref="ErrorCodes.NotFound"/> and
    ///             message similar to "Not Found" will be used instead.
    ///         </item>
    ///         <item>
    ///             When provided, the function should not return <see langword="null"/>. If it does, an error with error code
    ///             <see cref="ErrorCodes.BadRequest"/> and a message similar to "Function parameter should not return null" will
    ///             be used instead.
    ///         </item>
    ///     </list>
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> FlatMapAsync<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Result<TReturn>>> onSuccessSelector,
        Func<Error>? onNoneGetError = null) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector, onNoneGetError).ConfigureAwait(false);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the specified fallback result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or <paramref name="fallbackResult"/>.</returns>
    public static async Task<Result> Else(this Task<Result> sourceResult, Result fallbackResult) =>
        (await sourceResult.ConfigureAwait(false)).Else(fallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the result from evaluating the
    /// <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackResult">A function that returns the fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or the value returned from <paramref name="getFallbackResult"/>.
    ///     </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackResult"/> is <see langword="null"/>.</exception>
    public static async Task<Result> Else(this Task<Result> sourceResult, Func<Result> getFallbackResult) =>
        (await sourceResult.ConfigureAwait(false)).Else(getFallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or <paramref name="fallbackResult"/>.</returns>
    public static async Task<Result<T>> Else<T>(this Task<Result<T>> sourceResult, Result<T> fallbackResult) =>
        (await sourceResult.ConfigureAwait(false)).Else(fallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the result from evaluating the
    /// <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackResult">A function that returns the fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or the value returned from <paramref name="getFallbackResult"/>.
    ///     </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackResult"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> Else<T>(this Task<Result<T>> sourceResult, Func<Result<T>> getFallbackResult) =>
        (await sourceResult.ConfigureAwait(false)).Else(getFallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or <paramref name="fallbackResult"/>.</returns>
    public static async Task<Maybe<T>> Else<T>(this Task<Maybe<T>> sourceResult, Maybe<T> fallbackResult) =>
        (await sourceResult.ConfigureAwait(false)).Else(fallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the result from evaluating the
    /// <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackResult">A function that returns the fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or the value returned from <paramref name="getFallbackResult"/>.
    ///     </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackResult"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Else<T>(this Task<Maybe<T>> sourceResult, Func<Maybe<T>> getFallbackResult) =>
        (await sourceResult.ConfigureAwait(false)).Else(getFallbackResult);

    /// <summary>
    /// Filter the specified result into a <c>None</c> result if it is a <c>Success</c> result and the <paramref name="filter"/>
    /// function evaluates to <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="filter">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceResult"/> is <see langword="null"/> or if
    ///     <paramref name="filter"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Filter<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, bool> filter) =>
        (await sourceResult.ConfigureAwait(false)).Filter(filter);

    /// <summary>
    /// Filter the specified result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="filterAsync"/> function evaluates to <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="filterAsync">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceResult"/> is <see langword="null"/> or if
    ///     <paramref name="filterAsync"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> FilterAsync<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<bool>> filterAsync) =>
        await (await sourceResult.ConfigureAwait(false)).FilterAsync(filterAsync).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> FlatMap<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> FlatMapAsync<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> FlatMap<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> FlatMapAsync<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Result<T>> Flatten<T>(this Task<Result<Result<T>>> sourceResult) =>
        (await sourceResult.ConfigureAwait(false)).Flatten();

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Maybe<T>> Flatten<T>(this Task<Maybe<Maybe<T>>> sourceResult) =>
        (await sourceResult.ConfigureAwait(false)).Flatten();

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackValue">The fallback value to return if this is not a <c>Success</c> result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, <paramref name="fallbackValue"/>.
    ///     </returns>
    public static async Task<T?> GetValueOr<T>(this Task<Result<T>> sourceResult, T? fallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).GetValueOr(fallbackValue);

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackValue">A function that creates the fallback value to return if this is not a <c>Success</c>
    ///     result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the value returned by the
    ///     <paramref name="getFallbackValue"/> function.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    public static async Task<T?> GetValueOr<T>(this Task<Result<T>> sourceResult, Func<T?> getFallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).GetValueOr(getFallbackValue);

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackValue">The fallback value to return if this is not a <c>Success</c> result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, <paramref name="fallbackValue"/>.
    ///     </returns>
    public static async Task<T?> GetValueOr<T>(this Task<Maybe<T>> sourceResult, T? fallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).GetValueOr(fallbackValue);

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackValue">A function that creates the fallback value to return if this is not a <c>Success</c>
    ///     result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the value returned by the
    ///     <paramref name="getFallbackValue"/> function.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    public static async Task<T?> GetValueOr<T>(this Task<Maybe<T>> sourceResult, Func<T?> getFallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).GetValueOr(getFallbackValue);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="onSuccessSelector"/> returns <see langword="null"/> when
    ///     evaluated.</exception>
    public static async Task<Result<TReturn>> Map<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, TReturn> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).Map(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="onSuccessSelector"/> returns <see langword="null"/> when
    ///     evaluated.</exception>
    public static async Task<Result<TReturn>> MapAsync<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).MapAsync(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="onSuccessSelector"/> returns <see langword="null"/> when
    ///     evaluated.</exception>
    public static async Task<Maybe<TReturn>> Map<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, TReturn> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).Map(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="onSuccessSelector"/> returns <see langword="null"/> when
    ///     evaluated.</exception>
    public static async Task<Maybe<TReturn>> MapAsync<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).MapAsync(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The return type of the match method.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public static async Task<T> Match<T>(
        this Task<Result> sourceResult,
        Func<T> onSuccess,
        Func<Error, T> onFail) =>
        (await sourceResult.ConfigureAwait(false)).Match(onSuccess, onFail);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The return type of the match method.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>A task that represents the asynchronous match operation, which wraps the result of the matching function
    ///     evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public static async Task<T> MatchAsync<T>(
        this Task<Result> sourceResult,
        Func<Task<T>> onSuccess,
        Func<Error, Task<T>> onFail) =>
        await (await sourceResult.ConfigureAwait(false)).MatchAsync(onSuccess, onFail).ConfigureAwait(false);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>. The non-null value of the
    ///     <c>Success</c> result is passed to this function.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public static async Task<TReturn> Match<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, TReturn> onSuccess,
        Func<Error, TReturn> onFail) =>
            (await sourceResult.ConfigureAwait(false)).Match(onSuccess, onFail);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>. The non-null value of the
    ///     <c>Success</c> result is passed to this function.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>A task that represents the asynchronous match operation, which wraps the result of the matching function
    ///     evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public static async Task<TReturn> MatchAsync<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccess,
        Func<Error, Task<TReturn>> onFail) =>
            await (await sourceResult.ConfigureAwait(false)).MatchAsync(onSuccess, onFail).ConfigureAwait(false);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/>, <paramref name="onNone"/>, or <paramref name="onFail"/> function
    /// depending on whether the result is <c>Success</c>, <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>. The non-null value of the
    ///     <c>Success</c> result is passed to this function.</param>
    /// <param name="onNone">The function to evaluate if the result is <c>None</c>.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/>, or if
    ///     <paramref name="onNone"/> is <see langword="null"/>, or if <paramref name="onFail"/> is <see langword="null"/>.
    ///     </exception>
    public static async Task<TReturn> Match<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, TReturn> onSuccess,
        Func<TReturn> onNone,
        Func<Error, TReturn> onFail) =>
        (await sourceResult.ConfigureAwait(false)).Match(onSuccess, onNone, onFail);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/>, <paramref name="onNone"/>, or <paramref name="onFail"/> function
    /// depending on whether the result is <c>Success</c>, <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>. The non-null value of the
    ///     <c>Success</c> result is passed to this function.</param>
    /// <param name="onNone">The function to evaluate if the result is <c>None</c>.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>A task that represents the asynchronous match operation, which wraps the result of the matching function
    ///     evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/>, or if
    ///     <paramref name="onNone"/> is <see langword="null"/>, or if <paramref name="onFail"/> is <see langword="null"/>.
    ///     </exception>
    public static async Task<TReturn> MatchAsync<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccess,
        Func<Task<TReturn>> onNone,
        Func<Error, Task<TReturn>> onFail) =>
        await (await sourceResult.ConfigureAwait(false)).MatchAsync(onSuccess, onNone, onFail).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if <paramref name="sourceResult"/> is a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailCallback">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result> OnFail(this Task<Result> sourceResult, Action<Error> onFailCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnFail(onFailCallback);

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if <paramref name="sourceResult"/> is a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailCallback">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result> OnFailAsync(this Task<Result> sourceResult, Func<Error, Task> onFailCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnFailAsync(onFailCallback).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if <paramref name="sourceResult"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailCallback">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result<T>> OnFail<T>(this Task<Result<T>> sourceResult, Action<Error> onFailCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnFail(onFailCallback);

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if <paramref name="sourceResult"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailCallback">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result<T>> OnFailAsync<T>(this Task<Result<T>> sourceResult, Func<Error, Task> onFailCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnFailAsync(onFailCallback).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if <paramref name="sourceResult"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailCallback">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnFail<T>(this Task<Maybe<T>> sourceResult, Action<Error> onFailCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnFail(onFailCallback);

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if <paramref name="sourceResult"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailCallback">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnFailAsync<T>(this Task<Maybe<T>> sourceResult, Func<Error, Task> onFailCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnFailAsync(onFailCallback).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onNoneCallback"/> function if <paramref name="sourceResult"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNoneCallback">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnNone<T>(this Task<Maybe<T>> sourceResult, Action onNoneCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnNone(onNoneCallback);

    /// <summary>
    /// Invokes the <paramref name="onNoneCallback"/> function if <paramref name="sourceResult"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNoneCallback">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnNoneAsync<T>(this Task<Maybe<T>> sourceResult, Func<Task> onNoneCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnNoneAsync(onNoneCallback).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnSuccess<T>(this Task<Maybe<T>> sourceResult, Action<T> onSuccessCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnSuccess(onSuccessCallback);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnSuccessAsync<T>(this Task<Maybe<T>> sourceResult, Func<T, Task> onSuccessCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnSuccessAsync(onSuccessCallback).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result> OnSuccess(this Task<Result> sourceResult, Action onSuccessCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnSuccess(onSuccessCallback);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result> OnSuccessAsync(this Task<Result> sourceResult, Func<Task> onSuccessCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnSuccessAsync(onSuccessCallback).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> sourceResult, Action<T> onSuccessCallback) =>
        (await sourceResult.ConfigureAwait(false)).OnSuccess(onSuccessCallback);

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if <paramref name="sourceResult"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessCallback">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Result<T>> OnSuccessAsync<T>(this Task<Result<T>> sourceResult, Func<T, Task> onSuccessCallback) =>
        await (await sourceResult.ConfigureAwait(false)).OnSuccessAsync(onSuccessCallback).ConfigureAwait(false);

    /// <summary>
    /// Invokes the <paramref name="onNonSuccessCallback"/> function if the current result is a <c>non-Success</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNonSuccessCallback">A callback function to invoke if the source is a <c>non-Success</c> result.</param>
    /// <param name="onNoneGetError">An optional function that creates the <see cref="Error"/> that is returned if this is a
    ///     <c>None</c> result; otherwise this parameter is ignored. If <see langword="null"/> (and applicable), a function that
    ///     returns an error with message "Not Found" and error code <see cref="ErrorCodes.NotFound"/> is used instead.</param>
    /// <returns>The current result.</returns>
    public static async Task<TResult> OnNonSuccess<TResult>(
        this Task<TResult> sourceResult,
        Action<Error> onNonSuccessCallback,
        Func<Error>? onNoneGetError = null)
        where TResult : IResult =>
        (await sourceResult.ConfigureAwait(false)).OnNonSuccess(onNonSuccessCallback, onNoneGetError);

    /// <summary>
    /// Invokes the <paramref name="onNonSuccess"/> function if the current result is a <c>non-Success</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNonSuccess">A callback function to invoke if the source is a <c>non-Success</c> result.</param>
    /// <param name="onNoneGetError">An optional function that creates the <see cref="Error"/> that is returned if this is a
    ///     <c>None</c> result; otherwise this parameter is ignored. If <see langword="null"/> (and applicable), a function that
    ///     returns an error with message "Not Found" and error code <see cref="ErrorCodes.NotFound"/> is used instead.</param>
    /// <returns>The current result.</returns>
    public static async Task<TResult> OnNonSuccessAsync<TResult>(
        this Task<TResult> sourceResult,
        Func<Error, Task> onNonSuccess,
        Func<Error>? onNoneGetError = null)
        where TResult : IResult =>
        await (await sourceResult.ConfigureAwait(false)).OnNonSuccessAsync(onNonSuccess, onNoneGetError);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result; otherwise, returns a new <c>Success</c> result
    /// with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackValue">The fallback value if the result is not <c>Success</c>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="fallbackValue"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> Or<T>(this Task<Result<T>> sourceResult, [DisallowNull] T fallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).Or(fallbackValue);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result; otherwise, returns a new <c>Success</c> result
    /// with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackValue">A function that returns the fallback value if the result is not <c>Success</c>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    ///     </exception>
    public static async Task<Result<T>> Or<T>(this Task<Result<T>> sourceResult, Func<T> getFallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).Or(getFallbackValue);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result; otherwise, returns a new <c>Success</c> result
    /// with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackValue">The fallback value if the result is not <c>Success</c>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="fallbackValue"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Or<T>(this Task<Maybe<T>> sourceResult, [DisallowNull] T fallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).Or(fallbackValue);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result; otherwise, returns a new <c>Success</c> result
    /// with its value from evaluating the <paramref name="getFallbackValue"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackValue">A function that returns the fallback value if the result is not <c>Success</c>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    ///     </exception>
    public static async Task<Maybe<T>> Or<T>(this Task<Maybe<T>> sourceResult, Func<T> getFallbackValue) =>
        (await sourceResult.ConfigureAwait(false)).Or(getFallbackValue);

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailGetError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="onFailGetError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, <paramref name="sourceResult"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onFailGetError"/> is <see langword="null"/>.</exception>
    public static async Task<Result> WithError(this Task<Result> sourceResult, Func<Error, Error> onFailGetError) =>
        (await sourceResult.ConfigureAwait(false)).WithError(onFailGetError);

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailGetError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="onFailGetError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, <paramref name="sourceResult"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onFailGetError"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> WithError<T>(this Task<Result<T>> sourceResult, Func<Error, Error> onFailGetError) =>
        (await sourceResult.ConfigureAwait(false)).WithError(onFailGetError);

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onFailGetError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="onFailGetError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, <paramref name="sourceResult"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onFailGetError"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> WithError<T>(this Task<Maybe<T>> sourceResult, Func<Error, Error> onFailGetError) =>
        (await sourceResult.ConfigureAwait(false)).WithError(onFailGetError);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> FlatMap(
        this Task<Result> sourceResult,
        Func<Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> FlatMapAsync(
        this Task<Result> sourceResult,
        Func<Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> FlatMap<TReturn>(
        this Task<Result> sourceResult,
        Func<Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> FlatMapAsync<TReturn>(
        this Task<Result> sourceResult,
        Func<Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error. A <c>None</c> result is never returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> FlatMap<TReturn>(
        this Task<Result> sourceResult,
        Func<Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error. A <c>None</c> result is never returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> FlatMapAsync<TReturn>(
        this Task<Result> sourceResult,
        Func<Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector);
}
