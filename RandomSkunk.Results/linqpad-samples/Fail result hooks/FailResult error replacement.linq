<Query Kind="Program">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

void Main()
{
    // You can set an error replacement function that is invoked every time a Fail result is created. In
    // this example, the replace error function returns a copy of the error with IsSensitive set to true.
    // Note that none of the methods in the Support Code region set IsSensitive to true.
    FailResult.SetReplaceErrorFunction(EnsureIsSensitive);

    // Get some random results - re-run this script to get different results.
    Result result = GetRandomResult();
    Result<string> stringResult = GetRandomStringResult();
    Maybe<string> stringMaybe = GetRandomStringMaybe();

    if (result.IsFail)
        result.Error.ToString().Dump("Fail Result");

    if (stringResult.IsFail)
        stringResult.Error.ToString().Dump("Fail Result<T>");

    if (stringMaybe.IsFail)
        stringMaybe.Error.ToString().Dump("Fail Maybe<T>");
}

/// <summary>Returns an equivalent error with its IsSensitive property set to true.</summary>
private static Error EnsureIsSensitive(Error error) =>
    error.IsSensitive
        ? error
        : error with { IsSensitive = true };

#region Support Code

private static readonly Random _random = new();

private static Result GetRandomResult()
{
    switch (_random.Next(0, 2))
    {
        case 0:
            return Result.Success();
        default:
            return Result.Fail("Oh, no!");
    }
}

private static Result<string> GetRandomStringResult()
{
    switch (_random.Next(0, 2))
    {
        case 0:
            return Result<string>.Success("Success Result<T>");
        default:
            return Result<string>.Fail("Oof.");
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
            return Maybe<string>.Fail("Yikes!");
    }
}

#endregion
