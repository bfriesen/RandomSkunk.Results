namespace RandomSkunk.Results.UnitTests;

public class Map_extension_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void When_IsSuccess_Returns_success_result_from_function_evaluation()
        {
            var source = Result<int>.Create.Success(1);

            var actual = source.Map(value => value.ToString());

            actual.IsSuccess.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public void When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Result<int>.Create.Fail(error);

            var actual = source.Map(value => value.ToString());

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void Given_null_map_function_Throws_ArgumentNullException()
        {
            var source = Result<int>.Create.Fail();

            Action act = () => source.Map<int, string>(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Given_IsSuccess_and_map_function_returning_null_Throws_ArgumentException()
        {
            var source = Result<int>.Create.Success(1);

            Action act = () => source.Map<int, string>(value => null!);

            act.Should().ThrowExactly<ArgumentException>();
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void When_IsSome_Returns_some_result_from_function_evaluation()
        {
            var source = Maybe<int>.Create.Some(1);

            var actual = source.Map(value => value.ToString());

            actual.IsSome.Should().BeTrue();
            actual.GetValue().Should().Be("1");
        }

        [Fact]
        public void When_IsFail_Returns_Fail()
        {
            var error = new Error();
            var source = Maybe<int>.Create.Fail(error);

            var actual = source.Map(value => value.ToString());

            actual.IsFail.Should().BeTrue();
            actual.GetError().Should().BeSameAs(error);
        }

        [Fact]
        public void When_IsNone_Returns_None()
        {
            var source = Maybe<int>.Create.None();

            var actual = source.Map(value => value.ToString());

            actual.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Given_null_map_function_Throws_ArgumentNullException()
        {
            var source = Maybe<int>.Create.Fail();

            Action act = () => source.Map<int, string>(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Given_IsSome_and_map_function_returning_null_Throws_ArgumentException()
        {
            var source = Maybe<int>.Create.Some(1);

            Action act = () => source.Map<int, string>(value => null!);

            act.Should().ThrowExactly<ArgumentException>();
        }
    }
}
