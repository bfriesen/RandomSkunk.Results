namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Filter</c> and <c>FilterAsync</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Filter the specified result into a <c>None</c> result if it is a <c>Some</c> result and the
    /// <paramref name="filter"/> function evaluates to <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="filter">
    /// A function that filters a <c>Some</c> result into a <c>None</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="filter"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<T> Filter<T>(this Maybe<T> source, Func<T, bool> filter)
    {
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        if (source.IsSome)
        {
            return filter(source._value!)
                ? source
                : Maybe<T>.Create.None();
        }

        return source;
    }

    /// <summary>
    /// Filter the specified result into a <c>None</c> result if it is a <c>Some</c> result and the
    /// <paramref name="filterAsync"/> function evaluates to <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="filterAsync">
    /// A function that filters a <c>Some</c> result into a <c>None</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="filterAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task<Maybe<T>> FilterAsync<T>(
        this Maybe<T> source,
        Func<T, Task<bool>> filterAsync)
    {
        if (filterAsync is null) throw new ArgumentNullException(nameof(filterAsync));

        if (source.IsSome)
        {
            return await filterAsync(source._value!)
                ? source
                : Maybe<T>.Create.None();
        }

        return source;
    }
}
