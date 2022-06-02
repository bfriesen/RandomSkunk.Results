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

            var actual = source.GetError();

            actual.Should().BeSameAs(error);
        }

        [Fact]
        public void GetError_When_IsSuccess_Throws_InvalidStateException()
        {
            var source = Result.Success();

            Action act = () => source.GetError();

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

            var actual = source.GetError();

            actual.Should().BeSameAs(error);
        }

        [Fact]
        public void GetError_When_IsSuccess_Throws_InvalidStateException()
        {
            var source = 1.ToResult();

            Action act = () => source.GetError();

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessErrorUnlessFailMessage);
        }

        [Fact]
        public void GetValue_When_IsSuccess_Returns_value()
        {
            var source = 1.ToResult();

            var actual = source.GetValue();

            actual.Should().Be(1);
        }

        [Fact]
        public void GetValue_When_IsFail_Throws_InvalidStateException()
        {
            var source = Result<int>.Fail();

            Action act = () => source.GetValue();

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessValueUnlessSuccessMessage);
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void GetError_When_IsFail_Returns_error()
        {
            var error = new Error();
            var source = Maybe<int>.Fail(error);

            var actual = source.GetError();

            actual.Should().BeSameAs(error);
        }

        [Fact]
        public void GetError_When_IsSome_Throws_InvalidStateException()
        {
            var source = 1.ToMaybe();

            Action act = () => source.GetError();

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessErrorUnlessFailMessage);
        }

        [Fact]
        public void GetError_When_IsNone_Throws_InvalidStateException()
        {
            var source = Maybe<int>.None();

            Action act = () => source.GetError();

            act.Should().ThrowExactly<InvalidStateException>();
        }

        [Fact]
        public void GetValue_When_IsSome_Returns_value()
        {
            var source = 1.ToMaybe();

            var actual = source.GetValue();

            actual.Should().Be(1);
        }

        [Fact]
        public void GetValue_When_IsFail_Throws_InvalidStateException()
        {
            var source = Maybe<int>.Fail();

            Action act = () => source.GetValue();

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessValueUnlessSomeMessage);
        }

        [Fact]
        public void GetValue_When_IsNone_Throws_InvalidStateException()
        {
            var source = Maybe<int>.None();

            Action act = () => source.GetValue();

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessValueUnlessSomeMessage);
        }
    }
}
