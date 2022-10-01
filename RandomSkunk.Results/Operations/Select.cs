namespace RandomSkunk.Results;

/// <content> Defines the <c>Select</c> and <c>SelectAsync</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form as a
    /// <c>Success</c> result by passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is
    /// transformed to the new form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the value returned by <paramref name="onSuccessSelector"/>.</typeparam>
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> Select<TReturn>(Func<T, TReturn> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            _successOutcome => onSuccessSelector(_value!).ToResult(),
            _ => Result<TReturn>.Fail(GetError()),
        };
    }

    /// <inheritdoc cref="Select{TReturn}(Func{T, TReturn})"/>
    public async Task<Result<TReturn>> SelectAsync<TReturn>(Func<T, Task<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            _successOutcome => (await onSuccessSelector(_value!).ConfigureAwait(false)).ToResult(),
            _ => Result<TReturn>.Fail(GetError()),
        };
    }
}

/// <content> Defines the <c>Select</c> and <c>SelectAsync</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form as a
    /// <c>Success</c> result by passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is
    /// transformed to the new form as a <c>Fail</c> result with the same error; a <c>None</c> result is transformed to the new
    /// form as a <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the value returned by <paramref name="onSuccessSelector"/>.</typeparam>
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> Select<TReturn>(Func<T, TReturn> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            _successOutcome => onSuccessSelector(_value!).ToMaybe(),
            _noneOutcome => Maybe<TReturn>.None(),
            _ => Maybe<TReturn>.Fail(GetError()),
        };
    }

    /// <inheritdoc cref="Select{TReturn}(Func{T, TReturn})"/>
    public async Task<Maybe<TReturn>> SelectAsync<TReturn>(Func<T, Task<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            _successOutcome => (await onSuccessSelector(_value!).ConfigureAwait(false)).ToMaybe(),
            _noneOutcome => Maybe<TReturn>.None(),
            _ => Maybe<TReturn>.Fail(GetError()),
        };
    }
}

#pragma warning disable CS1712 // Type parameter has no matching typeparam tag in the XML comment (but other type parameters do)
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

/// <content> Defines the <c>Select</c> and <c>SelectAsync</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <inheritdoc cref="Result{T}.Select{TReturn}(Func{T, TReturn})"/>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    public static async Task<Result<TReturn>> Select<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, TReturn> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).Select(onSuccessSelector);

    /// <inheritdoc cref="Result{T}.Select{TReturn}(Func{T, TReturn})"/>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    public static async Task<Result<TReturn>> SelectAsync<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectAsync(onSuccessSelector).ConfigureAwait(false);

    /// <inheritdoc cref="Maybe{T}.Select{TReturn}(Func{T, TReturn})"/>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    public static async Task<Maybe<TReturn>> Select<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, TReturn> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).Select(onSuccessSelector);

    /// <inheritdoc cref="Maybe{T}.Select{TReturn}(Func{T, TReturn})"/>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    public static async Task<Maybe<TReturn>> SelectAsync<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<TReturn>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectAsync(onSuccessSelector).ConfigureAwait(false);

    /// <inheritdoc cref="Result{T}.Select{TReturn}(Func{T, TReturn})"/>
    /// <param name="sourceResult">The source result.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Result<TReturn> Select<TReturn>(
        this IResult<DBNull> sourceResult,
        Func<DBNull, TReturn> onSuccessSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (sourceResult is Result result)
        {
            if (result.IsSuccess)
                return onSuccessSelector(DBNull.Value).ToResult();

            return Result<TReturn>.Fail(result.Error);
        }

        if (sourceResult is Result<DBNull> resultOfDBNull)
            return resultOfDBNull.Select(onSuccessSelector);

        if (sourceResult is Maybe<DBNull> maybeOfDBNull)
            return maybeOfDBNull.SelectMany(dbNull => onSuccessSelector(dbNull).ToResult());

        if (sourceResult.IsSuccess)
            return onSuccessSelector(sourceResult.Value).ToResult();

        return Result<TReturn>.Fail(sourceResult.GetNonSuccessError());
    }

    /// <inheritdoc cref="Result{T}.Select{TReturn}(Func{T, TReturn})"/>
    /// <param name="sourceResult">The source result.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Result<TReturn>> Select<TReturn>(
        this Task<IResult<DBNull>> sourceResult,
        Func<DBNull, TReturn> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).Select(onSuccessSelector);
}

#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
#pragma warning restore CS1712 // Type parameter has no matching typeparam tag in the XML comment (but other type parameters do)
