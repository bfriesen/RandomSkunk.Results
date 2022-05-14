namespace RandomSkunk.Results.UnitTests;

public class Equals_extension_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void Given_equality_comparer_When_IsSuccess_and_equal_Returns_true()
        {
            var source = Result<int>.Create.Success(1);

            var actual = source.Equals<int>(1);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Given_equality_comparer_When_IsSuccess_and_not_equal_Returns_false()
        {
            var source = Result<int>.Create.Success(1);

            var actual = source.Equals<int>(2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_equality_comparer_When_IsFail_Returns_false()
        {
            var source = Result<int>.Create.Fail();

            var actual = source.Equals<int>(1);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_is_value_equal_function_When_IsSuccess_and_function_returns_true_Returns_true()
        {
            var source = Result<int>.Create.Success(1);

            var actual = source.Equals(value => value == 1);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Given_is_value_equal_function_When_IsSuccess_and_false_returned_Returns_false()
        {
            var source = Result<int>.Create.Success(1);

            var actual = source.Equals(value => value == 2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_is_value_equal_function_When_IsFail_Returns_false()
        {
            var source = Result<int>.Create.Fail();

            var actual = source.Equals(value => value == 2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_null_is_value_equal_function_Throws_ArgumentNullException()
        {
            var source = Result<int>.Create.Fail();

            Action act = () => source.Equals<int>(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void Given_equality_comparer_When_IsSome_and_equal_Returns_true()
        {
            var source = Maybe<int>.Create.Some(1);

            var actual = source.Equals<int>(1);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Given_equality_comparer_When_IsSome_and_not_equal_Returns_false()
        {
            var source = Maybe<int>.Create.Some(1);

            var actual = source.Equals<int>(2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_equality_comparer_When_IsFail_Returns_false()
        {
            var source = Maybe<int>.Create.Fail();

            var actual = source.Equals<int>(1);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_equality_comparer_When_IsNone_Returns_false()
        {
            var source = Maybe<int>.Create.None();

            var actual = source.Equals<int>(1);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_is_value_equal_function_When_IsSome_and_function_returns_true_Returns_true()
        {
            var source = Maybe<int>.Create.Some(1);

            var actual = source.Equals(value => value == 1);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Given_is_value_equal_function_When_IsSome_and_false_returned_Returns_false()
        {
            var source = Maybe<int>.Create.Some(1);

            var actual = source.Equals(value => value == 2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_is_value_equal_function_When_IsFail_Returns_false()
        {
            var source = Maybe<int>.Create.Fail();

            var actual = source.Equals(value => value == 2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_is_value_equal_function_When_IsNone_Returns_false()
        {
            var source = Maybe<int>.Create.None();

            var actual = source.Equals(value => value == 2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_null_is_value_equal_function_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Create.Fail();

            Action act = () => source.Equals<int>(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
