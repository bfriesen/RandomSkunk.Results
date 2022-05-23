namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>OnSome</c> and <c>OnSomeAsync</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Invokes the <paramref name="onSome"/> function if <paramref name="source"/> is a <c>Some</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onSome">A callback function to invoke if the source is a <c>Some</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static Maybe<T> OnSome<T>(this Maybe<T> source, Action<T> onSome)
    {
        if (source.IsSome)
            onSome(source._value!);

        return source;
    }

    /// <summary>
    /// Invokes the <paramref name="onSome"/> function if <paramref name="source"/> is a <c>Some</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onSome">A callback function to invoke if the source is a <c>Some</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Maybe<T>> OnSomeAsync<T>(this Maybe<T> source, Func<T, Task> onSome)
    {
        if (source.IsSome)
            await onSome(source._value!);

        return source;
    }

    /// <summary>
    /// Invokes the <paramref name="onSome"/> function if <paramref name="source"/> is a <c>Some</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onSome">A callback function to invoke if the source is a <c>Some</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Maybe<T>> OnSomeAsync<T>(this Task<Maybe<T>> source, Action<T> onSome)
    {
        var maybe = await source;

        if (maybe.IsSome)
            onSome(maybe._value!);

        return maybe;
    }

    /// <summary>
    /// Invokes the <paramref name="onSome"/> function if <paramref name="source"/> is a <c>Some</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onSome">A callback function to invoke if the source is a <c>Some</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Maybe<T>> OnSomeAsync<T>(this Task<Maybe<T>> source, Func<T, Task> onSome)
    {
        var maybe = await source;

        if (maybe.IsSome)
            await onSome(maybe._value!);

        return maybe;
    }
}
