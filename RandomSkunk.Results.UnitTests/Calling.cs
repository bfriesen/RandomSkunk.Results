using RandomSkunk.Results.Unsafe;

namespace RandomSkunk.Results.UnitTests;

public static class Calling
{
    public static Action GetError(Result result) => () => _ = result.GetError();

    public static Action GetError<T>(Result<T> result) => () => _ = result.GetError();

    public static Action GetError<T>(Maybe<T> result) => () => _ = result.GetError();

    public static Action GetValue<T>(Result<T> result) => () => _ = result.GetValue();

    public static Action GetValue<T>(Maybe<T> result) => () => _ = result.GetValue();
}
