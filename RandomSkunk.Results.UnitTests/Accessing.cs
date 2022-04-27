namespace RandomSkunk.Results.UnitTests;

public static class Accessing
{
    public static Action Error(ResultBase result) => () => _ = result.Error;

    public static Action Value<T>(ResultBase<T> result) => () => _ = result.Value;
}
