<Query Kind="Statements">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

// Each of the result types is a struct and therefore has a non-null default value.
// The default value for each result type is a "Fail" result with an error
// indicating that the result is uninitialized.

Result defaultResult = default(Result);
Result<string> defaultResultOfString = default(Result<string>);

defaultResult.Dump(nameof(defaultResult));
defaultResultOfString.Dump(nameof(defaultResultOfString));
