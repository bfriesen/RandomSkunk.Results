namespace RandomSkunk.Results;

/// <summary>
/// The base class for the result of an operation.
/// </summary>
public abstract class ResultBase
{
    private static string _defaultErrorMessage = "Error";

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultBase"/> class.
    /// </summary>
    /// <param name="callSite">Information about the code that created this result.</param>
    protected ResultBase(CallSite callSite) => CallSite = callSite;

    /// <summary>
    /// Gets or sets the default error message. This value is used when creating a <c>fail</c>
    /// result and an error message is not specified.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// When setting the property, if the value is <see langword="null"/>.
    /// </exception>
    public static string DefaultErrorMessage
    {
        get => _defaultErrorMessage;
        set => _defaultErrorMessage = value ?? throw new ArgumentNullException(nameof(value));
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
    /// Gets the error of the failed operation, or throws an
    /// <see cref="InvalidOperationException"/> if <see cref="IsFail"/> is false.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// If <see cref="IsFail"/> is not true.
    /// </exception>
    public virtual Error Error => throw Exceptions.CannotAccessErrorUnlessFail;

    /// <summary>
    /// Gets information about the code that created this result.
    /// </summary>
    public CallSite CallSite { get; }
}
