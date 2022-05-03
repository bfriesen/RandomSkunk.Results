namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result objects.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Determines whether the value of the result equals the <paramref name="otherValue"/>.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="otherValue">The value to compare.</param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>success</c> result and its value equals
    /// <paramref name="otherValue"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Equals<T>(this Result<T> source, T otherValue) =>
        source.Equals(otherValue, EqualityComparer<T>.Default);

    /// <summary>
    /// Determines whether the value of the result equals the <paramref name="otherValue"/>.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="otherValue">The value to compare.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> used to determine equality of the values.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>success</c> result and its value equals
    /// <paramref name="otherValue"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="comparer"/> is <see langword="null"/>.
    /// </exception>
    public static bool Equals<T>(this Result<T> source, T otherValue, IEqualityComparer<T> comparer)
    {
        if (comparer is null) throw new ArgumentNullException(nameof(comparer));

        return source.IsSuccess && comparer.Equals(source.Value, otherValue);
    }

    /// <summary>
    /// Determines whether the value of the result is equal to another value as defined by the
    /// <paramref name="isSuccessValue"/> function.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="isSuccessValue">
    /// A function that defines the equality of the result value.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>success</c> result and
    /// <paramref name="isSuccessValue"/> evaluates to <see langword="true"/> when passed
    /// its value; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="isSuccessValue"/> is <see langword="null"/>.
    /// </exception>
    public static bool Equals<T>(this Result<T> source, Func<T, bool> isSuccessValue)
    {
        if (isSuccessValue is null) throw new ArgumentNullException(nameof(isSuccessValue));

        return source.IsSuccess && isSuccessValue(source.Value);
    }

    /// <summary>
    /// Determines whether the value of the result equals the <paramref name="otherValue"/>.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="otherValue">The value to compare.</param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>some</c> result and its value equals
    /// <paramref name="otherValue"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Equals<T>(this MaybeResult<T> source, T otherValue) =>
        source.Equals(otherValue, EqualityComparer<T>.Default);

    /// <summary>
    /// Determines whether the value of the result equals the <paramref name="otherValue"/>.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="otherValue">The value to compare.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> used to determine equality of the values.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>some</c> result and its value equals
    /// <paramref name="otherValue"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="comparer"/> is <see langword="null"/>.
    /// </exception>
    public static bool Equals<T>(this MaybeResult<T> source, T otherValue, IEqualityComparer<T> comparer)
    {
        if (comparer is null) throw new ArgumentNullException(nameof(comparer));

        return source.IsSome && comparer.Equals(source.Value, otherValue);
    }

    /// <summary>
    /// Determines whether the value of the result is equal to another value as defined by the
    /// <paramref name="isSomeValue"/> function.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="isSomeValue">
    /// A function that defines the equality of the result value.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this is a <c>some</c> result and
    /// <paramref name="isSomeValue"/> evaluates to <see langword="true"/> when passed
    /// its value; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="isSomeValue"/> is <see langword="null"/>.
    /// </exception>
    public static bool Equals<T>(this MaybeResult<T> source, Func<T, bool> isSomeValue)
    {
        if (isSomeValue is null) throw new ArgumentNullException(nameof(isSomeValue));

        return source.IsSome && isSomeValue(source.Value);
    }

    /// <summary>
    /// Gets the value of the <c>success</c> result, or the specified default value if
    /// it is a <c>fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">
    /// The fallback value to return if this is not a <c>success</c> result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>success</c> result; otherwise,
    /// <paramref name="fallbackValue"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    [return: NotNull]
    public static T GetValueOr<T>(this Result<T> source, [DisallowNull] T fallbackValue)
    {
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return source.IsSuccess ? source.Value : fallbackValue;
    }

    /// <summary>
    /// Gets the value of the <c>success</c> result, or the specified default value if
    /// it is a <c>fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that creates the fallback value to return if this is not a <c>success</c>
    /// result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>success</c> result; otherwise, the value returned
    /// by the <paramref name="getFallbackValue"/> function.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    [return: NotNull]
    public static T GetValueOr<T>(this Result<T> source, Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        return source.IsSuccess ? source.Value : getFallbackValue() ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackValue));
    }

    /// <summary>
    /// Gets the value of the <c>success</c> result, or the specified default value if
    /// it is a <c>fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">
    /// The fallback value to return if this is not a <c>success</c> result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>success</c> result; otherwise,
    /// <paramref name="fallbackValue"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    [return: NotNull]
    public static T GetValueOr<T>(this MaybeResult<T> source, [DisallowNull] T fallbackValue)
    {
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return source.IsSome ? source.Value : fallbackValue;
    }

    /// <summary>
    /// Gets the value of the <c>success</c> result, or the specified default value if
    /// it is a <c>fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that creates the fallback value to return if this is not a <c>success</c>
    /// result.
    /// </param>
    /// <returns>
    /// The value of this result if this is a <c>success</c> result; otherwise, the value returned
    /// by the <paramref name="getFallbackValue"/> function.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    [return: NotNull]
    public static T GetValueOr<T>(this MaybeResult<T> source, Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        return source.IsSome ? source.Value : getFallbackValue() ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackValue));
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>success</c> result, or a new <c>success</c>
    /// result with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">The fallback value if the result is not <c>success</c>.</param>
    /// <returns>A <c>success</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if
    /// <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> Or<T>(this Result<T> source, [DisallowNull] T fallbackValue)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return source.IsSuccess ? source : Result.Success(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>success</c> result, or a new <c>success</c>
    /// result with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that returns the fallback value if the result is not <c>success</c>.
    /// </param>
    /// <returns>A <c>success</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if
    /// <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<T> Or<T>(this Result<T> source, [DisallowNull] Func<T> getFallbackValue)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        if (source.IsSuccess)
            return source;

        var fallbackValue = getFallbackValue()
             ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackValue));

        return Result.Success(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>some</c> result, or a new <c>some</c>
    /// result with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">The fallback value if the result is not <c>some</c>.</param>
    /// <returns>A <c>some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if
    /// <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public static MaybeResult<T> Or<T>(this MaybeResult<T> source, [DisallowNull] T fallbackValue)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return source.IsSome ? source : MaybeResult.Some(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>some</c> result, or a new <c>some</c>
    /// result with its value from evaluating the <paramref name="getFallbackValue"/> function.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that returns the fallback value if the result is not <c>some</c>.
    /// </param>
    /// <returns>A <c>some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if
    /// <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static MaybeResult<T> Or<T>(this MaybeResult<T> source, [DisallowNull] Func<T> getFallbackValue)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        if (source.IsSome)
            return source;

        var fallbackValue = getFallbackValue()
             ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackValue));

        return MaybeResult.Some(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>success</c> result, else returns the
    /// specified fallback result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>success</c>.</param>
    /// <returns>Either <paramref name="source"/> or <paramref name="fallbackResult"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if
    /// <paramref name="fallbackResult"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> Else<T>(this Result<T> source, [DisallowNull] Result<T> fallbackResult)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (fallbackResult is null) throw new ArgumentNullException(nameof(fallbackResult));

        return source.IsSuccess ? source : fallbackResult;
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>success</c> result, else returns the result
    /// from evaluating the <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackResult">
    /// A function that returns the fallback result if the result is not <c>success</c>.
    /// </param>
    /// <returns>
    /// Either <paramref name="source"/> or the value returned from
    /// <paramref name="getFallbackResult"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if
    /// <paramref name="getFallbackResult"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackResult"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<T> Else<T>(this Result<T> source, [DisallowNull] Func<Result<T>> getFallbackResult)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        if (source.IsSuccess)
            return source;

        var fallbackResult = getFallbackResult()
             ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackResult));

        return fallbackResult;
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>some</c> result, else returns the
    /// specified fallback result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>some</c>.</param>
    /// <returns>Either <paramref name="source"/> or <paramref name="fallbackResult"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if
    /// <paramref name="fallbackResult"/> is <see langword="null"/>.
    /// </exception>
    public static MaybeResult<T> Else<T>(this MaybeResult<T> source, [DisallowNull] MaybeResult<T> fallbackResult)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (fallbackResult is null) throw new ArgumentNullException(nameof(fallbackResult));

        return source.IsSome ? source : fallbackResult;
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>some</c> result, else returns the result
    /// from evaluating the <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackResult">
    /// A function that returns the fallback result if the result is not <c>some</c>.
    /// </param>
    /// <returns>
    /// Either <paramref name="source"/> or the value returned from
    /// <paramref name="getFallbackResult"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if
    /// <paramref name="getFallbackResult"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackResult"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static MaybeResult<T> Else<T>(this MaybeResult<T> source, [DisallowNull] Func<MaybeResult<T>> getFallbackResult)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        if (source.IsSome)
            return source;

        var fallbackResult = getFallbackResult()
             ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackResult));

        return fallbackResult;
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="map"/>
    /// function. The map function is only evaluated if the source is a <c>some</c> result, and
    /// the <see cref="Result{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="map">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="map"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="map"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<TResult> Map<T, TResult>(this Result<T> source, Func<T, TResult> map)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (map is null) throw new ArgumentNullException(nameof(map));

        if (source.IsSuccess)
        {
            var mappedValue = map(source.Value)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(map));

            return Result.Success(mappedValue);
        }

        return Result.Fail<TResult>(source.Error);
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the source is a <c>some</c> result, and
    /// the <see cref="Result{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="mapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<Result<TResult>> MapAsync<T, TResult>(
        this Result<T> source,
        Func<T, CancellationToken, Task<TResult>> mapAsync,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        if (source.IsSuccess)
        {
            var mappedValue = await mapAsync(source.Value, cancellationToken)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(mapAsync));

            return Result.Success(mappedValue);
        }

        return Result.Fail<TResult>(source.Error);
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the source is a <c>some</c> result, and
    /// the <see cref="Result{T}.Type"/> the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="mapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Task<Result<TResult>> MapAsync<T, TResult>(
        this Result<T> source,
        Func<T, Task<TResult>> mapAsync) =>
        source.MapAsync((value, cancellationToken) => mapAsync(value), default);

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="map"/>
    /// function. The map function is only evaluated if the source is a <c>some</c> result, and
    /// the <see cref="Result{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="map">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="map"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="map"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static MaybeResult<TResult> Map<T, TResult>(this MaybeResult<T> source, Func<T, TResult> map)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (map is null) throw new ArgumentNullException(nameof(map));

        if (source.IsSome)
        {
            var mappedValue = map(source.Value)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(map));

            return MaybeResult.Some(mappedValue);
        }

        if (source.IsNone)
            return MaybeResult.None<TResult>();

        return MaybeResult.Fail<TResult>(source.Error);
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the source is a <c>some</c> result, and
    /// the <see cref="MaybeResult{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="mapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<MaybeResult<TResult>> MapAsync<T, TResult>(
        this MaybeResult<T> source,
        Func<T, CancellationToken, Task<TResult>> mapAsync,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        if (source.IsSome)
        {
            var mappedValue = await mapAsync(source.Value, cancellationToken)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(mapAsync));

            return MaybeResult.Some(mappedValue);
        }

        if (source.IsNone)
            return MaybeResult.None<TResult>();

        return MaybeResult.Fail<TResult>(source.Error);
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the source is a <c>some</c> result, and
    /// the <see cref="MaybeResult{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="mapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Task<MaybeResult<TResult>> MapAsync<T, TResult>(
        this MaybeResult<T> source,
        Func<T, Task<TResult>> mapAsync) =>
        source.MapAsync((value, cancellationToken) => mapAsync(value), default);

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMap"/> function. The flat map function is only evaluated if the source
    /// is a <c>success</c> result. If the source is a <c>fail</c> result, the error is propagated
    /// to the returned <c>fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMap">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="flatMap"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="flatMap"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<TResult> FlatMap<T, TResult>(this Result<T> source, Func<T, Result<TResult>> flatMap)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (flatMap is null) throw new ArgumentNullException(nameof(flatMap));

        if (source.IsSuccess)
        {
            var mappedValue = flatMap(source.Value)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(flatMap));

            return mappedValue;
        }

        return Result.Fail<TResult>(source.Error);
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the
    /// source is a <c>success</c> result. If the source is a <c>fail</c> result, the error is
    /// propagated to the returned <c>fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="flatMapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="flatMapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<Result<TResult>> FlatMapAsync<T, TResult>(
        this Result<T> source,
        Func<T, CancellationToken, Task<Result<TResult>>> flatMapAsync,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        if (source.IsSuccess)
        {
            var mappedValue = await flatMapAsync(source.Value, cancellationToken)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(flatMapAsync));

            return mappedValue;
        }

        return Result.Fail<TResult>(source.Error);
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the
    /// source is a <c>success</c> result. If the source is a <c>fail</c> result, the error is
    /// propagated to the returned <c>fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="flatMapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="flatMapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Task<Result<TResult>> FlatMapAsync<T, TResult>(
        this Result<T> source,
        Func<T, Task<Result<TResult>>> flatMapAsync) =>
        source.FlatMapAsync((value, cancellationToken) => flatMapAsync(value), default);

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMap"/> function. The flat map function is only evaluated if the source
    /// is a <c>success</c> result. If the source is a <c>fail</c> result, the error is propagated
    /// to the returned <c>fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMap">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="flatMap"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="flatMap"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static MaybeResult<TResult> FlatMap<T, TResult>(this MaybeResult<T> source, Func<T, MaybeResult<TResult>> flatMap)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (flatMap is null) throw new ArgumentNullException(nameof(flatMap));

        if (source.IsSome)
        {
            var mappedValue = flatMap(source.Value)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(flatMap));

            return mappedValue;
        }

        if (source.IsNone)
            return MaybeResult.None<TResult>();

        return MaybeResult.Fail<TResult>(source.Error);
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the
    /// source is a <c>success</c> result. If the source is a <c>fail</c> result, the error is
    /// propagated to the returned <c>fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="flatMapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="flatMapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static async Task<MaybeResult<TResult>> FlatMapAsync<T, TResult>(
        this MaybeResult<T> source,
        Func<T, CancellationToken, Task<MaybeResult<TResult>>> flatMapAsync,
        CancellationToken cancellationToken)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        if (source.IsSome)
        {
            var mappedValue = await flatMapAsync(source.Value, cancellationToken)
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(flatMapAsync));

            return mappedValue;
        }

        if (source.IsNone)
            return MaybeResult.None<TResult>();

        return MaybeResult.Fail<TResult>(source.Error);
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the
    /// source is a <c>success</c> result. If the source is a <c>fail</c> result, the error is
    /// propagated to the returned <c>fail</c> result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="flatMapAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="flatMapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Task<MaybeResult<TResult>> FlatMapAsync<T, TResult>(
        this MaybeResult<T> source,
        Func<T, Task<MaybeResult<TResult>>> flatMapAsync) =>
        source.FlatMapAsync((value, cancellationToken) => flatMapAsync(value), default);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The flattened result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> Flatten<T>(this Result<Result<T>> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.FlatMap(nestedResult => nestedResult);
    }

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The flattened result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    public static MaybeResult<T> Flatten<T>(this MaybeResult<MaybeResult<T>> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.FlatMap(nestedResult => nestedResult);
    }

    /// <summary>
    /// Filter the specified result into a <c>none</c> result if it is a <c>some</c> result and the
    /// <paramref name="filter"/> function evaluates to <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="filter">
    /// A function that filters a <c>some</c> result into a <c>none</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="filter"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static MaybeResult<T> Filter<T>(this MaybeResult<T> source, Func<T, bool> filter)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        if (source.IsSome)
        {
            if (filter(source.Value))
                return source;

            return MaybeResult.None<T>();
        }

        return source;
    }

    /// <summary>
    /// Filter the specified result into a <c>none</c> result if it is a <c>some</c> result and the
    /// <paramref name="filterAsync"/> function evaluates to <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
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
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="filterAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static async Task<MaybeResult<T>> FilterAsync<T>(
        this MaybeResult<T> source,
        Func<T, CancellationToken, Task<bool>> filterAsync,
        CancellationToken cancellationToken)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (filterAsync is null) throw new ArgumentNullException(nameof(filterAsync));

        if (source.IsSome)
        {
            if (await filterAsync(source.Value, cancellationToken))
                return source;

            return MaybeResult.None<T>();
        }

        return source;
    }

    /// <summary>
    /// Filter the specified result into a <c>none</c> result if it is a <c>some</c> result and the
    /// <paramref name="filterAsync"/> function evaluates to <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="filterAsync">
    /// A function that filters a <c>some</c> result into a <c>none</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="filterAsync"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static Task<MaybeResult<T>> FilterAsync<T>(
        this MaybeResult<T> source,
        Func<T, Task<bool>> filterAsync) =>
        source.FilterAsync((value, cancellationToken) => filterAsync(value), default);
}
