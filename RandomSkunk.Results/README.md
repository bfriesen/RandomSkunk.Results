# RandomSkunk.Results [![NuGet](https://img.shields.io/nuget/vpre/RandomSkunk.Results.svg)](https://www.nuget.org/packages/RandomSkunk.Results)

This library contains three result types: `Result<T>`, which represents a result that has a required value; `Maybe<T>`, which represents a result that has an optional value; and `Result`, which represents a result that does not have a value.

## Usage

### Creation

To create a result, use one of the static factory methods.

```c#
int i = 123;
string s = "abc";
string n = null!;

// Results that have a required value:
Result<int> result1 = Result<int>.Success(i);
Result<int> result2 = Result<int>.Fail();

Result<string> result3 = Result<string>.FromValue(s); // Success("abc")
Result<string> result4 = Result<string>.FromValue(n); // Fail("Result value cannot be null.")

// using RandomSkunk.Result.FactoryExtensions;
Result<string> result5 = s.ToResult(); // Success("abc")
Result<string> result6 = n.ToResult(); // Fail("Result value cannot be null.")

// Results that have an optional value:
Maybe<int> result7 = Maybe<int>.Success(123);
Maybe<int> result8 = Maybe<int>.None();
Maybe<int> result9 = Maybe<int>.Fail();

Maybe<string> resultA = Maybe<string>.FromValue(s); // Success("abc")
Maybe<string> resultB = Maybe<string>.FromValue(n); // None

// using RandomSkunk.Result.FactoryExtensions;
Maybe<string> resultC = s.ToMaybe(); // Success(123)
Maybe<string> resultD = n.ToMaybe(); // None

// Results that do not have a value:
Result resultE = Result.Success();
Result resultF = Result.Fail();
```

#### From Exceptions

`Fail` results can be created directly from an exception. The caught exception is represented by the `Error.InnerError` property of the fail result's error.

```c#
Result<int> Divide(int x, int y)
{
    try
    {
        return Result<int>.Success(x / y);
    }
    catch (DivideByZeroException ex)
    {
        return Result<int>.Fail(ex);
    }
}
```

##### TryCatch classes

An entire try/catch statement can be replaced with a call to of the `TryCatch` class's methods.

```c#
Result<int> Divide(int x, int y) =>
    TryCatch<DivideByZeroException>.AsResult(() => x / y);
```

There are additional classes named "TryCatch" in, but with different number of generic arguments. The generic arguments allow you to specify exactly what type of exceptions to catch and in what order they should be caught. Then non-generic version catches the base `Exception` type.

### Handling Results

#### Direct Access

To access the value and error of results directly, add `using RandomSkunk.Results.Unsafe;` to the using directives, then call the `GetValue()` and `GetError()` extension methods on the result. Note that calling these extension methods will throw an `InvalidStateException` if not in the proper state. `IsSuccess` must be true in order to successfully call `GetValue()`, and `IsFail` must be true in order to successfully call `GetError()`.

```c#
using RandomSkunk.Results.Unsafe;

void Example(Result<int> result1, Maybe<int> result2, Result result3)
{
    if (result1.IsFail)
        Console.WriteLine($"Error: {result1.GetError()}");
    else
        Console.WriteLine($"Success: {result1.GetValue()}");

    if (result2.IsSuccess)
        Console.WriteLine($"Success: {result2.GetValue()}");
    else if (result2.IsNone)
        Console.WriteLine("None");
    else
        Console.WriteLine($"Error: {result2.GetError()}");

    if (result3.IsSuccess)
        Console.WriteLine("Success");
    else
        Console.WriteLine($"Error: {result3.GetError()}");
}
```

Each result type also has an enum `Type` property that returns the kind of result: `Success`, `Fail`, or `None`, depending on the result type.

```c#
void Example(Maybe<int> result)
{
    switch (result.Type)
    {
        case MaybeType.Success:
            Console.WriteLine($"Success: {result.GetValue()}");
            break;
        case MaybeType.None:
            Console.WriteLine("None");
            break;
        case MaybeType.Fail:
            Console.WriteLine($"Fail: {result.GetError()}");
            break;
    }
}
```

#### Match methods

The match methods map a result to a value using a series of function parameters for each of the possible outcomes of the result (`Success`, `Fail`, or `None`).

```c#
// Synchronous functions:
Result result1 = default;
string message1 = result1.Match(
    onSuccess: () => "Success",
    onFail: error => $"Fail: {error}");

// Asynchronous functions:
Maybe<Guid> result2 = default;
string message2 = await result2.MatchAsync(
    onSuccess: async userId =>
    {
        string userName = await GetUserName(userId);
        return $"Hello, {userName}!";
    },
    onNone: () => Task.FromResult("Unknown user"),
    onFail: error => Task.FromResult("Error"));
```

#### GetValueOr

*Applicable to `Result<T>` and `Maybe<T>` only.*

Gets the value of the `Success` result, or the specified fallback value if it is a `Fail` result.

```c#
Result<int>.Success(123).GetValueOr(456); // 123
Result<int>.Fail().GetValueOr(456); // 456

Maybe<int>.Success(123).GetValueOr(456); // 123
Maybe<int>.None().GetValueOr(456); // 456
Maybe<int>.Fail().GetValueOr(456); // 456
```

#### Or

*Applicable to `Result<T>` and `Maybe<T>` only.*

Returns the current result if it is a `Success` result; otherwise, returns a new `Success` result with the specified fallback value.

```c#
Result<int>.Success(123).Or(456); // Success(123)
Result<int>.Fail().Or(456); // Success(456)

Maybe<int>.Success(123).Or(456); // Success(123)
Maybe<int>.None().Or(456); // Success(456)
Maybe<int>.Fail().Or(456); // Success(456)
```

#### Else

*Applicable to all three result types.*

Returns the current result if it is a `Success` result, else returns the specified fallback result.

```c#
Result<int>.Success(123).Else(Result<int>.Success(456)); // Success(123)
Result<int>.Success(123).Else(Result<int>.Fail()); // Success(123)
Result<int>.Fail("A").Else(Result<int>.Success(456)); // Success(456)
Result<int>.Fail("A").Else(Result<int>.Fail("B")); // Fail("B")

Maybe<int>.Success(123).Else(Maybe<int>.Success(456)); // Success(123)
Maybe<int>.Success(123).Else(Maybe<int>.None()); // Success(123)
Maybe<int>.Success(123).Else(Maybe<int>.Fail("B")); // Success(123)
Maybe<int>.None().Else(Maybe<int>.Success(456)); // Success(456)
Maybe<int>.None().Else(Maybe<int>.None()); // None
Maybe<int>.None().Else(Maybe<int>.Fail("B")); // Fail("B")
Maybe<int>.Fail("A").Else(Maybe<int>.Success(456)); // Success(456)
Maybe<int>.Fail("A").Else(Maybe<int>.None()); // None
Maybe<int>.Fail("A").Else(Maybe<int>.Fail("B")); // Fail("B")
```

#### Select / SelectAsync

*Applicable to `Result<T>` and `Maybe<T>` only.*

Transforms the current result - if `Success` - into a new `Success` result using the specified `onSuccessSelector` function. Otherwise, if the current result is `Fail`, it is transformed into a new `Fail` result with the same error.

*The difference between `Select` and `SelectMany` is in the return value of their `onSuccessSelector` function. The selector for `Select` returns a regular (non-result) value, which is the value of the returned `Success` result. The selector for `SelectMany` returns a result value, which is itself the returned result (and might not be `Success`).*

```c#
Result<int>.Success(123).Select(value => value.ToString()); // Success("123")
Result<int>.Fail("A").Select(value => value.ToString()); // Fail("A")

Maybe<int>.Success(123).Select(value => value.ToString()); // Success("123")
Maybe<int>.None().Select(value => value.ToString()); // None
Maybe<int>.Fail("A").Select(value => value.ToString()); // Fail("A")
```

#### SelectMany

*Applicable to all three result types.*

Transforms the current result - if `Success` - into a new result using the specified `onSuccessSelector` function. Otherwise, if the current result is `Fail`, it is transformed into a new `Fail` result with the same error.

*The difference between `Select` and `SelectMany` is in the return value of their `onSuccessSelector` function. The selector for `Select` returns a regular (non-result) value, which is the value of the returned `Success` result. The selector for `SelectMany` returns a result value, which is itself the returned result (and might not be `Success`).*

```c#
Result<int>.Success(123).SelectMany(GetSuccessResult); // Success("123")
Result<int>.Success(123).SelectMany(GetFailResult); // Fail
Result<int>.Fail("A").SelectMany(GetSuccessResult); // Fail
Result<int>.Fail("A").SelectMany(GetFailResult); // Fail

Maybe<bool>.Success(true).SelectMany(GetSuccessResult); // Success("true")
Maybe<bool>.Success(true).SelectMany(GetNoneResult); // None
Maybe<bool>.Success(true).SelectMany(GetFailResult); // Fail("B")
Maybe<bool>.None().SelectMany(GetSuccessResult); // None
Maybe<bool>.None().SelectMany(GetNoneResult); // None
Maybe<bool>.None().SelectMany(GetFailResult); // None
Maybe<bool>.Fail("A").SelectMany(GetSuccessResult); // Fail("A")
Maybe<bool>.Fail("A").SelectMany(GetNoneResult); // Fail("A")
Maybe<bool>.Fail("A").SelectMany(GetFailResult); // Fail("A")

Result<string> GetSuccessResult(int value) => Result<string>.Success(value.ToString());
Result<string> GetFailResult(int value) => Result<string>.Fail("B");

Maybe<string> GetSuccessResult(bool value) => Maybe<string>.Success(value.ToString());
Maybe<string> GetNoneResult(bool value) => Maybe<string>.None();
Maybe<string> GetFailResult(bool value) => Maybe<string>.Fail("B");
```

#### Flatten

Flattens a `Result<Result<T>>` into a `Result<T>` or a `Maybe<Maybe<T>>` into a `Maybe<T>`.

```c#
Result<Result<int>> nestedResult = ...
Result<int> flattenedResult = nestedResult.Flatten();

Maybe<Maybe<int>> nestedMaybe = ...
Maybe<int> flattenedMaybe = nestedMaybe.Flatten();
```

#### Where

*Applicable to `Maybe<T>` and `Result<T>` only.*

Filters a `Success` result to `None` unless the specified filter function evaluates to true. `None` and `Fail` results are not affected.

```c#
Maybe<int>.Success(123).Where(value => value < 150); // Success(123)
Maybe<int>.Success(456).Where(value => value < 150); // None
Maybe<int>.None().Where(value => value < 150); // None
Maybe<int>.Fail("A").Where(value => value < 150); // Fail("A")
```

#### WithError

*Applicable to all three result types.*

Returns a new result with a different error if the source is a `Fail` result. `Success` and `None` results are not affected.

```c#
Result failResult = Result.Fail("Inner error");
Result successResult = Result.Success();

// Fail("Outer error"("Inner error"))
failResult.WithError(error => new Error("Outer error") { InnerError = error });

// Success
successResult.WithError(error => new Error("Outer error") { InnerError = error });
```

### Custom Errors

Custom errors can be created by inheriting from the `Error` record class.

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
Result<int> result = Result<int>.Fail(new NotFoundError(123));

// errorTitle: "Not Found Error"
string errorTitle = result.GetError().Title;

// errorMessage: "A record with the ID 123 could not be found."
string errorMessage = result.GetError().Message;

// errorCode: 404
int? errorCode = result.GetError().ErrorCode;
```

## Handling multiple results

TODO: Complete this section

### LINQ Extension Methods

TODO: Add introduction

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
