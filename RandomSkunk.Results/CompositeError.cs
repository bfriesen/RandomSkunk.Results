namespace RandomSkunk.Results;

/// <summary>
/// Defines an error that is composed of more than one error.
/// </summary>
public record class CompositeError : Error
{
    [StackTraceHidden]
    private CompositeError(IReadOnlyList<Error> errors)
        : base("More than one error occurred.", setStackTrace: true, extensions: GetExtensions(nameof(Errors), errors))
    {
    }

    /// <summary>
    /// Gets the errors that make up this instance of <see cref="CompositeError"/>.
    /// </summary>
    public IReadOnlyList<Error> Errors =>
        TryGet<IReadOnlyList<Error>>(nameof(Errors), out var errors)
            ? errors
            : Array.Empty<Error>();

    /// <summary>
    /// Creates an error from the specified sequence of errors.
    /// </summary>
    /// <param name="errors">A sequence of one or more errors.</param>
    /// <returns>If <paramref name="errors"/> contains a single error, that error; otherwise a <see cref="CompositeError"/>
    ///     consisting of the specified errors.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="errors"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="errors"/> is empty.</exception>
    [StackTraceHidden]
    public static Error Create(IEnumerable<Error> errors)
    {
        if (errors is null) throw new ArgumentNullException(nameof(errors));

        var errorList = errors.ToList();

        if (errorList.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(errors), "Sequence must contain at least one error.");

        if (errorList.Count == 1)
            return errorList[0];

        return new CompositeError(errorList);
    }
}
