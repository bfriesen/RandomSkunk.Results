# RandomSkunk.Results

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog],
and this project adheres to [Semantic Versioning].

## [Unreleased]

## [1.0.0-alpha13] - 2022-06-03

### Added

- RandomSkunk.Results:
    - Add `AsDisposable()` and `AsAsyncDisposable()` extension methods for `Result<T>` and `Maybe<T>`.
    - Add additional overloads of `Flatten` where the source is a mixed nested results. For example, `Result<Maybe<T>>` flattens to `Maybe<T>`.
    - Add `Result<T>.FromValue` method, which gracefully handles null values by returning a `Fail` result.
    - Add extension methods for `IEnumerable<T>` in the `RandomSkunk.Results.Linq` namespace that behave the same way that the `System.Linq` extension methods do. Except instead of returning `T` and perhaps throwing an exception or returning `null`, they return `Result<T>` (the "OnFail" methods) or `Maybe<T>` (the "OnNone" methods).
        - `FirstOrFail` | `FirstOrNone`
        - `LastOrFail` | `LastOrNone`
        - `SingleOrFail` | `SingleOrNone`
        - `ElementAtOrFail` | `ElementAtOrNone`

### Changed

- RandomSkunk.Results:
    - Remove `Create` factory field. Pull result factory methods up to the result types themselves. Add back `FailWith` factory field that only creates `Fail` results.
    - The `FactoryExtensions.ToResult` extension method now gracefully handles null values by calling `Result<T>.FromValue`.

## [1.0.0-alpha12] - 2022-05-31

### Added

- Add RandomSkunk.Results.Dapper project.
    - Extension methods mirroring Dapper's extension methods that start with `Try` and return result objects.
- RandomSkunk.Results:
    - Add `ToResult<T>()` and `ToMaybe<T>()` extension methods under the `RandomSkunk.Results.FactoryExtensions` namespace.
    - Add `Result<T>.AsMaybe()` and `Maybe<T>.AsResult()` methods.
- RandomSkunk.Results.AspNetCore:
    - In `ToActionResult()` extension methods, add ability to provide custom status code for success results.

### Changed

- RandomSkunk.Results:
    - Rename delegate extension methods to differentiate between new factory extension methods.
        - `ToResult()` -> `TryInvokeAsResult()`
        - `ToResult<T>()` -> `TryInvokeAsResult<T>()`
        - `ToMaybe<T>()` -> `TryInvokeAsMaybe<T>()`

## [1.0.0-alpha11] - 2022-05-26

### Fixed

- RandomSkunk.Results:
    - Fix `Error.ToString()` formatting for derived types.

### Changed

- RandomSkunk.Results:
    - Change result extension methods to instance methods wherever possible.
    - Move `InvalidStateException` to `RandomSkunk.Results.Unsafe` namespace.

### Added

- RandomSkunk.Results.Http:
    - Add extension methods for `Task<HttpResponseMessage>` and `Task<Result<HttpResponseMessage>>`.
- RandomSkunk.Result.AspNetCore:
    - Add `ToActionResult` extension methods for results.

## [1.0.0-alpha10] - 2022-05-24

### Fixed

- RandomSkunk.Results.Http:
    - Fix ambiguous overloads in `HttpResponseMessage` extension methods.

### Added

- RandomSkunk.Results:
    - Add JSON serialization support for `Error` class.
    - Add `ExtendedError` class, designed to losslessly capture additional properties when deserialized.
    - Add `OnFail` extension methods for all result types and `OnNone` extension methods for `Maybe<T>`.
    - Add overloads of almost all result extension methods that extend a `Task<Result>`, `Task<Result<T>>` or `Task<Maybe<T>>`. This allows easier method chaining for users.

### Changed

- RandomSkunk.Results
    - `Error` is now a record class instead of a regular class.

## [1.0.0-alpha09] - 2022-05-19

### Added

- RandomSkunk.Results:
    - Add `[DebuggerDisplay]` to each of the result structs and the error class.
    - Add `CrossMap` extension methods to convert from one type of result to another.
    - Add `ToResult`, `ToResult<T>`, `ToMaybe<T>`, `ToResultAsync`, `ToResultAsync<T>`, and `ToMaybeAsync<T>` extension methods to convert delegates into results.
    - Add `OnSuccess` and `OnSome` extension methods to conditionally invoke a callback and return the same result.
- RandomSkunk.Results.Http:
    - Add extension methods for `HttpClient` to convert HTTP operations into results.
    - Add `JsonSerializerOptions` parameter to `HttpResponseMessage` extension methods.

### Fixed

- RandomSkunk.Results:
    - Fix bugs in `Equals(object)` and `GetHashCode()`.

### Changed

- RandomSkunk.Results:
    - Add optional `getError` parameter to all `Map` and `FlapMap` extension methods.
- RandomSkunk.Results.Http:
    - Rename `ReadResultFromJsonAsync` extension method to `GetResultAsync`.

## [1.0.0-alpha08] - 2022-05-11

### Added

- RandomSkunk.Results:
    - Add `WithError` extension methods.

### Changed

- RandomSkunk.Results.AspNetCore:
    - Include `Error.InnerError` in the problem details.
    - Map `Error.Type` to `ProblemDetails.Title` instead of `ProblemDetails.Extensions["errorType"]`.
- RandomSkunk.Results.Http:
    - Set `Error.InnerError` from the problem details.
    - Map `ProblemDetails.Title` to `Error.Type` instead of being part of `Error.Message`.

### Fixed

- RandomSkunk.Results.Http:
    - Fix bug when getting values of `errorStackTrace` and `errorIdentifier`. Their values would be null even when specified in the JSON.

## [1.0.0-alpha07] - 2022-05-09

### Added

- RandomSkunk.Results:
    - Add `InnerError` property to `Error` class.
    - Add 'type' parameter to `FromException` and `Fail` factory methods.
    - Add `Error` property to `InvalidStateException` class.
    - Add `IsDefault` property to result types.

### Changed
- RandomSkunk.Results:
    - Move `Match` and `MatchAsync` methods to result types (from extension methods).

### Removed

- RandomSkunk.Results:
    - Remove implicit conversion operator from `Maybe<T>`.
    - Remove unused constructors from `InvalidStateException` class.

## [1.0.0-alpha06] - 2022-05-07

### Changed

- RandomSkunk.Results:
    - Replace factory methods on result types with `Create` property of type `IResultFactory`, `IResultFactory<T>`, or `IMaybeResultFactory<T>`. These interfaces define the same base factory methods that they replace, and extension methods define `Fail` overloads.
    - Rename `MaybeResult` to just `Maybe`. Other types are similarly renamed.
    - Move direct access to the value and error of a result to the `GetValue()` and `GetError()` extension methods under the `RandomSkunk.Results.Unsafe` namespace.
- RandomSkunk.Results.AspNetCore:
    - Add cancellation token to all async methods.
    - Never return a `Fail` result about an issue getting the problem details.

### Added

- RandomSkunk.Results:
    - Add `InvalidStateException`, which is thrown from the unsafe extension methods when the result an invalid state for directly reading its value or error.

## [1.0.0-alpha05] - 2022-05-05

### Added

- RandomSkunk.Results:
    - Add `Type` property to the `Error` class.
        - Initialized with the name of the error type (e.g. "Error" for the base `Error` class) by default.

### Changed

- RandomSkunk.Results:
    - Each of the result types is a `struct` instead of a `class`.
    - The `Error` class is no longer `sealed`.
    - Changed default value of `Error.DefaultMessage`.
- RandomSkunk.Results.AspNetCore:
    - Each extension method maps the `Type` property to "errorType" extension property of `ProblemDetails` object.
- RandomSkunk.Result.Http:
    - Extension method maps the "errorType" extension property of the problem details object to the `Type` of the `Error` object.

## [1.0.0-alpha04] - 2022-05-02

### Changed

- RandomSkunk.Results:
    - The `Error` class and the various `Fail` factory methods take a `message` parameter instead of a `messagePrefix` parameter.

### Added

- Add RandomSkunk.Result.Http project.
    - Extension methods for getting a result object directly from an `HttpResponseMessage`:
        - `HttpResponseMessage.ReadResultFromJsonAsync`
        - `HttpResponseMessage.ReadResultFromJsonAsync<T>`
        - `HttpResponseMessage.ReadMaybeResultFromJsonAsync<T>`
- Add RandomSkunk.Result.AspNetCore project.
    - Extension method for getting a `ProblemDetails` object from a result error:
        - `Error.GetProblemDetails`

### Removed

- Remove `CallSite` struct.
- Remove abstract base classes, `ResultBase` and `ResultBase<T>`.

*Note that version 1.0.0-alpha03 was burned due to publishing error.*

## [1.0.0-alpha02] - 2022-04-28

### Added

- Add result extension methods: `Or`, `Else`, `Map`, `MapAsync`, `FlapMap`, `FlapMapAsync`, `Flatten`, `Filter`, and `FilterAsync`.
- Add `Identifier` property to `Error` class,
- Add overloads of `Fail` result factory methods that take an `Error` parameter.
- Add `FromException` factory method to `Error` class.
- Add overloads of `Fail` result factory methods that take an `Exception` parameter.
- Add LINQ extension methods: `Select`, `SelectMany`, and `Where`.

## [1.0.0-alpha01] - 2022-04-28

### Added

- Add initial project, solution, and package structures.
- Add `Result`, `Result<T>`, and `MaybeResult<T>` classes.

[Keep a Changelog]: https://keepachangelog.com/en/1.0.0/
[Semantic Versioning]: https://semver.org/spec/v2.0.0.html

[Unreleased]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha12...HEAD
[1.0.0-alpha12]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha11...v1.0.0-alpha12
[1.0.0-alpha11]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha10...v1.0.0-alpha11
[1.0.0-alpha10]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha09...v1.0.0-alpha10
[1.0.0-alpha09]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha08...v1.0.0-alpha09
[1.0.0-alpha08]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha07...v1.0.0-alpha08
[1.0.0-alpha07]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha06...v1.0.0-alpha07
[1.0.0-alpha06]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha05...v1.0.0-alpha06
[1.0.0-alpha05]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha04...v1.0.0-alpha05
[1.0.0-alpha04]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha02...v1.0.0-alpha04
[1.0.0-alpha02]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha01...v1.0.0-alpha02
[1.0.0-alpha01]: https://github.com/bfriesen/RandomSkunk.Results/compare/v0.0.0...v1.0.0-alpha01
