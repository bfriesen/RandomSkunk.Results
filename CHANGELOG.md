# RandomSkunk.Results

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog],
and this project adheres to [Semantic Versioning].

## [Unreleased]

### Added

- RandomSkunk.Results:
    - Add `ErrorCodes` class.
    - Add `Error.ToString(bool includeStackTrace)` overload.
    - Add `MapAll` and `FlatMapAll` extension methods for value tuples of results.
    - Add `Filter` method and LINQ `Where` extension method for `Result<T>`.
    - Add `Result<DBNull> Result.AsDBNullResult()` method.
    - Add error code description to output of `Error.ToString()`.
    - Add `ErrorCodes.GetDescription` and `RegisterErrorCodes` static methods to get and set the description for an error code respectively.
- RandomSkunk.Results.Dapper:
    - Add `ResultSqlMapper.GridReader` class.

### Changed

- RandomSkunk.Results:
    - Return a `Fail` result instead of throwing exception when a supplied delegate parameter returns a null value.
    - Rename `CrossMap` to `FlatMap`.
    - Rename `Error.Type` to `Error.Title`.
    - Exclude stack trace from `Error.ToString()` output.
    - Stack trace generation happens only in the `Error` constructor.
    - When generating stack trace, exclude frames with methods decorated with the `[StackTraceHidden]` attribute.
    - In `Error.FromException`, the exception information is completely contained in the `InnerError` property. The outer error contains information about the callsite.
    - Replace delegate extensions with `TryCatch`, `TryCatch<TException>` and `TryCatch<TException1, TException2>` classes containing methods with the same functionality.
    - Move extension properties functionality to the base `Error` class.
    - Improve output of `Error.ToString`.
- RandomSkunk.Results.Dapper:
    - Catch general `Exception` instead of both `DbException` and `Exception` in all `ResultSqlMapper` extension methods.
    - `TryQueryMultiple` and `TryQueryMultipleAsync` return a `ResultSqlMapper.GridReader` instead of a `SqlMapper.GridReader`.
- RandomSkunk.Results.Http:
    - Improve the mapping of error code to HTTP status code. The positive or negative hundreds part of the decimal error code becomes the HTTP status code. Examples:
        - Error code `9500` maps to HTTP status code `500`.
        - Error code `-4040404` maps to HTTP status code `404`.
    - The mapping of error code to HTTP status code can be specified on a per-call basis.

### Removed

- RandomSkunk.Results:
    - Remove `FailFactory<TResult>` class and result `FailWith` fields.
    - Remove `ResultExtensions.DefaultOnNoneCallback` static property.
    - Remove most `Func<Error, Error>? onFail` parameters.
    - Remove `Func<Error>? onNoneGetError` parameters.
    - Remove `Error.DefaultMessage` static property.
    - Remove `ExtendedError` class.
- RandomSkunk.Results.Http:
    - Remove `HttpClientExtensions.DefaultGetHttpError` and `HttpClientExtensions.DefaultGetTimeoutError` static properties.
- RandomSkunk.Results.Dapper:
    - Remove `DbError` class.

## [1.0.0-alpha14] - 2022-06-30

[RandomSkunk.Results API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha14) ([diff](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha14/lib/netstandard2.1/diff/1.0.0-alpha13/))  
[RandomSkunk.Results.AspNetCore API](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha14) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha14/lib/netstandard2.1/diff/1.0.0-alpha13/))  
[RandomSkunk.Results.Dapper API](https://www.fuget.org/packages/RandomSkunk.Results.Dapper/1.0.0-alpha14) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.Dapper/1.0.0-alpha14/lib/netstandard2.1/diff/1.0.0-alpha13/))  
[RandomSkunk.Results.Http API](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha14) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha14/lib/netstandard2.1/diff/1.0.0-alpha13/))

### Added

- RandomSkunk.Results:
    - Add generated stack traces to additional fail factory methods.
    - Add `Result.AndAlso` and `Result.AndAlsoAsync` extension methods, allowing valueless results to be chained together.
    - Add `IResult` and `IResult<T>` interfaces, allowing different kinds of results to be used the same manner.
    - Add `OnAllSuccess`, `OnAnyNonSuccess`, and `MatchAll` extension methods for value tuples of results.
    - Add `OnNonSuccess` extension methods.
- RandomSkunk.Results.Dapper:
    - Add `EnsureOneRowAffected` and `EnsureNRowsAffected` extension methods.

### Changed

- RandomSkunk.Results:
    - Simplify error/fail factory methods that have an exception parameter by removing the `type` and `innerError` parameters and using the name of the exception's type as the error type and its inner exception to create the inner error instead.
    - Simplify `Result<T>.FromValue` method and `FactoryExtensions.ToResult<T>` extension methods by replacing `nullValueErrorMessage`, `nullValueErrorCode`, `nullValueErrorIdentifier`, and `nullValueErrorType` parameters with single `getNullValueError` function parameter.
    - Rename `Some` to `Success`.

### Removed

- RandomSkunk.Results:
    - Remove the side-effect versions of Match methods. Use the `OnSuccess`, `OnFail`, or `OnNone` methods to apply side-effects based on a result instead.

## [1.0.0-alpha13] - 2022-06-03

[RandomSkunk.Results API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha13) ([diff](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha13/lib/netstandard2.1/diff/1.0.0-alpha12/))  
[RandomSkunk.Results.AspNetCore API](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha13) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha13/lib/netstandard2.1/diff/1.0.0-alpha12/))  
[RandomSkunk.Results.Dapper API](https://www.fuget.org/packages/RandomSkunk.Results.Dapper/1.0.0-alpha13) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.Dapper/1.0.0-alpha13/lib/netstandard2.1/diff/1.0.0-alpha12/))  
[RandomSkunk.Results.Http API](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha13) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha13/lib/netstandard2.1/diff/1.0.0-alpha12/))

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

[RandomSkunk.Results API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha12) ([diff](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha12/lib/netstandard2.1/diff/1.0.0-alpha11/))  
[RandomSkunk.Results.AspNetCore API](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha12) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha12/lib/netstandard2.1/diff/1.0.0-alpha11/))  
[RandomSkunk.Results.Dapper API](https://www.fuget.org/packages/RandomSkunk.Results.Dapper/1.0.0-alpha12)  
[RandomSkunk.Results.Http API](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha12) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha12/lib/netstandard2.1/diff/1.0.0-alpha11/))

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

[RandomSkunk.Results API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha11) ([diff](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha11/lib/netstandard2.1/diff/1.0.0-alpha10/))  
[RandomSkunk.Results.AspNetCore API](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha11) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha11/lib/netstandard2.1/diff/1.0.0-alpha10/))  
[RandomSkunk.Results.Http API](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha11) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha11/lib/netstandard2.1/diff/1.0.0-alpha10/))

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

[RandomSkunk.Results API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha10) ([diff](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha10/lib/netstandard2.1/diff/1.0.0-alpha09/))  
[RandomSkunk.Results.AspNetCore API](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha10) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha10/lib/netstandard2.1/diff/1.0.0-alpha09/))  
[RandomSkunk.Results.Http API](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha10) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha10/lib/netstandard2.1/diff/1.0.0-alpha09/))

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

[RandomSkunk.Results API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha09) ([diff](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha09/lib/netstandard2.1/diff/1.0.0-alpha08/))  
[RandomSkunk.Results.AspNetCore API](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha09) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha09/lib/netstandard2.1/diff/1.0.0-alpha08/))  
[RandomSkunk.Results.Http API](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha09) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha09/lib/netstandard2.1/diff/1.0.0-alpha08/))

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

[RandomSkunk.Results API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha08) ([diff](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha08/lib/netstandard2.1/diff/1.0.0-alpha07/))  
[RandomSkunk.Results.AspNetCore API](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha08) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha08/lib/netstandard2.1/diff/1.0.0-alpha07/))  
[RandomSkunk.Results.Http API](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha08) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha08/lib/netstandard2.1/diff/1.0.0-alpha07/))

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

[RandomSkunk.Results API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha07) ([diff](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha07/lib/netstandard2.1/diff/1.0.0-alpha06/))  
[RandomSkunk.Results.AspNetCore API](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha07) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha07/lib/netstandard2.1/diff/1.0.0-alpha06/))  
[RandomSkunk.Results.Http API](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha07) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha07/lib/netstandard2.1/diff/1.0.0-alpha06/))

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

[RandomSkunk.Results API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha06) ([diff](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha06/lib/netstandard2.1/diff/1.0.0-alpha05/))  
[RandomSkunk.Results.AspNetCore API](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha06) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha06/lib/netstandard2.1/diff/1.0.0-alpha05/))  
[RandomSkunk.Results.Http API](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha06) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha06/lib/netstandard2.1/diff/1.0.0-alpha05/))

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

[RandomSkunk.Results API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha05) ([diff](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha05/lib/netstandard2.1/diff/1.0.0-alpha04/))  
[RandomSkunk.Results.AspNetCore API](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha05) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.AspNetCore/1.0.0-alpha05/lib/netstandard2.1/diff/1.0.0-alpha04/))  
[RandomSkunk.Results.Http API](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha05) ([diff](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha05/lib/netstandard2.1/diff/1.0.0-alpha04/))

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

[RandomSkunk.Results API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha04) ([diff](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha04/lib/netstandard2.1/diff/1.0.0-alpha02/))  
[RandomSkunk.Results.AspNetCore API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha04)  
[RandomSkunk.Results.Http API](https://www.fuget.org/packages/RandomSkunk.Results.Http/1.0.0-alpha04)

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

[RandomSkunk.Results API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha02) ([diff](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha02/lib/netstandard2.1/diff/1.0.0-alpha01/))

### Added

- Add result extension methods: `Or`, `Else`, `Map`, `MapAsync`, `FlapMap`, `FlapMapAsync`, `Flatten`, `Filter`, and `FilterAsync`.
- Add `Identifier` property to `Error` class,
- Add overloads of `Fail` result factory methods that take an `Error` parameter.
- Add `FromException` factory method to `Error` class.
- Add overloads of `Fail` result factory methods that take an `Exception` parameter.
- Add LINQ extension methods: `Select`, `SelectMany`, and `Where`.

## [1.0.0-alpha01] - 2022-04-28

[RandomSkunk.Results API](https://www.fuget.org/packages/RandomSkunk.Results/1.0.0-alpha01)

### Added

- Add initial project, solution, and package structures.
- Add `Result`, `Result<T>`, and `MaybeResult<T>` classes.

[Keep a Changelog]: https://keepachangelog.com/en/1.0.0/
[Semantic Versioning]: https://semver.org/spec/v2.0.0.html

[Unreleased]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha14...HEAD
[1.0.0-alpha14]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha13...v1.0.0-alpha14
[1.0.0-alpha13]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha12...v1.0.0-alpha13
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
