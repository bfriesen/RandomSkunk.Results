namespace RandomSkunk.Results;

/// <summary>
/// Provides result extension methods for collections.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Performs the specified result action on each element of the collection.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the collection
    /// will be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is
    /// returned immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceCollection"/>.</typeparam>
    /// <param name="sourceCollection">An <see cref="IReadOnlyCollection{T}"/> containing the elements to perform the action on.
    ///     </param>
    /// <param name="action">The action to perform on each element of the collection.</param>
    /// <returns>A <c>Success</c> result if all elements of the collection produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static Result<IReadOnlyCollection<T>> TryForEach<T>(this IReadOnlyCollection<T> sourceCollection, Func<T, Result> action)
    {
        foreach (var item in sourceCollection)
        {
            var result = action(item);
            if (result.TryGetError(out var error))
                return error;
        }

        return Result<IReadOnlyCollection<T>>.Success(sourceCollection);
    }

    /// <summary>
    /// Performs the specified result action on each element and index of the collection.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the collection
    /// will be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is
    /// returned immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceCollection"/>.</typeparam>
    /// <param name="sourceCollection">An <see cref="IReadOnlyCollection{T}"/> containing the elements to perform the action on.
    ///     </param>
    /// <param name="action">The action to perform on each element and index of the collection.</param>
    /// <returns>A <c>Success</c> result if all elements of the collection produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static Result<IReadOnlyCollection<T>> TryForEach<T>(this IReadOnlyCollection<T> sourceCollection, Func<T, int, Result> action)
    {
        var i = 0;
        foreach (var item in sourceCollection)
        {
            var result = action(item, i++);
            if (result.TryGetError(out var error))
                return error;
        }

        return Result<IReadOnlyCollection<T>>.Success(sourceCollection);
    }

    /// <summary>
    /// Performs the specified result action on each element of the collection.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the collection
    /// will be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is
    /// returned immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceCollection"/>.</typeparam>
    /// <param name="sourceCollection">An <see cref="IReadOnlyCollection{T}"/> containing the elements to perform the action on.
    ///     </param>
    /// <param name="action">The action to perform on each element of the collection.</param>
    /// <returns>A <c>Success</c> result if all elements of the collection produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result<IReadOnlyCollection<T>>> TryForEach<T>(this IReadOnlyCollection<T> sourceCollection, Func<T, Task<Result>> action)
    {
        foreach (var item in sourceCollection)
        {
            var result = await action(item).ConfigureAwait(ContinueOnCapturedContext);
            if (result.TryGetError(out var error))
                return error;
        }

        return Result<IReadOnlyCollection<T>>.Success(sourceCollection);
    }

    /// <summary>
    /// Performs the specified result action on each element and index of the collection.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the collection
    /// will be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is
    /// returned immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceCollection"/>.</typeparam>
    /// <param name="sourceCollection">An <see cref="IReadOnlyCollection{T}"/> containing the elements to perform the action on.
    ///     </param>
    /// <param name="action">The action to perform on each element and index of the collection.</param>
    /// <returns>A <c>Success</c> result if all elements of the collection produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result<IReadOnlyCollection<T>>> TryForEach<T>(this IReadOnlyCollection<T> sourceCollection, Func<T, int, Task<Result>> action)
    {
        var i = 0;
        foreach (var item in sourceCollection)
        {
            var result = await action(item, i++).ConfigureAwait(ContinueOnCapturedContext);
            if (result.TryGetError(out var error))
                return error;
        }

        return Result<IReadOnlyCollection<T>>.Success(sourceCollection);
    }

    /// <summary>
    /// Performs the specified result action on each element of the collection.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the collection
    /// will be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is
    /// returned immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceCollection"/>.</typeparam>
    /// <param name="sourceCollection">An <see cref="IReadOnlyCollection{T}"/> containing the elements to perform the action on.
    ///     </param>
    /// <param name="action">The action to perform on each element of the collection.</param>
    /// <returns>A <c>Success</c> result if all elements of the collection produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result<IReadOnlyCollection<T>>> TryForEach<T>(this Task<IReadOnlyCollection<T>> sourceCollection, Func<T, Result> action) =>
        (await sourceCollection.ConfigureAwait(ContinueOnCapturedContext)).TryForEach(action);

    /// <summary>
    /// Performs the specified result action on each element and index of the collection.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the collection
    /// will be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is
    /// returned immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceCollection"/>.</typeparam>
    /// <param name="sourceCollection">An <see cref="IReadOnlyCollection{T}"/> containing the elements to perform the action on.
    ///     </param>
    /// <param name="action">The action to perform on each element and index of the collection.</param>
    /// <returns>A <c>Success</c> result if all elements of the collection produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result<IReadOnlyCollection<T>>> TryForEach<T>(this Task<IReadOnlyCollection<T>> sourceCollection, Func<T, int, Result> action) =>
        (await sourceCollection.ConfigureAwait(ContinueOnCapturedContext)).TryForEach(action);

    /// <summary>
    /// Performs the specified result action on each element of the collection.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the collection
    /// will be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is
    /// returned immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceCollection"/>.</typeparam>
    /// <param name="sourceCollection">An <see cref="IReadOnlyCollection{T}"/> containing the elements to perform the action on.
    ///     </param>
    /// <param name="action">The action to perform on each element of the collection.</param>
    /// <returns>A <c>Success</c> result if all elements of the collection produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result<IReadOnlyCollection<T>>> TryForEach<T>(this Task<IReadOnlyCollection<T>> sourceCollection, Func<T, Task<Result>> action) =>
        await (await sourceCollection.ConfigureAwait(ContinueOnCapturedContext)).TryForEach(action).ConfigureAwait(ContinueOnCapturedContext);

    /// <summary>
    /// Performs the specified result action on each element and index of the collection.
    /// <para/>
    /// The result returned by the <paramref name="action"/> delegate determines whether subsequent elements in the collection
    /// will be evaluated: each <c>Success</c> allows the next element to be evaluated, while the first <c>Fail</c> result is
    /// returned immediately.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="sourceCollection"/>.</typeparam>
    /// <param name="sourceCollection">An <see cref="IReadOnlyCollection{T}"/> containing the elements to perform the action on.
    ///     </param>
    /// <param name="action">The action to perform on each element and index of the collection.</param>
    /// <returns>A <c>Success</c> result if all elements of the collection produce a <c>Success</c> result; otherwise, the first
    ///     <c>Fail</c> result produced by an element.</returns>
    public static async Task<Result<IReadOnlyCollection<T>>> TryForEach<T>(this Task<IReadOnlyCollection<T>> sourceCollection, Func<T, int, Task<Result>> action) =>
        await (await sourceCollection.ConfigureAwait(ContinueOnCapturedContext)).TryForEach(action).ConfigureAwait(ContinueOnCapturedContext);
}
