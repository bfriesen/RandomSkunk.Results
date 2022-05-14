namespace RandomSkunk.Results.UnitTests;

public class Flatten_extension_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void When_IsSuccess_Returns_inner_result()
        {
            var innerResult = Result<int>.Create.Success(1);
            var source = Result<Result<int>>.Create.Success(innerResult);

            var actual = source.Flatten();

            actual.Should().Be(innerResult);
        }

        [Fact]
        public void When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Result<Result<int>>.Create.Fail(error);

            var actual = source.Flatten();

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void When_IsSome_Returns_inner_result()
        {
            var innerResult = Maybe<int>.Create.Some(1);
            var source = Maybe<Maybe<int>>.Create.Some(innerResult);

            var actual = source.Flatten();

            actual.Should().Be(innerResult);
        }

        [Fact]
        public void When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Maybe<Maybe<int>>.Create.Fail(error);

            var actual = source.Flatten();

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void When_IsNone_Returns_None()
        {
            var source = Maybe<Maybe<int>>.Create.None();

            var actual = source.Flatten();

            actual.IsNone.Should().BeTrue();
        }
    }
}
