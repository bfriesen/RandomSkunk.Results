namespace RandomSkunk.Results;

/// <summary>
/// Encapsulates an asynchronous method that has no parameters and returns a value of the type specified by the
/// <typeparamref name="T"/> parameter.
/// </summary>
/// <typeparam name="T">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <returns>A task that represents the asynchronous operation, which wraps the return value.</returns>
public delegate Task<T> AsyncFunc<T>();
