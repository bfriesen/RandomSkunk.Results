namespace RandomSkunk.Results;

internal static class Exceptions
{
    public const string CannotAccessErrorUnlessFailMessage = $"{nameof(ResultBase.Error)} cannot be accessed unless {nameof(ResultBase.IsFail)} is true.";
    public const string CannotAccessValueUnlessSuccessMessage = $"{nameof(ResultBase<int>.Value)} cannot be accessed unless {nameof(ResultBase.IsSuccess)} is true.";
    public const string CannotAccessValueUnlessSomeMessage = $"{nameof(ResultBase<int>.Value)} cannot be accessed unless {nameof(MaybeResult<int>.IsSome)} is true.";
    public const string FunctionMustNotReturnNullMessage = "Function must not return null value.";

    public static InvalidOperationException CannotAccessErrorUnlessFail => new(CannotAccessErrorUnlessFailMessage);

    public static InvalidOperationException CannotAccessValueUnlessSuccess => new(CannotAccessValueUnlessSuccessMessage);

    public static InvalidOperationException CannotAccessValueUnlessSome => new(CannotAccessValueUnlessSomeMessage);

    public static ArgumentException FunctionMustNotReturnNull(string paramName) => new(FunctionMustNotReturnNullMessage, paramName);
}
