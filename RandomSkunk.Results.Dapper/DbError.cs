namespace RandomSkunk.Results.Dapper;

/// <summary>
/// Defines an error originating from a data source.
/// </summary>
public record class DbError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbError"/> class.
    /// </summary>
    /// <param name="message">
    /// The error message. If <see langword="null"/>, then the value of <see cref="Error.DefaultMessage"/> is used instead.
    /// </param>
    /// <param name="type">
    /// The type of the error. If <see langword="null"/>, then the name of the error type is used instead.
    /// </param>
    public DbError(string? message = null, string? type = null)
        : base(message, type)
    {
    }

#if NET5_0_OR_GREATER

    /// <summary>
    /// Gets a value indicating whether the error represented by this <see cref="DbError"/> could be a transient error, i.e. if
    /// retrying the triggering operation may succeed without any other change.
    /// </summary>
    public bool IsTransient { get; init; }

    /// <summary>
    /// Gets a standard SQL 5-character return code indicating the success or failure of the database operation, for database
    /// providers which support it. The first 2 characters represent the class of the return code (e.g. error, success), while
    /// the last 3 characters represent the subclass, allowing detection of error scenarios in a database-portable way. For
    /// database providers which don't support it, or for inapplicable error scenarios, contains <see langword="null"/>.
    /// </summary>
    public string? SqlState { get; init; }

#endif
#if NET6_0_OR_GREATER

    /// <summary>
    /// Gets the specific DbBatchCommand which triggered this DbError, if it was generated when executing a
    /// <see cref="DbBatch"/>.
    /// </summary>
    public DbBatchCommand? BatchCommand { get; init; }

#endif

    /// <summary>
    /// Creates an <see cref="DbError"/> object from the specified <see cref="DbException"/>.
    /// </summary>
    /// <param name="exception">The exception to create the error from.</param>
    /// <param name="message">The error message.</param>
    /// <param name="errorCode">
    /// The optional error code. If <see langword="null"/>, then the <see cref="ExternalException.ErrorCode"/> of the exception
    /// is used instead.
    /// </param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <returns>A new <see cref="Error"/> object.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="exception"/> is <see langword="null"/>.
    /// </exception>
    public static DbError FromDbException(
        DbException exception,
        string? message = null,
        int? errorCode = null,
        string? identifier = null)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        var exceptionMessage = $"{exception.GetType().Name}: {exception.Message}";

        if (string.IsNullOrWhiteSpace(message))
            message = exceptionMessage;
        else
            message += Environment.NewLine + exceptionMessage;

        Error? innerError = null;
        if (exception.InnerException != null)
        {
            if (exception.InnerException is DbException innerDbException)
                innerError = FromDbException(innerDbException);
            else
                innerError = FromException(exception.InnerException);
        }

        return new DbError(message, exception.GetType().Name)
        {
            StackTrace = exception.StackTrace,
            ErrorCode = errorCode ?? exception.ErrorCode,
            Identifier = identifier,
            InnerError = innerError,
#if NET5_0_OR_GREATER
            IsTransient = exception.IsTransient,
            SqlState = exception.SqlState,
#endif
#if NET6_0_OR_GREATER
            BatchCommand = exception.BatchCommand,
#endif
        };
    }
}
