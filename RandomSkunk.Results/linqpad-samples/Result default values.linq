<Query Kind="Statements">
  <NuGetReference Prerelease="true">RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

// Each of the result types is a struct and therefore has a non-null default value.
// The default value for each result type is a "Fail" result with a default error.

Result defaultResult = default(Result);
Result<string> defaultResultOfString = default(Result<string>);
Maybe<string> defaultMaybeOfString = default(Maybe<string>);

defaultResult.Dump(nameof(defaultResult));
defaultResultOfString.Dump(nameof(defaultResultOfString));
defaultMaybeOfString.Dump(nameof(defaultMaybeOfString));
