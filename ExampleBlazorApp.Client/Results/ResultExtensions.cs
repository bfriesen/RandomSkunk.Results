using Microsoft.JSInterop;
using RandomSkunk.Results;

namespace ExampleBlazorApp.Client.Results;

// These extension methods demonstrate how an app can create common error-handling extension methods.
public static class ResultExtensions
{
    public static Task<TResult> OnNonSuccessShowAlert<TResult>(this TResult source, IJSRuntime js)
        where TResult : IResult =>
        source.OnNonSuccessAsync(async error => await js.InvokeVoidAsync("alert", source.GetNonSuccessError().Message));

    public static async Task<TResult> OnNonSuccessShowAlert<TResult>(this Task<TResult> source, IJSRuntime js)
        where TResult : IResult =>
        await (await source).OnNonSuccessShowAlert(js);
}
