namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>OnSome</c> and <c>OnSomeAsync</c> methods.
/// </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Invokes the <paramref name="onSome"/> function if the current result is a <c>Some</c> result.
    /// </summary>
    /// <param name="onSome">A callback function to invoke if the source is a <c>Some</c> result.</param>
    /// <returns>The current result.</returns>
    public Maybe<T> OnSome(Action<T> onSome)
    {
        if (IsSome)
            onSome(_value!);

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onSome"/> function if the current result is a <c>Some</c> result.
    /// </summary>
    /// <param name="onSome">A callback function to invoke if the source is a <c>Some</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Maybe<T>> OnSomeAsync(Func<T, Task> onSome)
    {
        if (IsSome)
            await onSome(_value!);

        return this;
    }
}
