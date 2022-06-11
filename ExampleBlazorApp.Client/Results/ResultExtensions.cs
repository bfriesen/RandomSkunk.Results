using Microsoft.JSInterop;
using RandomSkunk.Results;

namespace ExampleBlazorApp.Client.Results;

public static class ResultExtensions
{
    public static async Task<Result<T>> OnFailShowAlert<T>(this Result<T> source, IJSRuntime js) =>
        await source.OnFailAsync(async error => await js.InvokeVoidAsync("alert", error.Message));

    public static async Task<Result<T>> OnFailShowAlert<T>(this Task<Result<T>> source, IJSRuntime js) =>
        await (await source).OnFailShowAlert(js);
}
