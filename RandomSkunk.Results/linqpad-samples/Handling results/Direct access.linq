<Query Kind="Program">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

void Main()
{
    // Directly accessing the Value or Error of a result is inherently dangerous and the result's
    // outcome *must* be checked prior to doing so. Accessing the Value or Error of a result when
    // it is not valid to do so will result in an InvalidStateException.
    
    // Get some random results - re-run this script to get different results.
    Result result = GetRandomResult();
    Result<string> stringResult = GetRandomStringResult();

    // Handling non-generic Result:
    if (result.IsSuccess)
    {
        "Success Result".Dump("Success Result");
    }
    else
    {
        // Make sure IsFail is true (or that IsSuccess is false) before directly accessing the result's error.
        result.Error.ToString().Dump("Fail Result");
    }
    
    // Handling Result<T>:
    if (stringResult.IsSuccess)
    {
        // Make sure IsSuccess is true (or that IsFail is false) before directly accessing the value.
        stringResult.Value.Dump("Success Result<T>");
    }
    else
    {
        // Make sure IsFail is true (or that IsSuccess is false) before directly accessing the error.
        stringResult.Error.ToString().Dump("Fail Result<T>");
    }
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