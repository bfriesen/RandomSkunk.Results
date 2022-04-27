using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RandomSkunk.Results
{
    /// <summary>
    /// The result of an operation that has a return value where that return value is allowed to be
    /// absent.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    public abstract class MaybeResult<T> : ResultBase<T>
    {
        private MaybeResult(CallSite callSite)
            : base(callSite) { }

        /// <summary>
        /// Creates a <c>some</c> result for an operation with a return value.
        /// </summary>
        /// <param name="value">
        /// The value of the <c>some</c> result. Must not be <see langword="null"/>.
        /// </param>
        /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
        /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
        /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
        /// <returns>A <c>some</c> result.</returns>
        public static MaybeResult<T> Some(
            [DisallowNull] T value,
            [CallerMemberName] string memberName = null!,
            [CallerFilePath] string filePath = null!,
            [CallerLineNumber] int lineNumber = 0) =>
            new SomeResult(value, new CallSite(memberName, filePath, lineNumber));

        /// <summary>
        /// Creates a <c>none</c> result for an operation with a return value.
        /// </summary>
        /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
        /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
        /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
        /// <returns>A <c>none</c> result.</returns>
        public static MaybeResult<T> None(
            [CallerMemberName] string memberName = null!,
            [CallerFilePath] string filePath = null!,
            [CallerLineNumber] int lineNumber = 0) =>
            new NoneResult(new CallSite(memberName, filePath, lineNumber));

        /// <summary>
        /// Creates a <c>fail</c> result for an operation with a return value.
        /// </summary>
        /// <param name="error">The error message that describes the failure.</param>
        /// <param name="errorCode">The optional error code that describes the failure.</param>
        /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
        /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
        /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
        /// <returns>A <c>fail</c> result.</returns>
        public static MaybeResult<T> Fail(
            string? error = null,
            int? errorCode = null,
            [CallerMemberName] string memberName = null!,
            [CallerFilePath] string filePath = null!,
            [CallerLineNumber] int lineNumber = 0) =>
            new FailResult(error, errorCode, new CallSite(memberName, filePath, lineNumber));

        /// <summary>
        /// Gets a value indicating whether the result object is the result of a successful
        /// operation <em>with</em> a value.
        /// </summary>
        public virtual bool IsSome => false;

        /// <summary>
        /// Gets a value indicating whether the result object is the result of a successful
        /// operation <em>without</em> a value.
        /// </summary>
        public virtual bool IsNone => false;

        /// <summary>
        /// Gets the type of the result: <see cref="MaybeResultType.Some"/>,
        /// <see cref="MaybeResultType.None"/>, or <see cref="MaybeResultType.Fail"/>.
        /// </summary>
        public abstract MaybeResultType Type { get; }

        /// <summary>
        /// Evaluates either the <paramref name="onSome"/>, <paramref name="onNone"/>, or
        /// <paramref name="onFail"/> function depending on whether the result type is <c>some</c>,
        /// <c>none</c>, or <c>fail</c>.
        /// </summary>
        /// <typeparam name="TResult">The return type of the functions.</typeparam>
        /// <param name="onSome">
        /// The function to evaluate if the result type is <c>some</c>. The value of the
        /// <c>some</c> result is passed to this function.
        /// </param>
        /// <param name="onNone">
        /// The function to evaluate if the result type is <c>none</c>.
        /// </param>
        /// <param name="onFail">
        /// The function to evaluate if the result type is <c>fail</c>. The error message and error
        /// code of the <c>fail</c> result are passed to this function.
        /// </param>
        /// <returns>The result of the matching function evaluation.</returns>
        public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone, Func<string?, int?, TResult> onFail)
        {
            ArgumentNullException.ThrowIfNull(onSome, nameof(onSome));
            ArgumentNullException.ThrowIfNull(onNone, nameof(onNone));
            ArgumentNullException.ThrowIfNull(onFail, nameof(onFail));

            return MatchCore(onSome, onNone, onFail);
        }

        /// <summary>
        /// Evaluates either the <paramref name="onSome"/>, <paramref name="onNone"/>, or
        /// <paramref name="onFail"/> function depending on whether the result type is <c>some</c>,
        /// <c>none</c>, or <c>fail</c>.
        /// </summary>
        /// <param name="onSome">
        /// The function to evaluate if the result type is <c>some</c>. The value of the
        /// <c>some</c> result is passed to this function.
        /// </param>
        /// <param name="onNone">
        /// The function to evaluate if the result type is <c>none</c>.
        /// </param>
        /// <param name="onFail">
        /// The function to evaluate if the result type is <c>fail</c>. The error message and error
        /// code of the <c>fail</c> result are passed to this function.
        /// </param>
        public void Match(Action<T> onSome, Action onNone, Action<string?, int?> onFail)
        {
            ArgumentNullException.ThrowIfNull(onSome, nameof(onSome));
            ArgumentNullException.ThrowIfNull(onNone, nameof(onNone));
            ArgumentNullException.ThrowIfNull(onFail, nameof(onFail));

            MatchCore(onSome, onNone, onFail);
        }

        /// <summary>
        /// Evaluates either the <paramref name="onSome"/>, <paramref name="onNone"/>, or
        /// <paramref name="onFail"/> function depending on whether the result type is <c>some</c>,
        /// <c>none</c>, or <c>fail</c>.
        /// </summary>
        /// <typeparam name="TResult">The return type of the functions.</typeparam>
        /// <param name="onSome">
        /// The function to evaluate if the result type is <c>some</c>. The value of the
        /// <c>some</c> result is passed to this function.
        /// </param>
        /// <param name="onNone">
        /// The function to evaluate if the result type is <c>none</c>.
        /// </param>
        /// <param name="onFail">
        /// The function to evaluate if the result type is <c>fail</c>. The error message and error
        /// code of the <c>fail</c> result are passed to this function.
        /// </param>
        /// <returns>The result of the matching function evaluation.</returns>
        public Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone, Func<string?, int?, Task<TResult>> onFail)
        {
            ArgumentNullException.ThrowIfNull(onSome, nameof(onSome));
            ArgumentNullException.ThrowIfNull(onNone, nameof(onNone));
            ArgumentNullException.ThrowIfNull(onFail, nameof(onFail));

            return MatchAsyncCore(onSome, onNone, onFail);
        }

        /// <summary>
        /// Evaluates either the <paramref name="onSome"/>, <paramref name="onNone"/>, or
        /// <paramref name="onFail"/> function depending on whether the result type is <c>some</c>,
        /// <c>none</c>, or <c>fail</c>.
        /// </summary>
        /// <param name="onSome">
        /// The function to evaluate if the result type is <c>some</c>. The value of the
        /// <c>some</c> result is passed to this function.
        /// </param>
        /// <param name="onNone">
        /// The function to evaluate if the result type is <c>none</c>.
        /// </param>
        /// <param name="onFail">
        /// The function to evaluate if the result type is <c>fail</c>. The error message and error
        /// code of the <c>fail</c> result are passed to this function.
        /// </param>
        public Task MatchAsync(Func<T, Task> onSome, Func<Task> onNone, Func<string?, int?, Task> onFail)
        {
            ArgumentNullException.ThrowIfNull(onSome, nameof(onSome));
            ArgumentNullException.ThrowIfNull(onNone, nameof(onNone));
            ArgumentNullException.ThrowIfNull(onFail, nameof(onFail));

            return MatchAsyncCore(onSome, onNone, onFail);
        }

        protected abstract TResult MatchCore<TResult>(Func<T, TResult> onSome, Func<TResult> onNone, Func<string?, int?, TResult> onFail);
        protected abstract void MatchCore(Action<T> onSome, Action onNone, Action<string?, int?> onFail);        
        protected abstract Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone, Func<string?, int?, Task<TResult>> onFail);
        protected abstract Task MatchAsyncCore(Func<T, Task> onSome, Func<Task> onNone, Func<string?, int?, Task> onFail);

        private sealed class SomeResult : MaybeResult<T>
        {
            public SomeResult(T value, CallSite callSite) : base(callSite) => Value = value ?? throw new ArgumentNullException(nameof(value));

            public override MaybeResultType Type => MaybeResultType.Some;
            public override bool IsSuccess => true;
            public override bool IsSome => true;
            [NotNull] public override T Value { get; }

            protected override TResult MatchCore<TResult>(Func<T, TResult> onSome, Func<TResult> onNone, Func<string?, int?, TResult> onFail) => onSome(Value);
            protected override void MatchCore(Action<T> onSome, Action onNone, Action<string?, int?> onFail) => onSome(Value);
            protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone, Func<string?, int?, Task<TResult>> onFail) => onSome(Value);
            protected override Task MatchAsyncCore(Func<T, Task> onSome, Func<Task> onNone, Func<string?, int?, Task> onFail) => onSome(Value);
        }

        private sealed class NoneResult : MaybeResult<T>
        {
            public NoneResult(CallSite callSite) : base(callSite) { }

            public override MaybeResultType Type => MaybeResultType.None;
            public override bool IsSuccess => true;
            public override bool IsNone => true;

            protected override TResult MatchCore<TResult>(Func<T, TResult> onSome, Func<TResult> onNone, Func<string?, int?, TResult> onFail) => onNone();
            protected override void MatchCore(Action<T> onSome, Action onNone, Action<string?, int?> onFail) => onNone();
            protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone, Func<string?, int?, Task<TResult>> onFail) => onNone();
            protected override Task MatchAsyncCore(Func<T, Task> onSome, Func<Task> onNone, Func<string?, int?, Task> onFail) => onNone();
        }

        private sealed class FailResult : MaybeResult<T>
        {
            public FailResult(string? error, int? errorCode, CallSite callSite) : base(callSite) => (Error, ErrorCode) = (error ?? "Error", errorCode);

            public override MaybeResultType Type => MaybeResultType.Fail;
            public override bool IsFail => true;
            public override string Error { get; }
            public override int? ErrorCode { get; }

            protected override TResult MatchCore<TResult>(Func<T, TResult> onSome, Func<TResult> onNone, Func<string?, int?, TResult> onFail) => onFail(Error, ErrorCode);
            protected override void MatchCore(Action<T> onSome, Action onNone, Action<string?, int?> onFail) => onFail(Error, ErrorCode);
            protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone, Func<string?, int?, Task<TResult>> onFail) => onFail(Error, ErrorCode);
            protected override Task MatchAsyncCore(Func<T, Task> onSome, Func<Task> onNone, Func<string?, int?, Task> onFail) => onFail(Error, ErrorCode);
        }
    }

}