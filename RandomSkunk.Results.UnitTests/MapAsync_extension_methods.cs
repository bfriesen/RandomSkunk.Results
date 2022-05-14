namespace RandomSkunk.Results.UnitTests;

public class MapAsync_extension_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public async Task Given_cancellation_token_When_IsSuccess_Returns_success_result_from_function_evaluation()
        {
            var source = Result<int>.Create.Success(1);

            var actual = await source.MapAsync((value, cancellationToken) => Task.FromResult(value.ToString()), default);

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public async Task Given_cancellation_token_When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Result<int>.Create.Fail(error);

            var actual = await source.MapAsync((value, cancellationToken) => Task.FromResult(value.ToString()), default);

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_cancellation_token_and_null_map_function_Throws_ArgumentNullException()
        {
            var source = Result<int>.Create.Fail();

            Func<Task> act = () => source.MapAsync<int, string>(null!, default);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_cancellation_token_and_IsSuccess_and_map_function_returning_null_Throws_ArgumentException()
        {
            var source = Result<int>.Create.Success(1);

            Func<Task> act = () => source.MapAsync<int, string>((value, cancellationToken) => Task.FromResult<string>(null!), default);

            await act.Should().ThrowExactlyAsync<ArgumentException>();
        }

        [Fact]
        public async Task Given_no_cancellation_token_When_IsSuccess_Returns_success_result_from_function_evaluation()
        {
            var source = Result<int>.Create.Success(1);

            var actual = await source.MapAsync((value, cancellationToken) => Task.FromResult(value.ToString()), default);

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public async Task Given_no_cancellation_token_When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Result<int>.Create.Fail(error);

            var actual = await source.MapAsync((value, cancellationToken) => Task.FromResult(value.ToString()), default);

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_no_cancellation_token_and_null_map_function_Throws_ArgumentNullException()
        {
            var source = Result<int>.Create.Fail();

            Func<Task> act = () => source.MapAsync<int, string>(null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_no_cancellation_token_and_IsSuccess_and_map_function_returning_null_Throws_ArgumentException()
        {
            var source = Result<int>.Create.Success(1);

            Func<Task> act = () => source.MapAsync(value => Task.FromResult<string>(null!));

            await act.Should().ThrowExactlyAsync<ArgumentException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public async Task Given_cancellation_token_When_IsSome_Returns_some_result_from_function_evaluation()
        {
            var source = Maybe<int>.Create.Some(1);

            var actual = await source.MapAsync(value => Task.FromResult(value.ToString()));

            actual.IsSome.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public async Task Given_cancellation_token_When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Maybe<int>.Create.Fail(error);

            var actual = await source.MapAsync(value => Task.FromResult(value.ToString()));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_cancellation_token_When_IsNone_Returns_None()
        {
            var source = Maybe<int>.Create.None();

            var actual = await source.MapAsync(value => Task.FromResult(value.ToString()));

            actual.IsNone.Should().BeTrue();
        }

        [Fact]
        public async Task Given_cancellation_token_and_null_map_function_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Create.Fail();

            Func<Task> act = () => source.MapAsync<int, string>(null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_cancellation_token_and_IsSome_and_map_function_returning_null_Throws_ArgumentException()
        {
            var source = Maybe<int>.Create.Some(1);

            Func<Task> act = () => source.MapAsync((value, cancellationToken) => Task.FromResult<string>(null!), default);

            await act.Should().ThrowExactlyAsync<ArgumentException>();
        }

        [Fact]
        public async Task Given_no_cancellation_token_When_IsSome_Returns_some_result_from_function_evaluation()
        {
            var source = Maybe<int>.Create.Some(1);

            var actual = await source.MapAsync(value => Task.FromResult(value.ToString()));

            actual.IsSome.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public async Task Given_no_cancellation_token_When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Maybe<int>.Create.Fail(error);

            var actual = await source.MapAsync(value => Task.FromResult(value.ToString()));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_no_cancellation_token_When_IsNone_Returns_None()
        {
            var source = Maybe<int>.Create.None();

            var actual = await source.MapAsync(value => Task.FromResult(value.ToString()));

            actual.IsNone.Should().BeTrue();
        }

        [Fact]
        public async Task Given_no_cancellation_token_and_null_map_function_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Create.Fail();

            Func<Task> act = () => source.MapAsync<int, string>(null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_no_cancellation_token_and_IsSome_and_map_function_returning_null_Throws_ArgumentException()
        {
            var source = Maybe<int>.Create.Some(1);

            Func<Task> act = () => source.MapAsync(value => Task.FromResult<string>(null!));

            await act.Should().ThrowExactlyAsync<ArgumentException>();
        }
    }
}
