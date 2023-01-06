namespace RandomSkunk.Results.UnitTests;

public class GetValueOr_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void Given_fallback_result_When_IsSuccess_Returns_value()
        {
            var source = 1.ToResult();

            var actual = source.GetValueOr(2);

            actual.Should().Be(1);
        }

        [Fact]
        public void Given_fallback_result_When_IsFail_Returns_fallback_value()
        {
            var source = Result<int>.Fail();

            var actual = source.GetValueOr(2);

            actual.Should().Be(2);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsSuccess_Returns_value()
        {
            var source = 1.ToResult();

            var actual = source.GetValueOr(() => 2);

            actual.Should().Be(1);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsFail_Returns_function_evaluation()
        {
            var source = Result<int>.Fail();

            var actual = source.GetValueOr(() => 2);

            actual.Should().Be(2);
        }

        [Fact]
        public void Given_null_fallback_result_function_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Action act = () => source.GetValueOr(getFallbackValue: null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void Given_fallback_result_When_IsSuccess_Returns_value()
        {
            var source = 1.ToMaybe();

            var actual = source.GetValueOr(2);

            actual.Should().Be(1);
        }

        [Fact]
        public void Given_fallback_result_When_IsFail_Returns_fallback_value()
        {
            var source = Maybe<int>.Fail();

            var actual = source.GetValueOr(2);

            actual.Should().Be(2);
        }

        [Fact]
        public void Given_fallback_result_When_IsNone_Returns_fallback_value()
        {
            var source = Maybe<int>.None;

            var actual = source.GetValueOr(2);

            actual.Should().Be(2);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsSuccess_Returns_value()
        {
            var source = 1.ToMaybe();

            var actual = source.GetValueOr(() => 2);

            actual.Should().Be(1);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsFail_Returns_function_evaluation()
        {
            var source = Maybe<int>.Fail();

            var actual = source.GetValueOr(() => 2);

            actual.Should().Be(2);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsNone_Returns_function_evaluation()
        {
            var source = Maybe<int>.None;

            var actual = source.GetValueOr(() => 2);

            actual.Should().Be(2);
        }

        [Fact]
        public void Given_null_fallback_result_function_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Fail();

            Action act = () => source.GetValueOr(getFallbackValue: null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
