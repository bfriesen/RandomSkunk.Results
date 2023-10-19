using Microsoft.JSInterop;
using RandomSkunk.Results;

namespace ExampleBlazorApp.Client;

// This class demonstrates how an app can create common error-handling extension methods for use throughout the app.
public static class ResultErrorHandlingExtensions
{
    public static Task<Result> OnFailLogAndAlert(this Result sourceResult, IJSRuntime js) =>
        sourceResult.OnFail(error => LogAndAlert(error, js));

    public static async Task<Result> OnFailLogAndAlert(this Task<Result> sourceResult, IJSRuntime js) =>
        await (await sourceResult).OnFailLogAndAlert(js);

    public static Task<Result<T>> OnFailLogAndAlert<T>(this Result<T> sourceResult, IJSRuntime js) =>
        sourceResult.OnFail(error => LogAndAlert(error, js));

    public static async Task<Result<T>> OnFailLogAndAlert<T>(this Task<Result<T>> sourceResult, IJSRuntime js) =>
        await (await sourceResult).OnFailLogAndAlert(js);

    private static async Task LogAndAlert(Error error, IJSRuntime js)
    {
        await js.InvokeVoidAsync("console.log", error.ToString());
        await js.InvokeVoidAsync("alert", error.Message);
    }
}
