namespace RandomSkunk.Results;

internal static class Exceptions
{
    public const string CannotAccessErrorUnlessFailMessage = $"{nameof(ResultBase.Error)} cannot be accessed unless {nameof(ResultBase.IsFail)} is true.";
    public const string CannotAccessValueUnlessSuccessMessage = $"{nameof(ResultBase<int>.Value)} cannot be accessed unless {nameof(ResultBase.IsSuccess)} is true.";
    public const string CannotAccessValueUnlessSomeMessage = $"{nameof(ResultBase<int>.Value)} cannot be accessed unless {nameof(MaybeResult<int>.IsSome)} is true.";

    public static Exception CannotAccessErrorUnlessFail => new InvalidOperationException(CannotAccessErrorUnlessFailMessage);

    public static Exception CannotAccessValueUnlessSuccess => new InvalidOperationException(CannotAccessValueUnlessSuccessMessage);

    public static Exception CannotAccessValueUnlessSome => new InvalidOperationException(CannotAccessValueUnlessSomeMessage);
}
