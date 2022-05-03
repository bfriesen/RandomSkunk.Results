# RandomSkunk.Results.AspNetCore [![NuGet](https://img.shields.io/nuget/vpre/RandomSkunk.Results.AspNetCore.svg)](https://www.nuget.org/packages/RandomSkunk.Results.AspNetCore)

This library contains an extension method - `GetProblemDetails` - for creating a `Microsoft.AspNetCore.Mvc.ProblemDetails` object from a `RandomSkunk.Results.Error` object. This can be in the sad path of a controller method returning `IActionResult`.

```c#
public IActionResult ExampleControllerMethod()
{
    Result result = // Do some work, returning a Result object

    if (result.IsSuccess)
        return Ok();

    int errorCode = result.Error.ErrorCode ?? 500;
    ProblemDetails problemDetails = result.Error.GetProblemDetails();
    return StatusCode(errorCode, problemDetails);
}
```
