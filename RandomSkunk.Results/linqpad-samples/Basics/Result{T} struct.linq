<Query Kind="Statements">
  <NuGetReference Prerelease="true">RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

// The Result<T> struct represents the outcome of an operation that returns a value.
// Once created, an instance of Result<T> cannot be changed - it is immutable.

// Note that there are no public constructors for the Result<T> struct - the only way to create one is by
// calling one of the Success or Fail methods.

// Successful operations are represented by a Success result with a non-null value.
Result<string> successResult = Result<string>.Success("abc");

// Note that passing null to the Success method will result in a thrown exception.
// Result<string> thrownResult = Result<string>.Success(null!);

// Unsuccessful operations are represented by a Fail result, which has an error that describes what went wrong.
Result<string> failResult = Result<string>.Fail("Something went wrong.");

// A Result<T> "Success" has the following properties:
// - IsSuccess: true
// - IsFail: false
// - Value: The non-null value of the result
// - Error: Throws an exception because IsFail is false
successResult.Dump(nameof(successResult));

// A Result<T> "Fail" has the following properties:
// - IsSuccess: false
// - IsFail: true
// - Value: Throws an exception because IsSuccess is false
// - Error: A non-null Error object that describes what went wrong
failResult.Dump(nameof(failResult));
