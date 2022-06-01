using static RandomSkunk.Results.MaybeType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Dispose</c> method.
/// </content>
public partial struct Maybe<T> : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// If this is a <c>Some</c> result and its value is <see cref="IDisposable"/>, dispose the result value. Otherwise, do
    /// nothing.
    /// </summary>
    public void Dispose()
    {
        if (_type == Some && _value is IDisposable disposable)
            disposable.Dispose();
    }

    /// <summary>
    /// If this is a <c>Some</c> result and its value is <see cref="IAsyncDisposable"/>, dispose the result value. Otherwise, do
    /// nothing.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        if (_type == Some && _value is IAsyncDisposable asyncDisposable)
            await asyncDisposable.DisposeAsync();
    }
}
