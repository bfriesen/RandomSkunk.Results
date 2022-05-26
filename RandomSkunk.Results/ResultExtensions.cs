namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result objects.
/// </summary>
public static partial class ResultExtensions
{
    private static Func<Error>? _defaultGetNoneError;

    /// <summary>
    /// Gets or sets the default value for <c>Func&lt;Error&gt; getNoneError</c> parameters.
    /// </summary>
    public static Func<Error> DefaultGetNoneError
    {
        get
        {
            if (_defaultGetNoneError is null)
            {
                var defaultError = new Error("Not Found", "NotFoundError") { ErrorCode = 404 };
                _defaultGetNoneError = () => defaultError;
            }

            return _defaultGetNoneError;
        }

        set => _defaultGetNoneError = value ?? throw new ArgumentNullException(nameof(value));
    }

    internal static Error Evaluate(this Func<Error, Error>? getError, Error error) =>
        getError is null
            ? error
            : getError(error);

    internal static Error Evaluate(this Func<Error>? getNoneError) =>
        getNoneError is null
            ? DefaultGetNoneError()
            : getNoneError();
}
