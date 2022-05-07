# RandomSkunk.Results [![NuGet](https://img.shields.io/nuget/vpre/RandomSkunk.Results.svg)](https://www.nuget.org/packages/RandomSkunk.Results)

This library contains three result types: `Result<T>`, which represents the result of an operation that has a return value; `Maybe<T>`, which represents the result of an operation that has an optional return value; and `Result`, which represents the result of an operation that does not have a return value.

## Usage

### Creation

To create a result, use one of the static factory methods.

```c#
// Results for operations that have a return value:
Result<int> result1 = Result<int>.Create.Success(123);
Result<int> result2 = Result<int>.Create.Fail();

// Results for operations that have an optional return value:
Maybe<int> result3 = Maybe<int>.Create.Some(123);
Maybe<int> result4 = Maybe<int>.Create.None();
Maybe<int> result5 = Maybe<int>.Create.Fail();

// Results for operations that do not have a return value:
Result result6 = Result.Create.Success();
Result result7 = Result.Create.Fail();
```

### Handling

There are two options for handling a result: by calling the `Match` or `MatchAsync` methods, which is safe but indirect; and by querying the properties of the result object, which is direct but potentially unsafe.

#### Match methods

There are four variations of match methods for each of the result types, depending on what kind of work needs to be done. All the methods take parameters that are functions (delegates). Two of the methods are synchronous and the other two are asynchronous (`Match` vs `MatchAsync`). Divided the other way, two of the methods have function parameters that return a value, and the other two have function parameters that do not have a return value (i.e. they return `void` or `Task`).

Note that calling the match methods will never throw an exception unless the provided function parameters themselves throw when called.

```c#
// Synchronous functions with no return value (return void):
Maybe<int> result = ...
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
Maybe<Guid> userIdResult = result3;
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

Each of the result types exposes a number of properties that can be checked to determine that state of the result. Note that the `Error` and `Value` properties will throw an exception if not in the proper state. `IsFail` must be true in order to access the `Error` property in all result types. For `ResultType<T>`, `IsSuccess` must be true in order to access the `Value` property, but for `Maybe<T>`, `IsSome` must be true in order to access the `Value` property.

```c#
// Required return value:
Result<int> result1 = ...
if (result1.IsFail)
    Console.WriteLine($"Error: {result1.Error}");
else
    Console.WriteLine($"Success: {result1.Value}");

// Optional return value:
Maybe<int> result2 = ...
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
Maybe<int> result = ...
switch (result.Type)
{
    case MaybeType.Some:
        Console.WriteLine($"Some: {result.Value}");
        break;
    case MaybeType.None:
        Console.WriteLine("None");
        break;
    case MaybeType.Fail:
        Console.WriteLine($"Fail: {result.Error}");
        break;
}
```

## Custom Errors and Factory Methods

Custom errors can be created by inheriting from the `Error` class, then passed to a `Fail` factory method.

```c#
public class NotFoundError : Error
{
    public NotFoundError(int id, string resourceType = "record")
        : base(
            message: $"A {resourceType} with the ID {id} could not be found.",
            errorCode: 404)
    {
    }
}

// Create a fail result with our custom error.
Result<T> result = Result<T>.Create.Fail(new NotFoundError(123));

 // errorType: "NotFoundError"
string errorType = result.Error.Type;

// errorMessage: "A record with the ID 123 could not be found."
string errorMessage = result.Error.Message;

 // errorCode: 404
string errorCode = result.Error.ErrorCode;
```

To make it easier to create specific fail results, extension methods targeting `IResultFactory`, `IResultFactory<T>`, or `IMaybeFactory<T>` can be created.

```c#
public static class ResultFactoryExtensions
{
    public static Result<T> NotFound<T>(
        this IResultFactory<T> resultFactory,
        int id,
        string resourceType = "record")
    {
        return resultFactory.Fail(new NotFoundError(id, resourceType));
    }
}

// Create a fail result with our factory extension method.
Result<T> result = Result<int>.Create.NotFound(123);
```
