namespace RandomSkunk.Results.UnitTests;

public class Else_methods
{
    public class For_Result
    {
        [Fact]
        public void Given_fallback_result_When_IsSuccess_Returns_source()
        {
            var source = Result.Success();
            var fallbackResult = Result.Fail();

            var actual = source.Else(fallbackResult);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_result_When_IsFail_Returns_fallback_result()
        {
            var source = Result.Fail();
            var fallbackResult = Result.Success();

            var actual = source.Else(fallbackResult);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsSuccess_Returns_source()
        {
            var source = Result.Success();
            var fallbackResult = Result.Fail();
            Result GetFallbackResult() => fallbackResult;

            var actual = source.Else(GetFallbackResult);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsFail_Returns_fallback_result()
        {
            var source = Result.Fail();
            var fallbackResult = Result.Success();
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
            var source = 1.ToResult();
            var fallbackResult = 2.ToResult();

            var actual = source.Else(fallbackResult);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_result_When_IsFail_Returns_fallback_result()
        {
            var source = Result<int>.Fail();
            var fallbackResult = 1.ToResult();

            var actual = source.Else(fallbackResult);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsSuccess_Returns_source()
        {
            var source = 1.ToResult();
            var fallbackResult = 2.ToResult();
            Result<int> GetFallbackResult() => fallbackResult;

            var actual = source.Else(GetFallbackResult);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsFail_Returns_function_evaluation()
        {
            var source = Result<int>.Fail();
            var fallbackResult = 1.ToResult();
            Result<int> GetFallbackResult() => fallbackResult;

            var actual = source.Else(GetFallbackResult);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_null_fallback_result_function_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Action act = () => source.Else(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void Given_fallback_result_When_IsSuccess_Returns_source()
        {
            var source = 1.ToMaybe();
            var fallbackResult = 2.ToMaybe();

            var actual = source.Else(fallbackResult);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_result_When_IsFail_Returns_fallback_result()
        {
            var source = Maybe<int>.Fail();
            var fallbackResult = 1.ToMaybe();

            var actual = source.Else(fallbackResult);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_fallback_result_When_IsNone_Returns_fallback_result()
        {
            var source = Maybe<int>.None;
            var fallbackResult = 1.ToMaybe();

            var actual = source.Else(fallbackResult);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsSuccess_Returns_source()
        {
            var source = 1.ToMaybe();
            var fallbackResult = 2.ToMaybe();
            Maybe<int> GetFallbackMaybe() => fallbackResult;

            var actual = source.Else(GetFallbackMaybe);

            actual.Should().Be(source);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsFail_Returns_function_evaluation()
        {
            var source = Maybe<int>.Fail();
            var fallbackResult = 1.ToMaybe();
            Maybe<int> GetFallbackMaybe() => fallbackResult;

            var actual = source.Else(GetFallbackMaybe);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_fallback_result_function_When_IsNone_Returns_function_evaluation()
        {
            var source = Maybe<int>.None;
            var fallbackResult = 1.ToMaybe();
            Maybe<int> GetFallbackMaybe() => fallbackResult;

            var actual = source.Else(GetFallbackMaybe);

            actual.Should().Be(fallbackResult);
        }

        [Fact]
        public void Given_null_fallback_result_function_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Fail();

            Action act = () => source.Else(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
