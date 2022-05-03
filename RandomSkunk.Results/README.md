# RandomSkunk.Results [![NuGet](https://img.shields.io/nuget/vpre/RandomSkunk.Results.svg)](https://www.nuget.org/packages/RandomSkunk.Results)

This library contains three result types: `Result<T>`, which represents the result of an operation that has a return value; `MaybeResult<T>`, which represents the result of an operation that has an optional return value; and `Result`, which represents the result of an operation that does not have a return value. `Result<T>`, and `MaybeResult<T>`.

## Usage

### Creation

To create a result, use one of the static factory methods.

```c#
// Results for operations that have a return value:
Result<int> result1 = Result.Success(123);
Result<int> result2 = Result.Fail<int>();

// Results for operations that have an optional return value:
MaybeResult<int> result3 = MaybeResult.Some(123);
MaybeResult<int> result4 = MaybeResult.None<int>();
MaybeResult<int> result5 = MaybeResult.Fail<int>();

// Results for operations that do not have a return value:
Result result6 = Result.Success();
Result result7 = Result.Fail();
```

### Handling

There are two options for handling a result: by calling the `Match` or `MatchAsync` methods, which is safe but indirect; and by querying the properties of the result object, which is direct but potentially unsafe.

#### Match methods

There are four variations of match methods for each of the result types, depending on what kind of work needs to be done. All the methods take parameters that are functions (delegates). Two of the methods are synchronous and the other two are asynchronous (`Match` vs `MatchAsync`). Divided the other way, two of the methods have function parameters that return a value, and the other two have function parameters that do not have a return value (i.e. they return `void` or `Task`).

Note that calling the match methods will never throw an exception unless the provided function parameters themselves throw when called.

```c#
// Synchronous functions with no return value (return void):
MaybeResult<int> result = ...
result.Match(
    some: value => Console.WriteLine($"Some: {value}"),
    none: () => Console.WriteLine("None"),
    fail: error => Console.WriteLine($"Fail: {error}"));

// Asynchronous functions with no return value (return Task):
Result<int> result = ...
await result.MatchAsync(
    success: async value => await Console.Out.WriteLineAsync($"Success: {value}"),
    fail: async error => await Console.Out.WriteLineAsync($"Fail: {error}"));

// Synchronous functions with a return value:
Result result = ...
string message = result.Match(
    success: () => "Success",
    fail: error => $"Fail: {error}");

// Asynchronous functions with a return value:
MaybeResult<Guid> userIdResult = result3;
string message = await userIdResult.MatchAsync(
    some: async userId =>
    {
        string userName = await GetUserName(userId);
        return $"Hello, {userName}!";
    },
    none: () => Task.FromResult("Unknown user"),
    fail: error => Task.FromResult("Error"));
```

#### Properties

Each of the result types exposes a number of properties that can be checked to determine that state of the result. Note that the `Error` and `Value` properties will throw an exception if not in the proper state. `IsFail` must be true in order to access the `Error` property in all result types. For `ResultType<T>`, `IsSuccess` must be true in order to access the `Value` property, but for `MaybeResult<T>`, `IsSome` must be true in order to access the `Value` property.

```c#
// Required return value:
Result<int> result1 = ...
if (result1.IsFail)
    Console.WriteLine($"Error: {result1.Error}");
else
    Console.WriteLine($"Success: {result1.Value}");

// Optional return value:
MaybeResult<int> result2 = ...
if (result2.IsSome)
    Console.WriteLine($"Some: {result2.Value}");
else if (result2.IsNone)
    Console.WriteLine("None");
else
    Console.WriteLine($"Error: {result2.Error}");

// No return value:
Result result3 = ...
if (result3.IsSuccess)
    Console.WriteLine("Success");
else
    Console.WriteLine($"Error: {result3.Error}");
```

Each result type also has an enum `Type` property that returns the kind of result: `Success`, `Fail`, `Some`, or `None`, depending on the result type.

```c#
MaybeResult<int> result = ...
switch (result.Type)
{
    case MaybeResultType.Some:
        Console.WriteLine($"Some: {result.Value}");
        break;
    case MaybeResultType.None:
        Console.WriteLine("None");
        break;
    case MaybeResultType.Fail:
        Console.WriteLine($"Fail: {result.Error}");
        break;
}
```
