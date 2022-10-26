<Query Kind="Program">
  <NuGetReference Prerelease="true">RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

void Main()
{
    // Set the replace error function to mark the error from every Fail result as sensitive.
    FailResult.SetReplaceErrorFunction(MarkAsSensitive);

    int dividend = 997;
    int divisor = Util.ReadLine<int>("Enter a number (zero causes an error - try it!)");

    string message =
        Divide(dividend, divisor)
            .Match(
                onSuccess: x => $"{x.Quotient} remainder {x.Remainder}",
                
                // For Fail results, we return the string representation of the error. Because the error
                // from every Fail is marked as sensitive, we'll get the abbreviated representation.
                onFail: error => error.ToString());

    message.Dump(nameof(message));
}

private static Error MarkAsSensitive(Error error) =>
    error with { IsSensitive = true };

private static Result<(int Quotient, int Remainder)> Divide(int dividend, int divisor)
{
    if (divisor == 0)
        return Result<(int, int)>.Fail("Divisor must not be zero.", ErrorCodes.BadRequest);

    var quotient = dividend / divisor;
    var remainder = dividend % divisor;

    return Result<(int, int)>.Success((quotient, remainder));
}
