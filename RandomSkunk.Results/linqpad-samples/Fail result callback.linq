<Query Kind="Program">
  <NuGetReference Prerelease="true">RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

void Main()
{
    // Set the callback function to log the error from every Fail result.
    FailResult.SetCallbackFunction(LogError);

    int dividend = 997;
    int divisor = Util.ReadLine<int>("Enter a number (zero causes an error - try it!)");

    Divide(dividend, divisor)
        .OnSuccess(x => $"{x.Quotient} remainder {x.Remainder}".Dump());
}

private static void LogError(Error error) =>
    error.ToString().Dump("Logged from callback function");

private static Result<(int Quotient, int Remainder)> Divide(int dividend, int divisor)
{
    if (divisor == 0)
        return Result<(int, int)>.Fail("Divisor must not be zero.", ErrorCodes.BadRequest);
    
    var quotient = dividend / divisor;
    var remainder = dividend % divisor;
    
    return Result<(int, int)>.Success((quotient, remainder));
}
