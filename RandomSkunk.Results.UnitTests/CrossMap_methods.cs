namespace RandomSkunk.Results.UnitTests;

public class CrossMap_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void Given_target_is_Result_When_source_is_Success_Returns_crossMap_function_evaluation()
        {
            var source = "a".ToResult();

            var actual = source.CrossMap(value => Result.Fail(value));

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Result_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = source.CrossMap(value => Result.Success());

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_target_is_Maybe_of_T_When_source_is_Success_Returns_crossMap_function_evaluation()
        {
            var source = "a".ToResult();

            var actual = source.CrossMap(value => Maybe<int>.Fail(value));

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Maybe_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = source.CrossMap(value => value.ToString().ToMaybe());

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().BeSameAs(error);
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void Given_target_is_Result_When_source_is_Some_Returns_crossMap_function_evaluation()
        {
            var source = "a".ToMaybe();

            var actual = source.CrossMap(value => Result.Fail(value));

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Result_When_source_is_None_Returns_Fail_result()
        {
            var source = Maybe<string>.None();

            var actual = source.CrossMap(value => Result.Success());

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be(ResultExtensions.DefaultOnNoneCallback().Message);
            actual.Error().ErrorCode.Should().Be(ResultExtensions.DefaultOnNoneCallback().ErrorCode);
            actual.Error().Type.Should().Be(ResultExtensions.DefaultOnNoneCallback().Type);
        }

        [Fact]
        public void Given_target_is_Result_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Maybe<string>.Fail(error);

            var actual = source.CrossMap(value => Result.Success());

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_source_is_Some_Returns_crossMap_function_evaluation()
        {
            var source = "a".ToMaybe();

            var actual = source.CrossMap(value => Result<int>.Fail(value));

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_source_is_None_Returns_Fail_result()
        {
            var source = Maybe<int>.None();

            var actual = source.CrossMap(value => value.ToString().ToResult());

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be(ResultExtensions.DefaultOnNoneCallback().Message);
            actual.Error().ErrorCode.Should().Be(ResultExtensions.DefaultOnNoneCallback().ErrorCode);
            actual.Error().Type.Should().Be(ResultExtensions.DefaultOnNoneCallback().Type);
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Maybe<int>.Fail(error);

            var actual = source.CrossMap(value => value.ToString().ToResult());

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().BeSameAs(error);
        }
    }
}
