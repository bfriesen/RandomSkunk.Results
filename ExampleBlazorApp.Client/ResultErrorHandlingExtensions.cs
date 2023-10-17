using Microsoft.JSInterop;
using RandomSkunk.Results;

namespace ExampleBlazorApp.Client;

// This class demonstrates how an app can create common error-handling extension methods for use throughout the app.
public static class ResultErrorHandlingExtensions
{
    public static Task<TResult> OnFailLogAndAlert<TResult>(this TResult sourceResult, IJSRuntime js)
        where TResult : IResult =>
        sourceResult.OnFail(async error =>
        {
            await js.InvokeVoidAsync("console.log", error.ToString());
            await js.InvokeVoidAsync("alert", error.Message);
        });

    public static async Task<TResult> OnFailLogAndAlert<TResult>(this Task<TResult> sourceResult, IJSRuntime js)
        where TResult : IResult =>
        await (await sourceResult).OnFailLogAndAlert(js);
}
