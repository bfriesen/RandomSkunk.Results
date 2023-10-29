using System.Collections.Generic;
using System.Linq;

namespace RandomSkunk.Results.UnitTests;

public class Collection_extension_methods
{
    public class For_TryForEach
    {
        [Fact]
        public void When_all_elements_produce_Success_results_Returns_Success()
        {
            var collection = Enumerable.Range(1, 10).ToArray();

            var result = collection.TryForEach(value => Result.Success());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void When_an_element_produces_Fail_result_Returns_Fail_and_no_more_elements_are_evaluated()
        {
            var collection = Enumerable.Range(1, 10).ToArray();

            var valuesEvaluated = new List<int>();

            var result = collection.TryForEach(value =>
            {
                valuesEvaluated.Add(value);
                return value < 5 ? Result.Success() : Result.Fail();
            });

            result.IsFail.Should().BeTrue();
            valuesEvaluated.Should().Equal(Enumerable.Range(1, 5));
        }
    }

    public class For_Async_TryForEach
    {
        [Fact]
        public async Task When_all_elements_produce_Success_results_Returns_Success()
        {
            var collection = Enumerable.Range(1, 10).ToArray();

            var result = await collection.TryForEach(value => Task.FromResult(Result.Success()));

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeSameAs(collection);
        }

        [Fact]
        public async Task When_an_element_produces_Fail_result_Returns_Fail_and_no_more_elements_are_evaluated()
        {
            var collection = Enumerable.Range(1, 10).ToArray();

            var valuesEvaluated = new List<int>();

            var result = await collection.TryForEach(value =>
            {
                valuesEvaluated.Add(value);
                return Task.FromResult(value < 5 ? Result.Success() : Result.Fail());
            });

            result.IsFail.Should().BeTrue();
            valuesEvaluated.Should().Equal(Enumerable.Range(1, 5));
        }
    }

    public class For_TryForEach_with_index
    {
        [Fact]
        public void When_all_elements_produce_Success_results_Returns_Success()
        {
            var collection = Enumerable.Range(1, 10).ToArray();

            var indices = new List<int>();

            var result = collection.TryForEach((value, index) =>
            {
                indices.Add(index);
                return Result.Success();
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeSameAs(collection);
            indices.Should().Equal(Enumerable.Range(0, 10));
        }

        [Fact]
        public void When_an_element_produces_Fail_result_Returns_Fail_and_no_more_elements_are_evaluated()
        {
            var collection = Enumerable.Range(1, 10).ToArray();

            var valuesEvaluated = new List<int>();
            var indices = new List<int>();

            var result = collection.TryForEach((value, index) =>
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

    public class For_Async_TryForEach_with_index
    {
        [Fact]
        public async Task When_all_elements_produce_Success_results_Returns_Success()
        {
            var collection = Enumerable.Range(1, 10).ToArray();

            var indices = new List<int>();

            var result = await collection.TryForEach((value, index) =>
            {
                indices.Add(index);
                return Task.FromResult(Result.Success());
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeSameAs(collection);
            indices.Should().Equal(Enumerable.Range(0, 10));
        }

        [Fact]
        public async Task When_an_element_produces_Fail_result_Returns_Fail_and_no_more_elements_are_evaluated()
        {
            var collection = Enumerable.Range(1, 10).ToArray();

            var valuesEvaluated = new List<int>();
            var indices = new List<int>();

            var result = await collection.TryForEach((value, index) =>
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
