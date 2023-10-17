namespace RandomSkunk.Results.UnitTests;

public class Truncate_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void When_IsSuccess_Returns_Success_result()
        {
            var source = Result<int>.Success(1);

            var truncated = source.Truncate();

            truncated.IsSuccess.Should().BeTrue();
            truncated.IsFail.Should().BeFalse();
        }

        [Fact]
        public void When_IsFail_Returns_Fail_result_with_same_error()
        {
            var source = Result<int>.Fail();

            var truncated = source.Truncate();

            truncated.IsSuccess.Should().BeFalse();
            truncated.IsFail.Should().BeTrue();
            truncated.Error.Should().BeSameAs(source.Error);
        }
    }
}
