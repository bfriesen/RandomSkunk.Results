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
            var source = Result<Result<int>>.Fail(error, false);

            var actual = source.Flatten();

            actual.IsFail.Should().BeTrue();
            actual.Error.Should().BeSameAs(error);
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void When_IsSuccess_Returns_inner_result()
        {
            var innerResult = 1.ToMaybe();
            var source = innerResult.ToMaybe();

            var actual = source.Flatten();

            actual.Should().Be(innerResult);
        }

        [Fact]
        public void When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Maybe<Maybe<int>>.Fail(error, false);

            var actual = source.Flatten();

            actual.IsFail.Should().BeTrue();
            actual.Error.Should().BeSameAs(error);
        }

        [Fact]
        public void When_IsNone_Returns_None()
        {
            var source = Maybe<Maybe<int>>.None;

            var actual = source.Flatten();

            actual.IsNone.Should().BeTrue();
        }
    }
}
