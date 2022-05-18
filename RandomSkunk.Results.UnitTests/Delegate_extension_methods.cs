namespace RandomSkunk.Results.UnitTests;

public class Delegate_extension_methods
{
    public class For_ToResult
    {
        public class Given_no_generic_exception
        {
            [Fact]
            public void When_source_throws_Returns_Fail_result()
            {
                Action source = () => throw new Exception("a");

                var actual = source.ToResult();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("Exception: a");
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Action source = () => { };

                var actual = source.ToResult();

                actual.IsSuccess.Should().BeTrue();
            }
        }

        public class Given_generic_exception
        {
            [Fact]
            public void When_source_throws_exception_argument_Returns_Fail_result()
            {
                Action source = () => throw new InvalidOperationException("a");

                var actual = source.ToResult<InvalidOperationException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("InvalidOperationException: a");
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Action source = () => { };

                var actual = source.ToResult<InvalidOperationException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Action source = () => throw new ApplicationException();

                Action act = () => source.ToResult<InvalidOperationException>();

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class Given_two_generic_exceptions
        {
            [Fact]
            public void When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                Action source = () => throw new InvalidOperationException("a");

                var actual = source.ToResult<InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("InvalidOperationException: a");
            }

            [Fact]
            public void When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                Action source = () => throw new DivideByZeroException("a");

                var actual = source.ToResult<InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("DivideByZeroException: a");
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Action source = () => { };

                var actual = source.ToResult<InvalidOperationException, DivideByZeroException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Action source = () => throw new ApplicationException();

                Action act = () => source.ToResult<InvalidOperationException, DivideByZeroException>();

                act.Should().ThrowExactly<ApplicationException>();
            }
        }
    }

    public class For_ToResult_of_T
    {
        public class Given_no_generic_exception
        {
            [Fact]
            public void When_source_throws_Returns_Fail_result()
            {
                Func<int> source = () => throw new Exception("a");

                var actual = source.ToResult();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("Exception: a");
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Func<int> source = () => 1;

                var actual = source.ToResult();

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public void When_source_returns_null_Throws_ArgumentException()
            {
                Func<string> source = () => null!;

                Action act = () => source.ToResult();

                act.Should().ThrowExactly<ArgumentException>();
            }
        }

        public class Given_generic_exception
        {
            [Fact]
            public void When_source_throws_exception_argument_Returns_Fail_result()
            {
                Func<int> source = () => throw new InvalidOperationException("a");

                var actual = source.ToResult<int, InvalidOperationException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("InvalidOperationException: a");
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Func<int> source = () => 1;

                var actual = source.ToResult<int, InvalidOperationException>();

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public void When_source_returns_null_Throws_ArgumentException()
            {
                Func<string> source = () => null!;

                Action act = () => source.ToResult<string, InvalidOperationException>();

                act.Should().ThrowExactly<ArgumentException>();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<int> source = () => throw new ApplicationException();

                Action act = () => source.ToResult<int, InvalidOperationException>();

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class Given_two_generic_exceptions
        {
            [Fact]
            public void When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                Func<int> source = () => throw new InvalidOperationException("a");

                var actual = source.ToResult<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("InvalidOperationException: a");
            }

            [Fact]
            public void When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                Func<int> source = () => throw new DivideByZeroException("a");

                var actual = source.ToResult<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("DivideByZeroException: a");
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Func<int> source = () => 1;

                var actual = source.ToResult<int, InvalidOperationException, DivideByZeroException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void When_source_returns_null_Throws_ArgumentException()
            {
                Func<string> source = () => null!;

                Action act = () => source.ToResult<string, InvalidOperationException, DivideByZeroException>();

                act.Should().ThrowExactly<ArgumentException>();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<int> source = () => throw new ApplicationException();

                Action act = () => source.ToResult<int, InvalidOperationException, DivideByZeroException>();

                act.Should().ThrowExactly<ApplicationException>();
            }
        }
    }

    public class For_ToMaybe_of_T
    {
        public class Given_no_generic_exception
        {
            [Fact]
            public void When_source_throws_Returns_Fail_result()
            {
                Func<int> source = () => throw new Exception("a");

                var actual = source.ToMaybe();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("Exception: a");
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Some_result()
            {
                Func<int> source = () => 1;

                var actual = source.ToMaybe();

                actual.IsSome.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public void When_source_returns_null_Returns_None_result()
            {
                Func<string> source = () => null!;

                var actual = source.ToMaybe();

                actual.IsNone.Should().BeTrue();
            }
        }

        public class Given_generic_exception
        {
            [Fact]
            public void When_source_throws_exception_argument_Returns_Fail_result()
            {
                Func<int> source = () => throw new InvalidOperationException("a");

                var actual = source.ToMaybe<int, InvalidOperationException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("InvalidOperationException: a");
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Some_result()
            {
                Func<int> source = () => 1;

                var actual = source.ToMaybe<int, InvalidOperationException>();

                actual.IsSome.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public void When_source_returns_null_Returns_None_result()
            {
                Func<string> source = () => null!;

                var actual = source.ToMaybe<string, InvalidOperationException>();

                actual.IsNone.Should().BeTrue();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<int> source = () => throw new ApplicationException();

                Action act = () => source.ToMaybe<int, InvalidOperationException>();

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class Given_two_generic_exceptions
        {
            [Fact]
            public void When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                Func<int> source = () => throw new InvalidOperationException("a");

                var actual = source.ToMaybe<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("InvalidOperationException: a");
            }

            [Fact]
            public void When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                Func<int> source = () => throw new DivideByZeroException("a");

                var actual = source.ToMaybe<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("DivideByZeroException: a");
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Some_result()
            {
                Func<int> source = () => 1;

                var actual = source.ToMaybe<int, InvalidOperationException, DivideByZeroException>();

                actual.IsSome.Should().BeTrue();
            }

            [Fact]
            public void When_source_returns_null_Returns_None_result()
            {
                Func<string> source = () => null!;

                var actual = source.ToMaybe<string, InvalidOperationException, DivideByZeroException>();

                actual.IsNone.Should().BeTrue();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<int> source = () => throw new ApplicationException();

                Action act = () => source.ToMaybe<int, InvalidOperationException, DivideByZeroException>();

                act.Should().ThrowExactly<ApplicationException>();
            }
        }
    }

    public class For_ToResultAsync
    {
        public class Given_no_generic_exception
        {
            [Fact]
            public async Task When_source_throws_Returns_Fail_result()
            {
                AsyncAction source = () => throw new Exception("a");

                var actual = await source.ToResultAsync();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("Exception: a");
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncAction source = () => Task.CompletedTask;

                var actual = await source.ToResultAsync();

                actual.IsSuccess.Should().BeTrue();
            }
        }

        public class Given_generic_exception
        {
            [Fact]
            public async Task When_source_throws_exception_argument_Returns_Fail_result()
            {
                AsyncAction source = () => throw new InvalidOperationException("a");

                var actual = await source.ToResultAsync<InvalidOperationException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("InvalidOperationException: a");
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncAction source = () => Task.CompletedTask;

                var actual = await source.ToResultAsync<InvalidOperationException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                AsyncAction source = () => throw new ApplicationException();

                Func<Task> act = () => source.ToResultAsync<InvalidOperationException>();

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }

        public class Given_two_generic_exceptions
        {
            [Fact]
            public async Task When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                AsyncAction source = () => throw new InvalidOperationException("a");

                var actual = await source.ToResultAsync<InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("InvalidOperationException: a");
            }

            [Fact]
            public async Task When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                AsyncAction source = () => throw new DivideByZeroException("a");

                var actual = await source.ToResultAsync<InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("DivideByZeroException: a");
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncAction source = () => Task.CompletedTask;

                var actual = await source.ToResultAsync<InvalidOperationException, DivideByZeroException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                AsyncAction source = () => throw new ApplicationException();

                Func<Task> act = () => source.ToResultAsync<InvalidOperationException, DivideByZeroException>();

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }
    }

    public class For_ToResultAsync_of_T
    {
        public class Given_no_generic_exception
        {
            [Fact]
            public async Task When_source_throws_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new Exception("a");

                var actual = await source.ToResultAsync();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("Exception: a");
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncFunc<int> source = () => Task.FromResult(1);

                var actual = await source.ToResultAsync();

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public async Task When_source_returns_null_Throws_ArgumentException()
            {
                AsyncFunc<string> source = () => Task.FromResult<string>(null!);

                Func<Task> act = () => source.ToResultAsync();

                await act.Should().ThrowExactlyAsync<ArgumentException>();
            }
        }

        public class Given_generic_exception
        {
            [Fact]
            public async Task When_source_throws_exception_argument_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new InvalidOperationException("a");

                var actual = await source.ToResultAsync<int, InvalidOperationException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("InvalidOperationException: a");
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncFunc<int> source = () => Task.FromResult(1);

                var actual = await source.ToResultAsync<int, InvalidOperationException>();

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public async Task When_source_returns_null_Throws_ArgumentException()
            {
                AsyncFunc<string> source = () => Task.FromResult<string>(null!);

                Func<Task> act = () => source.ToResultAsync<string, InvalidOperationException>();

                await act.Should().ThrowExactlyAsync<ArgumentException>();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                AsyncFunc<int> source = () => throw new ApplicationException();

                Func<Task> act = () => source.ToResultAsync<int, InvalidOperationException>();

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }

        public class Given_two_generic_exceptions
        {
            [Fact]
            public async Task When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new InvalidOperationException("a");

                var actual = await source.ToResultAsync<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("InvalidOperationException: a");
            }

            [Fact]
            public async Task When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new DivideByZeroException("a");

                var actual = await source.ToResultAsync<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("DivideByZeroException: a");
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncFunc<int> source = () => Task.FromResult(1);

                var actual = await source.ToResultAsync<int, InvalidOperationException, DivideByZeroException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_returns_null_Throws_ArgumentException()
            {
                AsyncFunc<string> source = () => Task.FromResult<string>(null!);

                Func<Task> act = () => source.ToResultAsync<string, InvalidOperationException, DivideByZeroException>();

                await act.Should().ThrowExactlyAsync<ArgumentException>();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                AsyncFunc<int> source = () => throw new ApplicationException();

                Func<Task> act = () => source.ToResultAsync<int, InvalidOperationException, DivideByZeroException>();

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }
    }

    public class For_ToMaybeAsync_of_T
    {
        public class Given_no_generic_exception
        {
            [Fact]
            public async Task When_source_throws_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new Exception("a");

                var actual = await source.ToMaybeAsync();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("Exception: a");
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Some_result()
            {
                AsyncFunc<int> source = () => Task.FromResult(1);

                var actual = await source.ToMaybeAsync();

                actual.IsSome.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public async Task When_source_returns_null_Returns_None_result()
            {
                AsyncFunc<string> source = () => Task.FromResult<string>(null!);

                var actual = await source.ToMaybeAsync();

                actual.IsNone.Should().BeTrue();
            }
        }

        public class Given_generic_exception
        {
            [Fact]
            public async Task When_source_throws_exception_argument_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new InvalidOperationException("a");

                var actual = await source.ToMaybeAsync<int, InvalidOperationException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("InvalidOperationException: a");
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Some_result()
            {
                AsyncFunc<int> source = () => Task.FromResult(1);

                var actual = await source.ToMaybeAsync<int, InvalidOperationException>();

                actual.IsSome.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public async Task When_source_returns_null_Returns_None_result()
            {
                AsyncFunc<string> source = () => Task.FromResult<string>(null!);

                var actual = await source.ToMaybeAsync<string, InvalidOperationException>();

                actual.IsNone.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                AsyncFunc<int> source = () => throw new ApplicationException();

                Func<Task> act = () => source.ToMaybeAsync<int, InvalidOperationException>();

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }

        public class Given_two_generic_exceptions
        {
            [Fact]
            public async Task When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new InvalidOperationException("a");

                var actual = await source.ToMaybeAsync<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("InvalidOperationException: a");
            }

            [Fact]
            public async Task When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new DivideByZeroException("a");

                var actual = await source.ToMaybeAsync<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.Error().Message.Should().Be("DivideByZeroException: a");
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Some_result()
            {
                AsyncFunc<int> source = () => Task.FromResult(1);

                var actual = await source.ToMaybeAsync<int, InvalidOperationException, DivideByZeroException>();

                actual.IsSome.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_returns_null_Returns_None_result()
            {
                AsyncFunc<string> source = () => Task.FromResult<string>(null!);

                var actual = await source.ToMaybeAsync<string, InvalidOperationException, DivideByZeroException>();

                actual.IsNone.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                AsyncFunc<int> source = () => throw new ApplicationException();

                Func<Task> act = () => source.ToMaybeAsync<int, InvalidOperationException, DivideByZeroException>();

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }
    }
}
