namespace RandomSkunk.Results.UnitTests;

public class ToFailIf_methods
{
    public class For_Result
    {
        [Fact]
        public void GivenSuccessResult_WhenPredicateReturnsTrue_ReturnsFailResult()
        {
            var result = Result.Success();
            var error = new Error();

            var actual = result.ToFailIf(() => true, () => error);

            actual.IsFail.Should().BeTrue();
            actual.Error.Message.Should().BeSameAs(error.Message);
            actual.Error.Title.Should().BeSameAs(error.Title);
        }

        [Fact]
        public void GivenSuccessResult_WhenPredicateReturnsFalse_ReturnsSameResult()
        {
            var result = Result.Success();
            var error = new Error();

            var actual = result.ToFailIf(() => false, () => error);

            actual.Should().Be(result);
        }

        [Fact]
        public void GivenFailResult_WhenPredicateReturnsTrue_ReturnsSameResult()
        {
            var result = Result.Fail();
            var error = new Error();

            var actual = result.ToFailIf(() => true, () => error);

            actual.Should().Be(result);
        }
    }

    public class For_Result_of_T
    {
        [Fact]
        public void GivenSuccessResult_WhenPredicateReturnsTrue_ReturnsFailResult()
        {
            var result = Result<int>.Success(123);
            var error = new Error();

            var actual = result.ToFailIf(value => true, value => error);

            actual.IsFail.Should().BeTrue();
            actual.Error.Message.Should().BeSameAs(error.Message);
            actual.Error.Title.Should().BeSameAs(error.Title);
        }

        [Fact]
        public void GivenSuccessResult_WhenPredicateReturnsFalse_ReturnsSameResult()
        {
            var result = Result<int>.Success(123);
            var error = new Error();

            var actual = result.ToFailIf(value => false, value => error);

            actual.Should().Be(result);
        }

        [Fact]
        public void GivenFailResult_WhenPredicateReturnsTrue_ReturnsSameResult()
        {
            var result = Result<int>.Fail();
            var error = new Error();

            var actual = result.ToFailIf(value => true, value => error);

            actual.Should().Be(result);
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void GivenSuccessResult_WhenPredicateReturnsTrue_ReturnsFailResult()
        {
            var result = Maybe<int>.Success(123);
            var error = new Error();

            var actual = result.ToFailIf(value => true, value => error);

            actual.IsFail.Should().BeTrue();
            actual.Error.Message.Should().BeSameAs(error.Message);
            actual.Error.Title.Should().BeSameAs(error.Title);
        }

        [Fact]
        public void GivenSuccessResult_WhenPredicateReturnsFalse_ReturnsSameResult()
        {
            var result = Maybe<int>.Success(123);
            var error = new Error();

            var actual = result.ToFailIf(value => false, value => error);

            actual.Should().Be(result);
        }

        [Fact]
        public void GivenFailResult_WhenPredicateReturnsTrue_ReturnsSameResult()
        {
            var result = Maybe<int>.Fail();
            var error = new Error();

            var actual = result.ToFailIf(value => true, value => error);

            actual.Should().Be(result);
        }

        [Fact]
        public void GivenNoneResult_WhenPredicateReturnsTrue_ReturnsSameResult()
        {
            var result = Maybe<int>.None();
            var error = new Error();

            var actual = result.ToFailIf(value => true, value => error);

            actual.Should().Be(result);
        }
    }
}
