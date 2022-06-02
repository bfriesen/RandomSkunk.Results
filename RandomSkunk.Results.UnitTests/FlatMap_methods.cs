namespace RandomSkunk.Results.UnitTests;

public class FlatMap_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void When_IsSuccess_Returns_function_evaluation()
        {
            var source = 1.ToResult();

            var actual = source.FlatMap(value => value.ToString().ToResult());

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public void When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Result<int>.Fail(error);

            var actual = source.FlatMap(value => value.ToString().ToResult());

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_null_flatmap_function_Throws_ArgumentNullException()
        {
            var source = Result<int>.Fail();

            Action act = () => source.FlatMap<string>(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void When_IsSome_Returns_function_evaluation()
        {
            var source = 1.ToMaybe();

            var actual = source.FlatMap(value => value.ToString().ToMaybe());

            actual.IsSome.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public void When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Maybe<int>.Fail(error);

            var actual = source.FlatMap(value => value.ToString().ToMaybe());

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void When_IsNone_Returns_None()
        {
            var source = Maybe<int>.None();

            var actual = source.FlatMap(value => value.ToString().ToMaybe());

            actual.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Given_null_flatmap_function_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Fail();

            Action act = () => source.FlatMap<string>(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
