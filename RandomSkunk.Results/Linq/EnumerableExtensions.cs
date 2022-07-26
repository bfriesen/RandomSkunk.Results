namespace RandomSkunk.Results.Linq;

/// <summary>
/// Provides extension methods for obtaining a single element from a sequence as a result object.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Returns a <c>Success</c> result of the first element of a sequence. If the sequence is empty, a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.NotFound"/> is returned. If the first element is <see langword="null"/>, a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.Gone"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> is <see langword="null"/>.</exception>
    public static Result<T> FirstOrFail<T>(this IEnumerable<T> sourceSequence)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));

        if (sourceSequence is IList<T> list)
        {
            if (list.Count > 0)
                return list[0].ToResultOrFailIfNull();
        }
        else
        {
            using IEnumerator<T> enumerator = sourceSequence.GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current.ToResultOrFailIfNull();
        }

        return Result<T>.Fail(SequenceContainsNoElements());
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the first element of a sequence. If the sequence is empty, a <c>None</c> result is
    /// returned. If the first element is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.Gone"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> is <see langword="null"/>.</exception>
    public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> sourceSequence)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));

        if (sourceSequence is IList<T> list)
        {
            if (list.Count > 0)
                return list[0].ToMaybeOrFailIfNull();
        }
        else
        {
            using IEnumerator<T> enumerator = sourceSequence.GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current.ToMaybeOrFailIfNull();
        }

        return Maybe<T>.None();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the first element of the sequence that satisfies a condition. If no element satisfies
    /// the condition, a <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If the first element
    /// that satisfies the condition is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.Gone"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> or <paramref name="predicate"/> is
    ///     <see langword="null"/>.</exception>
    public static Result<T> FirstOrFail<T>(this IEnumerable<T> sourceSequence, Func<T, bool> predicate)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        foreach (var item in sourceSequence)
            if (predicate(item)) return item.ToResultOrFailIfNull();

        return Result<T>.Fail(SequenceContainsNoMatchingElements());
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the first element of the sequence that satisfies a condition. If no element satisfies
    /// the condition, a <c>None</c> result is returned. If the first element that satisfies the condition is
    /// <see langword="null"/>, a <c>Fail</c> result with error code <see cref="ErrorCodes.Gone"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> or <paramref name="predicate"/> is
    ///     <see langword="null"/>.</exception>
    public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> sourceSequence, Func<T, bool> predicate)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        foreach (var item in sourceSequence)
            if (predicate(item)) return item.ToMaybeOrFailIfNull();

        return Maybe<T>.None();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of a sequence. If the sequence is empty, a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.NotFound"/> is returned. If the last element is <see langword="null"/>, a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.Gone"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return the last element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> is <see langword="null"/>.</exception>
    public static Result<T> LastOrFail<T>(this IEnumerable<T> sourceSequence)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));

        if (sourceSequence is IList<T> list)
        {
            int count = list.Count;
            if (count > 0)
                return list[count - 1].ToResultOrFailIfNull();
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
                return current.ToResultOrFailIfNull();
            }
        }

        return Result<T>.Fail(SequenceContainsNoElements());
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of a sequence. If the sequence is empty, a <c>None</c> result is
    /// returned. If the last element is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.Gone"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return the last element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> is <see langword="null"/>.</exception>
    public static Maybe<T> LastOrNone<T>(this IEnumerable<T> sourceSequence)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));

        if (sourceSequence is IList<T> list)
        {
            int count = list.Count;
            if (count > 0)
                return list[count - 1].ToMaybeOrFailIfNull();
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
                return current.ToMaybeOrFailIfNull();
            }
        }

        return Maybe<T>.None();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of a sequence that satisfies a condition. If no element satisfies the
    /// condition, a <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If the last element that
    /// satisfies the condition is <see langword="null"/>, a <c>Fail</c> result with error code <see cref="ErrorCodes.Gone"/> is
    /// returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> or <paramref name="predicate"/> is
    ///     <see langword="null"/>.</exception>
    public static Result<T> LastOrFail<T>(this IEnumerable<T> sourceSequence, Func<T, bool> predicate)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        var result = Result<T>.Fail(SequenceContainsNoMatchingElements());
        foreach (T item in sourceSequence)
            if (predicate(item)) result = item.ToResultOrFailIfNull();

        return result;
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of a sequence that satisfies a condition. If no element satisfies the
    /// condition, a <c>None</c> result is returned. If the last element that satisfies the condition is <see langword="null"/>,
    /// a <c>Fail</c> result with error code <see cref="ErrorCodes.Gone"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> or <paramref name="predicate"/> is
    ///     <see langword="null"/>.</exception>
    public static Maybe<T> LastOrNone<T>(this IEnumerable<T> sourceSequence, Func<T, bool> predicate)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        var result = Maybe<T>.None();
        foreach (T item in sourceSequence)
            if (predicate(item)) result = item.ToMaybeOrFailIfNull();

        return result;
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of a sequence. If the sequence is empty, a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.NotFound"/> is returned. If the sequence contains more than one element, a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.BadRequest"/> is returned. If the single element is <see langword="null"/>,
    /// a <c>Fail</c> result with error code <see cref="ErrorCodes.Gone"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return the single element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> is <see langword="null"/>.</exception>
    public static Result<T> SingleOrFail<T>(this IEnumerable<T> sourceSequence)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));

        if (sourceSequence is IList<T> list)
        {
            switch (list.Count)
            {
                case 0: return Result<T>.Fail(SequenceContainsNoElements());
                case 1: return list[0].ToResultOrFailIfNull();
            }
        }
        else
        {
            using IEnumerator<T> enumerator = sourceSequence.GetEnumerator();
            if (!enumerator.MoveNext())
                return Result<T>.Fail(SequenceContainsNoElements());

            T current = enumerator.Current;
            if (!enumerator.MoveNext())
                return current.ToResultOrFailIfNull();
        }

        return Result<T>.Fail(SequenceContainsMoreThanOneElement());
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of a sequence. If the sequence is empty, a <c>None</c> result is
    /// returned. If the sequence contains more than one element, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.BadRequest"/> is returned. If the single element is <see langword="null"/>, a <c>Fail</c> result
    /// with error code <see cref="ErrorCodes.Gone"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return the single element of.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> is <see langword="null"/>.</exception>
    public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> sourceSequence)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));

        if (sourceSequence is IList<T> list)
        {
            switch (list.Count)
            {
                case 0: return Maybe<T>.None();
                case 1: return list[0].ToMaybeOrFailIfNull();
            }
        }
        else
        {
            using IEnumerator<T> enumerator = sourceSequence.GetEnumerator();
            if (!enumerator.MoveNext())
                return Maybe<T>.None();

            T current = enumerator.Current;
            if (!enumerator.MoveNext())
                return current.ToMaybeOrFailIfNull();
        }

        return Maybe<T>.Fail(SequenceContainsMoreThanOneElement());
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of a sequence that satisfies a specified condition. If no element
    /// satisfies the condition, a <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If more than
    /// one element satisfies the condition, a <c>Fail</c> result with error code <see cref="ErrorCodes.BadRequest"/> is
    /// returned. If the single element that satisfies the condition is <see langword="null"/>, a <c>Fail</c> result with error
    /// code <see cref="ErrorCodes.Gone"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return the single element of.</param>
    /// <param name="predicate">A function to test an element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> or <paramref name="predicate"/> is
    ///     <see langword="null"/>.</exception>
    public static Result<T> SingleOrFail<T>(this IEnumerable<T> sourceSequence, Func<T, bool> predicate)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        Result<T> result = default;
        long count = 0;
        foreach (var item in sourceSequence)
        {
            if (predicate(item))
            {
                result = item.ToResultOrFailIfNull();
                count = checked(count + 1);
            }
        }

        return count switch
        {
            0 => Result<T>.Fail(SequenceContainsNoMatchingElements()),
            1 => result,
            _ => Result<T>.Fail(SequenceContainsMoreThanOneMatchingElement()),
        };
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of a sequence that satisfies a specified condition. If no element
    /// satisfies the condition, a <c>None</c> result is returned. If more than one element satisfies the condition, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.BadRequest"/> is returned. If the single element that satisfies
    /// the condition is <see langword="null"/>, a <c>Fail</c> result with error code <see cref="ErrorCodes.Gone"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return a single element from.</param>
    /// <param name="predicate">A function to test an element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> or <paramref name="predicate"/> is
    ///     <see langword="null"/>.</exception>
    public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> sourceSequence, Func<T, bool> predicate)
    {
        if (sourceSequence is null) throw new ArgumentNullException(nameof(sourceSequence));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        Maybe<T> result = default;
        long count = 0;
        foreach (var item in sourceSequence)
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
            _ => Maybe<T>.Fail(SequenceContainsMoreThanOneMatchingElement()),
        };
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the element at a specified index in a sequence. If the index is out of range, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If the element at the specified index
    /// is <see langword="null"/>, a <c>Fail</c> result with error code <see cref="ErrorCodes.Gone"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> is <see langword="null"/>.</exception>
    public static Result<T> ElementAtOrFail<T>(this IEnumerable<T> sourceSequence, int index)
    {
        if (sourceSequence == null) throw new ArgumentNullException(nameof(sourceSequence));

        if (index >= 0)
        {
            if (sourceSequence is IList<T> list)
            {
                if (index < list.Count)
                    return list[index].ToResultOrFailIfNull();
            }
            else
            {
                using IEnumerator<T> enumerator = sourceSequence.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (index == 0)
                        return enumerator.Current.ToResultOrFailIfNull();

                    index--;
                }
            }
        }

        return Result<T>.Fail(IndexOutOfRange());
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the element at a specified index in a sequence. If the index is out of range, a
    /// <c>None</c> result is returned. If the element at the specified index is <see langword="null"/>, a <c>Fail</c> result
    /// with error code <see cref="ErrorCodes.Gone"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> to return an element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sourceSequence"/> is <see langword="null"/>.</exception>
    public static Maybe<T> ElementAtOrNone<T>(this IEnumerable<T> sourceSequence, int index)
    {
        if (sourceSequence == null) throw new ArgumentNullException(nameof(sourceSequence));

        if (index >= 0)
        {
            if (sourceSequence is IList<T> list)
            {
                if (index < list.Count)
                    return list[index].ToMaybeOrFailIfNull();
            }
            else
            {
                using IEnumerator<T> enumerator = sourceSequence.GetEnumerator();
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
        value.ToResult().WithError(error =>
            error with { ErrorCode = ErrorCodes.Gone, Message = "The matching element was null." });

    private static Maybe<T> ToMaybeOrFailIfNull<T>(this T value) =>
        value is not null
            ? value.ToMaybe()
            : Maybe<T>.Fail(Errors.Gone("The matching element was null."));

    [StackTraceHidden]
    private static Error IndexOutOfRange() =>
        Errors.NotFound("Index was out of range. Must be non-negative and less than the size of the collection.");

    [StackTraceHidden]
    private static Error SequenceContainsNoElements() =>
        Errors.NotFound("Sequence contains no elements.");

    [StackTraceHidden]
    private static Error SequenceContainsNoMatchingElements() =>
        Errors.NotFound("Sequence contains no matching elements.");

    [StackTraceHidden]
    private static Error SequenceContainsMoreThanOneElement() =>
        Errors.BadRequest("Sequence contains more than one element.");

    [StackTraceHidden]
    private static Error SequenceContainsMoreThanOneMatchingElement() =>
        Errors.BadRequest("Sequence contains more than one matching element.");
}
