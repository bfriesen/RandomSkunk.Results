namespace RandomSkunk.Results.UnitTests;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
public class TryCatch_methods
{
    public class Given_no_generic_exception
    {
        public class For_ToResult
        {
            [Fact]
            public void When_source_throws_Returns_Fail_result()
            {
                static void ThrowException() => throw new Exception("a");

                var actual = TryCatch.AsResult(ThrowException);

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(Exception).FullName);
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                var actual = TryCatch.AsResult(() => { });

                actual.IsSuccess.Should().BeTrue();
            }
        }

        public class For_ToResult_of_T
        {
            [Fact]
            public void When_source_throws_Returns_Fail_result()
            {
                static int ThrowException() => throw new Exception("a");

                var actual = TryCatch.AsResult(ThrowException);

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(Exception).FullName);
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                var actual = TryCatch.AsResult(() => 1);

                actual.IsSuccess.Should().BeTrue();
                actual.Value.Should().Be(1);
            }
        }

        public class For_Async_ToResult
        {
            [Fact]
            public async Task When_source_throws_Returns_Fail_result()
            {
                var actual = await TryCatch.AsResult(async () => throw new Exception("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(Exception).FullName);
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch.AsResult(async () => { });

                actual.IsSuccess.Should().BeTrue();
            }
        }

        public class For_Async_ToResult_of_T
        {
            [Fact]
            public async Task When_source_throws_Returns_Fail_result()
            {
                var actual = await TryCatch.AsResult<int>(async () => throw new Exception("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(Exception).FullName);
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch.AsResult(async () => 1);

                actual.IsSuccess.Should().BeTrue();
                actual.Value.Should().Be(1);
            }
        }
    }

    public class Given_generic_exception
    {
        public class For_ToResult
        {
            [Fact]
            public void When_source_throws_exception_argument_Returns_Fail_result()
            {
                static void ThrowInvalidOperationException() => throw new InvalidOperationException("a");

                var actual = TryCatch<InvalidOperationException>.AsResult(ThrowInvalidOperationException);

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(InvalidOperationException).FullName);
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                var actual = TryCatch<InvalidOperationException>.AsResult(() => { });

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                static void ThrowApplicationException() => throw new ApplicationException();

                Action act = () => TryCatch<InvalidOperationException>.AsResult(ThrowApplicationException);

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class For_ToResult_of_T
        {
            [Fact]
            public void When_source_throws_exception_argument_Returns_Fail_result()
            {
                static int ThrowInvalidOperationException() => throw new InvalidOperationException("a");

                var actual = TryCatch<InvalidOperationException>.AsResult(ThrowInvalidOperationException);

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(InvalidOperationException).FullName);
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                var actual = TryCatch<InvalidOperationException>.AsResult(() => 1);

                actual.IsSuccess.Should().BeTrue();
                actual.Value.Should().Be(1);
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                static int ThrowApplicationException() => throw new ApplicationException();

                Action act = () => TryCatch<InvalidOperationException>.AsResult(ThrowApplicationException);

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class For_Async_ToResult
        {
            [Fact]
            public async Task When_source_throws_exception_argument_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException>.AsResult(
                    async () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(InvalidOperationException).FullName);
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch<InvalidOperationException>.AsResult(async () => { });

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<Task> act = async () => await TryCatch<InvalidOperationException>.AsResult(
                    async () => throw new ApplicationException());

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }

        public class For_Async_ToResult_of_T
        {
            [Fact]
            public async Task When_source_throws_exception_argument_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException>.AsResult<int>(
                    async () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(InvalidOperationException).FullName);
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch<InvalidOperationException>.AsResult(
                    async () => 1);

                actual.IsSuccess.Should().BeTrue();
                actual.Value.Should().Be(1);
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<Task> act = async () => await TryCatch<InvalidOperationException>.AsResult<int>(
                    async () => throw new ApplicationException());

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }
    }

    public class Given_two_generic_exceptions
    {
        public class For_ToResult
        {
            [Fact]
            public void When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                static void ThrowInvalidOperationException() => throw new InvalidOperationException("a");

                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(ThrowInvalidOperationException);

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(InvalidOperationException).FullName);
            }

            [Fact]
            public void When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                static void ThrowDivideByZeroException() => throw new DivideByZeroException("a");

                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(ThrowDivideByZeroException);

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(DivideByZeroException).FullName);
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(() => { });

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                static void ThrowApplicationException() => throw new ApplicationException();

                Action act = () => TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(ThrowApplicationException);

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class For_ToResult_of_T
        {
            [Fact]
            public void When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                static int ThrowInvalidOperationException() => throw new InvalidOperationException("a");

                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(ThrowInvalidOperationException);

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(InvalidOperationException).FullName);
            }

            [Fact]
            public void When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                static int ThrowDivideByZeroException() => throw new DivideByZeroException("a");

                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(ThrowDivideByZeroException);

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(DivideByZeroException).FullName);
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(() => 1);

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                static int ThrowApplicationException() => throw new ApplicationException();

                Action act = () => TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(ThrowApplicationException);

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class For_Async_ToResult
        {
            [Fact]
            public async Task When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(
                    async () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(InvalidOperationException).FullName);
            }

            [Fact]
            public async Task When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(
                    async () => throw new DivideByZeroException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(DivideByZeroException).FullName);
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(
                    async () => { });

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<Task> act = () => TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(
                    async () => throw new ApplicationException());

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }

        public class For_Async_ToResult_of_T
        {
            [Fact]
            public async Task When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsResult<int>(
                    async () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(InvalidOperationException).FullName);
            }

            [Fact]
            public async Task When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsResult<int>(
                    async () => throw new DivideByZeroException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(typeof(DivideByZeroException).FullName);
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(
                    async () => 1);

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<Task> act = () => TryCatch<InvalidOperationException, DivideByZeroException>.AsResult<int>(
                    async () => throw new ApplicationException());

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
