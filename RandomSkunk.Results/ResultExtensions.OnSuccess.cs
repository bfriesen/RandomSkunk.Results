namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>OnSuccess</c> and <c>OnSuccessAsync</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Invokes the <paramref name="onSuccess"/> function if <paramref name="source"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onSuccess">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static Result<T> OnSuccess<T>(this Result<T> source, Action<T> onSuccess)
    {
        if (source.IsSuccess)
            onSuccess(source._value!);

        return source;
    }

    /// <summary>
    /// Invokes the <paramref name="onSuccess"/> function if <paramref name="source"/> is a <c>Success</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="onSuccess">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The <paramref name="source"/> result.</returns>
    public static async Task<Result<T>> OnSuccessAsync<T>(this Result<T> source, Func<T, Task> onSuccess)
    {
        if (source.IsSuccess)
            await onSuccess(source._value!);

        return source;
    }
}
