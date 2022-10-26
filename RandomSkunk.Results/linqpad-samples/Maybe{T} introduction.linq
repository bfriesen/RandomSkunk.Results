<Query Kind="Statements">
  <NuGetReference Prerelease="true">RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

// The Maybe<T> struct represents the outcome of an operation that may or may not return a value.

// Successful operations that have a value are represented by a Success result with a non-null value.
Maybe<string> successMaybe = Maybe<string>.Success("abc");

// Note that passing null to the Success method will result in a thrown exception.
// Maybe<string> thrownResult = Maybe<string>.Success(null!);

// Successful operations that do not have a value are represented by a None result.
Maybe<string> noneMaybe = Maybe<string>.None();

// Unsuccessful operations are represented by a Fail result, which has an error that describes what went wrong.
Maybe<string> failMaybe = Maybe<string>.Fail();

// A Maybe<T> "Success" always has the following properties:
// - IsSuccess: true
// - IsNone: false
// - IsFail: false
// - Value: The non-null value of the result
// - Error: Throws an exception because IsFail is false
successMaybe.Dump();

// A Maybe<T> "None" always has the following properties:
// - IsSuccess: false
// - IsNone: true
// - IsFail: false
// - Value: Throws an exception because IsSuccess is false
// - Error: Throws an exception because IsFail is false
noneMaybe.Dump();

// A Maybe<T> "Fail" always has the following properties:
// - IsSuccess: false
// - IsNone: false
// - IsFail: true
// - Value: Throws an exception because IsSuccess is false
// - Error: A non-null Error object that describes what went wrong
failMaybe.Dump();
