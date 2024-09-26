namespace RandomSkunk.Results;

/// <summary>
/// Defines an error that is composed of more than one error.
/// </summary>
public record class AggregateError : Error
{
    internal static readonly string _innerErrorsFieldFullName =
        $"{GetTypeFullName(typeof(AggregateError))}.{nameof(InnerErrors)}";

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateError"/> class.
    /// </summary>
    /// <param name="innerErrors">The multiple errors that caused the current error.</param>
    /// <param name="messageDetail">The message of the aggregate error.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="innerErrors"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="innerErrors"/> does not contain at least two errors.
    ///     </exception>
    public AggregateError(IEnumerable<Error> innerErrors, string? messageDetail = null)
        : base((_innerErrorsFieldFullName, innerErrors?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(innerErrors))))
    {
        if (InnerErrors.Count < 2)
            throw new ArgumentException("Sequence must contain at least two errors.", nameof(innerErrors));

        var defaultMessage = $"{GetNumberName(InnerErrors.Count)} errors occurred.";
        var message = string.IsNullOrEmpty(messageDetail) ? defaultMessage : defaultMessage + ' ' + messageDetail;
        Message = message;
    }

    /// <summary>
    /// Gets the multiple <see cref="Error"/> instances that caused the current error.
    /// </summary>
    public IReadOnlyList<Error> InnerErrors =>
        TryGet<IReadOnlyList<Error>>(_innerErrorsFieldFullName, out var errors) ? errors : [];

    /// <summary>
    /// Creates a aggregate error from the specified non-empty sequence of errors if it contains more than one error, otherwise
    /// returns the single error.
    /// </summary>
    /// <param name="errors">A sequence of one or more errors.</param>
    /// <returns>If <paramref name="errors"/> contains a single error, that error; otherwise a <see cref="AggregateError"/>
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

        return new AggregateError(errorList);
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
