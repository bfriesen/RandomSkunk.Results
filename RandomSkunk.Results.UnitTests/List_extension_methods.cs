using System.Collections.Generic;
using System.Linq;

namespace RandomSkunk.Results.UnitTests;

public class List_extension_methods
{
    public class For_ForEach
    {
        [Fact]
        public void When_all_elements_produce_Success_results_Returns_Success()
        {
            var list = Enumerable.Range(1, 10).ToArray();

            var result = list.ForEach(value => Result.Success());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Equal(list);
        }

        [Fact]
        public void When_an_element_produces_Fail_result_Returns_Fail_and_no_more_elements_are_evaluated()
        {
            var list = Enumerable.Range(1, 10).ToArray();

            var valuesEvaluated = new List<int>();

            var result = list.ForEach(value =>
            {
                valuesEvaluated.Add(value);
                return value < 5 ? Result.Success() : Result.Fail();
            });

            result.IsFail.Should().BeTrue();
            valuesEvaluated.Should().Equal(Enumerable.Range(1, 5));
        }
    }

    public class For_Async_ForEach
    {
        [Fact]
        public async Task When_all_elements_produce_Success_results_Returns_Success()
        {
            var list = Enumerable.Range(1, 10).ToArray();

            var result = await list.ForEach(value => Task.FromResult(Result.Success()));

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Equal(list);
        }

        [Fact]
        public async Task When_an_element_produces_Fail_result_Returns_Fail_and_no_more_elements_are_evaluated()
        {
            var list = Enumerable.Range(1, 10).ToArray();

            var valuesEvaluated = new List<int>();

            var result = await list.ForEach(value =>
            {
                valuesEvaluated.Add(value);
                return Task.FromResult(value < 5 ? Result.Success() : Result.Fail());
            });

            result.IsFail.Should().BeTrue();
            valuesEvaluated.Should().Equal(Enumerable.Range(1, 5));
        }
    }

    public class For_ForEach_with_index
    {
        [Fact]
        public void When_all_elements_produce_Success_results_Returns_Success()
        {
            var list = Enumerable.Range(1, 10).ToArray();

            var indices = new List<int>();

            var result = list.ForEach((value, index) =>
            {
                indices.Add(index);
                return Result.Success();
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Equal(list);
            indices.Should().Equal(Enumerable.Range(0, 10));
        }

        [Fact]
        public void When_an_element_produces_Fail_result_Returns_Fail_and_no_more_elements_are_evaluated()
        {
            var list = Enumerable.Range(1, 10).ToArray();

            var valuesEvaluated = new List<int>();
            var indices = new List<int>();

            var result = list.ForEach((value, index) =>
            {
                valuesEvaluated.Add(value);
                indices.Add(index);
                return value < 5 ? Result.Success() : Result.Fail();
            });

            result.IsFail.Should().BeTrue();
            valuesEvaluated.Should().Equal(Enumerable.Range(1, 5));
            indices.Should().Equal(Enumerable.Range(0, 5));
        }
    }

    public class For_Async_ForEach_with_index
    {
        [Fact]
        public async Task When_all_elements_produce_Success_results_Returns_Success()
        {
            var list = Enumerable.Range(1, 10).ToArray();

            var indices = new List<int>();

            var result = await list.ForEach((value, index) =>
            {
                indices.Add(index);
                return Task.FromResult(Result.Success());
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Equal(list);
            indices.Should().Equal(Enumerable.Range(0, 10));
        }

        [Fact]
        public async Task When_an_element_produces_Fail_result_Returns_Fail_and_no_more_elements_are_evaluated()
        {
            var list = Enumerable.Range(1, 10).ToArray();

            var valuesEvaluated = new List<int>();
            var indices = new List<int>();

            var result = await list.ForEach((value, index) =>
            {
                valuesEvaluated.Add(value);
                indices.Add(index);
                return Task.FromResult(value < 5 ? Result.Success() : Result.Fail());
            });

            result.IsFail.Should().BeTrue();
            valuesEvaluated.Should().Equal(Enumerable.Range(1, 5));
            indices.Should().Equal(Enumerable.Range(0, 5));
        }
    }
}
