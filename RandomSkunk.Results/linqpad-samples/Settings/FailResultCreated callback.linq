<Query Kind="Program">
  <NuGetReference Prerelease="true">RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

void Main()
{
    // You can set a callback that is invoked every time a Fail result is created.
    // In this example, the callback function "logs" the error from every Fail result.
    ResultSettings.FailResultCreated = LogError;

    // Get some random results - re-run this script to get different results.
    Result result = GetRandomResult()
        .OnSuccess(() => "Success".Dump("Success Result"));

    Result<string> stringResult = GetRandomStringResult()
        .OnSuccess(value => value.Dump("Success Result<T>"));
    
    // Note that the code here in Main() doesn't dump result errors, and neither do any
    // of the methods in the Support Code region.
}

private static void LogError(Error error) =>
    error.ToString().Dump("Logged from callback function");

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
    switch (_random.Next(0, 3))
    {
        case 0:
            return Result<string>.Success("Success Result<T>");
        case 1:
            return Result<string>.None();
        default:
            return Result<string>.Fail("Yikes!");
    }
}

#endregion
