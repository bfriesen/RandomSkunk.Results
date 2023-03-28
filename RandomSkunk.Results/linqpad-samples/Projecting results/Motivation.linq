<Query Kind="Program">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

void Main()
{
    // Safely performing one result-bearing operation after another is somewhat cumbersome, consisting
    // of a lot of boilerplace code. In this example, we need to make three calls to methods that
    // return a result and show what happened. If the first one fails, we shouldn't call the second
    // or third one, and if the second one fails, we shouldn't call the third one.
    // 
    // What we need is a way to directly project one result into another result, where the success/fail
    // checking is done and if fail the error is passed along. The solution is the LINQ methods Select
    // and SelectMany, which we'll talk about in later LINQPad examples.

    // Get the first result.
    Result<int> firstResult = GetFirstResult();

    if (firstResult.TryGetError(out var error))
    {
        // If the first result is Fail, display the error.
        error.Dump("Fail");
    }
    else
    {
        // If the first result is Success, get the second result.
        Result<(int, bool)> secondResult = GetSecondResult(firstResult.Value);

        if (secondResult.TryGetError(out error))
        {
            // If the second result is Fail, display the error.
            error.Dump("Fail");
        }
        else
        {
            // If the second result is Success, get the final result.
            Result<string> finalResult = GetThirdResult(secondResult.Value);

            if (finalResult.TryGetError(out error))
            {
                // If the final result is Fail, display the error.
                error.Dump("Fail");
            }
            else
            {
                // Display the value of the final result.
                finalResult.Value.Dump("Success");
            }
        }
    }
}

#region Support Code

private static readonly Random _random = new();

private static Result<int> GetFirstResult()
{
    switch (_random.Next(0, 4))
    {
        case 0:
            return Result<int>.Fail("Unlucky first result!");
        default:
            return Result<int>.Success(_random.Next());
    }
}

private static Result<(int, bool)> GetSecondResult(int firstResultValue)
{
    switch (_random.Next(0, 4))
    {
        case 0:
            return Result<(int, bool)>.Fail("Unlucky second result!");
        default:
            return Result<(int, bool)>.Success((firstResultValue, (firstResultValue % 2) == 0));
    }
}

private static Result<string> GetThirdResult((int, bool) secondResultValue)
{
    switch (_random.Next(0, 4))
    {
        case 0:
            return Result<string>.Fail("Unlucky third result!");
        default:
            return Result<string>.Success($"[{secondResultValue.Item1}, {secondResultValue.Item2}]");
    }
}

#endregion