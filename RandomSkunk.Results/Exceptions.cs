namespace RandomSkunk.Results;

internal static class Exceptions
{
    public const string CannotAccessErrorUnlessFailMessage = "Result Error cannot be accessed unless it is a Fail result.";
    public const string CannotAccessValueUnlessSuccessMessage = "Result Value cannot be accessed unless it is a Success result.";
    public const string CannotAccessValueUnlessSomeMessage = "Result Value cannot be accessed unless it is a Some result.";
    public const string FunctionMustNotReturnNullMessage = "Function must not return a null value.";

    public static InvalidOperationException CannotAccessErrorUnlessFail => new(CannotAccessErrorUnlessFailMessage);

    public static InvalidOperationException CannotAccessValueUnlessSuccess => new(CannotAccessValueUnlessSuccessMessage);

    public static InvalidOperationException CannotAccessValueUnlessSome => new(CannotAccessValueUnlessSomeMessage);

    public static ArgumentException FunctionMustNotReturnNull(string paramName) => new(FunctionMustNotReturnNullMessage, paramName);
}
