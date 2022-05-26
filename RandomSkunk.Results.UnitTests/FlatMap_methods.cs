namespace RandomSkunk.Results.UnitTests;

public class FlatMap_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void When_IsSuccess_Returns_function_evaluation()
        {
            var source = Result<int>.Create.Success(1);

            var actual = source.FlatMap(value => Result<string>.Create.Success(value.ToString()));

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public void When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Result<int>.Create.Fail(error);

            var actual = source.FlatMap(value => Result<string>.Create.Success(value.ToString()));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_null_flatmap_function_Throws_ArgumentNullException()
        {
            var source = Result<int>.Create.Fail();

            Action act = () => source.FlatMap<string>(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void When_IsSome_Returns_function_evaluation()
        {
            var source = Maybe<int>.Create.Some(1);

            var actual = source.FlatMap(value => Maybe<string>.Create.Some(value.ToString()));

            actual.IsSome.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public void When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Maybe<int>.Create.Fail(error);

            var actual = source.FlatMap(value => Maybe<string>.Create.Some(value.ToString()));

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void When_IsNone_Returns_None()
        {
            var source = Maybe<int>.Create.None();

            var actual = source.FlatMap(value => Maybe<string>.Create.Some(value.ToString()));

            actual.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Given_null_flatmap_function_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Create.Fail();

            Action act = () => source.FlatMap<string>(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
