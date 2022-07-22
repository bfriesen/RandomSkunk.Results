namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result objects.
/// </summary>
public static partial class ResultExtensions
{
    private static readonly Func<Error> _defaultOnNoneCallback = () => Errors.NotFound(errorIdentifier: "A2238E9C-83FF-4540-A37B-8D681E27ED35");

    internal static Error Evaluate(this Func<Error, Error>? onError, Error error) =>
        onError is null
            ? error
            : onError(error);

    internal static Error EvaluateOnNone(this Func<Error>? onNone) =>
        onNone is null
            ? _defaultOnNoneCallback()
            : onNone();
}
