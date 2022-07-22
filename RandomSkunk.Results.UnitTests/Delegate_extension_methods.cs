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

                var actual = source.TryInvokeAsResult();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(Exception));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Action source = () => { };

                var actual = source.TryInvokeAsResult();

                actual.IsSuccess.Should().BeTrue();
            }
        }

        public class Given_generic_exception
        {
            [Fact]
            public void When_source_throws_exception_argument_Returns_Fail_result()
            {
                Action source = () => throw new InvalidOperationException("a");

                var actual = source.TryInvokeAsResult<InvalidOperationException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Action source = () => { };

                var actual = source.TryInvokeAsResult<InvalidOperationException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Action source = () => throw new ApplicationException();

                Action act = () => source.TryInvokeAsResult<InvalidOperationException>();

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class Given_two_generic_exceptions
        {
            [Fact]
            public void When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                Action source = () => throw new InvalidOperationException("a");

                var actual = source.TryInvokeAsResult<InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                Action source = () => throw new DivideByZeroException("a");

                var actual = source.TryInvokeAsResult<InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(DivideByZeroException));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Action source = () => { };

                var actual = source.TryInvokeAsResult<InvalidOperationException, DivideByZeroException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Action source = () => throw new ApplicationException();

                Action act = () => source.TryInvokeAsResult<InvalidOperationException, DivideByZeroException>();

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

                var actual = source.TryInvokeAsResult();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(Exception));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Func<int> source = () => 1;

                var actual = source.TryInvokeAsResult();

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public void When_source_returns_null_Throws_ArgumentException()
            {
                Func<string> source = () => null!;

                Action act = () => source.TryInvokeAsResult();

                act.Should().ThrowExactly<ArgumentException>();
            }
        }

        public class Given_generic_exception
        {
            [Fact]
            public void When_source_throws_exception_argument_Returns_Fail_result()
            {
                Func<int> source = () => throw new InvalidOperationException("a");

                var actual = source.TryInvokeAsResult<int, InvalidOperationException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Func<int> source = () => 1;

                var actual = source.TryInvokeAsResult<int, InvalidOperationException>();

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public void When_source_returns_null_Throws_ArgumentException()
            {
                Func<string> source = () => null!;

                Action act = () => source.TryInvokeAsResult<string, InvalidOperationException>();

                act.Should().ThrowExactly<ArgumentException>();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<int> source = () => throw new ApplicationException();

                Action act = () => source.TryInvokeAsResult<int, InvalidOperationException>();

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class Given_two_generic_exceptions
        {
            [Fact]
            public void When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                Func<int> source = () => throw new InvalidOperationException("a");

                var actual = source.TryInvokeAsResult<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                Func<int> source = () => throw new DivideByZeroException("a");

                var actual = source.TryInvokeAsResult<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(DivideByZeroException));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Func<int> source = () => 1;

                var actual = source.TryInvokeAsResult<int, InvalidOperationException, DivideByZeroException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void When_source_returns_null_Throws_ArgumentException()
            {
                Func<string> source = () => null!;

                Action act = () => source.TryInvokeAsResult<string, InvalidOperationException, DivideByZeroException>();

                act.Should().ThrowExactly<ArgumentException>();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<int> source = () => throw new ApplicationException();

                Action act = () => source.TryInvokeAsResult<int, InvalidOperationException, DivideByZeroException>();

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

                var actual = source.TryInvokeAsMaybe();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(Exception));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Func<int> source = () => 1;

                var actual = source.TryInvokeAsMaybe();

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public void When_source_returns_null_Returns_None_result()
            {
                Func<string> source = () => null!;

                var actual = source.TryInvokeAsMaybe();

                actual.IsNone.Should().BeTrue();
            }
        }

        public class Given_generic_exception
        {
            [Fact]
            public void When_source_throws_exception_argument_Returns_Fail_result()
            {
                Func<int> source = () => throw new InvalidOperationException("a");

                var actual = source.TryInvokeAsMaybe<int, InvalidOperationException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Func<int> source = () => 1;

                var actual = source.TryInvokeAsMaybe<int, InvalidOperationException>();

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public void When_source_returns_null_Returns_None_result()
            {
                Func<string> source = () => null!;

                var actual = source.TryInvokeAsMaybe<string, InvalidOperationException>();

                actual.IsNone.Should().BeTrue();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<int> source = () => throw new ApplicationException();

                Action act = () => source.TryInvokeAsMaybe<int, InvalidOperationException>();

                act.Should().ThrowExactly<ApplicationException>();
            }
        }

        public class Given_two_generic_exceptions
        {
            [Fact]
            public void When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                Func<int> source = () => throw new InvalidOperationException("a");

                var actual = source.TryInvokeAsMaybe<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                Func<int> source = () => throw new DivideByZeroException("a");

                var actual = source.TryInvokeAsMaybe<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(DivideByZeroException));
            }

            [Fact]
            public void When_source_does_not_throw_Returns_Success_result()
            {
                Func<int> source = () => 1;

                var actual = source.TryInvokeAsMaybe<int, InvalidOperationException, DivideByZeroException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void When_source_returns_null_Returns_None_result()
            {
                Func<string> source = () => null!;

                var actual = source.TryInvokeAsMaybe<string, InvalidOperationException, DivideByZeroException>();

                actual.IsNone.Should().BeTrue();
            }

            [Fact]
            public void When_source_throws_other_exception_Exception_is_not_caught()
            {
                Func<int> source = () => throw new ApplicationException();

                Action act = () => source.TryInvokeAsMaybe<int, InvalidOperationException, DivideByZeroException>();

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

                var actual = await source.TryInvokeAsResultAsync();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(Exception));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncAction source = () => Task.CompletedTask;

                var actual = await source.TryInvokeAsResultAsync();

                actual.IsSuccess.Should().BeTrue();
            }
        }

        public class Given_generic_exception
        {
            [Fact]
            public async Task When_source_throws_exception_argument_Returns_Fail_result()
            {
                AsyncAction source = () => throw new InvalidOperationException("a");

                var actual = await source.TryInvokeAsResultAsync<InvalidOperationException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncAction source = () => Task.CompletedTask;

                var actual = await source.TryInvokeAsResultAsync<InvalidOperationException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                AsyncAction source = () => throw new ApplicationException();

                Func<Task> act = () => source.TryInvokeAsResultAsync<InvalidOperationException>();

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }

        public class Given_two_generic_exceptions
        {
            [Fact]
            public async Task When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                AsyncAction source = () => throw new InvalidOperationException("a");

                var actual = await source.TryInvokeAsResultAsync<InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public async Task When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                AsyncAction source = () => throw new DivideByZeroException("a");

                var actual = await source.TryInvokeAsResultAsync<InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(DivideByZeroException));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncAction source = () => Task.CompletedTask;

                var actual = await source.TryInvokeAsResultAsync<InvalidOperationException, DivideByZeroException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                AsyncAction source = () => throw new ApplicationException();

                Func<Task> act = () => source.TryInvokeAsResultAsync<InvalidOperationException, DivideByZeroException>();

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

                var actual = await source.TryInvokeAsResultAsync();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(Exception));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncFunc<int> source = () => Task.FromResult(1);

                var actual = await source.TryInvokeAsResultAsync();

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public async Task When_source_returns_null_Throws_ArgumentException()
            {
                AsyncFunc<string> source = () => Task.FromResult<string>(null!);

                Func<Task> act = () => source.TryInvokeAsResultAsync();

                await act.Should().ThrowExactlyAsync<ArgumentException>();
            }
        }

        public class Given_generic_exception
        {
            [Fact]
            public async Task When_source_throws_exception_argument_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new InvalidOperationException("a");

                var actual = await source.TryInvokeAsResultAsync<int, InvalidOperationException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncFunc<int> source = () => Task.FromResult(1);

                var actual = await source.TryInvokeAsResultAsync<int, InvalidOperationException>();

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public async Task When_source_returns_null_Throws_ArgumentException()
            {
                AsyncFunc<string> source = () => Task.FromResult<string>(null!);

                Func<Task> act = () => source.TryInvokeAsResultAsync<string, InvalidOperationException>();

                await act.Should().ThrowExactlyAsync<ArgumentException>();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                AsyncFunc<int> source = () => throw new ApplicationException();

                Func<Task> act = () => source.TryInvokeAsResultAsync<int, InvalidOperationException>();

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }

        public class Given_two_generic_exceptions
        {
            [Fact]
            public async Task When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new InvalidOperationException("a");

                var actual = await source.TryInvokeAsResultAsync<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public async Task When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new DivideByZeroException("a");

                var actual = await source.TryInvokeAsResultAsync<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(DivideByZeroException));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncFunc<int> source = () => Task.FromResult(1);

                var actual = await source.TryInvokeAsResultAsync<int, InvalidOperationException, DivideByZeroException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_returns_null_Throws_ArgumentException()
            {
                AsyncFunc<string> source = () => Task.FromResult<string>(null!);

                Func<Task> act = () => source.TryInvokeAsResultAsync<string, InvalidOperationException, DivideByZeroException>();

                await act.Should().ThrowExactlyAsync<ArgumentException>();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                AsyncFunc<int> source = () => throw new ApplicationException();

                Func<Task> act = () => source.TryInvokeAsResultAsync<int, InvalidOperationException, DivideByZeroException>();

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

                var actual = await source.TryInvokeAsMaybeAsync();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(Exception));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncFunc<int> source = () => Task.FromResult(1);

                var actual = await source.TryInvokeAsMaybeAsync();

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public async Task When_source_returns_null_Returns_None_result()
            {
                AsyncFunc<string> source = () => Task.FromResult<string>(null!);

                var actual = await source.TryInvokeAsMaybeAsync();

                actual.IsNone.Should().BeTrue();
            }
        }

        public class Given_generic_exception
        {
            [Fact]
            public async Task When_source_throws_exception_argument_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new InvalidOperationException("a");

                var actual = await source.TryInvokeAsMaybeAsync<int, InvalidOperationException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncFunc<int> source = () => Task.FromResult(1);

                var actual = await source.TryInvokeAsMaybeAsync<int, InvalidOperationException>();

                actual.IsSuccess.Should().BeTrue();
                actual._value!.Should().Be(1);
            }

            [Fact]
            public async Task When_source_returns_null_Returns_None_result()
            {
                AsyncFunc<string> source = () => Task.FromResult<string>(null!);

                var actual = await source.TryInvokeAsMaybeAsync<string, InvalidOperationException>();

                actual.IsNone.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                AsyncFunc<int> source = () => throw new ApplicationException();

                Func<Task> act = () => source.TryInvokeAsMaybeAsync<int, InvalidOperationException>();

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }

        public class Given_two_generic_exceptions
        {
            [Fact]
            public async Task When_source_throws_exception_argument_1_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new InvalidOperationException("a");

                var actual = await source.TryInvokeAsMaybeAsync<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public async Task When_source_throws_exception_argument_2_Returns_Fail_result()
            {
                AsyncFunc<int> source = () => throw new DivideByZeroException("a");

                var actual = await source.TryInvokeAsMaybeAsync<int, InvalidOperationException, DivideByZeroException>();

                actual.IsFail.Should().BeTrue();
                actual.GetError().Message.Should().Be("a");
                actual.GetError().Title.Should().Be(nameof(DivideByZeroException));
            }

            [Fact]
            public async Task When_source_does_not_throw_Returns_Success_result()
            {
                AsyncFunc<int> source = () => Task.FromResult(1);

                var actual = await source.TryInvokeAsMaybeAsync<int, InvalidOperationException, DivideByZeroException>();

                actual.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_returns_null_Returns_None_result()
            {
                AsyncFunc<string> source = () => Task.FromResult<string>(null!);

                var actual = await source.TryInvokeAsMaybeAsync<string, InvalidOperationException, DivideByZeroException>();

                actual.IsNone.Should().BeTrue();
            }

            [Fact]
            public async Task When_source_throws_other_exception_Exception_is_not_caught()
            {
                AsyncFunc<int> source = () => throw new ApplicationException();

                Func<Task> act = () => source.TryInvokeAsMaybeAsync<int, InvalidOperationException, DivideByZeroException>();

                await act.Should().ThrowExactlyAsync<ApplicationException>();
            }
        }
    }
}
