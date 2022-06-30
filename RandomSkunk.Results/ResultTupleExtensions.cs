using System.Linq;

namespace RandomSkunk.Results;

#pragma warning disable SA1414 // Tuple types in signatures should have element names

/// <summary>
/// Defines extension methods for value tuples of results.
/// </summary>
public static class ResultTupleExtensions
{
    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult<T1>, IResult<T2>) OnAllSuccess<T1, T2>(
        this (IResult<T1>, IResult<T2>) results,
        Action<T1, T2> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult<T1>, IResult<T2>, IResult<T3>) OnAllSuccess<T1, T2, T3>(
        this (IResult<T1>, IResult<T2>, IResult<T3>) results,
        Action<T1, T2, T3> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>) OnAllSuccess<T1, T2, T3, T4>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>) results,
        Action<T1, T2, T3, T4> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>) OnAllSuccess<T1, T2, T3, T4, T5>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>) results,
        Action<T1, T2, T3, T4, T5> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="T6">The type of the sixth result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>) OnAllSuccess<T1, T2, T3, T4, T5, T6>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>) results,
        Action<T1, T2, T3, T4, T5, T6> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="T6">The type of the sixth result.</typeparam>
    /// <typeparam name="T7">The type of the seventh result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>, IResult<T7>) OnAllSuccess<T1, T2, T3, T4, T5, T6, T7>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>, IResult<T7>) results,
        Action<T1, T2, T3, T4, T5, T6, T7> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="T6">The type of the sixth result.</typeparam>
    /// <typeparam name="T7">The type of the seventh result.</typeparam>
    /// <typeparam name="T8">The type of the eighth result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>, IResult<T7>, IResult<T8>) OnAllSuccess<T1, T2, T3, T4, T5, T6, T7, T8>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>, IResult<T7>, IResult<T8>) results,
        Action<T1, T2, T3, T4, T5, T6, T7, T8> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess
            && results.Item8.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue(),
                results.Item8.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult<T1>, IResult<T2>)> OnAllSuccessAsync<T1, T2>(
        this (IResult<T1>, IResult<T2>) results,
        Func<T1, T2, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult<T1>, IResult<T2>, IResult<T3>)> OnAllSuccessAsync<T1, T2, T3>(
        this (IResult<T1>, IResult<T2>, IResult<T3>) results,
        Func<T1, T2, T3, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>)> OnAllSuccessAsync<T1, T2, T3, T4>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>) results,
        Func<T1, T2, T3, T4, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>)> OnAllSuccessAsync<T1, T2, T3, T4, T5>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>) results,
        Func<T1, T2, T3, T4, T5, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="T6">The type of the sixth result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>)> OnAllSuccessAsync<T1, T2, T3, T4, T5, T6>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>) results,
        Func<T1, T2, T3, T4, T5, T6, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="T6">The type of the sixth result.</typeparam>
    /// <typeparam name="T7">The type of the seventh result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>, IResult<T7>)> OnAllSuccessAsync<T1, T2, T3, T4, T5, T6, T7>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>, IResult<T7>) results,
        Func<T1, T2, T3, T4, T5, T6, T7, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="T6">The type of the sixth result.</typeparam>
    /// <typeparam name="T7">The type of the seventh result.</typeparam>
    /// <typeparam name="T8">The type of the eighth result.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>, IResult<T7>, IResult<T8>)> OnAllSuccessAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>, IResult<T7>, IResult<T8>) results,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess
            && results.Item8.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue(),
                results.Item8.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult) OnAllSuccess(
        this (IResult, IResult) results,
        Action<object, object> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult, IResult) OnAllSuccess(
        this (IResult, IResult, IResult) results,
        Action<object, object, object> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult, IResult, IResult) OnAllSuccess(
        this (IResult, IResult, IResult, IResult) results,
        Action<object, object, object, object> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult, IResult, IResult, IResult) OnAllSuccess(
        this (IResult, IResult, IResult, IResult, IResult) results,
        Action<object, object, object, object, object> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult, IResult, IResult, IResult, IResult) OnAllSuccess(
        this (IResult, IResult, IResult, IResult, IResult, IResult) results,
        Action<object, object, object, object, object, object> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult, IResult, IResult, IResult, IResult, IResult) OnAllSuccess(
        this (IResult, IResult, IResult, IResult, IResult, IResult, IResult) results,
        Action<object, object, object, object, object, object, object> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult, IResult, IResult, IResult, IResult, IResult, IResult) OnAllSuccess(
        this (IResult, IResult, IResult, IResult, IResult, IResult, IResult, IResult) results,
        Action<object, object, object, object, object, object, object, object> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess
            && results.Item8.IsSuccess)
        {
            onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue(),
                results.Item8.GetSuccessValue());
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult)> OnAllSuccessAsync(
        this (IResult, IResult) results,
        Func<object, object, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult, IResult)> OnAllSuccessAsync(
        this (IResult, IResult, IResult) results,
        Func<object, object, object, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult, IResult, IResult)> OnAllSuccessAsync(
        this (IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult, IResult, IResult, IResult)> OnAllSuccessAsync(
        this (IResult, IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, object, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult, IResult, IResult, IResult, IResult)> OnAllSuccessAsync(
        this (IResult, IResult, IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, object, object, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult, IResult, IResult, IResult, IResult, IResult)> OnAllSuccessAsync(
        this (IResult, IResult, IResult, IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, object, object, object, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAllSuccess"/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// A callback function to invoke if all results in the tuple are <c>Success</c> results.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAllSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult, IResult, IResult, IResult, IResult, IResult, IResult)> OnAllSuccessAsync(
        this (IResult, IResult, IResult, IResult, IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, object, object, object, object, Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess
            && results.Item8.IsSuccess)
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue(),
                results.Item8.GetSuccessValue()).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult) OnAnyNonSuccess(
        this (IResult, IResult) results,
        Action<Error> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2);
            onAnyNonSuccess(error);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult, IResult) OnAnyNonSuccess(
        this (IResult, IResult, IResult) results,
        Action<Error> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess
            || !results.Item3.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3);
            onAnyNonSuccess(error);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult, IResult, IResult) OnAnyNonSuccess(
        this (IResult, IResult, IResult, IResult) results,
        Action<Error> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess
            || !results.Item3.IsSuccess
            || !results.Item4.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4);
            onAnyNonSuccess(error);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult, IResult, IResult, IResult) OnAnyNonSuccess(
        this (IResult, IResult, IResult, IResult, IResult) results,
        Action<Error> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess
            || !results.Item3.IsSuccess
            || !results.Item4.IsSuccess
            || !results.Item5.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5);
            onAnyNonSuccess(error);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult, IResult, IResult, IResult, IResult) OnAnyNonSuccess(
        this (IResult, IResult, IResult, IResult, IResult, IResult) results,
        Action<Error> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess
            || !results.Item3.IsSuccess
            || !results.Item4.IsSuccess
            || !results.Item5.IsSuccess
            || !results.Item6.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6);
            onAnyNonSuccess(error);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult, IResult, IResult, IResult, IResult, IResult) OnAnyNonSuccess(
        this (IResult, IResult, IResult, IResult, IResult, IResult, IResult) results,
        Action<Error> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess
            || !results.Item3.IsSuccess
            || !results.Item4.IsSuccess
            || !results.Item5.IsSuccess
            || !results.Item6.IsSuccess
            || !results.Item7.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6,
                results.Item7);
            onAnyNonSuccess(error);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static (IResult, IResult, IResult, IResult, IResult, IResult, IResult, IResult) OnAnyNonSuccess(
        this (IResult, IResult, IResult, IResult, IResult, IResult, IResult, IResult) results,
        Action<Error> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess
            || !results.Item3.IsSuccess
            || !results.Item4.IsSuccess
            || !results.Item5.IsSuccess
            || !results.Item6.IsSuccess
            || !results.Item7.IsSuccess
            || !results.Item8.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6,
                results.Item7,
                results.Item8);
            onAnyNonSuccess(error);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult)> OnAnyNonSuccessAsync(
        this (IResult, IResult) results,
        Func<Error, Task> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2);
            await onAnyNonSuccess(error).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult, IResult)> OnAnyNonSuccessAsync(
        this (IResult, IResult, IResult) results,
        Func<Error, Task> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess
            || !results.Item3.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3);
            await onAnyNonSuccess(error).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult, IResult, IResult)> OnAnyNonSuccessAsync(
        this (IResult, IResult, IResult, IResult) results,
        Func<Error, Task> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess
            || !results.Item3.IsSuccess
            || !results.Item4.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4);
            await onAnyNonSuccess(error).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult, IResult, IResult, IResult)> OnAnyNonSuccessAsync(
        this (IResult, IResult, IResult, IResult, IResult) results,
        Func<Error, Task> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess
            || !results.Item3.IsSuccess
            || !results.Item4.IsSuccess
            || !results.Item5.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5);
            await onAnyNonSuccess(error).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult, IResult, IResult, IResult, IResult)> OnAnyNonSuccessAsync(
        this (IResult, IResult, IResult, IResult, IResult, IResult) results,
        Func<Error, Task> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess
            || !results.Item3.IsSuccess
            || !results.Item4.IsSuccess
            || !results.Item5.IsSuccess
            || !results.Item6.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6);
            await onAnyNonSuccess(error).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult, IResult, IResult, IResult, IResult, IResult)> OnAnyNonSuccessAsync(
        this (IResult, IResult, IResult, IResult, IResult, IResult, IResult) results,
        Func<Error, Task> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess
            || !results.Item3.IsSuccess
            || !results.Item4.IsSuccess
            || !results.Item5.IsSuccess
            || !results.Item6.IsSuccess
            || !results.Item7.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6,
                results.Item7);
            await onAnyNonSuccess(error).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Invokes the <paramref name="onAnyNonSuccess"/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAnyNonSuccess">
    /// A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.</exception>
    public static async Task<(IResult, IResult, IResult, IResult, IResult, IResult, IResult, IResult)> OnAnyNonSuccessAsync(
        this (IResult, IResult, IResult, IResult, IResult, IResult, IResult, IResult) results,
        Func<Error, Task> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess
            || !results.Item2.IsSuccess
            || !results.Item3.IsSuccess
            || !results.Item4.IsSuccess
            || !results.Item5.IsSuccess
            || !results.Item6.IsSuccess
            || !results.Item7.IsSuccess
            || !results.Item8.IsSuccess)
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6,
                results.Item7,
                results.Item8);
            await onAnyNonSuccess(error).ConfigureAwait(false);
        }

        return results;
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<T1, T2, TReturn>(
        this (IResult<T1>, IResult<T2>) results,
        Func<T1, T2, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<T1, T2, T3, TReturn>(
        this (IResult<T1>, IResult<T2>, IResult<T3>) results,
        Func<T1, T2, T3, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<T1, T2, T3, T4, TReturn>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>) results,
        Func<T1, T2, T3, T4, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<T1, T2, T3, T4, T5, TReturn>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>) results,
        Func<T1, T2, T3, T4, T5, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="T6">The type of the sixth result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<T1, T2, T3, T4, T5, T6, TReturn>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>) results,
        Func<T1, T2, T3, T4, T5, T6, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="T6">The type of the sixth result.</typeparam>
    /// <typeparam name="T7">The type of the seventh result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<T1, T2, T3, T4, T5, T6, T7, TReturn>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>, IResult<T7>) results,
        Func<T1, T2, T3, T4, T5, T6, T7, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6,
                results.Item7);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="T6">The type of the sixth result.</typeparam>
    /// <typeparam name="T7">The type of the seventh result.</typeparam>
    /// <typeparam name="T8">The type of the eighth result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>, IResult<T7>, IResult<T8>) results,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess
            && results.Item8.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue(),
                results.Item8.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6,
                results.Item7,
                results.Item8);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<T1, T2, TReturn>(
        this (IResult<T1>, IResult<T2>) results,
        Func<T1, T2, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<T1, T2, T3, TReturn>(
        this (IResult<T1>, IResult<T2>, IResult<T3>) results,
        Func<T1, T2, T3, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<T1, T2, T3, T4, TReturn>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>) results,
        Func<T1, T2, T3, T4, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<T1, T2, T3, T4, T5, TReturn>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>) results,
        Func<T1, T2, T3, T4, T5, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="T6">The type of the sixth result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<T1, T2, T3, T4, T5, T6, TReturn>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>) results,
        Func<T1, T2, T3, T4, T5, T6, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="T6">The type of the sixth result.</typeparam>
    /// <typeparam name="T7">The type of the seventh result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<T1, T2, T3, T4, T5, T6, T7, TReturn>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>, IResult<T7>) results,
        Func<T1, T2, T3, T4, T5, T6, T7, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6,
                results.Item7);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <typeparam name="T4">The type of the fourth result.</typeparam>
    /// <typeparam name="T5">The type of the fifth result.</typeparam>
    /// <typeparam name="T6">The type of the sixth result.</typeparam>
    /// <typeparam name="T7">The type of the seventh result.</typeparam>
    /// <typeparam name="T8">The type of the eighth result.</typeparam>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>(
        this (IResult<T1>, IResult<T2>, IResult<T3>, IResult<T4>, IResult<T5>, IResult<T6>, IResult<T7>, IResult<T8>) results,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess
            && results.Item8.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue(),
                results.Item8.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6,
                results.Item7,
                results.Item8);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<TReturn>(
        this (IResult, IResult) results,
        Func<object, object, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<TReturn>(
        this (IResult, IResult, IResult) results,
        Func<object, object, object, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<TReturn>(
        this (IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<TReturn>(
        this (IResult, IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, object, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<TReturn>(
        this (IResult, IResult, IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, object, object, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<TReturn>(
        this (IResult, IResult, IResult, IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, object, object, object, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6,
                results.Item7);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static TReturn MatchAll<TReturn>(
        this (IResult, IResult, IResult, IResult, IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, object, object, object, object, TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess
            && results.Item8.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue(),
                results.Item8.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6,
                results.Item7,
                results.Item8);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<TReturn>(
        this (IResult, IResult) results,
        Func<object, object, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<TReturn>(
        this (IResult, IResult, IResult) results,
        Func<object, object, object, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<TReturn>(
        this (IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<TReturn>(
        this (IResult, IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, object, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<TReturn>(
        this (IResult, IResult, IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, object, object, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<TReturn>(
        this (IResult, IResult, IResult, IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, object, object, object, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6,
                results.Item7);
            return onAnyNonSuccess(error);
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> function depending on whether
    /// all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match all method.</typeparam>
    /// <param name="results">A tuple of results.</param>
    /// <param name="onAllSuccess">
    /// The function to evaluate if all results are <c>Success</c>. The non-null values of each of the <c>Success</c> results are
    /// passed to this function.
    /// </param>
    /// <param name="onAnyNonSuccess">
    /// The function to evaluate if any results are <c>non-Success</c>. The error passed to this function depends on how many
    /// results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is passed to this function. If more
    /// than one result is <c>non-Success</c>, then a <see cref="CompositeError"/> is returned containing the error of each
    /// <c>non-Success</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that creates the <see cref="Error"/> for any <c>None</c> results (otherwise not applicable). If
    /// <see langword="null"/> (and applicable), a function that returns an error with message "Not Found" and error code 404 is
    /// used instead.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onAllSuccess"/> or <paramref name="onAnyNonSuccess"/> is <see langword="null"/>.
    /// </exception>
    public static Task<TReturn> MatchAllAsync<TReturn>(
        this (IResult, IResult, IResult, IResult, IResult, IResult, IResult, IResult) results,
        Func<object, object, object, object, object, object, object, object, Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess
            && results.Item2.IsSuccess
            && results.Item3.IsSuccess
            && results.Item4.IsSuccess
            && results.Item5.IsSuccess
            && results.Item6.IsSuccess
            && results.Item7.IsSuccess
            && results.Item8.IsSuccess)
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue(),
                results.Item2.GetSuccessValue(),
                results.Item3.GetSuccessValue(),
                results.Item4.GetSuccessValue(),
                results.Item5.GetSuccessValue(),
                results.Item6.GetSuccessValue(),
                results.Item7.GetSuccessValue(),
                results.Item8.GetSuccessValue());
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1,
                results.Item2,
                results.Item3,
                results.Item4,
                results.Item5,
                results.Item6,
                results.Item7,
                results.Item8);
            return onAnyNonSuccess(error);
        }
    }

    private static Error GetNonSuccessError(Func<Error>? getNoneError, params IResult[] results)
    {
        var errors = results
            .Where(r => !r.IsSuccess)
            .Select(r => r.GetNonSuccessError(getNoneError));
        return CompositeError.Create(errors);
    }
}

#pragma warning restore SA1414 // Tuple types in signatures should have element names
