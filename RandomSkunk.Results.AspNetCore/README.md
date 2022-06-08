# RandomSkunk.Results.AspNetCore [![NuGet](https://img.shields.io/nuget/vpre/RandomSkunk.Results.AspNetCore.svg)](https://www.nuget.org/packages/RandomSkunk.Results.AspNetCore)

## IActionResult extension methods

This library defines extension methods to convert each result type - `Result`, `Result<T>`, and `Maybe<T>` - directly into an equivalent `IActionResult`. For `Fail` results of any type, the error is converted to a `ProblemDetails` object (as described below) and used as the content of the returned action result; its HTTP status code comes from the error's error code. For `Success` results of type `Result<T>` and `Some` results of type `Maybe<T>`, the value is used as the content of the returned action result which has a `200 OK` status code. The HTTP status code to use for `Success` and `Some` results can be customized by providing the optional parameter in the `ToActionResult()` extension method.

```c#
// Result
Result result1 = Result.Success();
result1.ToActionResult(); // 200

Result result2 = Result.Fail("Bad Request", errorCode: 400);
result2.ToActionResult(); // 400 { "status": 400, "title": "Bad Request", "type": "Error" }

// Result<T>
Result<int[]> result3 = new[] { 1, 2, 3 }.ToResult();
result3.ToActionResult(); // 200 { 1, 2, 3 }

Result<int[]> result4 = Result<int[]>.Fail("Forbidden", errorCode: 403);
result4.ToActionResult(); // 403 { "status": 403, "title": "Forbidden", "type": "Error" }

// Maybe<T>
Maybe<int[]> result5 = new[] { 4, 5, 6 }.ToMaybe();
result5.ToActionResult(); // 200 { 4, 5, 6 }

Maybe<int[]> result6 = Maybe<int[]>.None();
result6.ToActionResult(); // 404 { "status": 404, "title": "Not Found", "type": "Error" }

Maybe<int[]> result7 = Maybe<int[]>.Fail("Not Acceptable", errorCode: 406);
result7.ToActionResult(); // 406 { "status": 406, "title": "Not Acceptable", "type": "Error" }
```

## GetProblemDetails extension method

The `GetProblemDetails` extension method creates a `Microsoft.AspNetCore.Mvc.ProblemDetails` object from a `RandomSkunk.Results.Error` object. This extension method is called from the `IActionResult` extension methods when for `Fail` results.

```c#
public IActionResult ExampleControllerMethod()
{
    Result result = ... // Do some work, returning a Result object

    return result.Match(
        onSuccess: () => Ok(),
        onFail: error =>
        {
            int errorCode = error.ErrorCode ?? 500;
            ProblemDetails problemDetails = error.GetProblemDetails();
            return StatusCode(errorCode, problemDetails);
        });
}
```
