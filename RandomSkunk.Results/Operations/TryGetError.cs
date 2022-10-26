namespace RandomSkunk.Results;

/// <content> Defines the <c>TryGetError</c> method. </content>
public partial struct Result
{
    /// <summary>
    /// Attempts to get the error of the result, returning whether this is a <c>Fail</c> result (and therefore has an error).
    /// </summary>
    /// <param name="error">When this method returns, contains the <see cref="Results.Error"/> of the <c>Fail</c> result, or
    ///     <see langword="null"/> if this is not a <c>Fail</c> result. This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if this is a <c>Fail</c> result; otherwise <see langword="false"/>.</returns>
    public bool TryGetError([NotNullWhen(true)] out Error? error)
    {
        if (_outcome == Outcome.Fail)
        {
            error = GetError();
            return true;
        }

        error = null;
        return false;
    }
}

/// <content> Defines the <c>TryGetError</c> method. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Attempts to get the error of the result, returning whether this is a <c>Fail</c> result (and therefore has an error).
    /// </summary>
    /// <param name="error">When this method returns, contains the <see cref="Results.Error"/> of the <c>Fail</c> result, or
    ///     <see langword="null"/> if this is not a <c>Fail</c> result. This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if this is a <c>Fail</c> result; otherwise <see langword="false"/>.</returns>
    public bool TryGetError([NotNullWhen(true)] out Error? error)
    {
        if (_outcome == Outcome.Fail)
        {
            error = GetError();
            return true;
        }

        error = null;
        return false;
    }
}

/// <content> Defines the <c>TryGetError</c> method. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Attempts to get the error of the result, returning whether this is a <c>Fail</c> result (and therefore has an error).
    /// </summary>
    /// <param name="error">When this method returns, contains the <see cref="Results.Error"/> of the <c>Fail</c> result, or
    ///     <see langword="null"/> if this is not a <c>Fail</c> result. This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if this is a <c>Fail</c> result; otherwise <see langword="false"/>.</returns>
    public bool TryGetError([NotNullWhen(true)] out Error? error)
    {
        if (_outcome == Outcome.Fail)
        {
            error = GetError();
            return true;
        }

        error = null;
        return false;
    }
}
