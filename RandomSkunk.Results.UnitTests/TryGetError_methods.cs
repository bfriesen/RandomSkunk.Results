namespace RandomSkunk.Results.UnitTests;

public class TryGetError_methods
{
    public class For_Result
    {
        [Fact]
        public void Given_Fail_result_Returns_true_with_out_parameter_set()
        {
            var expectedError = new Error();
            var result = Result.Fail(expectedError);

            var returnValue = result.TryGetError(out var error);

            returnValue.Should().BeTrue();
            error.Should().BeSameAs(expectedError);
        }

        [Fact]
        public void Given_Success_result_Returns_false_with_out_parameter_null()
        {
            var result = Result.Success();

            var returnValue = result.TryGetError(out var error);

            returnValue.Should().BeFalse();
            error.Should().BeNull();
        }
    }

    public class For_Result_of_T
    {
        [Fact]
        public void Given_Fail_result_Returns_true_with_out_parameter_set()
        {
            var expectedError = new Error();
            var result = Result<int>.Fail(expectedError);

            var returnValue = result.TryGetError(out var error);

            returnValue.Should().BeTrue();
            error.Should().BeSameAs(expectedError);
        }

        [Fact]
        public void Given_Success_result_Returns_false_with_out_parameter_null()
        {
            var result = Result<int>.Success(123);

            var returnValue = result.TryGetError(out var error);

            returnValue.Should().BeFalse();
            error.Should().BeNull();
        }
    }
}
