namespace RandomSkunk.Results.UnitTests;

public class WithError_methods
{
    public class For_Result
    {
        [Fact]
        public void When_IsFail_Returns_fail_result_with_error_from_function_evaluation()
        {
            var error = new Error();
            var source = Result.Fail(error, false);

            var actual = source.WithError(e => new Error { InnerError = e });

            actual.IsFail.Should().BeTrue();
            actual.Error.InnerError.Should().BeSameAs(error);
        }

        [Fact]
        public void When_IsSuccess_Returns_source()
        {
            var source = Result.Success();

            var actual = source.WithError(e => new Error { InnerError = e });

            actual.Should().Be(source);
        }
    }

    public class For_Result_of_T
    {
        [Fact]
        public void When_IsFail_Returns_fail_result_with_error_from_function_evaluation()
        {
            var error = new Error();
            var source = Result<int>.Fail(error, false);

            var actual = source.WithError(e => new Error { InnerError = e });

            actual.IsFail.Should().BeTrue();
            actual.Error.InnerError.Should().BeSameAs(error);
        }

        [Fact]
        public void When_IsSuccess_Returns_source()
        {
            var source = 1.ToResult();

            var actual = source.WithError(e => new Error { InnerError = e });

            actual.Should().Be(source);
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void When_IsFail_Returns_fail_result_with_error_from_function_evaluation()
        {
            var error = new Error();
            var source = Maybe<int>.Fail(error, false);

            var actual = source.WithError(e => new Error { InnerError = e });

            actual.IsFail.Should().BeTrue();
            actual.Error.InnerError.Should().BeSameAs(error);
        }

        [Fact]
        public void When_IsSuccess_Returns_source()
        {
            var source = 1.ToMaybe();

            var actual = source.WithError(e => new Error { InnerError = e });

            actual.Should().Be(source);
        }

        [Fact]
        public void When_IsNone_Returns_source()
        {
            var source = Maybe<int>.None;

            var actual = source.WithError(e => new Error { InnerError = e });

            actual.Should().Be(source);
        }
    }
}
