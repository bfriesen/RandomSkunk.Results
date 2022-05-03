# RandomSkunk.Results

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog],
and this project adheres to [Semantic Versioning].

## [Unreleased]

## [1.0.0-alpha04] - 2022-05-02

### Changed

- RandomSkunk.Results:
    - The `Error` class and the various `Fail` factory methods take a `message` parameter instead of a `messagePrefix` parameter.

### Added

- RandomSkunk.Result.Http project.
    - Extension methods for getting a result object directly from an `HttpResponseMessage`:
        - `HttpResponseMessage.ReadResultFromJsonAsync`
        - `HttpResponseMessage.ReadResultFromJsonAsync<T>`
        - `HttpResponseMessage.ReadMaybeResultFromJsonAsync<T>`
- RandomSkunk.Result.AspNetCore project.
    - Extension method for getting a `ProblemDetails` object directly from a result error:
        - `Error.GetProblemDetails`

### Removed

- `CallSite` struct.
- Abstract base classes, `ResultBase` and `ResultBase<T>`.

*Note that version 1.0.0-alpha03 was burned due to publishing error.*

## [1.0.0-alpha02] - 2022-04-28

### Added

- Result extension methods: `Or`, `Else`, `Map`, `MapAsync`, `FlapMap`, `FlapMapAsync`, `Flatten`, `Filter`, and `FilterAsync`.
- `Identifier` property to `Error` class,
- Overloads of `Fail` result factory methods that take an `Error` parameter.
- `FromException` factory method to `Error` class.
- Overloads of `Fail` result factory methods that take an `Exception` parameter.
- LINQ extension methods: `Select`, `SelectMany`, and `Where`.

## [1.0.0-alpha01] - 2022-04-28

### Added

- Initial project, solution, and package structures.
- `Result`, `Result<T>`, and `MaybeResult<T>` classes.

[Keep a Changelog]: https://keepachangelog.com/en/1.0.0/
[Semantic Versioning]: https://semver.org/spec/v2.0.0.html

[Unreleased]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha04...HEAD
[1.0.0-alpha04]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha02...v1.0.0-alpha04
[1.0.0-alpha02]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha01...v1.0.0-alpha02
[1.0.0-alpha01]: https://github.com/bfriesen/RandomSkunk.Results/compare/v0.0.0...v1.0.0-alpha01
