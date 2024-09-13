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

    // To safely get the error of any type of result, call the TryGetError(out Error) method.
    if (result.TryGetError(out Error? error))
        error.ToString().Dump("Result.TryGetError");
    else
        "No error".Dump("Result.TryGetError");
        
    if (stringResult.TryGetError(out error))
        error.ToString().Dump("Result<T>.TryGetError");
    else
        "No error".Dump("Result<T>.TryGetError");

    // To safely get the value of a Result<T>, call the GetValueOr(T) method if you have a fallback value...
    stringResult.GetValueOr("Custom fallback value").Dump("Result<T>.GetValueOr");

    // ...or call the GetValueOrDefault() method to fall back to the default value of T.
    stringResult.GetValueOrDefault().Dump("Result<T>.GetValueOrDefault");
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

#endregion
