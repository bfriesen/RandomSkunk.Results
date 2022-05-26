using RandomSkunk.Results.Unsafe;

namespace RandomSkunk.Results;

internal static class Exceptions
{
    public const string CannotAccessErrorUnlessFailMessage = "Result error cannot be accessed unless it is a Fail result.";
    public const string CannotAccessValueUnlessSuccessMessage = "Result value cannot be accessed unless it is a Success result.";
    public const string CannotAccessValueUnlessSomeMessage = "Result value cannot be accessed unless it is a Some result.";
    public const string FunctionMustNotReturnNullMessage = "Function must not return a null value.";

    public static InvalidStateException CannotAccessErrorUnlessFail() => new(CannotAccessErrorUnlessFailMessage);

    public static InvalidStateException CannotAccessValueUnlessSuccess(Error error) => new(CannotAccessValueUnlessSuccessMessage, error);

    public static InvalidStateException CannotAccessValueUnlessSome(Error? error = null) => new(CannotAccessValueUnlessSomeMessage, error);

    public static ArgumentException FunctionMustNotReturnNull(string paramName) => new(FunctionMustNotReturnNullMessage, paramName);
}
