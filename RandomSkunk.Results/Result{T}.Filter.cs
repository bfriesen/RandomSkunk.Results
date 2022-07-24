namespace RandomSkunk.Results;

/// <content> Defines the <c>Filter</c> and <c>FilterAsync</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the <paramref name="filter"/>
    /// function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <param name="filter">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="filter"/> is <see langword="null"/>.</exception>
    public Maybe<T> Filter(Func<T, bool> filter)
    {
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        if (IsSuccess)
        {
            return filter(_value!)
                ? AsMaybe()
                : Maybe<T>.None();
        }

        return AsMaybe();
    }

    /// <summary>
    /// Filters the current result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="filterAsync"/> function evaluates to <see langword="false"/>. Otherwise returns the result unchanged.
    /// </summary>
    /// <param name="filterAsync">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="filterAsync"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<T>> FilterAsync(Func<T, Task<bool>> filterAsync)
    {
        if (filterAsync is null) throw new ArgumentNullException(nameof(filterAsync));

        if (IsSuccess)
        {
            return await filterAsync(_value!).ConfigureAwait(false)
                ? AsMaybe()
                : Maybe<T>.None();
        }

        return AsMaybe();
    }
}
