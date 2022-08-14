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
                var actual = TryCatch.AsResult(() => throw new Exception("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(Exception));
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
                var actual = TryCatch.AsResult<int>(() => throw new Exception("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(Exception));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                var actual = TryCatch.AsResult(() => 1);

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }
        }

        public class For_ToMaybe_of_T
        {
            [Fact]
            public void When_source_throws_Returns_Fail_result()
            {
                var actual = TryCatch.AsMaybe<int>(() => throw new Exception("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(Exception));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                var actual = TryCatch.AsMaybe(() => 1);

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public void When_source_returns_null_Returns_None_result()
            {
                var actual = TryCatch.AsMaybe<string>(() => null!);

                actual.IsNone.Should().BeTrue();
            }
        }

        public class For_ToResultAsync
        {
            [Fact]
            public async Task When_source_throws_Returns_Fail_result()
            {
                var actual = await TryCatch.AsResultAsync(async () => throw new Exception("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(Exception));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch.AsResultAsync(async () => { });

                actual.IsSuccess.Should().BeTrue();
            }
        }

        public class For_ToResultAsync_of_T
        {
            [Fact]
            public async Task When_source_throws_Returns_Fail_result()
            {
                var actual = await TryCatch.AsResultAsync<int>(async () => throw new Exception("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(Exception));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch.AsResultAsync(async () => 1);

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }
        }

        public class For_ToMaybeAsync_of_T
        {
            [Fact]
            public async Task When_source_throws_Returns_Fail_result()
            {
                var actual = await TryCatch.AsMaybeAsync<int>(async () => throw new Exception("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(Exception));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch.AsMaybeAsync(async () => 1);

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public async Task When_source_returns_null_Returns_None_result()
            {
                var actual = await TryCatch.AsMaybeAsync<string>(async () => null!);

                actual.IsNone.Should().BeTrue();
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
                var actual = TryCatch<InvalidOperationException>.AsResult(
                    () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(InvalidOperationException));
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
                Action act = () => TryCatch<InvalidOperationException>.AsResult(
                    () => throw new ApplicationException());

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class For_ToResult_of_T
        {
            [Fact]
            public void When_source_throws_exception_argument_Returns_Fail_result()
            {
                var actual = TryCatch<InvalidOperationException>.AsResult<int>(
                    () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                var actual = TryCatch<InvalidOperationException>.AsResult(() => 1);

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Action act = () => TryCatch<InvalidOperationException>.AsResult<int>(
                    () => throw new ApplicationException());

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class For_ToMaybe_of_T
        {
            [Fact]
            public void When_source_throws_exception_argument_Returns_Fail_result()
            {
                var actual = TryCatch<InvalidOperationException>.AsMaybe<int>(() => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                var actual = TryCatch<InvalidOperationException>.AsMaybe(() => 1);

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public void When_source_returns_null_Returns_None_result()
            {
                var actual = TryCatch<InvalidOperationException>.AsMaybe<string>(() => null!);

                actual.IsNone.Should().BeTrue();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Action act = () => TryCatch<InvalidOperationException>.AsMaybe<int>(() => throw new ApplicationException());

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class For_ToResultAsync
        {
            [Fact]
            public async Task When_source_throws_exception_argument_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException>.AsResultAsync(
                    async () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch<InvalidOperationException>.AsResultAsync(async () => { });

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<Task> act = async () => await TryCatch<InvalidOperationException>.AsResultAsync(
                    async () => throw new ApplicationException());

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }

        public class For_ToResultAsync_of_T
        {
            [Fact]
            public async Task When_source_throws_exception_argument_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException>.AsResultAsync<int>(
                    async () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch<InvalidOperationException>.AsResultAsync(
                    async () => 1);

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<Task> act = async () => await TryCatch<InvalidOperationException>.AsResultAsync<int>(
                    async () => throw new ApplicationException());

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }

        public class For_ToMaybeAsync_of_T
        {
            [Fact]
            public async Task When_source_throws_exception_argument_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException>.AsMaybeAsync<int>(
                    async () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch<InvalidOperationException>.AsMaybeAsync(
                    async () => 1);

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public async Task When_source_returns_null_Returns_None_result()
            {
                var actual = await TryCatch<InvalidOperationException>.AsMaybeAsync<string>(
                    async () => null!);

                actual.IsNone.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<Task> act = async () => await TryCatch<InvalidOperationException>.AsMaybeAsync<int>(
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
                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(
                    () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(
                    () => throw new DivideByZeroException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(DivideByZeroException));
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
                Action act = () => TryCatch<InvalidOperationException, DivideByZeroException>.AsResult(
                    () => throw new ApplicationException());

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class For_ToResult_of_T
        {
            [Fact]
            public void When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsResult<int>(
                    () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsResult<int>(
                    () => throw new DivideByZeroException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(DivideByZeroException));
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
                Action act = () => TryCatch<InvalidOperationException, DivideByZeroException>.AsResult<int>(
                    () => throw new ApplicationException());

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class For_ToMaybe_of_T
        {
            [Fact]
            public void When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsMaybe<int>(
                    () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsMaybe<int>(
                    () => throw new DivideByZeroException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(DivideByZeroException));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsMaybe(() => 1);

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void When_source_returns_null_Returns_None_result()
            {
                var actual = TryCatch<InvalidOperationException, DivideByZeroException>.AsMaybe<string>(() => null!);

                actual.IsNone.Should().BeTrue();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Action act = () => TryCatch<InvalidOperationException, DivideByZeroException>.AsMaybe<int>(
                    () => throw new ApplicationException());

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class For_ToResultAsync
        {
            [Fact]
            public async Task When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsResultAsync(
                    async () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public async Task When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsResultAsync(
                    async () => throw new DivideByZeroException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(DivideByZeroException));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsResultAsync(
                    async () => { });

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<Task> act = () => TryCatch<InvalidOperationException, DivideByZeroException>.AsResultAsync(
                    async () => throw new ApplicationException());

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }

        public class For_ToResultAsync_of_T
        {
            [Fact]
            public async Task When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsResultAsync<int>(
                    async () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public async Task When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsResultAsync<int>(
                    async () => throw new DivideByZeroException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(DivideByZeroException));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsResultAsync(
                    async () => 1);

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<Task> act = () => TryCatch<InvalidOperationException, DivideByZeroException>.AsResultAsync<int>(
                    async () => throw new ApplicationException());

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }

        public class For_ToMaybeAsync_of_T
        {
            [Fact]
            public async Task When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsMaybeAsync<int>(
                    async () => throw new InvalidOperationException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public async Task When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsMaybeAsync<int>(
                    async () => throw new DivideByZeroException("a"));

                actual.IsFail.Should().BeTrue();

                var error = actual.Error;

                error.Message.Should().Be(Error._defaultFromExceptionMessage);
                error.Title.Should().Be(nameof(Error));

                error.InnerError.Should().NotBeNull();
                error.InnerError!.Message.Should().Be("a");
                error.InnerError.Title.Should().Be(nameof(DivideByZeroException));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsMaybeAsync(
                    async () => 1);

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_returns_null_Returns_None_result()
            {
                var actual = await TryCatch<InvalidOperationException, DivideByZeroException>.AsMaybeAsync<string>(
                    async () => null!);

                actual.IsNone.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<Task> act = async () => await TryCatch<InvalidOperationException, DivideByZeroException>.AsMaybeAsync<int>(
                    async () => throw new ApplicationException());

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
