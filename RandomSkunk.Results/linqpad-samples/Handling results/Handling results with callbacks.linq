<Query Kind="Program">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

void Main()
{
    // Sometimes, you need to perform some side effects depending on the outcome of the result.
    // The OnSuccess, OnNone, OnFail, and OnFailExcludingNone methods allow you to accomplish this.
    // Each of these methods returns the same result that it was called in, allowing you to
    // chain these methods together.

    // Get some random results - re-run this script to get different results.
    Result result = GetRandomResult();
    Result<string> stringResult = GetRandomStringResult();

    
    result
        // The callback function for Result.OnSuccess doesn't have any parameters because Result has no value.
        .OnSuccess(() => "Success Result".Dump("Success Result"))
        
        // The callback function for the OnFail method for Result has an Error parameter, which is the result's error.
        .OnFail(error => error.ToString().Dump("Fail Result"));
    
	// This differentiates between Fail and None results.
    stringResult
        // The callback function for Result<T> has a T parameter, which is the value of the result.
        .OnSuccess(value => value.Dump("Success Result<T>"))

        // The callback function for the OnNone method for Result<T> doesn't have any parameters, because a None result has no value.
        .OnNone(() => "None Result<T>".Dump("None Result<T>"))

        // The callback function for the OnFailExcludingNone method for Result<T> has an Error parameter, which is the result's error.
        .OnFailExcludingNone(error => error.ToString().Dump("Fail Result<T> Excluding None"));
	
	// This does not differentiate between Fail and None results.
    stringResult
        // The callback function for Result<T> has a T parameter, which is the value of the result.
        .OnSuccess(value => value.Dump("Success Result<T>"))

        // The callback function for the OnFail method for Result<T> has an Error parameter, which is the result's error.
        .OnFail(error => error.ToString().Dump("Fail Result<T> Including None"));
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
    switch (_random.Next(0, 3))
    {
        case 0:
            return Result<string>.Success("Success Result<T>");
        case 1:
            return Result<string>.None();
        default:
            return Result<string>.Fail();
    }
}

#endregion
