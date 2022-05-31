namespace RandomSkunk.Results.UnitTests;

public class CrossMapAsync_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public async Task Given_target_is_Result_When_source_is_Success_Returns_crossMap_function_evaluation()
        {
            var source = "a".ToResult();

            var actual = await source.CrossMapAsync(value => Task.FromResult(Result.Create.Fail(value)));

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Result<int>.Create.Fail(error);

            var actual = await source.CrossMapAsync(value => Task.FromResult(Result.Create.Success()));

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_target_is_Maybe_of_T_When_source_is_Success_Returns_crossMap_function_evaluation()
        {
            var source = "a".ToResult();

            var actual = await source.CrossMapAsync(value => Task.FromResult(Maybe<int>.Create.Fail(value)));

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Maybe_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Result<int>.Create.Fail(error);

            var actual = await source.CrossMapAsync(value => Task.FromResult(value.ToString().ToMaybe()));

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().BeSameAs(error);
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public async Task Given_target_is_Result_When_source_is_Some_Returns_crossMap_function_evaluation()
        {
            var source = "a".ToMaybe();

            var actual = await source.CrossMapAsync(value => Task.FromResult(Result.Create.Fail(value)));

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_When_source_is_None_Returns_Fail_result()
        {
            var source = Maybe<string>.Create.None();

            var actual = await source.CrossMapAsync(value => Task.FromResult(Result.Create.Success()));

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().Be(ResultExtensions.DefaultGetNoneError());
        }

        [Fact]
        public async Task Given_target_is_Result_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Maybe<string>.Create.Fail(error);

            var actual = await source.CrossMapAsync(value => Task.FromResult(Result.Create.Success()));

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_Some_Returns_crossMap_function_evaluation()
        {
            var source = "a".ToMaybe();

            var actual = await source.CrossMapAsync(value => Task.FromResult(Result<int>.Create.Fail(value)));

            actual.IsFail.Should().BeTrue();
            actual.Error().Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_None_Returns_Fail_result()
        {
            var source = Maybe<int>.Create.None();

            var actual = await source.CrossMapAsync(value => Task.FromResult(value.ToString().ToResult()));

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().Be(ResultExtensions.DefaultGetNoneError());
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Maybe<int>.Create.Fail(error);

            var actual = await source.CrossMapAsync(value => Task.FromResult(value.ToString().ToResult()));

            actual.IsFail.Should().BeTrue();
            actual.Error().Should().BeSameAs(error);
        }
    }
}
