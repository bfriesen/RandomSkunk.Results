using static RandomSkunk.Results.MaybeType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Match</c> and <c>MatchAsync</c> methods.
/// </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The non-null value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public TReturn Match<TReturn>(
        Func<T, TReturn> some,
        Func<TReturn> none,
        Func<Error, TReturn> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return _type switch
        {
            Some => some(_value!),
            None => none(),
            _ => fail(Error()),
        };
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The non-null value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public void Match(
        Action<T> some,
        Action none,
        Action<Error> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        switch (_type)
        {
            case Some: some(_value!); break;
            case None: none(); break;
            default: fail(Error()); break;
        }
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The non-null value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public Task<TReturn> MatchAsync<TReturn>(
        Func<T, Task<TReturn>> some,
        Func<Task<TReturn>> none,
        Func<Error, Task<TReturn>> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return _type switch
        {
            Some => some(_value!),
            None => none(),
            _ => fail(Error()),
        };
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The non-null value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The non-null error of the
    /// <c>Fail</c> result is passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public Task MatchAsync(
        Func<T, Task> some,
        Func<Task> none,
        Func<Error, Task> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return _type switch
        {
            Some => some(_value!),
            None => none(),
            _ => fail(Error()),
        };
    }
}
