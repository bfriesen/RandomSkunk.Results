<Query Kind="Program">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

void Main()
{
    // The Result<T> type has a SelectMany method, used to project the value of one result to
    // another result using a selector function in the form of Func<T, Result<TReturn>>. These
    // SelectMany methods are very similar to the LINQ SelectMany for IEnumerable<T>.

    // This is the selector that we'll be using in this example to project a string value to a
    // Result<DateTime>. A Success result indicates a successful parsing of the string to DateTime,
    // a Fail result indicates the string could not be parsed into a DateTime, and a None result
    // indicates the string was null or empty.
    Result<DateTime> TryParseAsResult(string? s) =>
        string.IsNullOrEmpty(s)
            ? Result<DateTime>.None()
            : DateTime.TryParse(s, out DateTime value)
                ? Result<DateTime>.Success(value)
                : Result<DateTime>.Fail(Errors.BadRequest($"Invalid DateTime string '{s}'."));

    // Get the source results. These methods return results with Success/Fail/None randomly determined.
    Result<string> sourceResult = GetRandomDateTimeStringResult();

    // Project the Result<string> to a Result<DateTime> by calling the SelectMany method and passing
    // TryParseAsResult as the selector function. If the source result is Success, then its value is
    // passed to the selector function in order to get the end result. If the source result is Fail,
    // the selector is not invoked, instead the Error of the source result becomes the Error of the
    // end result.
    Result<DateTime> endResult = sourceResult.SelectMany(TryParseAsResult);

    Display(sourceResult);
    Display(endResult);
}

#region Support Code

private static readonly Random _random = new();

private static Result<string> GetRandomDateTimeStringResult()
{
    switch (_random.Next(0, 8))
    {
        case 0:
            return Result<string>.Fail("Unlucky result!");
        case 1:
            return Result<string>.None();
        case 2:
            return Result<string>.Success(string.Empty);
        case 3:
            return Result<string>.Success(GetRandomString());
        default:
            return Result<string>.Success(DateTime.Now.ToString("O"));
    }
}

private static string GetRandomString()
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    return new string(Enumerable.Repeat(chars, 20)
        .Select(s => s[_random.Next(s.Length)]).ToArray());
}

private static void Display<T>(Result<T> result, [CallerArgumentExpression(nameof(result))] string? variableName = null)
{
    result.Match(
        onSuccess: value => value!.ToString(),
        onNone: () => "None",
        onFail: error => error.ToString()).Dump(variableName);
}

#endregion
