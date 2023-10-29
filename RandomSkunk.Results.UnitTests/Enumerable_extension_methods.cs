using System.Linq;

namespace RandomSkunk.Results.UnitTests;

public class Enumerable_extension_methods
{
    public class For_FirstOrNone
    {
        [Fact]
        public void When_not_empty_and_first_is_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.FirstOrNone();
            var listResult = list.FirstOrNone();

            seqResult.IsSuccess.Should().BeTrue();
            seqResult.Value.Should().Be(1);

            listResult.IsSuccess.Should().BeTrue();
            listResult.Value.Should().Be(1);
        }

        [Fact]
        public void When_empty_Returns_None()
        {
            var seq = Enumerable.Empty<int>();
            var list = seq.ToList();

            var seqResult = seq.FirstOrNone();
            var listResult = list.FirstOrNone();

            seqResult.IsNone.Should().BeTrue();
            listResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void When_not_empty_and_first_is_null_Returns_Fail_n1404()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);
            var list = seq.ToList();

            var seqResult = seq.FirstOrNone();
            var listResult = list.FirstOrNone();

            seqResult.IsFail.Should().BeTrue();
            seqResult.Error.ErrorCode.Should().Be(ErrorCodes.UnexpectedNullValue);

            listResult.IsFail.Should().BeTrue();
            listResult.Error.ErrorCode.Should().Be(ErrorCodes.UnexpectedNullValue);
        }

        [Fact]
        public void Given_predicate_When_first_matched_and_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);

            var seqResult = seq.FirstOrNone(x => x > 4);

            seqResult.IsSuccess.Should().BeTrue();
            seqResult.Value.Should().Be(5);
        }

        [Fact]
        public void Given_predicate_When_not_matched_Returns_None()
        {
            var seq = Enumerable.Empty<int>();

            var seqResult = seq.FirstOrNone(x => x == 20);

            seqResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Given_predicate_When_first_matched_and_null_Returns_Fail_n1404()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);

            var seqResult = seq.FirstOrNone(x => x is null);

            seqResult.IsFail.Should().BeTrue();
            seqResult.Error.ErrorCode.Should().Be(ErrorCodes.UnexpectedNullValue);
        }
    }

    public class For_LastOrNone
    {
        [Fact]
        public void When_not_empty_and_last_is_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.LastOrNone();
            var listResult = list.LastOrNone();

            seqResult.IsSuccess.Should().BeTrue();
            seqResult.Value.Should().Be(10);

            listResult.IsSuccess.Should().BeTrue();
            listResult.Value.Should().Be(10);
        }

        [Fact]
        public void When_empty_Returns_None()
        {
            var seq = Enumerable.Empty<int>();
            var list = seq.ToList();

            var seqResult = seq.LastOrNone();
            var listResult = list.LastOrNone();

            seqResult.IsNone.Should().BeTrue();
            listResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void When_not_empty_and_last_is_null_Returns_Fail_n1404()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);
            var list = seq.ToList();

            var seqResult = seq.LastOrNone();
            var listResult = list.LastOrNone();

            seqResult.IsFail.Should().BeTrue();
            seqResult.Error.ErrorCode.Should().Be(ErrorCodes.UnexpectedNullValue);

            listResult.IsFail.Should().BeTrue();
            listResult.Error.ErrorCode.Should().Be(ErrorCodes.UnexpectedNullValue);
        }

        [Fact]
        public void Given_predicate_When_last_matched_and_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);

            var seqResult = seq.LastOrNone(x => x < 6);

            seqResult.IsSuccess.Should().BeTrue();
            seqResult.Value.Should().Be(5);
        }

        [Fact]
        public void Given_predicate_When_not_matched_Returns_None()
        {
            var seq = Enumerable.Empty<int>();

            var seqResult = seq.LastOrNone(x => x == 20);

            seqResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Given_predicate_When_last_matched_and_null_Returns_Fail_n1404()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);

            var seqResult = seq.LastOrNone(x => x is null);

            seqResult.IsFail.Should().BeTrue();
            seqResult.Error.ErrorCode.Should().Be(ErrorCodes.UnexpectedNullValue);
        }
    }

    public class For_SingleOrNone
    {
        [Fact]
        public void When_not_empty_and_single_is_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 1);
            var list = seq.ToList();

            var seqResult = seq.SingleOrNone();
            var listResult = list.SingleOrNone();

            seqResult.IsSuccess.Should().BeTrue();
            seqResult.Value.Should().Be(1);

            listResult.IsSuccess.Should().BeTrue();
            listResult.Value.Should().Be(1);
        }

        [Fact]
        public void When_empty_Returns_None()
        {
            var seq = Enumerable.Empty<int>();
            var list = seq.ToList();

            var seqResult = seq.SingleOrNone();
            var listResult = list.SingleOrNone();

            seqResult.IsNone.Should().BeTrue();
            listResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void When_more_than_one_element_Returns_Fail_400()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.SingleOrNone();
            var listResult = list.SingleOrNone();

            seqResult.IsFail.Should().BeTrue();
            seqResult.Error.ErrorCode.Should().Be(ErrorCodes.BadRequest);

            listResult.IsFail.Should().BeTrue();
            listResult.Error.ErrorCode.Should().Be(ErrorCodes.BadRequest);
        }

        [Fact]
        public void When_not_empty_and_single_is_null_Returns_Fail_n1404()
        {
            var seq = Enumerable.Repeat<int?>(null, 1);
            var list = seq.ToList();

            var seqResult = seq.SingleOrNone();
            var listResult = list.SingleOrNone();

            seqResult.IsFail.Should().BeTrue();
            seqResult.Error.ErrorCode.Should().Be(ErrorCodes.UnexpectedNullValue);

            listResult.IsFail.Should().BeTrue();
            listResult.Error.ErrorCode.Should().Be(ErrorCodes.UnexpectedNullValue);
        }

        [Fact]
        public void Given_predicate_When_single_matched_and_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(5, 1);

            var seqResult = seq.SingleOrNone(x => x > 4);

            seqResult.IsSuccess.Should().BeTrue();
            seqResult.Value.Should().Be(5);
        }

        [Fact]
        public void Given_predicate_When_not_matched_Returns_None()
        {
            var seq = Enumerable.Empty<int>();

            var seqResult = seq.SingleOrNone(x => x == 20);

            seqResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Given_predicate_When_multiple_matched_Returns_Fail_400()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.SingleOrNone(x => x > 4);
            var listResult = list.SingleOrNone(x => x > 4);

            seqResult.IsFail.Should().BeTrue();
            seqResult.Error.ErrorCode.Should().Be(ErrorCodes.BadRequest);

            listResult.IsFail.Should().BeTrue();
            listResult.Error.ErrorCode.Should().Be(ErrorCodes.BadRequest);
        }

        [Fact]
        public void Given_predicate_When_single_matched_and_null_Returns_Fail_n1404()
        {
            var seq = Enumerable.Repeat<int?>(null, 1);

            var seqResult = seq.SingleOrNone(x => x is null);

            seqResult.IsFail.Should().BeTrue();
            seqResult.Error.ErrorCode.Should().Be(ErrorCodes.UnexpectedNullValue);
        }
    }

    public class For_ElementAtOrNone
    {
        [Fact]
        public void When_element_at_matched_and_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.FirstOrNone();
            var listResult = list.FirstOrNone();

            seqResult.IsSuccess.Should().BeTrue();
            seqResult.Value.Should().Be(1);

            listResult.IsSuccess.Should().BeTrue();
            listResult.Value.Should().Be(1);
        }

        [Fact]
        public void When_element_at_out_of_range_Returns_None()
        {
            var seq = Enumerable.Empty<int>();
            var list = seq.ToList();

            var seqResult = seq.FirstOrNone();
            var listResult = list.FirstOrNone();

            seqResult.IsNone.Should().BeTrue();
            listResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void When_element_at_matched_and_null_Returns_Fail_n1404()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);
            var list = seq.ToList();

            var seqResult = seq.FirstOrNone();
            var listResult = list.FirstOrNone();

            seqResult.IsFail.Should().BeTrue();
            seqResult.Error.ErrorCode.Should().Be(ErrorCodes.UnexpectedNullValue);

            listResult.IsFail.Should().BeTrue();
            listResult.Error.ErrorCode.Should().Be(ErrorCodes.UnexpectedNullValue);
        }
    }
}
