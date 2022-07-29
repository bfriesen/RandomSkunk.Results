# RandomSkunk.Results

Two things that make code harder to reason about are exceptions and `null`. The libraries in this repository aim to do something about exceptions by providing "result" structs to explicitly represent what happened during an operation that could potentially fail. To address `null`, these results don't accept `null` as any sort of input and don't provide `null` from any output<sup><span title="Technically, the GetValueOr methods allow a null fallback value and return it if the result is not Succeses. But the caller would have explicitly passed that null fallback value, so it shouldn't be surprising to get it back.">***</span></sup>.

There are three types of results:
- `Result`
    - For operations that can fail and do not have a return value.
    - Can be either:
        - `Success`, or
        - `Fail` with a non-null `Error` object.
- `Result<T>`
    - For operations that can fail and have a return value which is always provided.
    - Can be either:
        - `Success` with a non-null value, or
        - `Fail` with a non-null `Error` object.
- `Maybe<T>`
    - For operations that can fail and have a return value which may or may not be provided.
    - Can be one of:
        - `Success` with a non-null value,
        - `Fail` with a non-null `Error` object, or
        - `None` indicating no value.

---

There are a number of libraries in this repository: the main library, and several that depend on the main library and provide result functionality in specific domains.

## [RandomSkunk.Results](https://github.com/bfriesen/RandomSkunk.Results/tree/main/RandomSkunk.Results) [![NuGet](https://img.shields.io/nuget/vpre/RandomSkunk.Results.svg)](https://www.nuget.org/packages/RandomSkunk.Results)

*The main library. Implementations of the Result monad.*

## [RandomSkunk.Results.AspNetCore](https://github.com/bfriesen/RandomSkunk.Results/tree/main/RandomSkunk.Results.AspNetCore) [![NuGet](https://img.shields.io/nuget/vpre/RandomSkunk.Results.AspNetCore.svg)](https://www.nuget.org/packages/RandomSkunk.Results.AspNetCore)

*Using RandomSkunk.Results from AspNetCore apps.*

## [RandomSkunk.Results.Dapper](https://github.com/bfriesen/RandomSkunk.Results/tree/main/RandomSkunk.Results.Dapper) [![NuGet](https://img.shields.io/nuget/vpre/RandomSkunk.Results.Dapper.svg)](https://www.nuget.org/packages/RandomSkunk.Results.Dapper)

*Using RandomSkunk.Results with Dapper.*

## [RandomSkunk.Results.Http](https://github.com/bfriesen/RandomSkunk.Results/tree/main/RandomSkunk.Results.Http) [![NuGet](https://img.shields.io/nuget/vpre/RandomSkunk.Results.Http.svg)](https://www.nuget.org/packages/RandomSkunk.Results.Http)

*Using RandomSkunk.Results with System.Net.Http and System.Net.Http.Json.*

---

# Example Project

The solution in this repository contains an example Blazor Web Assembly app that demonstrates how to use the RandomSkunk.Result libraries. To run the app, select the `ExampleBlazorApp.Server` project as the startup project.

These are the interesting parts of the example app (with links to the code):

- ExampleBlazorApp.Client project
    - Typed HTTP Clients - *each one uses RandomSkunk.Results.Http to make HTTP requests to the server*
        - [WeatherForcastClient](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Client/HttpClients/WeatherForecastClient.cs#L16-L23)
        - WeatherProfileClient
            - [GetWeatherProfiles](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Client/HttpClients/WeatherProfileClient.cs#L16-L21)
            - [AddWeatherProfile](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Client/HttpClients/WeatherProfileClient.cs#L23-L31)
            - [EditWeatherProfile](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Client/HttpClients/WeatherProfileClient.cs#L33-L40)
    - Razor pages - *each one uses one of the typed HTTP clients to make a request to the server, then handles the result*
        - [FiveDayForecast](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Client/Pages/FiveDayForecast.razor#L62-L72)
        - WeatherProfiles
            - [FetchWeatherProfiles](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Client/Pages/WeatherProfiles.razor#L156-L173)
            - [SaveProfile](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Client/Pages/WeatherProfiles.razor#L175-L201)
- ExampleBlazorApp.Server project
    - WeatherRepository - *uses RandomSkunk.Results.Dapper to make queries to the database*
        - [GetMonthlyTemperature](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Server/Repositories/WeatherRepository.cs#L18-L36)
        - [GetWeatherProfiles](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Server/Repositories/WeatherRepository.cs#L38-L59)
        - [UpsertWeatherProfile](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Server/Repositories/WeatherRepository.cs#L105-L143)
    - [WeatherForecastSimulator](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Server/Services/WeatherForecastSimulator.cs#L23-L31) - *uses WeatherRepository to make queries to the database*
    - Controllers
        - [WeatherForecastController](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Server/Controllers/WeatherForecastController.cs#L18-L28) - *uses WeatherForecastSimulator to simulate weather forecasts*
        - WeatherProfilesController - *uses WeatherRepository to make queries to the database*
            - [GetWeatherProfiles](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Server/Controllers/WeatherProfilesController.cs#L19-L29)
            - [AddWeatherProfile](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Server/Controllers/WeatherProfilesController.cs#L31-L40)
            - [EditWeatherProfile](https://github.com/bfriesen/RandomSkunk.Results/blob/main/ExampleBlazorApp.Server/Controllers/WeatherProfilesController.cs#L42-L51)
