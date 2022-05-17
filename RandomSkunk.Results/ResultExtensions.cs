namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result objects.
/// </summary>
public static partial class ResultExtensions
{
    private static Error Evaluate(this Func<Error, Error>? getError, Error error) =>
        getError is null
            ? error
            : getError(error) ?? throw FunctionMustNotReturnNull(nameof(getError));
}
