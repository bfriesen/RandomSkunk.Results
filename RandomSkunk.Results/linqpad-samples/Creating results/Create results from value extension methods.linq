<Query Kind="Statements">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
  <Namespace>RandomSkunk.Results.FactoryExtensions</Namespace>
</Query>

// using RandomSkunk.Results.FactoryExtensions;

string someValue = "abc";
string? nullValue = null;

// A Result<T> or Maybe<T> "Success" can be created directly from with the ToResult() or
// ToMaybe() extension methods.
Result<string> resultFromValue = someValue.ToResult();
Maybe<string> maybeFromValue = someValue.ToMaybe();

// As expected, the Result<T> and Maybe<T> are "Success" and have the expected value.
resultFromValue.Dump(nameof(resultFromValue));
maybeFromValue.Dump(nameof(maybeFromValue));

// Unlike the Result<T>.Success and Maybe<T>.Success methods, Result<T>.FromValue
// and Maybe<T>.FromValue do not throw an exception when a null value is passed.
Result<string> resultFromNull = nullValue.ToResult();
Maybe<string> maybeFromNull = nullValue.ToMaybe();

// A Result<T> created from null is "Fail".
resultFromNull.Dump(nameof(resultFromNull));

// A Maybe<T> created from null is "None".
maybeFromNull.Dump(nameof(maybeFromNull));
