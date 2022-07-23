using static RandomSkunk.Results.Exceptions;

namespace RandomSkunk.Results.Linq;

/// <summary>
/// Extension methods to enable linq support.
/// </summary>
public static class LinqExtensions
{
    /// <summary>
    /// <para>
    /// Alias for the <see cref="Result{T}.Map{TReturn}(Func{T, TReturn})"/> method.
    /// </para>
    /// Maps <paramref name="sourceResult"/> to a new result using the specified <paramref name="selector"/> function. The map
    /// function is only evaluated if the target is a <c>Success</c> result, and the <see cref="Result{T}.Type"/> of the new
    /// result will always be the same as the target result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="selector">A function that maps the value of the incoming result to the value of the outgoing result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="selector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="selector"/> returns <see langword="null"/> when evaluated.
    ///     </exception>
    public static Result<TReturn> Select<T, TReturn>(
        this Result<T> sourceResult,
        Func<T, TReturn> selector) =>
        sourceResult.Map(selector);

    /// <summary>
    /// <para>
    /// Alias for the <see cref="Result{T}.FlatMap{TReturn}(Func{T, Result{TReturn}})"/> method.
    /// </para>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="selector"/> function.
    /// Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="selector">A function that maps the value of the incoming result to the value of the outgoing result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="selector"/> is <see langword="null"/>.</exception>
    public static Result<TReturn> SelectMany<T, TReturn>(
        this Result<T> sourceResult,
        Func<T, Result<TReturn>> selector) =>
        sourceResult.FlatMap(selector);

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="resultSelector"/> returns <see langword="null"/> when evaluated.
    ///     </exception>
    public static Result<TReturn> SelectMany<T, TIntermediate, TReturn>(
        this Result<T> sourceResult,
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return sourceResult.FlatMap(
            sourceValue => intermediateSelector(sourceValue).FlatMap(
                intermediateValue =>
                    (resultSelector(sourceValue!, intermediateValue!)
                        ?? throw FunctionMustNotReturnNull(nameof(resultSelector))).ToResult()));
    }

    /// <summary>
    /// <para>
    /// Alias for the <see cref="Maybe{T}.Map{TReturn}(Func{T, TReturn})"/> method.
    /// </para>
    /// Maps <paramref name="sourceResult"/> to a new result using the specified <paramref name="selector"/> function. The map
    /// function is only evaluated if the target is a <c>Success</c> result, and the <see cref="Result{T}.Type"/> of the new
    /// result will always be the same as the target result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="selector">A function that maps the value of the incoming result to the value of the outgoing result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="selector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="selector"/> returns <see langword="null"/> when evaluated.
    ///     </exception>
    public static Maybe<TReturn> Select<T, TReturn>(
        this Maybe<T> sourceResult,
        Func<T, TReturn> selector) =>
        sourceResult.Map(selector);

    /// <summary>
    /// <para>
    /// Alias for the <see cref="Maybe{T}.FlatMap{TReturn}(Func{T, Maybe{TReturn}})"/> method.
    /// </para>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="selector"/> function. Else
    /// if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this is a
    /// <c>None</c> result, return a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="selector">A function that maps the value of the incoming result to the value of the outgoing result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="selector"/> is <see langword="null"/>.</exception>
    public static Maybe<TReturn> SelectMany<T, TReturn>(
        this Maybe<T> sourceResult,
        Func<T, Maybe<TReturn>> selector) =>
        sourceResult.FlatMap(selector);

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="resultSelector"/> returns <see langword="null"/> when evaluated.
    ///     </exception>
    public static Maybe<TReturn> SelectMany<T, TIntermediate, TReturn>(
        this Maybe<T> sourceResult,
        Func<T, Maybe<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return sourceResult.FlatMap(
            sourceValue => intermediateSelector(sourceValue).FlatMap(
                intermediateValue =>
                    (resultSelector(sourceValue!, intermediateValue!) ?? throw FunctionMustNotReturnNull(nameof(resultSelector))).ToMaybe()));
    }

    /// <summary>
    /// <para>
    /// Alias for the <see cref="Result{T}.FlatMap{TReturn}(Func{T, Maybe{TReturn}})"/> method.
    /// </para>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="selector"/> function.
    /// Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. A <c>None</c> result is
    /// never returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="selector">A function that maps the value of the incoming result to the value of the outgoing result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="selector"/> is <see langword="null"/>.</exception>
    public static Maybe<TReturn> SelectMany<T, TReturn>(
        this Result<T> sourceResult,
        Func<T, Maybe<TReturn>> selector) =>
        sourceResult.FlatMap(selector);

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="resultSelector"/> returns <see langword="null"/> when evaluated.
    ///     </exception>
    public static Maybe<TReturn> SelectMany<T, TIntermediate, TReturn>(
        this Result<T> sourceResult,
        Func<T, Maybe<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return sourceResult.FlatMap(
            sourceValue => intermediateSelector(sourceValue).FlatMap(
                intermediateValue =>
                    (resultSelector(sourceValue!, intermediateValue!)
                        ?? throw FunctionMustNotReturnNull(nameof(resultSelector))).ToMaybe()));
    }

    /// <summary>
    /// <para>
    /// Alias for the <see cref="Maybe{T}.FlatMap{TReturn}(Func{T, Result{TReturn}}, Func{Error}?)"/>
    /// method.
    /// </para>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="selector"/> function. Else
    /// if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this is a
    /// <c>None</c> result, return a <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="selector">A function that maps the value of the incoming result to the value of the outgoing result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="selector"/> is <see langword="null"/>.</exception>
    public static Result<TReturn> SelectMany<T, TReturn>(
        this Maybe<T> sourceResult,
        Func<T, Result<TReturn>> selector) =>
        sourceResult.FlatMap(selector);

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="resultSelector"/> returns <see langword="null"/> when evaluated.
    ///     </exception>
    public static Result<TReturn> SelectMany<T, TIntermediate, TReturn>(
        this Maybe<T> sourceResult,
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return sourceResult.FlatMap(
            sourceValue => intermediateSelector(sourceValue).FlatMap(
                intermediateValue =>
                    (resultSelector(sourceValue!, intermediateValue!) ?? throw FunctionMustNotReturnNull(nameof(resultSelector))).ToResult()));
    }

    /// <summary>
    /// <para>
    /// Alias for the <see cref="Maybe{T}.Filter(Func{T, bool})"/> method.
    /// </para>
    /// Filter the specified result into a <c>None</c> result if it is a <c>Success</c> result and the
    /// <paramref name="predicate"/> function evaluates to <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="predicate">A function that filters a <c>Success</c> result into a <c>None</c> result by returning
    ///     <see langword="false"/>.</param>
    /// <returns>The filtered result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static Maybe<T> Where<T>(this Maybe<T> sourceResult, Func<T, bool> predicate) =>
        sourceResult.Filter(predicate);
}
