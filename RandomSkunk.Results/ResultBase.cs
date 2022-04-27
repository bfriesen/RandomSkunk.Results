namespace RandomSkunk.Results;

/// <summary>
/// The base class for the result of an operation.
/// </summary>
public abstract class ResultBase
{
    private static string _defaultError = "Error";

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultBase"/> class.
    /// </summary>
    /// <param name="callSite">Information about the code that created this result.</param>
    protected ResultBase(CallSite callSite) => CallSite = callSite;

    /// <summary>
    /// Gets or sets the default error message. This value is used when creating a <c>fail</c>
    /// result and an error message is not specified.
    /// </summary>
    public static string DefaultError
    {
        get => _defaultError;
        set => _defaultError = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets a value indicating whether the result object is the result of a successful
    /// operation.
    /// </summary>
    public virtual bool IsSuccess => false;

    /// <summary>
    /// Gets a value indicating whether the result object is the result of a failed operation.
    /// </summary>
    public virtual bool IsFail => false;

    /// <summary>
    /// Gets the error message of the failed operation, or throws an
    /// <see cref="InvalidOperationException"/> if <see cref="IsFail"/> is false.
    /// </summary>
    public virtual string Error => throw new InvalidOperationException("Error cannot be accessed if IsFail is false.");

    /// <summary>
    /// Gets the optional error code of the failed operation, or throws an
    /// <see cref="InvalidOperationException"/> if <see cref="IsFail"/> is false.
    /// </summary>
    public virtual int? ErrorCode => throw new InvalidOperationException("ErrorCode cannot be accessed if IsFail is false.");

    /// <summary>
    /// Gets information about the code that created this result.
    /// </summary>
    public CallSite CallSite { get; }
}
