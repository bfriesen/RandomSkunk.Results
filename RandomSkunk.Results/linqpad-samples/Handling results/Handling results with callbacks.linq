<Query Kind="Program">
  <NuGetReference Prerelease="true">RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

void Main()
{
    // Sometime, you need to perform some side effects depending on the outcome of the result.
    // The OnSuccess, OnNone (for Maybe<T>), and OnFail methods allow you to accomplish this.
    // Each of these methods returns the same result that it was called in, allowing you to
    // chain these methods together.

    // Get some random results - re-run this script to get different results.
    Result result = GetRandomResult();
    Result<string> stringResult = GetRandomStringResult();
    Maybe<string> stringMaybe = GetRandomStringMaybe();

    
    result
        // The callback function for Result.OnSuccess doesn't have any parameters because Result has no value.
        .OnSuccess(() => "Success Result".Dump("Success Result"))
        
        // The callback function for the OnFail method for Result has an Error parameter, which is the result's error.
        .OnFail(error => error.ToString().Dump("Fail Result"));
    
    stringResult
        // The callback function for Result<T> has a T parameter, which is the value of the result.
        .OnSuccess(value => value.Dump("Success Result<T>"))
        
        // The callback function for the OnFail method for Result<T> has an Error parameter, which is the result's error.
        .OnFail(error => error.ToString().Dump("Fail Result<T>"));
    
    stringMaybe
        // The callback function for Maybe<T> has a T parameter, which is the value of the result.
        .OnSuccess(value => value.Dump("Success Maybe<T>"))
    
        // The callback function for the OnNone method for Maybe<T> doesn't have any parameters, because a None result has no value.
        .OnNone(() => "None Maybe<T>".Dump("None Maybe<T>"))
        
        // The callback function for the OnFail method for Maybe<T> has an Error parameter, which is the result's error.
        .OnFail(error => error.ToString().Dump("Fail Maybe<T>"));
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
            return Maybe<string>.None();
        default:
            return Maybe<string>.Fail();
    }
}

#endregion
