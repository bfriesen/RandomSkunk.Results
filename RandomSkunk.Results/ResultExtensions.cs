namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result objects.
/// </summary>
public static partial class ResultExtensions
{
    private static Func<Error> _defaultGetNoneError = () => new Error("Not Found", errorCode: 404, type: "NotFoundError");

    /// <summary>
    /// Gets or sets the default value for <c>Func&lt;Error&gt; getNoneError</c> parameters.
    /// </summary>
    public static Func<Error> DefaultGetNoneError
    {
        get => _defaultGetNoneError;
        set => _defaultGetNoneError = value ?? throw new ArgumentNullException(nameof(value));
    }

    private static Error Evaluate(this Func<Error, Error>? getError, Error error) =>
        getError is null
            ? error
            : getError(error) ?? throw FunctionMustNotReturnNull(nameof(getError));

    private static Error Evaluate(this Func<Error>? getNoneError) =>
        getNoneError is null
            ? _defaultGetNoneError() ?? throw FunctionMustNotReturnNull(nameof(_defaultGetNoneError))
            : getNoneError() ?? throw FunctionMustNotReturnNull(nameof(getNoneError));
}
