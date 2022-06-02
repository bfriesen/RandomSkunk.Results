namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Flatten</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Result<T> Flatten<T>(this Result<Result<T>> source) =>
        source.FlatMap(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Result Flatten<T>(this Result<Result> source) =>
        source.CrossMap(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Maybe<T> Flatten<T>(this Result<Maybe<T>> source) =>
        source.CrossMap(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> source) =>
        source.FlatMap(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Result Flatten<T>(this Maybe<Result> source) =>
        source.CrossMap(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Result<T> Flatten<T>(this Maybe<Result<T>> source) =>
        source.CrossMap(nestedResult => nestedResult);
}
