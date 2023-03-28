<Query Kind="Program">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

void Main()
{
    // The Result<T> and Maybe<T> types have a SelectMany method, used to project the value of one
    // result to another result using a selector function in the form of Func<T, Result<TReturn>> or
    // Func<T, Maybe<TReturn>>. These SelectMany methods are very similar to the LINQ SelectMany for
    // IEnumerable<T>.

    // These are the selectors that we'll be using in this example to project a string value to a
    // Result<DateTime> or Maybe<DateTime>. A Success result indicates a successful parsing of the
    // string to DateTime and a Fail result indicates the string could not be parsed into a DateTime.
    Result<DateTime> TryParseAsResult(string? s) =>
        DateTime.TryParse(s, out DateTime value)
            ? Result<DateTime>.Success(value)
            : Result<DateTime>.Fail(Errors.BadRequest($"Invalid DateTime string '{s}'."));

    // When parsing as maybe, if the string is null or empty, return None. Otherwise, parse the same
    // as the TryParseAsResult method.
    Maybe<DateTime> TryParseAsMaybe(string? s) =>
        string.IsNullOrEmpty(s)
            ? Maybe<DateTime>.None
            : DateTime.TryParse(s, out DateTime value)
                ? Maybe<DateTime>.Success(value)
                : Maybe<DateTime>.Fail(Errors.BadRequest($"Invalid DateTime string '{s}'."));

    // Get the source results. These methods return results with Success/Fail/None randomly determined.
    Result<string> sourceResult = GetRandomDateTimeStringResult();
    Maybe<string> sourceMaybe = GetRandomDateTimeStringMaybe();

    // Project the Result<string> to a Result<DateTime> by calling the SelectMany method and passing
    // TryParseAsResult as the selector function. If the source result is Success, then its value is
    // passed to the selector function in order to get the end result. If the source result is Fail,
    // the selector is not invoked, instead the Error of the source result becomes the Error of the
    // end result.
    Result<DateTime> endResult = sourceResult.SelectMany(TryParseAsResult);

    // Project the Maybe<string> to a Maybe<DateTime> by calling the SelectMany method and passing
    // TryParseAsMaybe as the selector function. If the source result is Success, then its value is
    // passed to the selector function in order to get the end result. If the source result is Fail,
    // the selector is not invoked, instead the Error of the source result becomes the Error of the
    // end result. If the source result is None, then the end result is also None.
    Maybe<DateTime> endMaybe = sourceMaybe.SelectMany(TryParseAsMaybe);

    // Project the Result<string> to a Maybe<DateTime> by calling the SelectMany method and passing
    // TryParseAsMaybe as the selector function. If the source result is Success, then its value is
    // passed to the selector function in order to get the end result. If the source result is Fail,
    // the selector is not invoked, instead the Error of the source result becomes the Error of the
    // end result.
    Maybe<DateTime> maybeFromResult = sourceResult.SelectMany(TryParseAsMaybe);

    // Project the Maybe<string> to a Result<DateTime> by calling the SelectMany method and passing
    // TryParseAsResult as the selector function. If the source result is Success, then its value is
    // passed to the selector function in order to get the end result. If the source result is Fail,
    // the selector is not invoked, instead the Error of the source result becomes the Error of the
    // end result. If the source result is None, then the end result Fail with an Error indicating
    // there was no value.
    Result<DateTime> resultFromMaybe = sourceMaybe.SelectMany(TryParseAsResult);

    Display(sourceResult);
    Display(sourceMaybe);
    Display(endResult);
    Display(endMaybe);
    Display(maybeFromResult);
    Display(resultFromMaybe);
}

#region Support Code

private static readonly Random _random = new();

private static Result<string> GetRandomDateTimeStringResult()
{
    switch (_random.Next(0, 6))
    {
        case 0:
            return Result<string>.Fail("Unlucky result!");
        case 1:
            return Result<string>.Success(string.Empty);
        case 2:
            return Result<string>.Success(GetRandomString());
        default:
            return Result<string>.Success(DateTime.Now.ToString("O"));
    }
}

private static Maybe<string> GetRandomDateTimeStringMaybe()
{
    switch (_random.Next(0, 8))
    {
        case 0:
            return Maybe<string>.Fail("Unlucky maybe!");
        case 1:
            return Maybe<string>.None;
        case 2:
            return Maybe<string>.Success(string.Empty);
        case 3:
            return Maybe<string>.Success(GetRandomString());
        default:
            return Maybe<string>.Success(DateTime.Now.ToString("O"));
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
        onFail: error => error.ToString()).Dump(variableName);
}

private static void Display<T>(Maybe<T> result, [CallerArgumentExpression(nameof(result))] string? variableName = null)
{
    result.Match(
        onSuccess: value => value!.ToString(),
        onNone: () => "None",
        onFail: error => error.ToString()).Dump(variableName);
}

#endregion
