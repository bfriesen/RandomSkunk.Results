namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Filter</c> and <c>FilterAsync</c> methods.
/// </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Filter the current result into a <c>None</c> result if it is a <c>Some</c> result and the
    /// <paramref name="filter"/> function evaluates to <see langword="false"/>.
    /// </summary>
    /// <param name="filter">
    /// A function that filters a <c>Some</c> result into a <c>None</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="filter"/> is <see langword="null"/>.
    /// </exception>
    public Maybe<T> Filter(Func<T, bool> filter)
    {
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        if (IsSome)
        {
            return filter(_value!)
                ? this
                : Maybe<T>.Create.None();
        }

        return this;
    }

    /// <summary>
    /// Filter the current result into a <c>None</c> result if it is a <c>Some</c> result and the
    /// <paramref name="filterAsync"/> function evaluates to <see langword="false"/>.
    /// </summary>
    /// <param name="filterAsync">
    /// A function that filters a <c>Some</c> result into a <c>None</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="filterAsync"/> is <see langword="null"/>.
    /// </exception>
    public async Task<Maybe<T>> FilterAsync(Func<T, Task<bool>> filterAsync)
    {
        if (filterAsync is null) throw new ArgumentNullException(nameof(filterAsync));

        if (IsSome)
        {
            return await filterAsync(_value!)
                ? this
                : Maybe<T>.Create.None();
        }

        return this;
    }
}
