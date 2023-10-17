namespace RandomSkunk.Results.UnitTests;

public class HasValue_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void Given_equality_comparer_When_IsSuccess_and_equal_Returns_true()
        {
            var source = 1.ToResult();

            var actual = source.HasValue(1);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Given_equality_comparer_When_IsSuccess_and_not_equal_Returns_false()
        {
            var source = 1.ToResult();

            var actual = source.HasValue(2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_equality_comparer_When_IsFail_Returns_false()
        {
            var source = Result<int>.Fail();

            var actual = source.HasValue(1);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_is_value_equal_function_When_IsSuccess_and_function_returns_true_Returns_true()
        {
            var source = 1.ToResult();

            var actual = source.HasValue(value => value == 1);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Given_is_value_equal_function_When_IsSuccess_and_false_returned_Returns_false()
        {
            var source = 1.ToResult();

            var actual = source.HasValue(value => value == 2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_is_value_equal_function_When_IsFail_Returns_false()
        {
            var source = Result<int>.Fail();

            var actual = source.HasValue(value => value == 2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_null_is_value_equal_function_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Action act = () => source.HasValue(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
