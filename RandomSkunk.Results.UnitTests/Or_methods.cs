namespace RandomSkunk.Results.UnitTests;

public class Or_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void Given_fallback_value_When_IsSuccess_Returns_source()
        {
            var source = Result<int>.Create.Success(1);

            var actual = source.Or(2);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_value_When_IsFail_Returns_success_result_from_fallback_value()
        {
            var source = Result<int>.Create.Fail();

            var actual = source.Or(2);

            actual.Should().NotBe(source);
            actual.IsSuccess.Should().BeTrue();
            actual._value.Should().Be(2);
        }

        [Fact]
        public void Given_null_fallback_value_Throws_ArgumentNullException()
        {
            var source = Result<string>.Create.Fail();

            Action act = () => source.Or((string)null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Given_fallback_value_function_When_IsSuccess_Returns_source()
        {
            var source = Result<int>.Create.Success(1);

            var actual = source.Or(() => 2);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_value_function_When_IsFail_Returns_success_result_from_function_evaluation()
        {
            var source = Result<int>.Create.Fail();

            var actual = source.Or(() => 2);

            actual.Should().NotBe(source);
            actual.IsSuccess.Should().BeTrue();
            actual._value.Should().Be(2);
        }

        [Fact]
        public void Given_null_fallback_value_function_Throws_ArgumentNullException()
        {
            var source = Result<string>.Create.Fail();

            Action act = () => source.Or((Func<string>)null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Given_fallback_value_function_returning_null_Throws_ArgumentException()
        {
            var source = Result<string>.Create.Fail();

            Action act = () => source.Or(() => null!);

            act.Should().ThrowExactly<ArgumentException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void Given_fallback_value_When_IsSome_Returns_source()
        {
            var source = Maybe<int>.Create.Some(1);

            var actual = source.Or(2);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_value_When_IsFail_Returns_some_result_from_fallback_value()
        {
            var source = Maybe<int>.Create.Fail();

            var actual = source.Or(2);

            actual.Should().NotBe(source);
            actual.IsSome.Should().BeTrue();
            actual._value.Should().Be(2);
        }

        [Fact]
        public void Given_fallback_value_When_IsNone_Returns_some_result_from_fallback_value()
        {
            var source = Maybe<int>.Create.None();

            var actual = source.Or(2);

            actual.Should().NotBe(source);
            actual.IsSome.Should().BeTrue();
            actual._value.Should().Be(2);
        }

        [Fact]
        public void Given_null_fallback_value_Throws_ArgumentNullException()
        {
            var source = Maybe<string>.Create.Fail();

            Action act = () => source.Or((string)null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Given_fallback_value_function_When_IsSome_Returns_source()
        {
            var source = Maybe<int>.Create.Some(1);

            var actual = source.Or(() => 2);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_value_function_When_IsFail_Returns_some_result_from_function_evaluation()
        {
            var source = Maybe<int>.Create.Fail();

            var actual = source.Or(() => 2);

            actual.Should().NotBe(source);
            actual.IsSome.Should().BeTrue();
            actual._value.Should().Be(2);
        }

        [Fact]
        public void Given_fallback_value_function_When_IsNone_Returns_some_result_from_function_evaluation()
        {
            var source = Maybe<int>.Create.None();

            var actual = source.Or(() => 2);

            actual.Should().NotBe(source);
            actual.IsSome.Should().BeTrue();
            actual._value.Should().Be(2);
        }

        [Fact]
        public void Given_null_fallback_value_function_Throws_ArgumentNullException()
        {
            var source = Maybe<string>.Create.Fail();

            Action act = () => source.Or((Func<string>)null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Given_fallback_value_function_returning_null_Throws_ArgumentException()
        {
            var source = Maybe<string>.Create.Fail();

            Action act = () => source.Or(() => null!);

            act.Should().ThrowExactly<ArgumentException>();
        }
    }
}
