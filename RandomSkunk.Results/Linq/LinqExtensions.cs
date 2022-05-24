using static RandomSkunk.Results.Exceptions;

namespace RandomSkunk.Results.Linq;

/// <summary>
/// Extension methods to enable linq support.
/// </summary>
public static class LinqExtensions
{
    /// <summary>
    /// <para>
    /// Alias for the <see cref="ResultExtensions.Map{T, TReturn}(Result{T}, Func{T, TReturn}, Func{Error, Error}?)"/>
    /// method.
    /// </para>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="selector"/>
    /// function. The map function is only evaluated if the target is a <c>Some</c> result, and
    /// the <see cref="Result{T}.Type"/> of the new result will always be the same as the target
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="selector">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="selector"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="selector"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<TReturn> Select<T, TReturn>(
        this Result<T> source,
        Func<T, TReturn> selector) =>
        source.Map(selector);

    /// <summary>
    /// <para>
    /// Alias for the <see cref="ResultExtensions.FlatMap{T, TReturn}(Result{T}, Func{T, Result{TReturn}}, Func{Error, Error}?)"/>
    /// method.
    /// </para>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="selector"/> function. The flat map function is only evaluated if the source
    /// is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is propagated
    /// to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="selector">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="selector"/> is <see langword="null"/>.
    /// </exception>
    public static Result<TReturn> SelectMany<T, TReturn>(
        this Result<T> source,
        Func<T, Result<TReturn>> selector) =>
        source.FlatMap(selector);

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector
    /// function on the values of the source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">
    /// The type of the intermediate result, as collected by
    /// <paramref name="intermediateSelector"/>.
    /// </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="intermediateSelector">
    /// A transform function to apply to the value of the input result.
    /// </param>
    /// <param name="resultSelector">
    /// A transform function to apply to the value of the intermediate result.
    /// </param>
    /// <returns>
    /// An <see cref="Maybe{T}"/> whose value is the result of invoking the transform
    /// function <paramref name="intermediateSelector"/> on the value of <paramref name="source"/>
    /// and then mapping the values of that result and the source result to the final result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    /// <paramref name="resultSelector"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="resultSelector"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<TReturn> SelectMany<T, TIntermediate, TReturn>(
        this Result<T> source,
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.FlatMap(
            sourceValue => intermediateSelector(sourceValue).FlatMap(
                intermediateValue => Result<TReturn>.Create.Success(
                    resultSelector(sourceValue!, intermediateValue!)
                        ?? throw FunctionMustNotReturnNull(nameof(resultSelector)))));
    }

    /// <summary>
    /// <para>
    /// Alias for the <see cref="ResultExtensions.Map{T, TReturn}(Maybe{T}, Func{T, TReturn}, Func{Error, Error}?)"/>
    /// method.
    /// </para>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="selector"/>
    /// function. The map function is only evaluated if the target is a <c>Some</c> result, and
    /// the <see cref="Result{T}.Type"/> of the new result will always be the same as the target
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="selector">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="selector"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="selector"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Maybe<TReturn> Select<T, TReturn>(
        this Maybe<T> source,
        Func<T, TReturn> selector) =>
        source.Map(selector);

    /// <summary>
    /// <para>
    /// Alias for the <see cref="ResultExtensions.FlatMap{T, TReturn}(Maybe{T}, Func{T, Maybe{TReturn}}, Func{Error, Error}?)"/>
    /// method.
    /// </para>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="selector"/> function. The flat map function is only evaluated if the source
    /// is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is propagated
    /// to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="selector">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="selector"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<TReturn> SelectMany<T, TReturn>(
        this Maybe<T> source,
        Func<T, Maybe<TReturn>> selector) =>
        source.FlatMap(selector);

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector
    /// function on the values of the source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">
    /// The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    /// </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="intermediateSelector">
    /// A transform function to apply to the value of the input result.
    /// </param>
    /// <param name="resultSelector">
    /// A transform function to apply to the value of the intermediate result.
    /// </param>
    /// <returns>
    /// An <see cref="Maybe{T}"/> whose value is the result of invoking the transform
    /// function <paramref name="intermediateSelector"/> on the value of <paramref name="source"/>
    /// and then mapping the values of that result and the source result to the final result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    /// <paramref name="resultSelector"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="resultSelector"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Maybe<TReturn> SelectMany<T, TIntermediate, TReturn>(
        this Maybe<T> source,
        Func<T, Maybe<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.FlatMap(
            sourceValue => intermediateSelector(sourceValue).FlatMap(
                intermediateValue => Maybe<TReturn>.Create.Some(
                    resultSelector(sourceValue!, intermediateValue!)
                        ?? throw FunctionMustNotReturnNull(nameof(resultSelector)))));
    }

    /// <summary>
    /// <para>Alias for the <see cref="ResultExtensions.Filter{T}(Maybe{T}, Func{T, bool})"/> method.</para>
    /// Filter the specified result into a <c>None</c> result if it is a <c>Some</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="predicate">
    /// A function that filters a <c>Some</c> result into a <c>None</c> result by returning
    /// <see langword="false"/>.
    /// </param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="predicate"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<T> Where<T>(this Maybe<T> source, Func<T, bool> predicate) =>
        source.Filter(predicate);
}
