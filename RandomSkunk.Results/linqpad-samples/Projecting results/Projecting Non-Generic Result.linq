<Query Kind="Program">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

void Main()
{
    // The non-generic Result type has SelectMany methods similar to those of Result<T>. Since Result
    // has no value, the selector function for this method does not have a parameter.

    Result sourceResult = GetNonGenericResult();
    Result<int> sourceResultOfInt = GetResultOfInt();
    Maybe<int> sourceMaybeOfInt = GetMaybeOfInt();

    // Projecting from Result to another Result.
    Result resultFromResult = sourceResult
        .SelectMany(() => GetNonGenericResult());

    // Projecting from a Result to Result<int>.
    Result<int> resultOfIntFromResult = sourceResult
        .SelectMany(() => GetResultOfInt());
    
    // Projecting from Result to Maybe<int>.
    Maybe<int> maybeOfIntFromResult = sourceResult
        .SelectMany(() => GetMaybeOfInt());

    // The SelectMany methods of Result<T> and Maybe<T> have overloads that return Result.

    // Projecting from Result<int> to Result.
    Result resultFromResultOfInt = sourceResultOfInt
        .SelectMany((int value) => GetNonGenericResult());
        
    // Projecting from Maybe<int> to Result.
    Result resultFromMaybeOfInt = sourceMaybeOfInt
        .SelectMany((int value) => GetNonGenericResult());

    Display(sourceResult);
    Display(sourceResultOfInt);
    Display(sourceMaybeOfInt);
    Display(resultFromResult);
    Display(resultOfIntFromResult);
    Display(maybeOfIntFromResult);
    Display(resultFromResultOfInt);
    Display(resultFromMaybeOfInt);
}

#region Support Code

private static readonly Random _random = new();

private static Result GetNonGenericResult()
{
    switch (_random.Next(0, 3))
    {
        case 0:
            return Result.Fail("Unlucky Result!");
        default:
            return Result.Success();
    }
}

private static Result<int> GetResultOfInt()
{
    switch (_random.Next(0, 3))
    {
        case 0:
            return Result<int>.Fail("Unlucky Result<int>!");
        default:
            return Result<int>.Success(_random.Next());
    }
}

private static Maybe<int> GetMaybeOfInt()
{
    switch (_random.Next(0, 6))
    {
        case 0:
            return Maybe<int>.Fail("Unlucky Maybe<int>!");
        case 1:
            return Maybe<int>.None;
        default:
            return Maybe<int>.Success(_random.Next());
    }
}

private static void Display(Result result, [CallerArgumentExpression(nameof(result))] string? variableName = null)
{
    result.Match(
        onSuccess: () => "Success!",
        onFail: error => error.ToString()).Dump(variableName);
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
