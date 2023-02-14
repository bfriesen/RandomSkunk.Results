<Query Kind="Statements">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

string someValue = "abc";

// A Result<T> or Maybe<T> "Success" can be created directly from a value with the
// FromValue method.
Result<string> resultFromValue = Result<string>.FromValue(someValue);
Maybe<string> maybeFromValue = Maybe<string>.FromValue(someValue);

// As expected, the Result<T> and Maybe<T> are "Success" and have the expected value.
resultFromValue.Dump(nameof(resultFromValue));
maybeFromValue.Dump(nameof(maybeFromValue));

// Unlike the Result<T>.Success and Maybe<T>.Success methods, Result<T>.FromValue
// and Maybe<T>.FromValue do not throw an exception when a null value is passed.
Result<string> resultFromNull = Result<string>.FromValue(null);
Maybe<string> maybeFromNull = Maybe<string>.FromValue(null);

// A Result<T> created from null is "Fail".
resultFromNull.Dump(nameof(resultFromNull));

// A Maybe<T> created from null is "None".
maybeFromNull.Dump(nameof(maybeFromNull));
