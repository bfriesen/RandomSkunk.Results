namespace RandomSkunk.Results;

/// <summary>
/// Provides result extension methods for sequences.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Returns a <c>Success</c> result of the first element of a sequence. If the sequence is empty, a <c>None</c> result is
    /// returned. If the first element is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> is <see langword="null"/>.</exception>
    public static Result<T> FirstOrNone<T>(this IEnumerable<T> sourceSequence)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));

        if (sourceSequence is IList<T> list)
        {
            if (list.Count > 0)
                return list[0].ToResultOrFailIfNull("The first element was null.");
        }
        else
        {
            using IEnumerator<T> enumerator = sourceSequence.GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current.ToResultOrFailIfNull("The first element was null.");
        }

        return Errors.NoValue();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the first element of the sequence that satisfies a condition. If no element satisfies
    /// the condition, a <c>None</c> result is returned. If the first element that satisfies the condition is
    /// <see langword="null"/>, a <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> or <paramref name="predicate"/> is
    ///     <see langword="null"/>.</exception>
    public static Result<T> FirstOrNone<T>(this IEnumerable<T> sourceSequence, Func<T, bool> predicate)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        foreach (var item in sourceSequence)
        {
            if (predicate(item))
                return item.ToResultOrFailIfNull("The first matching element was null.");
        }

        return Errors.NoValue();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of a sequence. If the sequence is empty, a <c>None</c> result is
    /// returned. If the last element is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return the last element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> is <see langword="null"/>.</exception>
    public static Result<T> LastOrNone<T>(this IEnumerable<T> sourceSequence)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));

        if (sourceSequence is IList<T> list)
        {
            int count = list.Count;
            if (count > 0)
                return list[count - 1].ToResultOrFailIfNull("The last element was null.");
        }
        else
        {
            using IEnumerator<T> enumerator = sourceSequence.GetEnumerator();
            if (enumerator.MoveNext())
            {
                T current;
                do
                {
                    current = enumerator.Current;
                } while (enumerator.MoveNext());
                return current.ToResultOrFailIfNull("The last element was null.");
            }
        }

        return Errors.NoValue();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of a sequence that satisfies a condition. If no element satisfies the
    /// condition, a <c>None</c> result is returned. If the last element that satisfies the condition is <see langword="null"/>,
    /// a <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> or <paramref name="predicate"/> is
    ///     <see langword="null"/>.</exception>
    public static Result<T> LastOrNone<T>(this IEnumerable<T> sourceSequence, Func<T, bool> predicate)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        Result<T> result = Errors.NoValue();
        foreach (T item in sourceSequence)
            if (predicate(item)) result = item.ToResultOrFailIfNull("The last matching element was null.");

        return result;
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of a sequence. If the sequence is empty, a <c>None</c> result is
    /// returned. If the sequence contains more than one element, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.BadRequest"/> is returned. If the single element is <see langword="null"/>, a <c>Fail</c> result
    /// with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return the single element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> is <see langword="null"/>.</exception>
    public static Result<T> SingleOrNone<T>(this IEnumerable<T> sourceSequence)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));

        if (sourceSequence is IList<T> list)
        {
            var count = list.Count;
            return count switch
            {
                0 => Errors.NoValue(),
                1 => list[0].ToResultOrFailIfNull("The single element was null."),
                _ => SequenceContainsMoreThanOneElement(count),
            };
        }
        else
        {
            using IEnumerator<T> enumerator = sourceSequence.GetEnumerator();
            if (!enumerator.MoveNext())
                return Errors.NoValue();

            T current = enumerator.Current;
            if (!enumerator.MoveNext())
                return current.ToResultOrFailIfNull("The single element was null.");
        }

        return SequenceContainsMoreThanOneElement();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of a sequence that satisfies a specified condition. If no element
    /// satisfies the condition, a <c>None</c> result is returned. If more than one element satisfies the condition, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.BadRequest"/> is returned. If the single element that satisfies
    /// the condition is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return a single element from.</param>
    /// <param name="predicate">A function to test an element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> or <paramref name="predicate"/> is
    ///     <see langword="null"/>.</exception>
    public static Result<T> SingleOrNone<T>(this IEnumerable<T> sourceSequence, Func<T, bool> predicate)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        Result<T> result = default;
        ulong count = 0;
        foreach (var item in sourceSequence)
        {
            if (predicate(item))
            {
                result = item.ToResultOrFailIfNull("The single matching element was null.");
                count = checked(count + 1);
            }
        }

        return count switch
        {
            0 => Errors.NoValue(),
            1 => result,
            _ => SequenceContainsMoreThanOneMatchingElement(count),
        };
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the element at a specified index in a sequence. If the index is out of range, a
    /// <c>None</c> result is returned. If the element at the specified index is <see langword="null"/>, a <c>Fail</c> result
    /// with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> is <see langword="null"/>.</exception>
    public static Result<T> ElementAtOrNone<T>(this IEnumerable<T> sourceSequence, int index)
    {
        if (sourceSequence == null) throw new ArgumentNullException(nameof(sourceSequence));

        if (index >= 0)
        {
            if (sourceSequence is IList<T> list)
            {
                if (index < list.Count)
                    return list[index].ToResultOrFailIfNull("The element at index {0} was null.", index);
            }
            else
            {
                using IEnumerator<T> enumerator = sourceSequence.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (index == 0)
                        return enumerator.Current.ToResultOrFailIfNull("The element at index {0} was null.", index);

                    index--;
                }
            }
        }

        return Errors.NoValue();
    }

    private static Result<T> ToResultOrFailIfNull<T>(this T value, string errorMessageFormat, params object[] errorMessageArgs) =>
        value is not null
            ? value
            : Errors.UnexpectedNullValue(string.Format(errorMessageFormat, errorMessageArgs));

    private static Error SequenceContainsMoreThanOneElement(int? count = null) =>
        Errors.BadRequest(count is null
            ? "Sequence contains more than one element."
            : $"Sequence contains {count} elements, when it should contain exactly one.");

    private static Error SequenceContainsMoreThanOneMatchingElement(ulong count) =>
        Errors.BadRequest($"Sequence contains {count} matching elements, when it should contain exactly one.");
}