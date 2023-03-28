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
    Maybe<string> stringMaybe = GetRandomStringMaybe();

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

    // Handling Maybe<T>:
    if (stringMaybe.IsSuccess)
    {
        // Make sure IsSuccess is true (or that IsFail and IsNone are both false) before directly accessing the value.
        stringMaybe.Value.Dump("Success Maybe<T>");
    }
    else if (stringMaybe.IsNone)
    {
        "None Maybe<T>".Dump("None Maybe<T>");
    }
    else
    {
        // Make sure IsFail is true (or that IsSuccess and IsNone are both false) before directly accessing the error.
        stringMaybe.Error.ToString().Dump("Fail Maybe<T>");
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