<Query Kind="Program">
  <NuGetReference>RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

void Main()
{
    // Success results can be implicitly created from a value.
    Result<int> resultOfInt = 123;
    Maybe<string> maybeOfString = "abc";

    resultOfInt.Dump(nameof(resultOfInt));
    maybeOfString.Dump(nameof(maybeOfString));

    // Fail results can be implicitly created from an Error.
    Result result = Errors.BadGateway();
    Result<string> resultOfString = Errors.BadRequest();
    Maybe<int> maybeOfInt = Errors.GatewayTimeout();

    result.Dump(nameof(result));
    resultOfString.Dump(nameof(resultOfString));
    maybeOfInt.Dump(nameof(maybeOfInt));
}
