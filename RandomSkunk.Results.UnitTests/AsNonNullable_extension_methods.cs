namespace RandomSkunk.Results.UnitTests;

public class AsNonNullable_extension_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void When_IsSuccess_Returns_Success_result_with_same_value()
        {
            Result<int?> nullableResult = Result<int?>.Success(123);

            Result<int> nonNullableResult = nullableResult.AsNonNullable();

            nonNullableResult.IsSuccess.Should().BeTrue();
            nonNullableResult.Value.Should().Be(123);
        }

        [Fact]
        public void When_IsFail_Returns_Fail_result_with_same_error()
        {
            Error error = new() { ErrorCode = 123 };
            Result<int?> nullableResult = Result<int?>.Fail(error);

            Result<int> nonNullableResult = nullableResult.AsNonNullable();

            nonNullableResult.IsFail.Should().BeTrue();
            nonNullableResult.Error.ErrorCode.Should().Be(123);
        }

        [Fact]
        public async Task Given_task_of_result_When_IsSuccess_Returns_task_of_Success_result_with_same_value()
        {
            Result<int?> nullableResult = Result<int?>.Success(123);
            Task<Result<int?>> nullableResultTask = Task.FromResult(nullableResult);

            Task<Result<int>> nonNullableResultTask = nullableResultTask.AsNonNullable();

            Result<int> nonNullableResult = await nonNullableResultTask;

            nonNullableResult.IsSuccess.Should().BeTrue();
            nonNullableResult.Value.Should().Be(123);
        }

        [Fact]
        public async Task Given_task_of_result_When_IsFail_Returns_task_of_Fail_result_with_same_error()
        {
            Error error = new() { ErrorCode = 123 };
            Result<int?> nullableResult = Result<int?>.Fail(error);
            Task<Result<int?>> nullableResultTask = Task.FromResult(nullableResult);

            Task<Result<int>> nonNullableResultTask = nullableResultTask.AsNonNullable();

            Result<int> nonNullableResult = await nonNullableResultTask;

            nonNullableResult.IsFail.Should().BeTrue();
            nonNullableResult.Error.ErrorCode.Should().Be(123);
        }
    }
}
