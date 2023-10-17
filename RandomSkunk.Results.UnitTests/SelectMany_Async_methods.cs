namespace RandomSkunk.Results.UnitTests;

public class SelectMany_Async_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public async Task Given_target_is_Result_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = "a".ToResult();

            var actual = await source.SelectMany(value => Task.FromResult(Result.Fail(value)));

            actual.IsFail.Should().BeTrue();
            actual.Error.Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_When_source_is_None_Returns_Fail_result()
        {
            var source = Result<string>.None();

            var actual = await source.SelectMany(value => Task.FromResult(Result.Success()));

            actual.IsFail.Should().BeTrue();

            var expectedError = Errors.NoValue();

            actual.Error.Message.Should().Be(expectedError.Message);
            actual.Error.ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.Error.Title.Should().Be(expectedError.Title);
        }

        [Fact]
        public async Task Given_target_is_Result_and_onNoneSelector_is_provided_When_source_is_None_Returns_result_from_onNoneSelector()
        {
            var source = Result<string>.None();

            var actual = await source.SelectMany(value => Task.FromResult(Result.Success()), () => Task.FromResult(Result.Fail("error from onNoneSelector")));

            actual.IsFail.Should().BeTrue();
            actual.Error.Message.Should().Be("error from onNoneSelector");
        }

        [Fact]
        public async Task Given_target_is_Result_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Result<string>.Fail(error);

            var actual = await source.SelectMany(value => Task.FromResult(Result.Success()));

            actual.IsFail.Should().BeTrue();
            actual.Error.Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_target_is_Result_and_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Func<Task> act = () => source.SelectMany((Func<int, Task<Result>>)null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = "a".ToResult();

            var actual = await source.SelectMany(value => Task.FromResult(Result<int>.Fail(value)));

            actual.IsFail.Should().BeTrue();
            actual.Error.Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_None_Returns_Fail_result()
        {
            var source = Result<int>.None();

            var actual = await source.SelectMany(value => Task.FromResult(value.ToString().ToResult()));

            actual.IsFail.Should().BeTrue();

            var expectedError = Errors.NoValue();

            actual.Error.Message.Should().Be(expectedError.Message);
            actual.Error.ErrorCode.Should().Be(expectedError.ErrorCode);
            actual.Error.Title.Should().Be(expectedError.Title);
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_and_onNoneSelector_is_provided_When_source_is_None_Returns_result_from_onNoneSelector()
        {
            var source = Result<int>.None();

            var actual = await source.SelectMany(value => Task.FromResult(value.ToString().ToResult()), () => Task.FromResult(Result<string>.Fail("error from onNoneSelector")));

            actual.IsFail.Should().BeTrue();
            actual.Error.Message.Should().Be("error from onNoneSelector");
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = await source.SelectMany(value => Task.FromResult(value.ToString().ToResult()));

            actual.IsFail.Should().BeTrue();
            actual.Error.Should().BeSameAs(error);
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_and_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Func<Task> act = () => source.SelectMany((Func<int, Task<Result<string>>>)null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }

    public class For_Result
    {
        [Fact]
        public async Task Given_target_is_Result_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = Result.Success();

            var actual = await source.SelectMany(() => Task.FromResult(Result.Fail("a")));

            actual.IsFail.Should().BeTrue();
            actual.Error.Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_When_source_is_Fail_Returns_Fail()
        {
            var source = Result.Fail("a");

            var actual = await source.SelectMany(() => Task.FromResult(Result.Success()));

            actual.IsFail.Should().BeTrue();
            actual.Error.Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_When_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result.Fail();

            Func<Task> act = () => source.SelectMany((Func<Task<Result>>)null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_Success_Returns_onSuccess_function_evaluation()
        {
            var source = Result.Success();

            var actual = await source.SelectMany(() => Task.FromResult(Result<int>.Success(1)));

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().Be(1);
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_source_is_Fail_Returns_Fail_result()
        {
            var source = Result.Fail("a");

            var actual = await source.SelectMany(() => Task.FromResult(Result<int>.Success(1)));

            actual.IsFail.Should().BeTrue();
            actual.Error.Message.Should().Be("a");
        }

        [Fact]
        public async Task Given_target_is_Result_of_T_When_onSuccess_function_is_null_Throws_ArgumentNullException()
        {
            var source = Result.Fail();

            Func<Task> act = () => source.SelectMany((Func<Task<Result<string>>>)null!);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }
}
