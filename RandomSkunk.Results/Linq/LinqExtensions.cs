namespace RandomSkunk.Results.Linq;

/// <summary>
/// Extension methods to enable linq support.
/// </summary>
public static class LinqExtensions
{
    /// <summary>
    /// <para>
    /// Alias for the <see cref="ResultExtensions.Map{T, TResult}(Result{T}, Func{T, TResult})"/>
    /// method.
    /// </para>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="selector"/>
    /// function. The map function is only evaluated if the target is a <c>some</c> result, and
    /// the <see cref="Result{T}.Type"/> and <see cref="ResultBase.CallSite"/> of the new result
    /// will always be the same as the target result.
    /// </summary>
    /// <typeparam name="TSource">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The target result.</param>
    /// <param name="selector">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="selector"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="selector"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<TResult> Select<TSource, TResult>(
        this Result<TSource> source,
        Func<TSource, TResult> selector) =>
        source.Map(selector);

    /// <summary>
    /// <para>
    /// Alias for the <see cref="ResultExtensions.FlatMap{T, TResult}(Result{T}, Func{T, Result{TResult}})"/>
    /// method.
    /// </para>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="selector"/> function. The flat map function is only evaluated if the target
    /// is a <c>success</c> result. If the target is a <c>none</c> or <c>fail</c> result, the
    /// callsite is propagated to the returned <c>none</c> or <c>fail</c> result. If the target is
    /// a <c>fail</c> result, the error is propagated as well.
    /// </summary>
    /// <typeparam name="TSource">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The target result.</param>
    /// <param name="selector">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="selector"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="selector"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<TResult> SelectMany<TSource, TResult>(
        this Result<TSource> source,
        Func<TSource, Result<TResult>> selector) =>
        source.FlatMap(selector);

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector
    /// function on the values of the source and intermediate results.
    /// </summary>
    /// <typeparam name="TSource">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TIntermediate">
    /// The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The target result.</param>
    /// <param name="intermediateSelector">
    /// A transform function to apply to the value of the input result.
    /// </param>
    /// <param name="resultSelector">
    /// A transform function to apply to the value of the intermediate result.
    /// </param>
    /// <returns>
    /// An <see cref="MaybeResult{T}"/> whose value is the result of invoking the transform
    /// function <paramref name="intermediateSelector"/> on the value of <paramref name="source"/>
    /// and then mapping the values of that result and the source result to the final result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/>, or if
    /// <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    /// <paramref name="resultSelector"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="intermediateSelector"/> returns <see langword="null"/> when evaluated,
    /// or if <paramref name="resultSelector"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<TResult> SelectMany<TSource, TIntermediate, TResult>(
        this Result<TSource> source,
        Func<TSource, Result<TIntermediate>> intermediateSelector,
        Func<TSource, TIntermediate, TResult> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.FlatMap(
            x => (intermediateSelector(x) ?? throw Exceptions.FunctionMustNotReturnNull(nameof(intermediateSelector))).FlatMap(
                y => Result.Success(resultSelector(x!, y!) ?? throw Exceptions.FunctionMustNotReturnNull(nameof(resultSelector)))));
    }

    /// <summary>
    /// <para>
    /// Alias for the <see cref="ResultExtensions.Map{T, TResult}(MaybeResult{T}, Func{T, TResult})"/>
    /// method.
    /// </para>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="selector"/>
    /// function. The map function is only evaluated if the target is a <c>some</c> result, and
    /// the <see cref="Result{T}.Type"/> and <see cref="ResultBase.CallSite"/> of the new result
    /// will always be the same as the target result.
    /// </summary>
    /// <typeparam name="TSource">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The target result.</param>
    /// <param name="selector">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="selector"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="selector"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static MaybeResult<TResult> Select<TSource, TResult>(
        this MaybeResult<TSource> source,
        Func<TSource, TResult> selector) =>
        source.Map(selector);

    /// <summary>
    /// <para>
    /// Alias for the <see cref="ResultExtensions.FlatMap{T, TResult}(MaybeResult{T}, Func{T, MaybeResult{TResult}})"/>
    /// method.
    /// </para>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="selector"/> function. The flat map function is only evaluated if the target
    /// is a <c>success</c> result. If the target is a <c>none</c> or <c>fail</c> result, the
    /// callsite is propagated to the returned <c>none</c> or <c>fail</c> result. If the target is
    /// a <c>fail</c> result, the error is propagated as well.
    /// </summary>
    /// <typeparam name="TSource">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The target result.</param>
    /// <param name="selector">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="selector"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="selector"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static MaybeResult<TResult> SelectMany<TSource, TResult>(
        this MaybeResult<TSource> source,
        Func<TSource, MaybeResult<TResult>> selector) =>
        source.FlatMap(selector);

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector
    /// function on the values of the source and intermediate results.
    /// </summary>
    /// <typeparam name="TSource">The type of the return value of the operation.</typeparam>
    /// <typeparam name="TIntermediate">
    /// The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the new result.</typeparam>
    /// <param name="source">The target result.</param>
    /// <param name="intermediateSelector">
    /// A transform function to apply to the value of the input result.
    /// </param>
    /// <param name="resultSelector">
    /// A transform function to apply to the value of the intermediate result.
    /// </param>
    /// <returns>
    /// An <see cref="MaybeResult{T}"/> whose value is the result of invoking the transform
    /// function <paramref name="intermediateSelector"/> on the value of <paramref name="source"/>
    /// and then mapping the values of that result and the source result to the final result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/>, or if
    /// <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    /// <paramref name="resultSelector"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="intermediateSelector"/> returns <see langword="null"/> when evaluated,
    /// or if <paramref name="resultSelector"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static MaybeResult<TResult> SelectMany<TSource, TIntermediate, TResult>(
        this MaybeResult<TSource> source,
        Func<TSource, MaybeResult<TIntermediate>> intermediateSelector,
        Func<TSource, TIntermediate, TResult> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.FlatMap(
            x => (intermediateSelector(x) ?? throw Exceptions.FunctionMustNotReturnNull(nameof(intermediateSelector))).FlatMap(
                y => MaybeResult.Some(resultSelector(x!, y!) ?? throw Exceptions.FunctionMustNotReturnNull(nameof(resultSelector)))));
    }

    /// <summary>
    /// <para>Alias for the <see cref="ResultExtensions.Filter"/> method.</para>
    /// Filter the specified result into a <c>none</c> result if it is a <c>some</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>. The callsite is
    /// always propagated to the returned result.
    /// </summary>
    /// <typeparam name="TSource">The type of the return value of the operation.</typeparam>
    /// <param name="source">The target result.</param>
    /// <param name="predicate">
    /// A function that filters a <c>some</c> result into a <c>none</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="source"/> is <see langword="null"/> or if <paramref name="predicate"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static MaybeResult<TSource> Where<TSource>(this MaybeResult<TSource> source, Func<TSource, bool> predicate) =>
        source.Filter(predicate);
}
