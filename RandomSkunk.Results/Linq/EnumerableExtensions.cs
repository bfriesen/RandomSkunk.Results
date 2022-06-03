namespace RandomSkunk.Results.Linq;

/// <summary>
/// Provides extension methods for obtaining a single element from a sequence as a result object.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Returns a <c>Success</c> result of the first element of a sequence; a <c>Fail</c> result with error code <c>404</c> if
    /// the sequence contains no elements; or a <c>Fail</c> result with error code <c>410</c> if the first element is
    /// <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Result<T> FirstOrFail<T>(this IEnumerable<T> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        if (source is IList<T> list)
        {
            if (list.Count > 0)
                return list[0].ToResultOrFailIfNull();
        }
        else
        {
            using IEnumerator<T> enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current.ToResultOrFailIfNull();
        }

        return Result<T>.FailWith.SequenceContainsNoElements();
    }

    /// <summary>
    /// Returns a <c>Some</c> result of the first element of a sequence; a <c>None</c> result if the sequence contains no
    /// elements; or a <c>Fail</c> result with error code <c>410</c> if the first element is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        if (source is IList<T> list)
        {
            if (list.Count > 0)
                return list[0].ToMaybeOrFailIfNull();
        }
        else
        {
            using IEnumerator<T> enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current.ToMaybeOrFailIfNull();
        }

        return Maybe<T>.None();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the first element of the sequence that satisfies a condition; a <c>Fail</c> result
    /// with error code <c>404</c> if no such element is found; or a <c>Fail</c> result with error code <c>410</c> if the first
    /// matched element is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> FirstOrFail<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        foreach (var item in source)
            if (predicate(item)) return item.ToResultOrFailIfNull();

        return Result<T>.FailWith.SequenceContainsNoMatchingElements();
    }

    /// <summary>
    /// Returns a <c>Some</c> result of the first element of the sequence that satisfies a condition; a <c>None</c> result if no
    /// such element is found; or a <c>Fail</c> result with error code <c>410</c> if the first matched element is
    /// <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        foreach (var item in source)
            if (predicate(item)) return item.ToMaybeOrFailIfNull();

        return Maybe<T>.None();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of a sequence; <c>Fail</c> result with error code <c>404</c> if the
    /// sequence contains no elements; or a <c>Fail</c> result with error code <c>410</c> if the last element is
    /// <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to return the last element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Result<T> LastOrFail<T>(this IEnumerable<T> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        if (source is IList<T> list)
        {
            int count = list.Count;
            if (count > 0)
                return list[count - 1].ToResultOrFailIfNull();
        }
        else
        {
            using IEnumerator<T> enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
            {
                T current;
                do
                {
                    current = enumerator.Current;
                } while (enumerator.MoveNext());
                return current.ToResultOrFailIfNull();
            }
        }

        return Result<T>.FailWith.SequenceContainsNoElements();
    }

    /// <summary>
    /// Returns a <c>Some</c> result of the last element of a sequence; <c>None</c> result if the sequence contains no elements;
    /// or a <c>Fail</c> result with error code <c>410</c> if the last element is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to return the last element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Maybe<T> LastOrNone<T>(this IEnumerable<T> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        if (source is IList<T> list)
        {
            int count = list.Count;
            if (count > 0)
                return list[count - 1].ToMaybeOrFailIfNull();
        }
        else
        {
            using IEnumerator<T> enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
            {
                T current;
                do
                {
                    current = enumerator.Current;
                } while (enumerator.MoveNext());
                return current.ToMaybeOrFailIfNull();
            }
        }

        return Maybe<T>.None();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of a sequence that satisfies a condition; a <c>Fail</c> result with
    /// error code <c>404</c> if no such element is found; or a <c>Fail</c> result with error code <c>410</c> if the last matched
    /// element is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> LastOrFail<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        var result = Result<T>.FailWith.SequenceContainsNoMatchingElements();
        foreach (T item in source)
            if (predicate(item)) result = item.ToResultOrFailIfNull();

        return result;
    }

    /// <summary>
    /// Returns a <c>Some</c> result of the last element of a sequence that satisfies a condition; a <c>None</c> result if no
    /// such element is found; or a <c>Fail</c> result with error code <c>410</c> if the last matched element is
    /// <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<T> LastOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        var result = Maybe<T>.None();
        foreach (T item in source)
            if (predicate(item)) result = item.ToMaybeOrFailIfNull();

        return result;
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of a sequence; a <c>Fail</c> result with error code <c>404</c> if the
    /// sequence is empty or the single element is <see langword="null"/>; a <c>Fail</c> result with error code <c>400</c> if the
    /// sequence contains more than one element; or a <c>Fail</c> result with error code <c>410</c> if the single element is
    /// <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to return the single element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Result<T> SingleOrFail<T>(this IEnumerable<T> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        if (source is IList<T> list)
        {
            switch (list.Count)
            {
                case 0: return Result<T>.FailWith.SequenceContainsNoElements();
                case 1: return list[0].ToResultOrFailIfNull();
            }
        }
        else
        {
            using IEnumerator<T> enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                return Result<T>.FailWith.SequenceContainsNoElements();

            T current = enumerator.Current;
            if (!enumerator.MoveNext())
                return current.ToResultOrFailIfNull();
        }

        return Result<T>.FailWith.SequenceContainsMoreThanOneElement();
    }

    /// <summary>
    /// Returns a <c>Some</c> result of the only element of a sequence; a <c>None</c> result if the sequence is empty or the
    /// single element is <see langword="null"/>; a <c>Fail</c> result with error code <c>400</c> if the sequence contains more
    /// than one element; or a <c>Fail</c> result with error code <c>410</c> if the single element is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to return the single element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        if (source is IList<T> list)
        {
            switch (list.Count)
            {
                case 0: return Maybe<T>.None();
                case 1: return list[0].ToMaybeOrFailIfNull();
            }
        }
        else
        {
            using IEnumerator<T> enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                return Maybe<T>.None();

            T current = enumerator.Current;
            if (!enumerator.MoveNext())
                return current.ToMaybeOrFailIfNull();
        }

        return Maybe<T>.FailWith.SequenceContainsMoreThanOneElement();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of a sequence that satisfies a specified condition; a <c>Fail</c>
    /// result with error code <c>404</c> if no such element exists; a <c>Fail</c> result with error code <c>400</c> if more than
    /// one element satisfies the condition; or a <c>Fail</c> result with error code <c>410</c> if the single matched element is
    /// <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to return the single element of.</param>
    /// <param name="predicate">A function to test an element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> SingleOrFail<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        Result<T> result = default;
        long count = 0;
        foreach (var item in source)
        {
            if (predicate(item))
            {
                result = item.ToResultOrFailIfNull();
                count = checked(count + 1);
            }
        }

        return count switch
        {
            0 => Result<T>.FailWith.SequenceContainsNoMatchingElements(),
            1 => result,
            _ => Result<T>.FailWith.SequenceContainsMoreThanOneMatchingElement(),
        };
    }

    /// <summary>
    /// Returns a <c>Some</c> result of the only element of a sequence that satisfies a specified condition; a <c>None</c> result
    /// if no such element exists; a <c>Fail</c> result with error code <c>400</c> if more than one element satisfies the
    /// condition; or a <c>Fail</c> result with error code <c>410</c> if the single matched element is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to return a single element from.</param>
    /// <param name="predicate">A function to test an element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        Maybe<T> result = default;
        long count = 0;
        foreach (var item in source)
        {
            if (predicate(item))
            {
                result = item.ToMaybeOrFailIfNull();
                count = checked(count + 1);
            }
        }

        return count switch
        {
            0 => Maybe<T>.None(),
            1 => result,
            _ => Maybe<T>.FailWith.SequenceContainsMoreThanOneMatchingElement(),
        };
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the element at a specified index in a sequence; a <c>Fail</c> result with error code
    /// <c>404</c> if the index is out of range; or a <c>Fail</c> result with error code <c>410</c> if the matched element is
    /// <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Result<T> ElementAtOrFail<T>(this IEnumerable<T> source, int index)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        if (index >= 0)
        {
            if (source is IList<T> list)
            {
                if (index < list.Count)
                    return list[index].ToResultOrFailIfNull();
            }
            else
            {
                using IEnumerator<T> enumerator = source.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (index == 0)
                        return enumerator.Current.ToResultOrFailIfNull();

                    index--;
                }
            }
        }

        return Result<T>.FailWith.IndexOutOfRange();
    }

    /// <summary>
    /// Returns a <c>Some</c> result of the element at a specified index in a sequence; a <c>None</c> result if the index is out
    /// of range; or a <c>Fail</c> result with error code <c>410</c> if the matched element is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
    public static Maybe<T> ElementAtOrNone<T>(this IEnumerable<T> source, int index)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        if (index >= 0)
        {
            if (source is IList<T> list)
            {
                if (index < list.Count)
                    return list[index].ToMaybeOrFailIfNull();
            }
            else
            {
                using IEnumerator<T> enumerator = source.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (index == 0)
                        return enumerator.Current.ToMaybeOrFailIfNull();

                    index--;
                }
            }
        }

        return Maybe<T>.None();
    }

    private static Result<T> ToResultOrFailIfNull<T>(this T value) =>
        value.ToResult("The matching element was null.", 410);

    private static Maybe<T> ToMaybeOrFailIfNull<T>(this T value) =>
        value is not null
            ? value.ToMaybe()
            : Maybe<T>.Fail("The matching element was null.", new StackTrace().ToString(), 410);

    private static Result<T> IndexOutOfRange<T>(this ResultFactory<T> source) =>
        source.Error("Index was out of range. Must be non-negative and less than the size of the collection.", new StackTrace(1).ToString(), 404);

    private static Result<T> SequenceContainsNoElements<T>(this ResultFactory<T> source) =>
        source.Error("Sequence contains no elements.", new StackTrace(1).ToString(), 404);

    private static Result<T> SequenceContainsNoMatchingElements<T>(this ResultFactory<T> source) =>
        source.Error("Sequence contains no matching elements.", new StackTrace(1).ToString(), 404);

    private static Result<T> SequenceContainsMoreThanOneElement<T>(this ResultFactory<T> source) =>
        source.Error("Sequence contains more than one element.", new StackTrace(1).ToString(), 400);

    private static Maybe<T> SequenceContainsMoreThanOneElement<T>(this MaybeFactory<T> source) =>
        source.Error("Sequence contains more than one element.", new StackTrace(1).ToString(), 400);

    private static Result<T> SequenceContainsMoreThanOneMatchingElement<T>(this ResultFactory<T> source) =>
        source.Error("Sequence contains more than one matching element.", new StackTrace(1).ToString(), 400);

    private static Maybe<T> SequenceContainsMoreThanOneMatchingElement<T>(this MaybeFactory<T> source) =>
        source.Error("Sequence contains more than one matching element.", new StackTrace(1).ToString(), 400);
}
