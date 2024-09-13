<Query Kind="Statements">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

// Success results can be implicitly created from a value.
Result<int> resultOfInt = 123;

resultOfInt.Dump(nameof(resultOfInt));

// Fail results can be implicitly created from an Error.
Result result = Errors.BadGateway();
Result<string> resultOfString = Errors.BadRequest();

result.Dump(nameof(result));
resultOfString.Dump(nameof(resultOfString));