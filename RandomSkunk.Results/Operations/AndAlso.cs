namespace RandomSkunk.Results;

/// <content> Defines the <c>AndAlso</c> methods. </content>
public partial struct Result
{
    /// <summary>
    /// Combines this result with another result if this is a <c>Success</c> result; otherwise, return this <c>Fail</c> result.
    /// </summary>
    /// <param name="onSuccess">A function returning another <see cref="Result"/> that is only evaluated if this is a
    ///     <c>Success</c> result.</param>
    /// <returns>The combined result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/>.</exception>
    public Result AndAlso(Func<Result> onSuccess)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

        return _outcome switch
        {
            _successOutcome => onSuccess(),
            _ => this,
        };
    }

    /// <inheritdoc cref="AndAlso(Func{Result})"/>
    public async Task<Result> AndAlso(Func<Task<Result>> onSuccess)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

        return _outcome switch
        {
            _successOutcome => await onSuccess().ConfigureAwait(false),
            _ => this,
        };
    }
}
