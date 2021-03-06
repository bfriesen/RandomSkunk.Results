namespace RandomSkunk.Results.UnitTests;

public class Map_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void When_IsSuccess_Returns_Success_result_from_function_evaluation()
        {
            var source = 1.ToResult();

            var actual = source.Map(value => value.ToString());

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public void When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = source.Map(value => value.ToString());

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_null_map_function_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Action act = () => source.Map<string>(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void When_IsSuccess_Returns_Success_result_from_function_evaluation()
        {
            var source = 1.ToMaybe();

            var actual = source.Map(value => value.ToString());

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public void When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Maybe<int>.Fail(error);

            var actual = source.Map(value => value.ToString());

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void When_IsNone_Returns_None()
        {
            var source = Maybe<int>.None();

            var actual = source.Map(value => value.ToString());

            actual.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Given_null_map_function_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Fail();

            Action act = () => source.Map<string>(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
