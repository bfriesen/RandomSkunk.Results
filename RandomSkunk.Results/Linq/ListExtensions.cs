using RandomSkunk.Results;

namespace System.Linq;

/// <summary>
/// Provides result extension methods for lists.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Performs the specified result action on each element of the list.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the list will be
    /// evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceList"/>.</typeparam>
    /// <param name="sourceList">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element of the list.</param>
    /// <returns>A <c>Success</c> result if all elements of the list produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static Result<IReadOnlyList<T>> ForEach<T>(this IReadOnlyList<T> sourceList, Func<T, Result> action)
    {
        foreach (var item in sourceList)
        {
            var result = action(item);
            if (result.TryGetError(out var error))
                return Result<IReadOnlyList<T>>.Fail(error);
        }

        return Result<IReadOnlyList<T>>.Success(sourceList);
    }

    /// <summary>
    /// Performs the specified result action on each element and index of the list.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the list will be
    /// evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceList"/>.</typeparam>
    /// <param name="sourceList">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element and index of the list.</param>
    /// <returns>A <c>Success</c> result if all elements of the list produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static Result<IReadOnlyList<T>> ForEach<T>(this IReadOnlyList<T> sourceList, Func<T, int, Result> action)
    {
        for (int i = 0; i < sourceList.Count; i++)
        {
            var result = action(sourceList[i], i);
            if (result.TryGetError(out var error))
                return Result<IReadOnlyList<T>>.Fail(error);
        }

        return Result<IReadOnlyList<T>>.Success(sourceList);
    }

    /// <summary>
    /// Performs the specified result action on each element of the list.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the list will be
    /// evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceList"/>.</typeparam>
    /// <param name="sourceList">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element of the list.</param>
    /// <returns>A <c>Success</c> result if all elements of the list produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result<IReadOnlyList<T>>> ForEach<T>(this IReadOnlyList<T> sourceList, Func<T, Task<Result>> action)
    {
        foreach (var item in sourceList)
        {
            var result = await action(item).ConfigureAwait(ContinueOnCapturedContext);
            if (result.TryGetError(out var error))
                return Result<IReadOnlyList<T>>.Fail(error);
        }

        return Result<IReadOnlyList<T>>.Success(sourceList);
    }

    /// <summary>
    /// Performs the specified result action on each element and index of the list.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the list will be
    /// evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceList"/>.</typeparam>
    /// <param name="sourceList">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element and index of the list.</param>
    /// <returns>A <c>Success</c> result if all elements of the list produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result<IReadOnlyList<T>>> ForEach<T>(this IReadOnlyList<T> sourceList, Func<T, int, Task<Result>> action)
    {
        for (int i = 0; i < sourceList.Count; i++)
        {
            var result = await action(sourceList[i], i).ConfigureAwait(ContinueOnCapturedContext);
            if (result.TryGetError(out var error))
                return Result<IReadOnlyList<T>>.Fail(error);
        }

        return Result<IReadOnlyList<T>>.Success(sourceList);
    }

    /// <summary>
    /// Performs the specified result action on each element of the list.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the list will be
    /// evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceList"/>.</typeparam>
    /// <param name="sourceList">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element of the list.</param>
    /// <returns>A <c>Success</c> result if all elements of the list produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result<IReadOnlyList<T>>> ForEach<T>(this Task<IReadOnlyList<T>> sourceList, Func<T, Result> action) =>
        (await sourceList.ConfigureAwait(ContinueOnCapturedContext)).ForEach(action);

    /// <summary>
    /// Performs the specified result action on each element and index of the list.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the list will be
    /// evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceList"/>.</typeparam>
    /// <param name="sourceList">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element and index of the list.</param>
    /// <returns>A <c>Success</c> result if all elements of the list produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result<IReadOnlyList<T>>> ForEach<T>(this Task<IReadOnlyList<T>> sourceList, Func<T, int, Result> action) =>
        (await sourceList.ConfigureAwait(ContinueOnCapturedContext)).ForEach(action);

    /// <summary>
    /// Performs the specified result action on each element of the list.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the list will be
    /// evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceList"/>.</typeparam>
    /// <param name="sourceList">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element of the list.</param>
    /// <returns>A <c>Success</c> result if all elements of the list produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result<IReadOnlyList<T>>> ForEach<T>(this Task<IReadOnlyList<T>> sourceList, Func<T, Task<Result>> action) =>
        await (await sourceList.ConfigureAwait(ContinueOnCapturedContext)).ForEach(action).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Performs the specified result action on each element and index of the list.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the list will be
    /// evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is returned
    /// immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceList"/>.</typeparam>
    /// <param name="sourceList">An <see cref="IEnumerable{T}"/> containing the elements to perform the action on.</param>
    /// <param name="action">The action to perform on each element and index of the list.</param>
    /// <returns>A <c>Success</c> result if all elements of the list produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result<IReadOnlyList<T>>> ForEach<T>(this Task<IReadOnlyList<T>> sourceList, Func<T, int, Task<Result>> action) =>
        await (await sourceList.ConfigureAwait(ContinueOnCapturedContext)).ForEach(action).ConfigureAwait(ContinueOnCapturedContext);
}
