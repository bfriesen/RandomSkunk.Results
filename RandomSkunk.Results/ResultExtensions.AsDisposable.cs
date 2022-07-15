namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>AsDisposable</c> and <c>AsAsyncDisposable</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Gets an object that, when disposed, disposes the source result's value.
    /// </summary>
    /// <typeparam name="TDisposable">The <see cref="IDisposable"/> type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An <see cref="IDisposable"/> object.</returns>
    public static IDisposable AsDisposable<TDisposable>(this Result<TDisposable> sourceResult)
        where TDisposable : IDisposable =>
        new DisposableResult<TDisposable>(sourceResult);

    /// <summary>
    /// Gets an object that, when disposed, disposes the source result's value.
    /// </summary>
    /// <typeparam name="TDisposable">The <see cref="IDisposable"/> type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An <see cref="IDisposable"/> object.</returns>
    public static IDisposable AsDisposable<TDisposable>(this Maybe<TDisposable> sourceResult)
        where TDisposable : IDisposable =>
        new DisposableMaybe<TDisposable>(sourceResult);

    /// <summary>
    /// Gets an object that, when disposed, disposes the source result's value.
    /// </summary>
    /// <typeparam name="TAsyncDisposable">The <see cref="IAsyncDisposable"/> type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An <see cref="IAsyncDisposable"/> object.</returns>
    public static IAsyncDisposable AsAsyncDisposable<TAsyncDisposable>(this Result<TAsyncDisposable> sourceResult)
        where TAsyncDisposable : IAsyncDisposable =>
        new AsyncDisposableResult<TAsyncDisposable>(sourceResult);

    /// <summary>
    /// Gets an object that, when disposed, disposes the source result's value.
    /// </summary>
    /// <typeparam name="TAsyncDisposable">The <see cref="IAsyncDisposable"/> type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An <see cref="IAsyncDisposable"/> object.</returns>
    public static IAsyncDisposable AsAsyncDisposable<TAsyncDisposable>(this Maybe<TAsyncDisposable> sourceResult)
        where TAsyncDisposable : IAsyncDisposable =>
        new AsyncDisposableMaybe<TAsyncDisposable>(sourceResult);

    private class DisposableResult<TDisposable> : IDisposable
        where TDisposable : IDisposable
    {
        private readonly Result<TDisposable> _source;

        public DisposableResult(Result<TDisposable> source) => _source = source;

        public void Dispose() => _source.OnSuccess(value => value.Dispose());
    }

    private class DisposableMaybe<TDisposable> : IDisposable
        where TDisposable : IDisposable
    {
        private readonly Maybe<TDisposable> _source;

        public DisposableMaybe(Maybe<TDisposable> source) => _source = source;

        public void Dispose() => _source.OnSuccess(value => value.Dispose());
    }

    private class AsyncDisposableResult<TAsyncDisposable> : IAsyncDisposable
        where TAsyncDisposable : IAsyncDisposable
    {
        private readonly Result<TAsyncDisposable> _source;

        public AsyncDisposableResult(Result<TAsyncDisposable> source) => _source = source;

        public async ValueTask DisposeAsync() => await _source.OnSuccessAsync(async value => await value.DisposeAsync()).ConfigureAwait(false);
    }

    private class AsyncDisposableMaybe<TAsyncDisposable> : IAsyncDisposable
        where TAsyncDisposable : IAsyncDisposable
    {
        private readonly Maybe<TAsyncDisposable> _source;

        public AsyncDisposableMaybe(Maybe<TAsyncDisposable> source) => _source = source;

        public async ValueTask DisposeAsync() => await _source.OnSuccessAsync(async value => await value.DisposeAsync()).ConfigureAwait(false);
    }
}
