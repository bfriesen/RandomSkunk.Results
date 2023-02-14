<Query Kind="Program">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

void Main()
{
    // Each result has methods for safely accessing its Value (if applicable) or Error.
    
    // Get some random results - re-run this script to get different results.
    Result result = GetRandomResult();
    Result<string> stringResult = GetRandomStringResult();
    Maybe<string> stringMaybe = GetRandomStringMaybe();

    // To safely get the error of any type of result, call the TryGetError(out Error) method.
    if (result.TryGetError(out Error? error))
        error.ToString().Dump("Result.TryGetError");
    else
        "No error".Dump("Result.TryGetError");
        
    if (stringResult.TryGetError(out error))
        error.ToString().Dump("Result<T>.TryGetError");
    else
        "No error".Dump("Result<T>.TryGetError");
        
    if (stringMaybe.TryGetError(out error))
        error.ToString().Dump("Maybe<T>.TryGetError");
    else
        "No error".Dump("Maybe<T>.TryGetError");

    // To safely get the value of a Result<T> or Maybe<T>, call the GetValueOr(T) method if you have a fallback value...
    stringResult.GetValueOr("Custom fallback value").Dump("Result<T>.GetValueOr");
    stringMaybe.GetValueOr("Another custom fallback value").Dump("Maybe<T>.GetValueOr");

    // ...or call the GetValueOrDefault() method to fall back to the default value of T.
    stringResult.GetValueOrDefault().Dump("Result<T>.GetValueOrDefault");
    stringMaybe.GetValueOrDefault().Dump("Maybe<T>.GetValueOrDefault");
}

#region Support Code

private static readonly Random _random = new();

private static Result GetRandomResult()
{
    switch (_random.Next(0, 2))
    {
        case 0:
            return Result.Success();
        default:
            return Result.Fail();
    }
}

private static Result<string> GetRandomStringResult()
{
    switch (_random.Next(0, 2))
    {
        case 0:
            return Result<string>.Success("Success Result<T>");
        default:
            return Result<string>.Fail();
    }
}

private static Maybe<string> GetRandomStringMaybe()
{
    switch (_random.Next(0, 3))
    {
        case 0:
            return Maybe<string>.Success("Success Maybe<T>");
        case 1:
            return Maybe<string>.None;
        default:
            return Maybe<string>.Fail();
    }
}

#endregion
