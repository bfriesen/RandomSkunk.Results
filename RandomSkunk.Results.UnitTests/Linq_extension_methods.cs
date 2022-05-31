using RandomSkunk.Results.Linq;

namespace RandomSkunk.Results.UnitTests;

public class Linq_extension_methods
{
    public class For_Result_of_T
    {
        [Fact]
        public void Given_all_success_results_Returns_success_result()
        {
            var result = from n in 1.ToResult()
                         let n2 = n * 2
                         from s in $"n2: {n2}".ToResult()
                         select s;

            result.IsSuccess.Should().BeTrue();
            result._value.Should().Be("n2: 2");
        }

        [Fact]
        public void Given_early_fail_result_Returns_fail_result()
        {
            var result = from n in Result<int>.Create.Fail("A")
                         let n2 = n * 2
                         from s in $"n2: {n2}".ToResult()
                         select s;

            result.IsFail.Should().BeTrue();
            result.Error().Message.Should().Be("A");
        }

        [Fact]
        public void Given_late_fail_result_Returns_fail_result()
        {
            var result = from n in 1.ToResult()
                         let n2 = n * 2
                         from s in Result<string>.Create.Fail("B")
                         select s;

            result.IsFail.Should().BeTrue();
            result.Error().Message.Should().Be("B");
        }

        [Fact]
        public void Given_early_and_late_fail_results_Returns_fail_result()
        {
            var result = from n in Result<int>.Create.Fail("A")
                         let n2 = n * 2
                         from s in Result<string>.Create.Fail("B")
                         select s;

            result.IsFail.Should().BeTrue();
            result.Error().Message.Should().Be("A");
        }
    }

    public class For_Maybe_of_T
    {
        [Fact]
        public void Given_all_success_results_and_successful_where_filter_Returns_success_result()
        {
            var result = from n in 1.ToMaybe()
                         let n2 = n * 2
                         where n2 < 3
                         from s in $"n2: {n2}".ToMaybe()
                         select s;

            result.IsSome.Should().BeTrue();
            result._value.Should().Be("n2: 2");
        }

        [Fact]
        public void Given_early_fail_result_Returns_fail_result()
        {
            var result = from n in Maybe<int>.Create.Fail("A")
                         let n2 = n * 2
                         where n2 < 3
                         from s in $"n2: {n2}".ToMaybe()
                         select s;

            result.IsFail.Should().BeTrue();
            result.Error().Message.Should().Be("A");
        }

        [Fact]
        public void Given_early_none_result_Returns_none_result()
        {
            var result = from n in Maybe<int>.Create.None()
                         let n2 = n * 2
                         where n2 < 3
                         from s in $"n2: {n2}".ToMaybe()
                         select s;

            result.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Given_late_fail_result_Returns_fail_result()
        {
            var result = from n in 1.ToMaybe()
                         let n2 = n * 2
                         where n2 < 3
                         from s in Maybe<string>.Create.Fail("B")
                         select s;

            result.IsFail.Should().BeTrue();
            result.Error().Message.Should().Be("B");
        }

        [Fact]
        public void Given_late_none_result_Returns_none_result()
        {
            var result = from n in 1.ToMaybe()
                         let n2 = n * 2
                         where n2 < 3
                         from s in Maybe<string>.Create.None()
                         select s;

            result.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Given_where_filter_returns_false_Returns_none_result()
        {
            var result = from n in 1.ToMaybe()
                         let n2 = n * 2
                         where n2 < 1
                         from s in $"n2: {n2}".ToMaybe()
                         select s;

            result.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Given_early_fail_result_and_where_filter_returns_false_Returns_fail_result()
        {
            var result = from n in Maybe<int>.Create.Fail("A")
                         let n2 = n * 2
                         where n2 < 1
                         from s in $"n2: {n2}".ToMaybe()
                         select s;

            result.IsFail.Should().BeTrue();
            result.Error().Message.Should().Be("A");
        }

        [Fact]
        public void Given_late_fail_result_and_where_filter_returns_false_Returns_none_result()
        {
            var result = from n in 1.ToMaybe()
                         let n2 = n * 2
                         where n2 < 1
                         from s in Maybe<string>.Create.Fail("B")
                         select s;

            result.IsNone.Should().BeTrue();
        }
    }
}
