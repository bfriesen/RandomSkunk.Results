# RandomSkunk.Results.Http [![NuGet](https://img.shields.io/nuget/vpre/RandomSkunk.Results.Http.svg)](https://www.nuget.org/packages/RandomSkunk.Results.Http)

This library contains three extension methods - `ReadResultFromJsonAsync`, `ReadResultFromJsonAsync<T>`, and `ReadMaybeResultFromJsonAsync<T>` - for creating result objects (`Result`, `Result<T>`, and `MaybeResult<T>` respectively) from an `HttpResponseMessage`.

```c#
public async Task<Result> ExampleResultMethod()
{
    HttpClient httpClient = new HttpClient();
    HttpResponseMessage response = await httpClient.GetAsync("https://example.com");

    Result result = await response.ReadResultFromJsonAsync();
    return result;
}

public async Task<Result<T>> ExampleResultMethod<T>()
{
    HttpClient httpClient = new HttpClient();
    HttpResponseMessage response = await httpClient.GetAsync("https://example.com");

    Result<T> result = await response.ReadResultFromJsonAsync<T>();
    return result;
}

public async Task<MaybeResult<T>> ExampleMaybeResultMethod<T>()
{
    HttpClient httpClient = new HttpClient();
    HttpResponseMessage response = await httpClient.GetAsync("https://example.com");

    MaybeResult<T> result = await response.ReadMaybeResultFromJsonAsync<T>();
    return result;
}
```
