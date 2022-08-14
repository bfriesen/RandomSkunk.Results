namespace RandomSkunk.Results;

/// <content> Defines the <c>HasValue</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Determines whether the value of the result equals the <paramref name="otherValue"/>.
    /// </summary>
    /// <param name="otherValue">The value to compare.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> used to determine equality of the values. If
    ///     <see langword="null"/>, then <see cref="EqualityComparer{T}.Default"/> is used instead.</param>
    /// <returns><see langword="true"/> if this is a <c>Success</c> result and its value equals <paramref name="otherValue"/>;
    ///     otherwise, <see langword="false"/>.</returns>
    public bool HasValue(T otherValue, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        return _outcome == Outcome.Success && comparer.Equals(_value!, otherValue);
    }

    /// <summary>
    /// Determines whether the value of the result is equal to another value as defined by the <paramref name="comparison"/>
    /// function.
    /// </summary>
    /// <param name="comparison">A function that defines the equality of the result value.</param>
    /// <returns><see langword="true"/> if this is a <c>Success</c> result and <paramref name="comparison"/> evaluates to
    ///     <see langword="true"/> when passed its value; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="comparison"/> is <see langword="null"/>.</exception>
    public bool HasValue(Func<T, bool> comparison)
    {
        if (comparison is null) throw new ArgumentNullException(nameof(comparison));

        return _outcome == Outcome.Success && comparison(_value!);
    }
}

/// <content> Defines the <c>HasValue</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Determines whether the value of the result equals the <paramref name="otherValue"/>.
    /// </summary>
    /// <param name="otherValue">The value to compare.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> used to determine equality of the values. If
    ///     <see langword="null"/>, then <see cref="EqualityComparer{T}.Default"/> is used instead.</param>
    /// <returns><see langword="true"/> if this is a <c>Success</c> result and its value equals <paramref name="otherValue"/>;
    ///     otherwise, <see langword="false"/>.</returns>
    public bool HasValue(T otherValue, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        return _outcome == MaybeOutcome.Success && comparer.Equals(_value!, otherValue);
    }

    /// <summary>
    /// Determines whether the value of the result is equal to another value as defined by the <paramref name="comparison"/>
    /// function.
    /// </summary>
    /// <param name="comparison">A function that defines the equality of the result value.</param>
    /// <returns><see langword="true"/> if this is a <c>Success</c> result and <paramref name="comparison"/> evaluates to
    ///     <see langword="true"/> when passed its value; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="comparison"/> is <see langword="null"/>.</exception>
    public bool HasValue(Func<T, bool> comparison)
    {
        if (comparison is null) throw new ArgumentNullException(nameof(comparison));

        return _outcome == MaybeOutcome.Success && comparison(_value!);
    }
}
