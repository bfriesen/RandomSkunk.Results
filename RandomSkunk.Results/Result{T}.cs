using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RandomSkunk.Results
{
    /// <summary>
    /// The result of an operation that has a return value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    public abstract class Result<T> : ResultBase<T>
    {
        private Result(CallSite callSite)
            : base(callSite) { }

        /// <summary>
        /// Creates a <c>success</c> result for an operation with a return value.
        /// </summary>
        /// <param name="value">The value of the <c>success</c> result.</param>
        /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
        /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
        /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
        /// <returns>A <c>success</c> result.</returns>
        public static Result<T> Success(
            [DisallowNull] T value,
            [CallerMemberName] string memberName = null!,
            [CallerFilePath] string filePath = null!,
            [CallerLineNumber] int lineNumber = 0) =>
            new SuccessResult(value, new CallSite(memberName, filePath, lineNumber));

        /// <summary>
        /// Creates a <c>fail</c> result for an operation with a return value.
        /// </summary>
        /// <param name="error">The error message that describes the failure.</param>
        /// <param name="errorCode">The optional error code that describes the failure.</param>
        /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
        /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
        /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
        /// <returns>A <c>fail</c> result.</returns>
        public static Result<T> Fail(
            string? error = null,
            int? errorCode = null,
            [CallerMemberName] string memberName = null!,
            [CallerFilePath] string filePath = null!,
            [CallerLineNumber] int lineNumber = 0) =>
            new FailResult(error ?? "Error", errorCode, new CallSite(memberName, filePath, lineNumber));

        /// <summary>
        /// Gets the type of the result: <see cref="ResultType.Success"/> or
        /// <see cref="ResultType.Fail"/>.
        /// </summary>
        public abstract ResultType Type { get; }

        /// <summary>
        /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
        /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
        /// </summary>
        /// <typeparam name="TResult">The return type of the functions.</typeparam>
        /// <param name="onSuccess">
        /// The function to evaluate if the result type is <c>success</c>. The value of the <c>success</c> result
        /// is passed to this function.
        /// </param>
        /// <param name="onFail">
        /// The function to evaluate if the result type is <c>fail</c>. The error message and error code
        /// of the <c>fail</c> result are passed to this function.
        /// </param>
        /// <returns>The result of the matching function evaluation.</returns>
        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<string, int?, TResult> onFail)
        {
            ArgumentNullException.ThrowIfNull(onSuccess, nameof(onSuccess));
            ArgumentNullException.ThrowIfNull(onFail, nameof(onFail));

            return MatchCore(onSuccess, onFail);
        }

        /// <summary>
        /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
        /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
        /// </summary>
        /// <param name="onSuccess">
        /// The function to evaluate if the result type is <c>success</c>. The value of the <c>success</c> result
        /// is passed to this function.
        /// </param>
        /// <param name="onFail">
        /// The function to evaluate if the result type is <c>fail</c>. The error message and error code
        /// of the <c>fail</c> result are passed to this function.
        /// </param>
        public void Match(Action<T> onSuccess, Action<string, int?> onFail)
        {
            ArgumentNullException.ThrowIfNull(onSuccess, nameof(onSuccess));
            ArgumentNullException.ThrowIfNull(onFail, nameof(onFail));

            MatchCore(onSuccess, onFail);
        }

        /// <summary>
        /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
        /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
        /// </summary>
        /// <typeparam name="TResult">The return type of the functions.</typeparam>
        /// <param name="onSuccess">
        /// The function to evaluate if the result type is <c>success</c>. The value of the <c>success</c> result
        /// is passed to this function.
        /// </param>
        /// <param name="onFail">
        /// The function to evaluate if the result type is <c>fail</c>. The error message and error code
        /// of the <c>fail</c> result are passed to this function.
        /// </param>
        /// <returns>The result of the matching function evaluation.</returns>
        public Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> onSuccess, Func<string, int?, Task<TResult>> onFail)
        {
            ArgumentNullException.ThrowIfNull(onSuccess, nameof(onSuccess));
            ArgumentNullException.ThrowIfNull(onFail, nameof(onFail));

            return MatchAsyncCore(onSuccess, onFail);
        }

        /// <summary>
        /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
        /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
        /// </summary>
        /// <param name="onSuccess">
        /// The function to evaluate if the result type is <c>success</c>. The value of the <c>success</c> result
        /// is passed to this function.
        /// </param>
        /// <param name="onFail">
        /// The function to evaluate if the result type is <c>fail</c>. The error message and error code
        /// of the <c>fail</c> result are passed to this function.
        /// </param>
        public Task MatchAsync(Func<T, Task> onSuccess, Func<string, int?, Task> onFail)
        {
            ArgumentNullException.ThrowIfNull(onSuccess, nameof(onSuccess));
            ArgumentNullException.ThrowIfNull(onFail, nameof(onFail));

            return MatchAsyncCore(onSuccess, onFail);
        }
        
        protected abstract TResult MatchCore<TResult>(Func<T, TResult> onSuccess, Func<string, int?, TResult> onFail);
        protected abstract void MatchCore(Action<T> onSuccess, Action<string, int?> onFail);
        protected abstract Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSuccess, Func<string, int?, Task<TResult>> onFail);
        protected abstract Task MatchAsyncCore(Func<T, Task> onSuccess, Func<string, int?, Task> onFail);

        private sealed class SuccessResult : Result<T>
        {
            public SuccessResult(T value, CallSite callSite) : base(callSite) => Value = value ?? throw new ArgumentNullException(nameof(value));

            public override ResultType Type => ResultType.Success;
            public override bool IsSuccess => true;
            [NotNull] public override T Value { get; }

            protected override TResult MatchCore<TResult>(Func<T, TResult> onSuccess, Func<string, int?, TResult> onFail) => onSuccess(Value);
            protected override void MatchCore(Action<T> onSuccess, Action<string, int?> onFail) => onSuccess(Value);
            protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSuccess, Func<string, int?, Task<TResult>> onFail) => onSuccess(Value);
            protected override Task MatchAsyncCore(Func<T, Task> onSuccess, Func<string, int?, Task> onFail) => onSuccess(Value);
        }

        private sealed class FailResult : Result<T>
        {
            public FailResult(string error, int? errorCode, CallSite callSite) : base(callSite) => (Error, ErrorCode) = (error, errorCode);

            public override ResultType Type => ResultType.Fail;
            public override bool IsFail => true;
            public override string Error { get; }
            public override int? ErrorCode { get; }

            protected override TResult MatchCore<TResult>(Func<T, TResult> onSuccess, Func<string, int?, TResult> onFail) => onFail(Error, ErrorCode);
            protected override void MatchCore(Action<T> onSuccess, Action<string, int?> onFail) => onFail(Error, ErrorCode);
            protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSuccess, Func<string, int?, Task<TResult>> onFail) => onFail(Error, ErrorCode);
            protected override Task MatchAsyncCore(Func<T, Task> onSuccess, Func<string, int?, Task> onFail) => onFail(Error, ErrorCode);
        }
    }
}
