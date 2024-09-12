namespace RandomSkunk.Results;

/// <summary>
/// Defines an error that is composed of more than one error.
/// </summary>
public record class CompositeError : Error
{
    private static readonly string _errorsFieldFullName =
        $"{GetTypeFullName(typeof(CompositeError))}.{nameof(Errors)}";

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeError"/> class.
    /// </summary>
    /// <param name="errors">The errors that make up this instance of <see cref="CompositeError"/>.</param>
    /// <param name="messageDetail">The message of the composite error.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="errors"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="errors"/> does not contain at least two errors.</exception>
    public CompositeError(IEnumerable<Error> errors, string? messageDetail = null)
        : base((_errorsFieldFullName, errors?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(errors))))
    {
        if (Errors.Count < 2)
            throw new ArgumentException("Sequence must contain at least two errors.", nameof(errors));

        var defaultMessage = $"{GetNumberName(Errors.Count)} errors occurred.";
        var message = string.IsNullOrEmpty(messageDetail) ? defaultMessage : defaultMessage + ' ' + messageDetail;
        Message = message;
    }

    /// <summary>
    /// Gets the errors that make up this instance of <see cref="CompositeError"/>.
    /// </summary>
    public IReadOnlyList<Error> Errors =>
        TryGet<IReadOnlyList<Error>>(_errorsFieldFullName, out var errors) ? errors : [];

    /// <summary>
    /// Creates a composite error from the specified non-empty sequence of errors if it contains more than one error, otherwise
    /// returns the single error.
    /// </summary>
    /// <param name="errors">A sequence of one or more errors.</param>
    /// <returns>If <paramref name="errors"/> contains a single error, that error; otherwise a <see cref="CompositeError"/>
    ///     consisting of the specified errors.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="errors"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="errors"/> is empty.</exception>
    public static Error CreateOrGetSingle(IEnumerable<Error> errors)
    {
        if (errors is null) throw new ArgumentNullException(nameof(errors));

        var errorList = errors.ToList();

        if (errorList.Count < 1)
            throw new ArgumentException("Sequence must contain at least one error.", nameof(errors));

        if (errorList.Count == 1)
            return errorList[0];

        return new CompositeError(errorList);
    }

    private static string GetNumberName(int number) =>
        number switch
        {
            2 => "Two",
            3 => "Three",
            4 => "Four",
            5 => "Five",
            6 => "Six",
            7 => "Seven",
            8 => "Eight",
            9 => "Nine",
            _ => number.ToString(),
        };
}
