namespace RandomSkunk.Results.UnitTests;

public class ToNoneIf_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void GivenSuccessResult_WhenPredicateReturnsTrue_ReturnsFailResult()
        {
            var result = Result<int>.Success(123);

            var actual = result.ToNoneIf(value => true);

            actual.IsNone.Should().BeTrue();
        }

        [Fact]
        public void GivenSuccessResult_WhenPredicateReturnsFalse_ReturnsSameResult()
        {
            var result = Result<int>.Success(123);

            var actual = result.ToNoneIf(value => false);

            actual.Should().Be(result);
        }

        [Fact]
        public void GivenFailResult_WhenPredicateReturnsTrue_ReturnsSameResult()
        {
            var result = Result<int>.Fail();

            var actual = result.ToNoneIf(value => true);

            actual.Should().Be(result);
        }

        [Fact]
        public void GivenNoneResult_WhenPredicateReturnsTrue_ReturnsSameResult()
        {
            var result = Result<int>.None();

            var actual = result.ToNoneIf(value => true);

            actual.Should().Be(result);
        }
    }
}
