namespace RandomSkunk.Results.UnitTests;

public class CrossMap_extension_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void Given_target_is_Result_When_source_is_Success_Returns_crossMap_function_evaluation()
        {
            var source = Result<string>.Create.Success("a");

            var actual = source.CrossMap(value => Result.Create.Fail(value));

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Result_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Result<int>.Create.Fail(error);

            var actual = source.CrossMap(value => Result.Create.Success());

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_target_is_Maybe_of_T_When_source_is_Success_Returns_crossMap_function_evaluation()
        {
            var source = Result<string>.Create.Success("a");

            var actual = source.CrossMap(value => Maybe<int>.Create.Fail(value));

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Maybe_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Result<int>.Create.Fail(error);

            var actual = source.CrossMap(value => Maybe<string>.Create.Some(value.ToString()));

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().BeSameAs(error);
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void Given_target_is_Result_When_source_is_Some_Returns_crossMap_function_evaluation()
        {
            var source = Maybe<string>.Create.Some("a");

            var actual = source.CrossMap(value => Result.Create.Fail(value));

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Result_When_source_is_None_Returns_Fail_result()
        {
            var source = Maybe<string>.Create.None();

            var actual = source.CrossMap(value => Result.Create.Success());

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().Be(ResultExtensions.DefaultGetNoneError());
        }

        [Fact]
        public void Given_target_is_Result_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Maybe<string>.Create.Fail(error);

            var actual = source.CrossMap(value => Result.Create.Success());

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_source_is_Some_Returns_crossMap_function_evaluation()
        {
            var source = Maybe<string>.Create.Some("a");

            var actual = source.CrossMap(value => Result<int>.Create.Fail(value));

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be("a");
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_source_is_None_Returns_Fail_result()
        {
            var source = Maybe<int>.Create.None();

            var actual = source.CrossMap(value => Result<string>.Create.Success(value.ToString()));

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().Be(ResultExtensions.DefaultGetNoneError());
        }

        [Fact]
        public void Given_target_is_Result_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Maybe<int>.Create.Fail(error);

            var actual = source.CrossMap(value => Result<string>.Create.Success(value.ToString()));

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().BeSameAs(error);
        }
    }
}
