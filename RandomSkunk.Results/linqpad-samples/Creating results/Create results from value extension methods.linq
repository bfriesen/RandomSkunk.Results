<Query Kind="Statements">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
  <Namespace>RandomSkunk.Results.FactoryExtensions</Namespace>
</Query>

// using RandomSkunk.Results.FactoryExtensions;

string someValue = "abc";
string? nullValue = null;

// A Result<T> "Success" can be created directly from the ToResult() extension method.
Result<string> resultFromValue = someValue.ToResult();

// As expected, the Result<T> is "Success" and has the expected value.
resultFromValue.Dump(nameof(resultFromValue));

// Unlike the Result<T>.Success method, Result<T>.FromValue does not throw an exception
// when a null value is passed.
Result<string> resultFromNull = nullValue.ToResult();

// A Result<T> created from null is "None", and has an error indicating that
// it has no value.
resultFromNull.Dump(nameof(resultFromNull));
