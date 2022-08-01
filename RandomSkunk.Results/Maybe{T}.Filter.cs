namespace RandomSkunk.Results;

/// <content> Defines the <c>Filter</c> and <c>FilterAsync</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public Maybe<T> Filter(Func<T, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (IsSuccess)
        {
            return predicate(_value!)
                ? this
                : Maybe<T>.None();
        }

        return this;
    }

    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<T>> FilterAsync(Func<T, Task<bool>> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (IsSuccess)
        {
            return await predicate(_value!).ConfigureAwait(false)
                ? this
                : Maybe<T>.None();
        }

        return this;
    }
}
