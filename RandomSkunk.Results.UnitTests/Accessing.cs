namespace RandomSkunk.Results.UnitTests;

public static class Accessing
{
    public static Action Error(Result result) => () => _ = result.Error;

    public static Action Error<T>(Result<T> result) => () => _ = result.Error;

    public static Action Error<T>(Maybe<T> result) => () => _ = result.Error;

    public static Action Value<T>(Result<T> result) => () => _ = result.Value;

    public static Action Value<T>(Maybe<T> result) => () => _ = result.Value;
}
