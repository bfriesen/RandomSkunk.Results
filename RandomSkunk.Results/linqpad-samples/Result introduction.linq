<Query Kind="Statements">
  <NuGetReference Prerelease="true">RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

// The Result struct represents the outcome of an operation that does not return a value.

// Successful operations are represented by a Success result.
Result successResult = Result.Success();

// Unsuccessful operations are represented by a Fail result, which has an error that describes what went wrong.
Result failResult = Result.Fail("Something went wrong.");

// A Result "Success" always has the following properties:
// - IsSuccess: true
// - IsFail: false
// - Error: Throws an exception because IsFail is false
successResult.Dump(nameof(successResult));

// A Result "Fail" always has the following properties:
// - IsSuccess: false
// - IsFail: true
// - Error: A non-null Error object that describes what went wrong
failResult.Dump(nameof(failResult));