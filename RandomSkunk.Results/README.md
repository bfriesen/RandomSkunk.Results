# RandomSkunk.Results [![NuGet](https://img.shields.io/nuget/vpre/RandomSkunk.Results.svg)](https://www.nuget.org/packages/RandomSkunk.Results)

This library contains three result types: `Result<T>`, which represents a result that has a required value; `Maybe<T>`, which represents a result that has an optional value; and `Result`, which represents a result that does not have a value.

## Usage

### Creation

To create a result, use one of the static factory methods.

```c#
// Results that have a required value:
Result<int> result1 = Result<int>.Success(123);
Result<int> result1a = 123.ToResult(); // using RandomSkunk.Result.FactoryExtensions;
Result<int> result2 = Result<int>.Fail();

// Results that have an optional value:
Maybe<int> result3 = Maybe<int>.Some(123);
Maybe<int> result3a = 123.ToMaybe(); // using RandomSkunk.Result.FactoryExtensions;
Maybe<int> result4 = Maybe<int>.None();
Maybe<int> result5 = Maybe<int>.Fail();

Maybe<string> result6 = Maybe<string>.FromValue("abc"); // Some("abc")
Maybe<string> result7 = Maybe<string>.FromValue(null); // None

// Results that do not have a value:
Result result8 = Result.Success();
Result result9 = Result.Fail();
```

### Handling Results

There are two options for handling a result: by calling the `Match` or `MatchAsync` extension methods, which is safe but indirect; and `GetValue()` or `GetError()` extension methods, which is direct but unsafe.

#### Match methods

There are four variations of match methods for each of the result types, depending on what kind of work needs to be done. All the methods take parameters that are functions (delegates). Two of the methods are synchronous and the other two are asynchronous (`Match` vs `MatchAsync`). Divided the other way, two of the methods have function parameters that return a value, and the other two have function parameters that do not have a return value (i.e. they return `void` or `Task`).

Note that calling the match methods will never throw an exception unless the provided function parameters themselves throw when called.

```c#
// Synchronous functions with no return value (return void):
Maybe<int> result = ...
result.Match(
    onSome: value => Console.WriteLine($"Some: {value}"),
    onNone: () => Console.WriteLine("None"),
    onFail: error => Console.WriteLine($"Fail: {error}"));

// Asynchronous functions with no return value (return Task):
Result<int> result = ...
await result.MatchAsync(
    onSuccess: async value => await Console.Out.WriteLineAsync($"Success: {value}"),
    onFail: async error => await Console.Out.WriteLineAsync($"Fail: {error}"));

// Synchronous functions with a return value:
Result result = ...
string message = result.Match(
    onSuccess: () => "Success",
    onFail: error => $"Fail: {error}");

// Asynchronous functions with a return value:
Maybe<Guid> userIdResult = ...
string message = await userIdResult.MatchAsync(
    onSome: async userId =>
    {
        string userName = await GetUserName(userId);
        return $"Hello, {userName}!";
    },
    onNone: () => Task.FromResult("Unknown user"),
    onFail: error => Task.FromResult("Error"));
```

#### Unsafe Access

To access the value and error of results directly, add `using RandomSkunk.Results.Unsafe;` to the using directives, then call the `GetValue()` and `GetError()` extension methods on the result. Note that calling these extension methods will throw an `InvalidStateException` if not in the proper state. For `ResultType<T>`, `IsSuccess` must be true in order to successfully call `GetValue()`, and for `Maybe<T>`, `IsSome` must be true in order to successfully call `GetValue()`. For all result types, `IsFail` must be true in order to successfully call `GetError()`.

```c#
// Required return value:
Result<int> result1 = ...
if (result1.IsFail)
    Console.WriteLine($"Error: {result1.GetError()}");
else
    Console.WriteLine($"Success: {result1.GetValue()}");

// Optional return value:
Maybe<int> result2 = ...
if (result2.IsSome)
    Console.WriteLine($"Some: {result2.GetValue()}");
else if (result2.IsNone)
    Console.WriteLine("None");
else
    Console.WriteLine($"Error: {result2.GetError()}");

// No return value:
Result result3 = ...
if (result3.IsSuccess)
    Console.WriteLine("Success");
else
    Console.WriteLine($"Error: {result3.GetError()}");
```

Each result type also has an enum `Type` property that returns the kind of result: `Success`, `Fail`, `Some`, or `None`, depending on the result type.

```c#
Maybe<int> result = ...
switch (result.Type)
{
    case MaybeType.Some:
        Console.WriteLine($"Some: {result.GetValue()}");
        break;
    case MaybeType.None:
        Console.WriteLine("None");
        break;
    case MaybeType.Fail:
        Console.WriteLine($"Fail: {result.GetError()}");
        break;
}
```

## Custom Errors and Factory Methods

Custom errors can be created by inheriting from the `Error` class, then passed to a `Fail` factory method.

```c#
public record class NotFoundError : Error
{
    public NotFoundError(int id, string resourceType = "record")
        : base($"A {resourceType} with the ID {id} could not be found.")
    {
        ErrorCode = 404;
    }
}

// Create a fail result with our custom error.
Result<T> result = Result<T>.Fail(new NotFoundError(123));

 // errorType: "NotFoundError"
string errorType = result.Error.Type;

// errorMessage: "A record with the ID 123 could not be found."
string errorMessage = result.Error.Message;

 // errorCode: 404
string errorCode = result.Error.ErrorCode;
```

To make it easier to create specific fail results, extension methods targeting `IResultFactory`, `IResultFactory<T>`, or `IMaybeFactory<T>` can be created.

```c#
public static Result<T> NotFound<T>(
    this ResultFailFactory<T> factory,
    int id,
    string resourceType = "record")
{
    return factory.Error(
        new NotFoundError(id, resourceType)
            { StackTrace = new StackTrace(1).ToString() });
}

// Create a fail result with our factory extension method.
Result<T> result = Result<int>.FailWith.NotFound(id, "User");
```

## Result Extension Methods

There are numerous extension methods for the result types, though most are only applicable to `Result<T>` and `Maybe<T>` (not `Result`).

### Equals

*Applicable to `Result<T>` and `Maybe<T>` only.*

Compares a result to a value. true if the source result is `Success` or `Some` and its value equals the specified value.

```c#
Result<int>.Success(123).Equals(123); // true
Result<int>.Success(123).Equals(456); // false
Result<int>.Fail().Equals(123); // false

Maybe<int>.Some(123).Equals(123); // true
Maybe<int>.Some(123).Equals(456); // false
Maybe<int>.None().Equals(123); // false
Maybe<int>.Fail().Equals(123); // false
```

### GetValueOr

*Applicable to `Result<T>` and `Maybe<T>` only.*

Gets the value of a result if it is `Success` or `Some`, otherwise returns the specified fallback value.

```c#
Result<int>.Success(123).GetValueOr(456); // 123
Result<int>.Fail().GetValueOr(456); // 456

Maybe<int>.Some(123).GetValueOr(456); // 123
Maybe<int>.None().GetValueOr(456); // 456
Maybe<int>.Fail().GetValueOr(456); // 456
```

### Or

*Applicable to `Result<T>` and `Maybe<T>` only.*

the source result if it is `Success` or `Some`, otherwise returns a new `Success` or `Some` result with the specified fallback value.

```c#
Result<int>.Success(123).Or(456); // Success(123)
Result<int>.Fail().Or(456); // Success(456)

Maybe<int>.Some(123).Or(456); // Some(123)
Maybe<int>.None().Or(456); // Some(456)
Maybe<int>.Fail().Or(456); // Some(456)
```

### Else

*Applicable to all three result types.*

the source result if it is `Success` or `Some`, else returns the specified fallback result.

```c#
Result<int>.Success(123).Else(Result<int>.Success(456)); // Success(123)
Result<int>.Success(123).Else(Result<int>.Fail()); // Success(123)
Result<int>.Fail("A").Else(Result<int>.Success(456)); // Success(456)
Result<int>.Fail("A").Else(Result<int>.Fail("B")); // Fail("B")

Maybe<int>.Some(123).Else(Maybe<int>.Some(456)); // Some(123)
Maybe<int>.Some(123).Else(Maybe<int>.None()); // Some(123)
Maybe<int>.Some(123).Else(Maybe<int>.Fail("B")); // Some(123)
Maybe<int>.None().Else(Maybe<int>.Some(456)); // Some(456)
Maybe<int>.None().Else(Maybe<int>.None()); // None
Maybe<int>.None().Else(Maybe<int>.Fail("B")); // Fail("B")
Maybe<int>.Fail("A").Else(Maybe<int>.Some(456)); // Some(456)
Maybe<int>.Fail("A").Else(Maybe<int>.None()); // None
Maybe<int>.Fail("A").Else(Maybe<int>.Fail("B")); // Fail("B")
```

### Map / MapAsync

*Applicable to `Result<T>` and `Maybe<T>` only.*

Transforms the source result into a new result. If the source result is `Success` or `Some`, return a new `Success` or `Some` result with its value obtained by evaluating the specified `map` or `mapAsync` function. If the source result is `Fail`, return a new `Fail` result with the same error as the source. If the source result is `None`, return `None`.

```c#
Result<int>.Success(123).Map(value => value.ToString()); // Success("123")
Result<int>.Fail("A").Map(value => value.ToString()); // Fail("A")

Maybe<int>.Some(123).Map(value => value.ToString()); // Some("123")
Maybe<int>.None().Map(value => value.ToString()); // None
Maybe<int>.Fail("A").Map(value => value.ToString()); // Fail("A")
```

Each of the `Map` and `MapAsync` overloads has an optional `Func<Error, Error> getError` parameter, which allows the caller to replace the error of the returned `Fail` result. This function is evaluated only if the source result is a `Fail` result.

### FlatMap / FlatMapAsync

*Applicable to `Result<T>` and `Maybe<T>` only.*

Transforms the source result into a new result. If the source result is `Success` or `Some`, returns the result obtained by evaluating the specified `flatMap` or `flatMapAsync` function. If the source result is `Fail`, returns a new `Fail` result with the same error as the source. If the source result is `None`, returns `None`.

```c#
Result<int>.Success(123).FlatMap(GetSuccessResult); // Success("123")
Result<int>.Success(123).FlatMap(GetFailResult); // Fail
Result<int>.Fail("A").FlatMap(GetSuccessResult); // Fail
Result<int>.Fail("A").FlatMap(GetFailResult); // Fail

Maybe<bool>.Some(true).FlatMap(GetSomeResult); // Some("true")
Maybe<bool>.Some(true).FlatMap(GetNoneResult); // None
Maybe<bool>.Some(true).FlatMap(GetFailResult); // Fail("B")
Maybe<bool>.None().FlatMap(GetSomeResult); // None
Maybe<bool>.None().FlatMap(GetNoneResult); // None
Maybe<bool>.None().FlatMap(GetFailResult); // None
Maybe<bool>.Fail("A").FlatMap(GetSomeResult); // Fail("A")
Maybe<bool>.Fail("A").FlatMap(GetNoneResult); // Fail("A")
Maybe<bool>.Fail("A").FlatMap(GetFailResult); // Fail("A")

Result<string> GetSuccessResult(int value) => Result<string>.Success(value.ToString());
Result<string> GetFailResult(int value) => Result<string>.Fail("B");

Maybe<string> GetSomeResult(bool value) => Maybe<string>.Some(value.ToString());
Maybe<string> GetNoneResult(bool value) => Maybe<string>.None();
Maybe<string> GetFailResult(bool value) => Maybe<string>.Fail("B");
```

Each of the `FlatMap` and `FlatMapAsync` overloads has an optional `Func<Error, Error> getError` parameter, which allows the caller to replace the error of the returned `Fail` result. This function is evaluated only if the source result is a `Fail` result.

### CrossMap / CrossMapAsync

*Applicable to `Result<T>` and `Maybe<T>` only.*

Transforms the source result into a different type of result.

```c#
// Result<T> to Result
Result<int>.Success(123).CrossMap(GetSuccessResult); // Success
Result<int>.Success(123).CrossMap(GetFailResult); // Fail
Result<int>.Fail("A").CrossMap(GetSuccessResult); // Fail("A")
Result<int>.Fail("A").CrossMap(GetFailResult); // Fail("A")

// Result<T> to Maybe<T>
Result<int>.Success(123).CrossMap(GetSomeMaybeOfString); // Some("123")
Result<int>.Success(123).CrossMap(GetNoneMaybeOfString); // None
Result<int>.Success(123).CrossMap(GetFailMaybeOfString); // Fail("B")
Result<int>.Fail("A").CrossMap(GetSomeMaybeOfString); // Fail("A")
Result<int>.Fail("A").CrossMap(GetNoneMaybeOfString); // Fail("A")
Result<int>.Fail("A").CrossMap(GetFailMaybeOfString); // Fail("A")

// Maybe<T> to Result
Maybe<int>.Some(123).CrossMap(GetSuccessResult); // Success
Maybe<int>.Some(123).CrossMap(GetFailResult); // Fail("B")
Maybe<int>.None().CrossMap(GetSuccessResult); // Fail(none)
Maybe<int>.None().CrossMap(GetFailResult); // Fail(none)
Maybe<int>.Fail("A").CrossMap(GetSuccessResult); // Fail("A")
Maybe<int>.Fail("A").CrossMap(GetFailResult); // Fail("A")

// Maybe<T> to Result<T>
Maybe<int>.Some(123).CrossMap(GetSuccessResultOfString); // Success("123")
Maybe<int>.Some(123).CrossMap(GetFailResultOfString); // Fail("B")
Maybe<int>.None().CrossMap(GetSuccessResultOfString); // Fail(none)
Maybe<int>.None().CrossMap(GetFailResultOfString); // Fail(none)
Maybe<int>.Fail("A").CrossMap(GetSuccessResultOfString); // Fail("A")
Maybe<int>.Fail("A").CrossMap(GetFailResultOfString); // Fail("A")

Result GetSuccessResult(int value) => Result.Success();
Result GetFailResult(int value) => Result.Fail("B");

Result<string> GetSuccessResultOfString(int value) => Result<string>.Success(value.ToString());
Result<string> GetFailResultOfString(int value) => Result<string>.Fail("B");

Maybe<string> GetSomeMaybeOfString(int value) => Maybe<string>.Some(value.ToString());
Maybe<string> GetNoneMaybeOfString(int value) => Maybe<string>.None();
Maybe<string> GetFailMaybeOfString(int value) => Maybe<string>.Fail("B");
```

Each of the `CrossMap` and `CrossMapAsync` overloads has an optional `Func<Error, Error> getError` parameter, which allows the caller to replace the error of the returned `Fail` result when the source result is a `Fail` result.

The cross-map methods for `Maybe<T>` also have an optional `Func<Error> getNoneError` parameter, which allows the caller to specify the error of the returned `Fail` result when the source result is a `None` result.

### Flatten

*Applicable to `Result<T>` and `Maybe<T>` only.*

Flattens a `Result<Result<T>>` into a `Result<T>` or a `Maybe<Maybe<T>>` into a `Maybe<T>`.

```c#
Result<Result<int>> nestedResult = ...
Result<int> flattenedResult = nestedResult.Flatten();

Maybe<Maybe<int>> nestedMaybe = ...
Maybe<int> flattenedMaybe = nestedMaybe.Flatten();
```

### Filter / FilterAsync

*Applicable to `Maybe<T>` only.*

Filters a `Some` result to `None` unless the specified filter function evaluates to true. `None` and `Fail` results are not affected.

```c#
Maybe<int>.Some(123).Filter(value => value < 150); // Some(123)
Maybe<int>.Some(456).Filter(value => value < 150); // None
Maybe<int>.None().Filter(value => value < 150); // None
Maybe<int>.Fail("A").Filter(value => value < 150); // Fail("A")
```

### WithError

*Applicable to all three result types.*

Returns a new result with a different error if the source is a `Fail` result. `Success`, `Some`, and `None` result are not affected.

```c#
Result failResult = Result.Fail("Inner error");
Result successResult = Result.Success();

// Fail("Outer error"("Inner error"))
failResult.WithError(error => new Error("Outer error") { InnerError = error });

// Success
successResult.WithError(error => new Error("Outer error") { InnerError = error });
```

## LINQ Extension Methods

In the `RandomSkunk.Results.Linq` namespace, there are aliases for the `Map`, `FlatMap`, and `Filter` extension methods named `Select`, `SelectMany`, and `Where`. These methods allow you to use LINQ to transform results.

```c#
using RandomSkunk.Results.Linq;

// Given methods with the following signatures:
Maybe<Person> GetPerson(Guid id)
Maybe<Department> GetDepartment(Guid id)

Guid personId;

// Chain the results together:
Maybe<Department> result =
    from person in GetPerson(personId)
    where person.IsActive
    from department in GetDepartment(person.DepartmentId)
    select department;
```
