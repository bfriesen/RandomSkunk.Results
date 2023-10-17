namespace RandomSkunk.Results.UnitTests;

public class Flatten_extension_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void When_IsSuccess_Returns_inner_result()
        {
            var innerResult = 1.ToResult();
            var source = innerResult.ToResult();

            var actual = source.Flatten();

            actual.Should().Be(innerResult);
        }

        [Fact]
        public void When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Result<Result<int>>.Fail(error);

            var actual = source.Flatten();

            actual.IsFail.Should().BeTrue();
            actual.Error.Should().BeSameAs(error);
        }
    }
}
