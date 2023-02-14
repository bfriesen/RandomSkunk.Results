<Query Kind="Statements">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

// The Result struct represents the outcome of an operation that does not return a value.
// Once created, an instance of Result cannot be changed - it is immutable.

// Note that there are no public constructors for the Result struct - the only way to create one is by
// calling one of the Success or Fail methods.

// Successful operations are represented by a Success result.
Result successResult = Result.Success();

// Unsuccessful operations are represented by a Fail result, which has an error that describes what went wrong.
Result failResult = Result.Fail("Something went wrong.");

// A Result "Success" has the following properties:
// - IsSuccess: true
// - IsFail: false
// - Error: Throws an exception because IsFail is false
successResult.Dump(nameof(successResult));

// A Result "Fail" has the following properties:
// - IsSuccess: false
// - IsFail: true
// - Error: A non-null Error object that describes what went wrong
failResult.Dump(nameof(failResult));
