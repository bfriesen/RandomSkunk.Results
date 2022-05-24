namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>OnNone</c> and <c>OnNoneAsync</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Invokes the <paramref name="onNone"/> function if <paramref name="source"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onNone">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static Maybe<T> OnNone<T>(this Maybe<T> source, Action onNone)
    {
        if (source.IsNone)
            onNone();

        return source;
    }

    /// <summary>
    /// Invokes the <paramref name="onNone"/> function if <paramref name="source"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onNone">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Maybe<T>> OnNoneAsync<T>(this Maybe<T> source, Func<Task> onNone)
    {
        if (source.IsNone)
            await onNone();

        return source;
    }

    /// <summary>
    /// Invokes the <paramref name="onNone"/> function if <paramref name="source"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onNone">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Maybe<T>> OnNone<T>(this Task<Maybe<T>> source, Action onNone) =>
        (await source).OnNone(onNone);

    /// <summary>
    /// Invokes the <paramref name="onNone"/> function if <paramref name="source"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onNone">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Maybe<T>> OnNoneAsync<T>(this Task<Maybe<T>> source, Func<Task> onNone) =>
        await (await source).OnNoneAsync(onNone);
}
