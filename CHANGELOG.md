# RandomSkunk.Results

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog],
and this project adheres to [Semantic Versioning].

## [Unreleased]

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
[1.0.0-alpha01]: https://github.com/bfriesen/RandomSkunk.Results/compare/v0.0.0...v1.0.0-alpha01
[Unreleased]: https://github.com/bfriesen/RandomSkunk.Results/compare/v1.0.0-alpha01...HEAD
