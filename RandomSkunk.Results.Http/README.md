# RandomSkunk.Results.Http [![NuGet](https://img.shields.io/nuget/vpre/RandomSkunk.Results.Http.svg)](https://www.nuget.org/packages/RandomSkunk.Results.Http)

## HttpClient extension methods

To make it easier to get a result object representing an HTTP operation, several extension methods for `HttpClient` are provided.

```c#
public async Task<Maybe<T>> ExampleTryGetFromJsonAsync<T>()
{
    HttpClient httpClient = new HttpClient();
    Maybe<T> result = await httpClient.TryGetFromJsonAsync<T>("http://example.com");
    return result;
}

public async Task<Result> ExampleTryPostAsJsonAsync<T>(T value)
{
    HttpClient httpClient = new HttpClient();
    Result<HttpResponseMessage> result = await httpClient.TryPostAsJsonAsync("http://example.com", value);
    return await result.EnsureSuccessStatusCodeAsync();
}

public async Task<Result> ExampleTryPatchAsJsonAsync<T>(T value)
{
    HttpClient httpClient = new HttpClient();
    Result<HttpResponseMessage> result = await httpClient.TryPatchAsJsonAsync("http://example.com", value);
    return await result.EnsureSuccessStatusCodeAsync();
}

public async Task<Result> ExampleTryPutAsJsonAsync<T>(T value)
{
    HttpClient httpClient = new HttpClient();
    Result<HttpResponseMessage> result = await httpClient.TryPutAsJsonAsync("http://example.com", value);
    return await result.EnsureSuccessStatusCodeAsync();
}

public async Task<Result> ExampleTryDeleteAsync()
{
    HttpClient httpClient = new HttpClient();
    Result<HttpResponseMessage> result = await httpClient.TryDeleteAsync("http://example.com");
    return await result.EnsureSuccessStatusCodeAsync();
}

public async Task<Result<HttpResponseMessage>> ExampleTrySendAsync(
    HttpRequestMessage request)
{
    HttpClient httpClient = new HttpClient();
    Result<HttpResponseMessage> result = await httpClient.TrySendAsync(request);
    return result;
}
```

## HttpResponseMessage extension methods

There are three extension methods - `GetResultAsync`, `ReadResultFromJsonAsync<T>`, and `ReadMaybeFromJsonAsync<T>` - for creating result objects (`Result`, `Result<T>`, and `Maybe<T>` respectively) from an `HttpResponseMessage`.

```c#
public async Task<Result> ExampleResultMethod()
{
    HttpClient httpClient = new HttpClient();
    HttpResponseMessage response = await httpClient.GetAsync("https://example.com");

    Result result = await response.GetResultAsync();
    return result;
}

public async Task<Result<T>> ExampleResultMethod<T>()
{
    HttpClient httpClient = new HttpClient();
    HttpResponseMessage response = await httpClient.GetAsync("https://example.com");

    Result<T> result = await response.ReadResultFromJsonAsync<T>();
    return result;
}

public async Task<Maybe<T>> ExampleMaybeMethod<T>()
{
    HttpClient httpClient = new HttpClient();
    HttpResponseMessage response = await httpClient.GetAsync("https://example.com");

    Maybe<T> result = await response.ReadMaybeFromJsonAsync<T>();
    return result;
}
```
