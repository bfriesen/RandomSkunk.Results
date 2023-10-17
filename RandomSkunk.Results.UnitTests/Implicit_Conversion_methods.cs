namespace RandomSkunk.Results.UnitTests;

public class Implicit_Conversion_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void Given_nonnull_value_Returns_Success_result()
        {
            string? value = "abc";

            Result<string> result = value;

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(value);
        }

        [Fact]
        public void Given_null_value_Returns_Fail_result()
        {
            string? value = null;

            Result<string> result = value;

            result.IsFail.Should().BeTrue();

            var expectedError = Errors.NoValue();

            result.Error.Message.Should().Be(expectedError.Message);
            result.Error.ErrorCode.Should().Be(expectedError.ErrorCode);
            result.Error.Title.Should().Be(expectedError.Title);
        }
    }
}
