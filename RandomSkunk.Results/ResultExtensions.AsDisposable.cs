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
    /// <param name="source">The source result.</param>
    /// <returns>An <see cref="IDisposable"/> object.</returns>
    public static IDisposable AsDisposable<TDisposable>(this Result<TDisposable> source)
        where TDisposable : IDisposable =>
        new DisposableResult<TDisposable>(source);

    /// <summary>
    /// Gets an object that, when disposed, disposes the source result's value.
    /// </summary>
    /// <typeparam name="TDisposable">The <see cref="IDisposable"/> type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>An <see cref="IDisposable"/> object.</returns>
    public static IDisposable AsDisposable<TDisposable>(this Maybe<TDisposable> source)
        where TDisposable : IDisposable =>
        new DisposableMaybe<TDisposable>(source);

    /// <summary>
    /// Gets an object that, when disposed, disposes the source result's value.
    /// </summary>
    /// <typeparam name="TAsyncDisposable">The <see cref="IAsyncDisposable"/> type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>An <see cref="IAsyncDisposable"/> object.</returns>
    public static IAsyncDisposable AsAsyncDisposable<TAsyncDisposable>(this Result<TAsyncDisposable> source)
        where TAsyncDisposable : IAsyncDisposable =>
        new AsyncDisposableResult<TAsyncDisposable>(source);

    /// <summary>
    /// Gets an object that, when disposed, disposes the source result's value.
    /// </summary>
    /// <typeparam name="TAsyncDisposable">The <see cref="IAsyncDisposable"/> type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <returns>An <see cref="IAsyncDisposable"/> object.</returns>
    public static IAsyncDisposable AsAsyncDisposable<TAsyncDisposable>(this Maybe<TAsyncDisposable> source)
        where TAsyncDisposable : IAsyncDisposable =>
        new AsyncDisposableMaybe<TAsyncDisposable>(source);

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

        public void Dispose() => _source.OnSome(value => value.Dispose());
    }

    private class AsyncDisposableResult<TAsyncDisposable> : IAsyncDisposable
        where TAsyncDisposable : IAsyncDisposable
    {
        private readonly Result<TAsyncDisposable> _source;

        public AsyncDisposableResult(Result<TAsyncDisposable> source) => _source = source;

        public async ValueTask DisposeAsync() => await _source.OnSuccessAsync(async value => await value.DisposeAsync());
    }

    private class AsyncDisposableMaybe<TAsyncDisposable> : IAsyncDisposable
        where TAsyncDisposable : IAsyncDisposable
    {
        private readonly Maybe<TAsyncDisposable> _source;

        public AsyncDisposableMaybe(Maybe<TAsyncDisposable> source) => _source = source;

        public async ValueTask DisposeAsync() => await _source.OnSomeAsync(async value => await value.DisposeAsync());
    }
}
