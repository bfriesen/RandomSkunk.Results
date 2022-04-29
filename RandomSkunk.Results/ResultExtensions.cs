namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result objects.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Returns <paramref name="result"/> if it is a <c>success</c> result, or a new <c>success</c>
    /// result with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="fallbackValue">The fallback value if the result is not <c>success</c>.</param>
    /// <returns>A <c>success</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if
    /// <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> Or<T>(this Result<T> result, [DisallowNull] T fallbackValue)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        if (result.IsSuccess)
            return result;

        return Result.Success(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="result"/> if it is a <c>success</c> result, or a new <c>success</c>
    /// result with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="getFallbackValue">
    /// A function that returns the fallback value if the result is not <c>success</c>.
    /// </param>
    /// <returns>A <c>success</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if
    /// <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<T> Or<T>(this Result<T> result, [DisallowNull] Func<T> getFallbackValue)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        if (result.IsSuccess)
            return result;

        var fallbackValue = getFallbackValue()
             ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackValue));

        return Result.Success(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="result"/> if it is a <c>some</c> result, or a new <c>some</c>
    /// result with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="fallbackValue">The fallback value if the result is not <c>some</c>.</param>
    /// <returns>A <c>some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if
    /// <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public static MaybeResult<T> Or<T>(this MaybeResult<T> result, [DisallowNull] T fallbackValue)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        if (result.IsSome)
            return result;

        return MaybeResult.Some(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="result"/> if it is a <c>some</c> result, or a new <c>some</c>
    /// result with its value from evaluating the <paramref name="getFallbackValue"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="getFallbackValue">
    /// A function that returns the fallback value if the result is not <c>some</c>.
    /// </param>
    /// <returns>A <c>some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if
    /// <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static MaybeResult<T> Or<T>(this MaybeResult<T> result, [DisallowNull] Func<T> getFallbackValue)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        if (result.IsSome)
            return result;

        var fallbackValue = getFallbackValue()
             ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackValue));

        return MaybeResult.Some(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="result"/> if it is a <c>success</c> result, else returns the
    /// specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>success</c>.</param>
    /// <returns>Either <paramref name="result"/> or <paramref name="fallbackResult"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if
    /// <paramref name="fallbackResult"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> Else<T>(this Result<T> result, [DisallowNull] Result<T> fallbackResult)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (fallbackResult is null) throw new ArgumentNullException(nameof(fallbackResult));

        if (result.IsSuccess)
            return result;

        return fallbackResult;
    }

    /// <summary>
    /// Returns <paramref name="result"/> if it is a <c>success</c> result, else returns the result
    /// from evaluating the <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="getFallbackResult">
    /// A function that returns the fallback result if the result is not <c>success</c>.
    /// </param>
    /// <returns>
    /// Either <paramref name="result"/> or the value returned from
    /// <paramref name="getFallbackResult"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if
    /// <paramref name="getFallbackResult"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackResult"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<T> Else<T>(this Result<T> result, [DisallowNull] Func<Result<T>> getFallbackResult)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        if (result.IsSuccess)
            return result;

        var fallbackResult = getFallbackResult()
             ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackResult));

        return fallbackResult;
    }

    /// <summary>
    /// Returns <paramref name="result"/> if it is a <c>some</c> result, else returns the
    /// specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>some</c>.</param>
    /// <returns>Either <paramref name="result"/> or <paramref name="fallbackResult"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if
    /// <paramref name="fallbackResult"/> is <see langword="null"/>.
    /// </exception>
    public static MaybeResult<T> Else<T>(this MaybeResult<T> result, [DisallowNull] MaybeResult<T> fallbackResult)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (fallbackResult is null) throw new ArgumentNullException(nameof(fallbackResult));

        if (result.IsSome)
            return result;

        return fallbackResult;
    }

    /// <summary>
    /// Returns <paramref name="result"/> if it is a <c>some</c> result, else returns the result
    /// from evaluating the <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="getFallbackResult">
    /// A function that returns the fallback result if the result is not <c>some</c>.
    /// </param>
    /// <returns>
    /// Either <paramref name="result"/> or the value returned from
    /// <paramref name="getFallbackResult"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if
    /// <paramref name="getFallbackResult"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackResult"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static MaybeResult<T> Else<T>(this MaybeResult<T> result, [DisallowNull] Func<MaybeResult<T>> getFallbackResult)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        if (result.IsSome)
            return result;

        var fallbackResult = getFallbackResult()
             ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackResult));

        return fallbackResult;
    }

    /// <summary>
    /// Maps <paramref name="result"/> to a new result using the specified <paramref name="map"/>
    /// function. The map function is only evaluated if the target is a <c>some</c> result, and
    /// the <see cref="Result{T}.Type"/> of the new result will always be the same as the target
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="map">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="map"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="map"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<TResult> Map<T, TResult>(this Result<T> result, Func<T, TResult> map)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (map is null) throw new ArgumentNullException(nameof(map));

        if (result.IsSuccess)
        {
            var mappedValue = map(result.Value)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(map));

            return Result.Success(mappedValue);
        }

        return Result.Fail<TResult>(result.Error);
    }

    /// <summary>
    /// Maps <paramref name="result"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the target is a <c>some</c> result, and
    /// the <see cref="Result{T}.Type"/> of the new result will always be the same as the target
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="mapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<Result<TResult>> MapAsync<T, TResult>(
        this Result<T> result,
        Func<T, CancellationToken, Task<TResult>> mapAsync,
        CancellationToken cancellationToken = default)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        if (result.IsSuccess)
        {
            var mappedValue = await mapAsync(result.Value, cancellationToken)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(mapAsync));

            return Result.Success(mappedValue);
        }

        return Result.Fail<TResult>(result.Error);
    }

    /// <summary>
    /// Maps <paramref name="result"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the target is a <c>some</c> result, and
    /// the <see cref="Result{T}.Type"/> the new result will always be the same as the target
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="mapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Task<Result<TResult>> MapAsync<T, TResult>(
        this Result<T> result,
        Func<T, Task<TResult>> mapAsync) =>
        result.MapAsync((value, cancellationToken) => mapAsync(value), default);

    /// <summary>
    /// Maps <paramref name="result"/> to a new result using the specified <paramref name="map"/>
    /// function. The map function is only evaluated if the target is a <c>some</c> result, and
    /// the <see cref="Result{T}.Type"/> of the new result will always be the same as the target
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="map">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="map"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="map"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static MaybeResult<TResult> Map<T, TResult>(this MaybeResult<T> result, Func<T, TResult> map)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (map is null) throw new ArgumentNullException(nameof(map));

        if (result.IsSome)
        {
            var mappedValue = map(result.Value)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(map));

            return MaybeResult.Some(mappedValue);
        }

        if (result.IsNone)
            return MaybeResult.None<TResult>();

        return MaybeResult.Fail<TResult>(result.Error);
    }

    /// <summary>
    /// Maps <paramref name="result"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the target is a <c>some</c> result, and
    /// the <see cref="MaybeResult{T}.Type"/> of the new result will always be the same as the target
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="mapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<MaybeResult<TResult>> MapAsync<T, TResult>(
        this MaybeResult<T> result,
        Func<T, CancellationToken, Task<TResult>> mapAsync,
        CancellationToken cancellationToken = default)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        if (result.IsSome)
        {
            var mappedValue = await mapAsync(result.Value, cancellationToken)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(mapAsync));

            return MaybeResult.Some(mappedValue);
        }

        if (result.IsNone)
            return MaybeResult.None<TResult>();

        return MaybeResult.Fail<TResult>(result.Error);
    }

    /// <summary>
    /// Maps <paramref name="result"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the target is a <c>some</c> result, and
    /// the <see cref="MaybeResult{T}.Type"/> of the new result will always be the same as the target
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="mapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Task<MaybeResult<TResult>> MapAsync<T, TResult>(
        this MaybeResult<T> result,
        Func<T, Task<TResult>> mapAsync) =>
        result.MapAsync((value, cancellationToken) => mapAsync(value), default);

    /// <summary>
    /// Maps <paramref name="result"/> to a another result using the specified
    /// <paramref name="flatMap"/> function. The flat map function is only evaluated if the target
    /// is a <c>success</c> result. If the target is a <c>fail</c> result, the error and callsite
    /// are propagated to the returned <c>fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="flatMap">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="flatMap"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="flatMap"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<TResult> FlatMap<T, TResult>(this Result<T> result, Func<T, Result<TResult>> flatMap)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (flatMap is null) throw new ArgumentNullException(nameof(flatMap));

        if (result.IsSuccess)
        {
            var mappedValue = flatMap(result.Value)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(flatMap));

            return mappedValue;
        }

        return Result.Fail<TResult>(result.Error);
    }

    /// <summary>
    /// Maps <paramref name="result"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the target
    /// is a <c>success</c> result. If the target is a <c>fail</c> result, the error and callsite
    /// are propagated to the returned <c>fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="flatMapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="flatMapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<Result<TResult>> FlatMapAsync<T, TResult>(
        this Result<T> result,
        Func<T, CancellationToken, Task<Result<TResult>>> flatMapAsync,
        CancellationToken cancellationToken = default)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        if (result.IsSuccess)
        {
            var mappedValue = await flatMapAsync(result.Value, cancellationToken)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(flatMapAsync));

            return mappedValue;
        }

        return Result.Fail<TResult>(result.Error);
    }

    /// <summary>
    /// Maps <paramref name="result"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the target
    /// is a <c>success</c> result. If the target is a <c>fail</c> result, the error and callsite
    /// are propagated to the returned <c>fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="flatMapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="flatMapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Task<Result<TResult>> FlatMapAsync<T, TResult>(
        this Result<T> result,
        Func<T, Task<Result<TResult>>> flatMapAsync) =>
        result.FlatMapAsync((value, cancellationToken) => flatMapAsync(value), default);

    /// <summary>
    /// Maps <paramref name="result"/> to a another result using the specified
    /// <paramref name="flatMap"/> function. The flat map function is only evaluated if the target
    /// is a <c>success</c> result. If the target is a <c>none</c> or <c>fail</c> result, the
    /// callsite is propagated to the returned <c>none</c> or <c>fail</c> result. If the target is
    /// a <c>fail</c> result, the error is propagated as well.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="flatMap">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="flatMap"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="flatMap"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static MaybeResult<TResult> FlatMap<T, TResult>(this MaybeResult<T> result, Func<T, MaybeResult<TResult>> flatMap)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (flatMap is null) throw new ArgumentNullException(nameof(flatMap));

        if (result.IsSome)
        {
            var mappedValue = flatMap(result.Value)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(flatMap));

            return mappedValue;
        }

        if (result.IsNone)
            return MaybeResult.None<TResult>();

        return MaybeResult.Fail<TResult>(result.Error);
    }

    /// <summary>
    /// Maps <paramref name="result"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the target
    /// is a <c>success</c> result. If the target is a <c>none</c> or <c>fail</c> result, the
    /// callsite is propagated to the returned <c>none</c> or <c>fail</c> result. If the target is
    /// a <c>fail</c> result, the error is propagated as well.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="flatMapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="flatMapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<MaybeResult<TResult>> FlatMapAsync<T, TResult>(
        this MaybeResult<T> result,
        Func<T, CancellationToken, Task<MaybeResult<TResult>>> flatMapAsync,
        CancellationToken cancellationToken)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        if (result.IsSome)
        {
            var mappedValue = await flatMapAsync(result.Value, cancellationToken)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(flatMapAsync));

            return mappedValue;
        }

        if (result.IsNone)
            return MaybeResult.None<TResult>();

        return MaybeResult.Fail<TResult>(result.Error);
    }

    /// <summary>
    /// Maps <paramref name="result"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the target
    /// is a <c>success</c> result. If the target is a <c>none</c> or <c>fail</c> result, the
    /// callsite is propagated to the returned <c>none</c> or <c>fail</c> result. If the target is
    /// a <c>fail</c> result, the error is propagated as well.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="flatMapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="flatMapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Task<MaybeResult<TResult>> FlatMapAsync<T, TResult>(
        this MaybeResult<T> result,
        Func<T, Task<MaybeResult<TResult>>> flatMapAsync) =>
        result.FlatMapAsync((value, cancellationToken) => flatMapAsync(value), default);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="result">The target result.</param>
    /// <returns>The flattened result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> Flatten<T>(this Result<Result<T>> result)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));

        return result.FlatMap(r => r);
    }

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="result">The target result.</param>
    /// <returns>The flattened result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/>.
    /// </exception>
    public static MaybeResult<T> Flatten<T>(this MaybeResult<MaybeResult<T>> result)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));

        return result.FlatMap(r => r);
    }

    /// <summary>
    /// Filter the specified result into a <c>none</c> result if it is a <c>some</c> result and the
    /// <paramref name="filter"/> function evaluates to <see langword="false"/>. The callsite is
    /// always propagated to the returned result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="filter">
    /// A function that filters a <c>some</c> result into a <c>none</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="filter"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static MaybeResult<T> Filter<T>(this MaybeResult<T> result, Func<T, bool> filter)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        if (result.IsSome)
        {
            if (filter(result.Value))
                return result;

            return MaybeResult.None<T>();
        }

        return result;
    }

    /// <summary>
    /// Filter the specified result into a <c>none</c> result if it is a <c>some</c> result and the
    /// <paramref name="filterAsync"/> function evaluates to <see langword="false"/>. The callsite is
    /// always propagated to the returned result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="filterAsync">
    /// A function that filters a <c>some</c> result into a <c>none</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="filterAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task<MaybeResult<T>> FilterAsync<T>(
        this MaybeResult<T> result,
        Func<T, CancellationToken, Task<bool>> filterAsync,
        CancellationToken cancellationToken)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));
        if (filterAsync is null) throw new ArgumentNullException(nameof(filterAsync));

        if (result.IsSome)
        {
            if (await filterAsync(result.Value, cancellationToken))
                return result;

            return MaybeResult.None<T>();
        }

        return result;
    }

    /// <summary>
    /// Filter the specified result into a <c>none</c> result if it is a <c>some</c> result and the
    /// <paramref name="filterAsync"/> function evaluates to <see langword="false"/>. The callsite is
    /// always propagated to the returned result.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="result">The target result.</param>
    /// <param name="filterAsync">
    /// A function that filters a <c>some</c> result into a <c>none</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="result"/> is <see langword="null"/> or if <paramref name="filterAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static Task<MaybeResult<T>> FilterAsync<T>(
        this MaybeResult<T> result,
        Func<T, Task<bool>> filterAsync) =>
        result.FilterAsync((value, cancellationToken) => filterAsync(value), default);
}
