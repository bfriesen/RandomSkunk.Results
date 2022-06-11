namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result objects.
/// </summary>
public static partial class ResultExtensions
{
    private static Func<Error> _defaultOnNoneCallback = DefaultOnNone;

    /// <summary>
    /// Gets or sets the default value for <c>Func&lt;Error&gt; onNone</c> parameters.
    /// </summary>
    public static Func<Error> DefaultOnNoneCallback
    {
        get => _defaultOnNoneCallback;
        set => _defaultOnNoneCallback = value ?? throw new ArgumentNullException(nameof(value));
    }

    internal static Error Evaluate(this Func<Error, Error>? onError, Error error) =>
        onError is null
            ? error
            : onError(error);

    internal static Error EvaluateOnNone(this Func<Error>? onNone) =>
        onNone is null
            ? _defaultOnNoneCallback()
            : onNone();

    private static Error DefaultOnNone() => new("Not Found", "NotFoundError") { ErrorCode = 404, StackTrace = new StackTrace(true).ToString() };
}
