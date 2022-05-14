namespace RandomSkunk.Results.UnitTests;

public class Else_extension_methods
{
    public class For_Result
    {
        [Fact]
        public void Given_fallback_result_When_IsSuccess_Returns_source()
        {
            var source = Result.Create.Success();
            var fallbackResult = Result.Create.Fail();

            var actual = source.Else(fallbackResult);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_result_When_IsFail_Returns_fallback_result()
        {
            var source = Result.Create.Fail();
            var fallbackResult = Result.Create.Success();

            var actual = source.Else(fallbackResult);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsSuccess_Returns_source()
        {
            var source = Result.Create.Success();
            var fallbackResult = Result.Create.Fail();
            Result GetFallbackResult() => fallbackResult;

            var actual = source.Else(GetFallbackResult);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsFail_Returns_fallback_result()
        {
            var source = Result.Create.Fail();
            var fallbackResult = Result.Create.Success();
            Result GetFallbackResult() => fallbackResult;

            var actual = source.Else(GetFallbackResult);

            actual.Should().Be(fallbackResult);
        }
    }

    public class For_Result_of_T
    {
        [Fact]
        public void Given_fallback_result_When_IsSuccess_Returns_source()
        {
            var source = Result<int>.Create.Success(1);
            var fallbackResult = Result<int>.Create.Success(2);

            var actual = source.Else(fallbackResult);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_result_When_IsFail_Returns_fallback_result()
        {
            var source = Result<int>.Create.Fail();
            var fallbackResult = Result<int>.Create.Success(1);

            var actual = source.Else(fallbackResult);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsSuccess_Returns_source()
        {
            var source = Result<int>.Create.Success(1);
            var fallbackResult = Result<int>.Create.Success(2);
            Result<int> GetFallbackResult() => fallbackResult;

            var actual = source.Else(GetFallbackResult);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsFail_Returns_function_evaluation()
        {
            var source = Result<int>.Create.Fail();
            var fallbackResult = Result<int>.Create.Success(1);
            Result<int> GetFallbackResult() => fallbackResult;

            var actual = source.Else(GetFallbackResult);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_null_fallback_result_function_Throws_ArgumentNullException()
        {
            var source = Result<int>.Create.Fail();

            Action act = () => source.Else(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void Given_fallback_result_When_IsSome_Returns_source()
        {
            var source = Maybe<int>.Create.Some(1);
            var fallbackResult = Maybe<int>.Create.Some(2);

            var actual = source.Else(fallbackResult);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_result_When_IsFail_Returns_fallback_result()
        {
            var source = Maybe<int>.Create.Fail();
            var fallbackResult = Maybe<int>.Create.Some(1);

            var actual = source.Else(fallbackResult);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_fallback_result_When_IsNone_Returns_fallback_result()
        {
            var source = Maybe<int>.Create.None();
            var fallbackResult = Maybe<int>.Create.Some(1);

            var actual = source.Else(fallbackResult);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsSome_Returns_source()
        {
            var source = Maybe<int>.Create.Some(1);
            var fallbackResult = Maybe<int>.Create.Some(2);
            Maybe<int> GetFallbackMaybe() => fallbackResult;

            var actual = source.Else(GetFallbackMaybe);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsFail_Returns_function_evaluation()
        {
            var source = Maybe<int>.Create.Fail();
            var fallbackResult = Maybe<int>.Create.Some(1);
            Maybe<int> GetFallbackMaybe() => fallbackResult;

            var actual = source.Else(GetFallbackMaybe);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsNone_Returns_function_evaluation()
        {
            var source = Maybe<int>.Create.None();
            var fallbackResult = Maybe<int>.Create.Some(1);
            Maybe<int> GetFallbackMaybe() => fallbackResult;

            var actual = source.Else(GetFallbackMaybe);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_null_fallback_result_function_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Create.Fail();

            Action act = () => source.Else(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
