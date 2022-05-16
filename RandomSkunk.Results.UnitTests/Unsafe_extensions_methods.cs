namespace RandomSkunk.Results.UnitTests;

public class Unsafe_extensions_methods
{
    public class For_Result
    {
        [Fact]
        public void GetError_When_IsFail_Returns_error()
        {
            var error = new Error();
            var source = Result.Create.Fail(error);

            var actual = source.GetError();

            actual.Should().BeSameAs(error);
        }

        [Fact]
        public void GetError_When_IsSuccess_Throws_InvalidStateException()
        {
            var source = Result.Create.Success();

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
            var source = Result<int>.Create.Fail(error);

            var actual = source.GetError();

            actual.Should().BeSameAs(error);
        }

        [Fact]
        public void GetError_When_IsSuccess_Throws_InvalidStateException()
        {
            var source = Result<int>.Create.Success(1);

            Action act = () => source.GetError();

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessErrorUnlessFailMessage);
        }

        [Fact]
        public void GetValue_When_IsSuccess_Returns_value()
        {
            var source = Result<int>.Create.Success(1);

            var actual = source.GetValue();

            actual.Should().Be(1);
        }

        [Fact]
        public void GetValue_When_IsFail_Throws_InvalidStateException()
        {
            var source = Result<int>.Create.Fail();

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
            var source = Maybe<int>.Create.Fail(error);

            var actual = source.GetError();

            actual.Should().BeSameAs(error);
        }

        [Fact]
        public void GetError_When_IsSome_Throws_InvalidStateException()
        {
            var source = Maybe<int>.Create.Some(1);

            Action act = () => source.GetError();

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessErrorUnlessFailMessage);
        }

        [Fact]
        public void GetError_When_IsNone_Throws_InvalidStateException()
        {
            var source = Maybe<int>.Create.None();

            Action act = () => source.GetError();

            act.Should().ThrowExactly<InvalidStateException>();
        }

        [Fact]
        public void GetValue_When_IsSome_Returns_value()
        {
            var source = Maybe<int>.Create.Some(1);

            var actual = source.GetValue();

            actual.Should().Be(1);
        }

        [Fact]
        public void GetValue_When_IsFail_Throws_InvalidStateException()
        {
            var source = Maybe<int>.Create.Fail();

            Action act = () => source.GetValue();

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessValueUnlessSomeMessage);
        }

        [Fact]
        public void GetValue_When_IsNone_Throws_InvalidStateException()
        {
            var source = Maybe<int>.Create.None();

            Action act = () => source.GetValue();

            act.Should().ThrowExactly<InvalidStateException>()
                .WithMessage(CannotAccessValueUnlessSomeMessage);
        }
    }
}
