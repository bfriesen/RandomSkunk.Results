<Query Kind="Program">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

void Main()
{
    // The Result<T> type has a Select method, used to project the value of one result to another
    // result using a selector function in the form of Func<T, TReturn>. These Select methods are
    // very similar to the LINQ Select for IEnumerable<T>.
    
    // This is the selector method that we'll be using in this example to project a DateTime value to
    // it's "F" (full) formatted string value.
    string ToFullString(DateTime dateTime) => dateTime.ToString("F");
    
    // Get the source results. These methods return results with Success/Fail/None randomly determined.
    Result<DateTime> sourceResult = GetRandomDateTimeResult();

    // Project the Result<DateTime> to a Result<string> by calling the Select method and passing
    // ToFullString as the selector function. If the source result is Success, then its value is
    // passed to the selector function in order to get the value of the Success end result. If the
    // source result is Fail, then the selector is not invoked, instead the Error of the source result
    // becomes the Error of the Fail end result.
    Result<string> endResult = sourceResult.Select(ToFullString);
        
    Display(sourceResult);
    Display(endResult);
}

#region Support Code

private static readonly Random _random = new();

private static Result<DateTime> GetRandomDateTimeResult()
{
    switch (_random.Next(0, 2))
    {
        case 0:
            return Result<DateTime>.Fail("Unlucky result!");
        default:
            return Result<DateTime>.Success(DateTime.Now);
    }
}

private static void Display<T>(Result<T> result, [CallerArgumentExpression(nameof(result))] string? variableName = null)
{
    result.Match(
        onSuccess: value => value!.ToString(),
        onFail: error => error.ToString()).Dump(variableName);
}

#endregion
