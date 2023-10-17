namespace RandomSkunk.Results.UnitTests;

public class Unsafe_extensions_methods
{
    public class For_Result
    {
        [Fact]
        public void GetError_When_IsFail_Returns_error()
        {
            var error = new Error();
            var source = Result.Fail(error);

            var actual = source.Error;

            actual.Should().BeSameAs(error);
        }

        [Fact]
        public void GetError_When_IsSuccess_Throws_InvalidStateException()
        {
            var source = Result.Success();

            Action act = () => _ = source.Error;

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessErrorUnlessFailMessage);
        }
    }

    public class For_Result_of_T
    {
        [Fact]
        public void GetError_When_IsFail_Returns_error()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = source.Error;

            actual.Should().BeSameAs(error);
        }

        [Fact]
        public void GetError_When_IsSuccess_Throws_InvalidStateException()
        {
            var source = 1.ToResult();

            Action act = () => _ = source.Error;

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessErrorUnlessFailMessage);
        }

        [Fact]
        public void GetValue_When_IsSuccess_Returns_value()
        {
            var source = 1.ToResult();

            var actual = source.Value;

            actual.Should().Be(1);
        }

        [Fact]
        public void GetValue_When_IsFail_Throws_InvalidStateException()
        {
            var source = Result<int>.Fail();

            Action act = () => _ = source.Value;

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessValueUnlessSuccessMessage);
        }
    }
}
