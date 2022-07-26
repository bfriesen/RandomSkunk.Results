namespace RandomSkunk.Results.Dapper;

/// <summary>
/// Defines an error originating from a data source.
/// </summary>
public record class DbError : Error
{
    internal const string _defaultExceptionFailMessage = "A database exception was thrown.";

    /// <summary>
    /// Initializes a new instance of the <see cref="DbError"/> class.
    /// </summary>
    /// <param name="message">The error message. If <see langword="null"/>, then the value of <see cref="Error.DefaultMessage"/>
    ///     is used instead.</param>
    /// <param name="title">The title for the error. If <see langword="null"/>, then the name of the error type is used instead.
    ///     </param>
    /// <param name="setStackTrace">Whether to set the stack trace of the error to the current location.</param>
    public DbError(string? message = null, string? title = null, bool setStackTrace = false)
        : base(message, title, setStackTrace)
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
    /// <param name="errorCode">The optional error code. If <see langword="null"/>, then the
    ///     <see cref="ExternalException.ErrorCode"/> of the exception is used instead.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="title">The optional title for the error. If <see langword="null"/>, then "Error" is used instead.
    ///     </param>
    /// <returns>A new <see cref="Error"/> object.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="exception"/> is <see langword="null"/>.</exception>
    [StackTraceHidden]
    public static Error FromDbException(
        DbException exception,
        string message = _defaultExceptionFailMessage,
        int? errorCode = null,
        string? identifier = null,
        string? title = null)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        Error? innerError = null;
        if (exception.InnerException != null)
        {
            innerError = CreateInnerError(exception.InnerException);
        }

        return new Error(message ?? _defaultExceptionFailMessage, title, true)
        {
            ErrorCode = errorCode,
            Identifier = identifier,
            InnerError = innerError,
        };
    }

    private static Error CreateInnerError(Exception innerException)
    {
        int? errorCode = null;
        if (innerException is ExternalException externalException)
            errorCode = externalException.ErrorCode;

        Error? innerError = null;
        if (innerException.InnerException != null)
            innerError = CreateInnerError(innerException.InnerException);

        if (innerException is DbException dbException)
        {
            return new DbError(dbException.Message, dbException.GetType().Name, true)
            {
                StackTrace = dbException.StackTrace,
                ErrorCode = errorCode,
                InnerError = innerError,
#if NET5_0_OR_GREATER
                IsTransient = dbException.IsTransient,
                SqlState = dbException.SqlState,
#endif
#if NET6_0_OR_GREATER
                BatchCommand = dbException.BatchCommand,
#endif
            };
        }
        else
        {
            return new Error(innerException.Message, innerException.GetType().Name, true)
            {
                StackTrace = innerException.StackTrace,
                ErrorCode = errorCode,
                InnerError = innerError,
            };
        }
    }

    private static DbError CreateInnerDbError(DbException exception)
    {
        return new DbError(exception.Message, exception.GetType().Name)
        {
            StackTrace = exception.StackTrace,
            ErrorCode = exception.ErrorCode,
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
