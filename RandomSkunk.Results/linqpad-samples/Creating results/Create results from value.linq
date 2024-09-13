<Query Kind="Statements">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

string someValue = "abc";

// A Result<T> "Success" can be created directly from a value with the
// FromValue method.
Result<string> resultFromValue = Result<string>.FromValue(someValue);

// As expected, the Result<T> is "Success" and has the expected value.
resultFromValue.Dump(nameof(resultFromValue));

// Unlike the Result<T>.Success method, Result<T>.FromValue does not throw
// an exception when a null value is passed.
Result<string> resultFromNull = Result<string>.FromValue(null);

// A Result<T> created from null is "None", and has an error indicating that
// it has no value.
resultFromNull.Dump(nameof(resultFromNull));
