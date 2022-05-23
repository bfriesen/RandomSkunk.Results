namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>OnFail</c> and <c>OnFailAsync</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if <paramref name="source"/> is a <c>Fail</c> result.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static Result OnFail(this Result source, Action<Error> onFail)
    {
        if (source.IsFail)
            onFail(source.Error());

        return source;
    }

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if <paramref name="source"/> is a <c>Fail</c> result.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Result> OnFailAsync(this Result source, Func<Error, Task> onFail)
    {
        if (source.IsFail)
            await onFail(source.Error());

        return source;
    }

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if <paramref name="source"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static Result<T> OnFail<T>(this Result<T> source, Action<Error> onFail)
    {
        if (source.IsFail)
            onFail(source.Error());

        return source;
    }

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if <paramref name="source"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Result<T>> OnFailAsync<T>(this Result<T> source, Func<Error, Task> onFail)
    {
        if (source.IsFail)
            await onFail(source.Error());

        return source;
    }

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if <paramref name="source"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static Maybe<T> OnFail<T>(this Maybe<T> source, Action<Error> onFail)
    {
        if (source.IsFail)
            onFail(source.Error());

        return source;
    }

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if <paramref name="source"/> is a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Maybe<T>> OnFailAsync<T>(this Maybe<T> source, Func<Error, Task> onFail)
    {
        if (source.IsFail)
            await onFail(source.Error());

        return source;
    }
}
