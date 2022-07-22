using Microsoft.JSInterop;
using RandomSkunk.Results;

namespace ExampleBlazorApp.Client;

// This class demonstrates how an app can create common error-handling extension methods for use throughout the app.
public static class ResultErrorHandlingExtensions
{
    public static Task<TResult> OnNonSuccessLogAndAlert<TResult>(this TResult sourceResult, IJSRuntime js)
        where TResult : IResult =>
        sourceResult.OnNonSuccessAsync(async error =>
        {
            await js.InvokeVoidAsync("console.log", error.ToString());
            await js.InvokeVoidAsync("alert", error.Message);
        });

    public static async Task<TResult> OnNonSuccessLogAndAlert<TResult>(this Task<TResult> sourceResult, IJSRuntime js)
        where TResult : IResult =>
        await (await sourceResult).OnNonSuccessLogAndAlert(js);
}
