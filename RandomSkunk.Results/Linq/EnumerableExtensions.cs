using RandomSkunk.Results;

namespace System.Linq;

/// <summary>
/// Provides result extension methods for sequences.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Performs the specified result action on each element of the sequence.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the sequence will
    /// be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element of the sequence.</param>
    /// <returns>A <c>Success</c> result if all elements of the sequence produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static Result ForEach<T>(this IEnumerable<T> sourceSequence, Func<T, Result> action)
    {
        foreach (var item in sourceSequence)
        {
            var result = action(item);
            if (result.IsFail) return result;
        }

        return Result.Success();
    }

    /// <summary>
    /// Performs the specified result action on each element and index of the sequence.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the sequence will
    /// be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element and index of the sequence.</param>
    /// <returns>A <c>Success</c> result if all elements of the sequence produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static Result ForEach<T>(this IEnumerable<T> sourceSequence, Func<T, int, Result> action)
    {
        var i = 0;
        foreach (var item in sourceSequence)
        {
            var result = action(item, i++);
            if (result.IsFail) return result;
        }

        return Result.Success();
    }

    /// <summary>
    /// Performs the specified result action on each element of the sequence.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the sequence will
    /// be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element of the sequence.</param>
    /// <returns>A <c>Success</c> result if all elements of the sequence produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result> ForEach<T>(this IEnumerable<T> sourceSequence, Func<T, Task<Result>> action)
    {
        foreach (var item in sourceSequence)
        {
            var result = await action(item).ConfigureAwait(ContinueOnCapturedContext);
            if (result.IsFail) return result;
        }

        return Result.Success();
    }

    /// <summary>
    /// Performs the specified result action on each element and index of the sequence.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the sequence will
    /// be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element and index of the sequence.</param>
    /// <returns>A <c>Success</c> result if all elements of the sequence produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result> ForEach<T>(this IEnumerable<T> sourceSequence, Func<T, int, Task<Result>> action)
    {
        var i = 0;
        foreach (var item in sourceSequence)
        {
            var result = await action(item, i++).ConfigureAwait(ContinueOnCapturedContext);
            if (result.IsFail) return result;
        }

        return Result.Success();
    }

    /// <summary>
    /// Performs the specified result action on each element of the sequence.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the sequence will
    /// be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element of the sequence.</param>
    /// <returns>A <c>Success</c> result if all elements of the sequence produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result> ForEach<T>(this Task<IEnumerable<T>> sourceSequence, Func<T, Result> action) =>
        (await sourceSequence.ConfigureAwait(ContinueOnCapturedContext)).ForEach(action);

    /// <summary>
    /// Performs the specified result action on each element and index of the sequence.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the sequence will
    /// be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element and index of the sequence.</param>
    /// <returns>A <c>Success</c> result if all elements of the sequence produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result> ForEach<T>(this Task<IEnumerable<T>> sourceSequence, Func<T, int, Result> action) =>
        (await sourceSequence.ConfigureAwait(ContinueOnCapturedContext)).ForEach(action);

    /// <summary>
    /// Performs the specified result action on each element of the sequence.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the sequence will
    /// be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element of the sequence.</param>
    /// <returns>A <c>Success</c> result if all elements of the sequence produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result> ForEach<T>(this Task<IEnumerable<T>> sourceSequence, Func<T, Task<Result>> action) =>
        await (await sourceSequence.ConfigureAwait(ContinueOnCapturedContext)).ForEach(action).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Performs the specified result action on each element and index of the sequence.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the sequence will
    /// be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceSequence"/>.</typeparam>
    /// <param name="sourceSequence">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element and index of the sequence.</param>
    /// <returns>A <c>Success</c> result if all elements of the sequence produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result> ForEach<T>(this Task<IEnumerable<T>> sourceSequence, Func<T, int, Task<Result>> action) =>
        await (await sourceSequence.ConfigureAwait(ContinueOnCapturedContext)).ForEach(action).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Returns a <c>Success</c> result of the first element of a sequence. If the sequence is empty, a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.NotFound"/> is returned. If the first element is <see langword="null"/>, a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
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
                return list[0].ToResultOrFailIfNull("The first element was null.");
        }
        else
        {
            using IEnumerator<T> enumerator = sourceSequence.GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current.ToResultOrFailIfNull("The first element was null.");
        }

        return SequenceContainsNoElements();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the first element of a sequence result. If the result is <c>Fail</c>, then a
    /// <c>Fail</c> result with the same error is returned. If the sequence is empty, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.NotFound"/> is returned. If the first element is <see langword="null"/>, a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the first element of.</param>
    /// <returns>A result representing the operation.</returns>
    public static Result<T> FirstOrFail<T, TEnumerable>(this Result<TEnumerable> sourceResult)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.FirstOrFail());

    /// <summary>
    /// Returns a <c>Success</c> result of the first element of a sequence result. If the result is <c>Fail</c>, then a
    /// <c>Fail</c> result with the same error is returned. If the sequence is empty, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.NotFound"/> is returned. If the first element is <see langword="null"/>, a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the first element of.</param>
    /// <returns>A result representing the operation.</returns>
    public static async Task<Result<T>> FirstOrFail<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).FirstOrFail<T, TEnumerable>();

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
    /// Returns a <c>Success</c> result of the first element of a sequence result. If the result is <c>Fail</c>, then a
    /// <c>Fail</c> result with the same error is returned. If the sequence is empty, a <c>None</c> result is returned. If the
    /// first element is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the first element of.</param>
    /// <returns>A result representing the operation.</returns>
    public static Result<T> FirstOrNone<T, TEnumerable>(this Result<TEnumerable> sourceResult)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.FirstOrNone());

    /// <summary>
    /// Returns a <c>Success</c> result of the first element of a sequence result. If the result is <c>Fail</c>, then a
    /// <c>Fail</c> result with the same error is returned. If the sequence is empty, a <c>None</c> result is returned. If the
    /// first element is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the first element of.</param>
    /// <returns>A result representing the operation.</returns>
    public static async Task<Result<T>> FirstOrNone<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).FirstOrNone<T, TEnumerable>();

    /// <summary>
    /// Returns a <c>Success</c> result of the first element of the sequence that satisfies a condition. If no element satisfies
    /// the condition, a <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If the first element
    /// that satisfies the condition is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
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
        {
            if (predicate(item))
                return item.ToResultOrFailIfNull("The first matching element was null.");
        }

        return SequenceContainsNoMatchingElements();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the first element of the sequence result that satisfies a condition. If the result is
    /// <c>Fail</c>, then a <c>Fail</c> result with the same error is returned. If no element satisfies the condition, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If the first element that satisfies the
    /// condition is <see langword="null"/>, a <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is
    /// returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the first element of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static Result<T> FirstOrFail<T, TEnumerable>(this Result<TEnumerable> sourceResult, Func<T, bool> predicate)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.FirstOrFail(predicate));

    /// <summary>
    /// Returns a <c>Success</c> result of the first element of the sequence result that satisfies a condition. If the result is
    /// <c>Fail</c>, then a <c>Fail</c> result with the same error is returned. If no element satisfies the condition, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If the first element that satisfies the
    /// condition is <see langword="null"/>, a <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is
    /// returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the first element of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> FirstOrFail<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult, Func<T, bool> predicate)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).FirstOrFail(predicate);

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
    /// Returns a <c>Success</c> result of the first element of the sequence result that satisfies a condition. If the result is
    /// <c>Fail</c>, then a <c>Fail</c> result with the same error is returned. If no element satisfies the condition, a
    /// <c>None</c> result is returned. If the first element that satisfies the condition is <see langword="null"/>, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the first element of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static Result<T> FirstOrNone<T, TEnumerable>(this Result<TEnumerable> sourceResult, Func<T, bool> predicate)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.FirstOrNone(predicate));

    /// <summary>
    /// Returns a <c>Success</c> result of the first element of the sequence result that satisfies a condition. If the result is
    /// <c>Fail</c>, then a <c>Fail</c> result with the same error is returned. If no element satisfies the condition, a
    /// <c>None</c> result is returned. If the first element that satisfies the condition is <see langword="null"/>, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the first element of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> FirstOrNone<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult, Func<T, bool> predicate)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).FirstOrNone(predicate);

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of a sequence. If the sequence is empty, a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.NotFound"/> is returned. If the last element is <see langword="null"/>, a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
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

        return SequenceContainsNoElements();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of a sequence result. If the result is <c>Fail</c>, then a
    /// <c>Fail</c> result with the same error is returned. If the sequence is empty, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.NotFound"/> is returned. If the last element is <see langword="null"/>, a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the last element of.</param>
    /// <returns>A result representing the operation.</returns>
    public static Result<T> LastOrFail<T, TEnumerable>(this Result<TEnumerable> sourceResult)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.LastOrFail());

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of a sequence result. If the result is <c>Fail</c>, then a
    /// <c>Fail</c> result with the same error is returned. If the sequence is empty, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.NotFound"/> is returned. If the last element is <see langword="null"/>, a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the last element of.</param>
    /// <returns>A result representing the operation.</returns>
    public static async Task<Result<T>> LastOrFail<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).LastOrFail<T, TEnumerable>();

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
    /// Returns a <c>Success</c> result of the last element of a sequence result. If the result is <c>Fail</c>, then a
    /// <c>Fail</c> result with the same error is returned. If the sequence is empty, a <c>None</c> result is returned. If the
    /// last element is <see langword="null"/>, a <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/>
    /// is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the last element of.</param>
    /// <returns>A result representing the operation.</returns>
    public static Result<T> LastOrNone<T, TEnumerable>(this Result<TEnumerable> sourceResult)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.LastOrNone());

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of a sequence result. If the result is <c>Fail</c>, then a
    /// <c>Fail</c> result with the same error is returned. If the sequence is empty, a <c>None</c> result is returned. If the
    /// last element is <see langword="null"/>, a <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/>
    /// is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the last element of.</param>
    /// <returns>A result representing the operation.</returns>
    public static async Task<Result<T>> LastOrNone<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).LastOrNone<T, TEnumerable>();

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of a sequence that satisfies a condition. If no element satisfies the
    /// condition, a <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If the last element that
    /// satisfies the condition is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
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

        Result<T> result = SequenceContainsNoMatchingElements();
        foreach (T item in sourceSequence)
            if (predicate(item)) result = item.ToResultOrFailIfNull("The last matching element was null.");

        return result;
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of the sequence result that satisfies a condition. If the result is
    /// <c>Fail</c>, then a <c>Fail</c> result with the same error is returned. If no element satisfies the condition, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If the last element that satisfies the
    /// condition is <see langword="null"/>, a <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is
    /// returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the last element of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static Result<T> LastOrFail<T, TEnumerable>(this Result<TEnumerable> sourceResult, Func<T, bool> predicate)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.LastOrFail(predicate));

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of the sequence result that satisfies a condition. If the result is
    /// <c>Fail</c>, then a <c>Fail</c> result with the same error is returned. If no element satisfies the condition, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If the last element that satisfies the
    /// condition is <see langword="null"/>, a <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is
    /// returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the last element of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> LastOrFail<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult, Func<T, bool> predicate)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).LastOrFail(predicate);

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
    /// Returns a <c>Success</c> result of the last element of the sequence result that satisfies a condition. If the result is
    /// <c>Fail</c>, then a <c>Fail</c> result with the same error is returned. If no element satisfies the condition, a
    /// <c>None</c> result is returned. If the last element that satisfies the condition is <see langword="null"/>, a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the last element of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static Result<T> LastOrNone<T, TEnumerable>(this Result<TEnumerable> sourceResult, Func<T, bool> predicate)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.LastOrNone(predicate));

    /// <summary>
    /// Returns a <c>Success</c> result of the last element of the sequence result that satisfies a condition. If the result is
    /// <c>Fail</c>, then a <c>Fail</c> result with the same error is returned. If no element satisfies the condition, a
    /// <c>None</c> result is returned. If the last element that satisfies the condition is <see langword="null"/>, a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the last element of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> LastOrNone<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult, Func<T, bool> predicate)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).LastOrNone(predicate);

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of a sequence. If the sequence is empty, a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.NotFound"/> is returned. If the sequence contains more than one element, a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.BadRequest"/> is returned. If the single element is <see langword="null"/>,
    /// a <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
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
            var count = list.Count;
            return count switch
            {
                0 => SequenceContainsNoElements(),
                1 => list[0].ToResultOrFailIfNull("The single element was null."),
                _ => SequenceContainsMoreThanOneElement(count),
            };
        }
        else
        {
            using IEnumerator<T> enumerator = sourceSequence.GetEnumerator();
            if (!enumerator.MoveNext())
                return SequenceContainsNoElements();

            T value = enumerator.Current;

            if (enumerator.MoveNext())
                return SequenceContainsMoreThanOneElement();

            return value.ToResultOrFailIfNull("The single element was null.");
        }
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of a sequence result. If the result is <c>Fail</c>, then a
    /// <c>Fail</c> result with the same error is returned. If the sequence is empty, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.NotFound"/> is returned. If the single element is <see langword="null"/>, a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the single element of.</param>
    /// <returns>A result representing the operation.</returns>
    public static Result<T> SingleOrFail<T, TEnumerable>(this Result<TEnumerable> sourceResult)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.SingleOrFail());

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of a sequence result. If the result is <c>Fail</c>, then a
    /// <c>Fail</c> result with the same error is returned. If the sequence is empty, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.NotFound"/> is returned. If the single element is <see langword="null"/>, a <c>Fail</c> result with
    /// error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the single element of.</param>
    /// <returns>A result representing the operation.</returns>
    public static async Task<Result<T>> SingleOrFail<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SingleOrFail<T, TEnumerable>();

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
    /// Returns a <c>Success</c> result of the only element of a sequence result. If the result is <c>Fail</c>, then a
    /// <c>Fail</c> result with the same error is returned. If the sequence is empty, a <c>None</c> result is returned. If the
    /// single element is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the single element of.</param>
    /// <returns>A result representing the operation.</returns>
    public static Result<T> SingleOrNone<T, TEnumerable>(this Result<TEnumerable> sourceResult)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.SingleOrNone());

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of a sequence result. If the result is <c>Fail</c>, then a
    /// <c>Fail</c> result with the same error is returned. If the sequence is empty, a <c>None</c> result is returned. If the
    /// single element is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the single element of.</param>
    /// <returns>A result representing the operation.</returns>
    public static async Task<Result<T>> SingleOrNone<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SingleOrNone<T, TEnumerable>();

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of a sequence that satisfies a specified condition. If no element
    /// satisfies the condition, a <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If more than
    /// one element satisfies the condition, a <c>Fail</c> result with error code <see cref="ErrorCodes.BadRequest"/> is
    /// returned. If the single element that satisfies the condition is <see langword="null"/>, a <c>Fail</c> result with error
    /// code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
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
            0 => SequenceContainsNoMatchingElements(),
            1 => result,
            _ => SequenceContainsMoreThanOneMatchingElement(count),
        };
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of the sequence result that satisfies a condition. If the result is
    /// <c>Fail</c>, then a <c>Fail</c> result with the same error is returned. If no element satisfies the condition, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If the single element that satisfies
    /// the condition is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the single element of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static Result<T> SingleOrFail<T, TEnumerable>(this Result<TEnumerable> sourceResult, Func<T, bool> predicate)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.SingleOrFail(predicate));

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of the sequence result that satisfies a condition. If the result is
    /// <c>Fail</c>, then a <c>Fail</c> result with the same error is returned. If no element satisfies the condition, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If the single element that satisfies
    /// the condition is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the single element of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> SingleOrFail<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult, Func<T, bool> predicate)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SingleOrFail(predicate);

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
    /// Returns a <c>Success</c> result of the only element of the sequence result that satisfies a condition. If the result is
    /// <c>Fail</c>, then a <c>Fail</c> result with the same error is returned. If no element satisfies the condition, a
    /// <c>None</c> result is returned. If the single element that satisfies the condition is <see langword="null"/>, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the single element of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static Result<T> SingleOrNone<T, TEnumerable>(this Result<TEnumerable> sourceResult, Func<T, bool> predicate)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.SingleOrNone(predicate));

    /// <summary>
    /// Returns a <c>Success</c> result of the only element of the sequence result that satisfies a condition. If the result is
    /// <c>Fail</c>, then a <c>Fail</c> result with the same error is returned. If no element satisfies the condition, a
    /// <c>None</c> result is returned. If the single element that satisfies the condition is <see langword="null"/>, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the single element of.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A result representing the operation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> SingleOrNone<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult, Func<T, bool> predicate)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).SingleOrNone(predicate);

    /// <summary>
    /// Returns a <c>Success</c> result of the element at a specified index in a sequence. If the index is out of range, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.NotFound"/> is returned. If the element at the specified index
    /// is <see langword="null"/>, a <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
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

        return IndexOutOfRange();
    }

    /// <summary>
    /// Returns a <c>Success</c> result of the element at a specified index in a sequence result. If the result is <c>Fail</c>,
    /// then a <c>Fail</c> result with the same error is returned. If the index is out of range, a <c>Fail</c> result with error
    /// code <see cref="ErrorCodes.NotFound"/> is returned. If the element at the specified index is <see langword="null"/>, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>A result representing the operation.</returns>
    public static Result<T> ElementAtOrFail<T, TEnumerable>(this Result<TEnumerable> sourceResult, int index)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.ElementAtOrFail(index));

    /// <summary>
    /// Returns a <c>Success</c> result of the element at a specified index in a sequence result. If the result is <c>Fail</c>,
    /// then a <c>Fail</c> result with the same error is returned. If the index is out of range, a <c>Fail</c> result with error
    /// code <see cref="ErrorCodes.NotFound"/> is returned. If the element at the specified index is <see langword="null"/>, a
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>A result representing the operation.</returns>
    public static async Task<Result<T>> ElementAtOrFail<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult, int index)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ElementAtOrFail<T, TEnumerable>(index);

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

    /// <summary>
    /// Returns a <c>Success</c> result of the element at a specified index in a sequence result. If the result is <c>Fail</c>,
    /// then a <c>Fail</c> result with the same error is returned. If the index is out of range, a <c>None</c> result is
    /// returned. If the element at the specified index is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>A result representing the operation.</returns>
    public static Result<T> ElementAtOrNone<T, TEnumerable>(this Result<TEnumerable> sourceResult, int index)
        where TEnumerable : IEnumerable<T> =>
        sourceResult.SelectMany(sourceSequence => sourceSequence.ElementAtOrNone(index));

    /// <summary>
    /// Returns a <c>Success</c> result of the element at a specified index in a sequence result. If the result is <c>Fail</c>,
    /// then a <c>Fail</c> result with the same error is returned. If the index is out of range, a <c>None</c> result is
    /// returned. If the element at the specified index is <see langword="null"/>, a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.UnexpectedNullValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TEnumerable">The <see cref="IEnumerable{T}"/> type of <paramref name="sourceResult"/>.</typeparam>
    /// <param name="sourceResult">The <see cref="IEnumerable{T}"/> result to return the element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>A result representing the operation.</returns>
    public static async Task<Result<T>> ElementAtOrNone<T, TEnumerable>(this Task<Result<TEnumerable>> sourceResult, int index)
        where TEnumerable : IEnumerable<T> =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).ElementAtOrNone<T, TEnumerable>(index);

    private static Result<T> ToResultOrFailIfNull<T>(this T value, string errorMessageFormat, params object[] errorMessageArgs) =>
        value is not null
            ? value
            : Errors.UnexpectedNullValue(string.Format(errorMessageFormat, errorMessageArgs));

    private static Error IndexOutOfRange() =>
        Errors.NotFound("Index was out of range. Must be non-negative and less than the size of the collection.");

    private static Error SequenceContainsNoElements() =>
        Errors.NotFound("Sequence contains no elements.");

    private static Error SequenceContainsNoMatchingElements() =>
        Errors.NotFound("Sequence contains no matching elements.");

    private static Error SequenceContainsMoreThanOneElement(int? count = null) =>
        Errors.BadRequest(count is null
            ? "Sequence contains more than one element."
            : $"Sequence contains {count} elements, when it should contain exactly one.");

    private static Error SequenceContainsMoreThanOneMatchingElement(ulong count) =>
        Errors.BadRequest($"Sequence contains {count} matching elements, when it should contain exactly one.");
}
