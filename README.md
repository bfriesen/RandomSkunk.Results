# RandomSkunk.Results

Two things that make code harder to reason about are exceptions and `null`. The libraries in this repository aim to do something about exceptions by providing "result" structs to explicitly represent what happened during an operation that could potentially fail. To address `null`, these results don't accept `null` as any sort of input and don't provide `null` from any output<sup><abbr title="Technically, the GetValueOr methods will allow a null fallback value and return it if the result is not Succeses. But the caller will have explicitly passed that null fallback value, so they shouldn't be surprised to get it back.">*</abbr></sup>.

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
