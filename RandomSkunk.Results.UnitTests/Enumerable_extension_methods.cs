using RandomSkunk.Results.Linq;
using System.Linq;

namespace RandomSkunk.Results.UnitTests;

public class Enumerable_extension_methods
{
    public class For_First
    {
        [Fact]
        public void OrFail_When_not_empty_and_first_is_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.FirstOrFail();
            var listResult = list.FirstOrFail();

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(1);

            listResult.IsSuccess.Should().BeTrue();
            listResult._value!.Should().Be(1);
        }

        [Fact]
        public void OrFail_When_empty_Returns_Fail_404()
        {
            var seq = Enumerable.Empty<int>();
            var list = seq.ToList();

            var seqResult = seq.FirstOrFail();
            var listResult = list.FirstOrFail();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.NotFound);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.NotFound);
        }

        [Fact]
        public void OrFail_When_not_empty_and_first_is_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);
            var list = seq.ToList();

            var seqResult = seq.FirstOrFail();
            var listResult = list.FirstOrFail();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }

        [Fact]
        public void OrNone_When_not_empty_and_first_is_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.FirstOrNone();
            var listResult = list.FirstOrNone();

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(1);

            listResult.IsSuccess.Should().BeTrue();
            listResult._value!.Should().Be(1);
        }

        [Fact]
        public void OrNone_When_empty_Returns_None()
        {
            var seq = Enumerable.Empty<int>();
            var list = seq.ToList();

            var seqResult = seq.FirstOrNone();
            var listResult = list.FirstOrNone();

            seqResult.IsNone.Should().BeTrue();
            listResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void OrNone_When_not_empty_and_first_is_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);
            var list = seq.ToList();

            var seqResult = seq.FirstOrNone();
            var listResult = list.FirstOrNone();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }

        [Fact]
        public void OrFail_Given_predicate_When_first_matched_and_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);

            var seqResult = seq.FirstOrFail(x => x > 4);

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(5);
        }

        [Fact]
        public void OrFail_Given_predicate_When_not_matched_Returns_Fail_404()
        {
            var seq = Enumerable.Empty<int>();

            var seqResult = seq.FirstOrFail(x => x == 20);

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.NotFound);
        }

        [Fact]
        public void OrFail_Given_predicate_When_matched_and_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);

            var seqResult = seq.FirstOrFail(x => x is null);

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }

        [Fact]
        public void OrNone_Given_predicate_When_first_matched_and_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);

            var seqResult = seq.FirstOrNone(x => x > 4);

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(5);
        }

        [Fact]
        public void OrNone_Given_predicate_When_not_matched_Returns_None()
        {
            var seq = Enumerable.Empty<int>();

            var seqResult = seq.FirstOrNone(x => x == 20);

            seqResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void OrNone_Given_predicate_When_first_matched_and_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);

            var seqResult = seq.FirstOrNone(x => x is null);

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }
    }

    public class For_Last
    {
        [Fact]
        public void OrFail_When_not_empty_and_last_is_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.LastOrFail();
            var listResult = list.LastOrFail();

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(10);

            listResult.IsSuccess.Should().BeTrue();
            listResult._value!.Should().Be(10);
        }

        [Fact]
        public void OrFail_When_empty_Returns_Fail_404()
        {
            var seq = Enumerable.Empty<int>();
            var list = seq.ToList();

            var seqResult = seq.LastOrFail();
            var listResult = list.LastOrFail();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.NotFound);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.NotFound);
        }

        [Fact]
        public void OrFail_When_not_empty_and_last_is_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);
            var list = seq.ToList();

            var seqResult = seq.LastOrFail();
            var listResult = list.LastOrFail();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }

        [Fact]
        public void OrNone_When_not_empty_and_last_is_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.LastOrNone();
            var listResult = list.LastOrNone();

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(10);

            listResult.IsSuccess.Should().BeTrue();
            listResult._value!.Should().Be(10);
        }

        [Fact]
        public void OrNone_When_empty_Returns_None()
        {
            var seq = Enumerable.Empty<int>();
            var list = seq.ToList();

            var seqResult = seq.LastOrNone();
            var listResult = list.LastOrNone();

            seqResult.IsNone.Should().BeTrue();
            listResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void OrNone_When_not_empty_and_last_is_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);
            var list = seq.ToList();

            var seqResult = seq.LastOrNone();
            var listResult = list.LastOrNone();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }

        [Fact]
        public void OrFail_Given_predicate_When_last_matched_and_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);

            var seqResult = seq.LastOrFail(x => x < 6);

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(5);
        }

        [Fact]
        public void OrFail_Given_predicate_When_not_matched_Returns_Fail_404()
        {
            var seq = Enumerable.Empty<int>();

            var seqResult = seq.LastOrFail(x => x == 20);

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.NotFound);
        }

        [Fact]
        public void OrFail_Given_predicate_When_matched_and_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);

            var seqResult = seq.LastOrFail(x => x is null);

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }

        [Fact]
        public void OrNone_Given_predicate_When_last_matched_and_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);

            var seqResult = seq.LastOrNone(x => x < 6);

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(5);
        }

        [Fact]
        public void OrNone_Given_predicate_When_not_matched_Returns_None()
        {
            var seq = Enumerable.Empty<int>();

            var seqResult = seq.LastOrNone(x => x == 20);

            seqResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void OrNone_Given_predicate_When_last_matched_and_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);

            var seqResult = seq.LastOrNone(x => x is null);

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }
    }

    public class For_Single
    {
        [Fact]
        public void OrFail_When_not_empty_and_single_is_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 1);
            var list = seq.ToList();

            var seqResult = seq.SingleOrFail();
            var listResult = list.SingleOrFail();

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(1);

            listResult.IsSuccess.Should().BeTrue();
            listResult._value!.Should().Be(1);
        }

        [Fact]
        public void OrFail_When_empty_Returns_Fail_404()
        {
            var seq = Enumerable.Empty<int>();
            var list = seq.ToList();

            var seqResult = seq.SingleOrFail();
            var listResult = list.SingleOrFail();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.NotFound);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.NotFound);
        }

        [Fact]
        public void OrFail_When_more_than_one_element_Returns_Fail_400()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.SingleOrFail();
            var listResult = list.SingleOrFail();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.BadRequest);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.BadRequest);
        }

        [Fact]
        public void OrFail_When_not_empty_and_single_is_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 1);
            var list = seq.ToList();

            var seqResult = seq.SingleOrFail();
            var listResult = list.SingleOrFail();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }

        [Fact]
        public void OrNone_When_not_empty_and_single_is_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 1);
            var list = seq.ToList();

            var seqResult = seq.SingleOrNone();
            var listResult = list.SingleOrNone();

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(1);

            listResult.IsSuccess.Should().BeTrue();
            listResult._value!.Should().Be(1);
        }

        [Fact]
        public void OrNone_When_empty_Returns_None()
        {
            var seq = Enumerable.Empty<int>();
            var list = seq.ToList();

            var seqResult = seq.SingleOrNone();
            var listResult = list.SingleOrNone();

            seqResult.IsNone.Should().BeTrue();
            listResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void OrNone_When_more_than_one_element_Returns_Fail_400()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.SingleOrNone();
            var listResult = list.SingleOrNone();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.BadRequest);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.BadRequest);
        }

        [Fact]
        public void OrNone_When_not_empty_and_single_is_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 1);
            var list = seq.ToList();

            var seqResult = seq.SingleOrNone();
            var listResult = list.SingleOrNone();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }

        [Fact]
        public void OrFail_Given_predicate_When_single_matched_and_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(5, 1);

            var seqResult = seq.SingleOrFail(x => x > 4);

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(5);
        }

        [Fact]
        public void OrFail_Given_predicate_When_multiple_matched_Returns_Fail_400()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.SingleOrFail(x => x > 4);
            var listResult = list.SingleOrFail(x => x > 4);

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.BadRequest);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.BadRequest);
        }

        [Fact]
        public void OrFail_Given_predicate_When_not_matched_Returns_Fail_404()
        {
            var seq = Enumerable.Empty<int>();

            var seqResult = seq.SingleOrFail(x => x == 20);

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.NotFound);
        }

        [Fact]
        public void OrFail_Given_predicate_When_single_matched_and_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 1);

            var seqResult = seq.SingleOrFail(x => x is null);

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }

        [Fact]
        public void OrNone_Given_predicate_When_single_matched_and_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(5, 1);

            var seqResult = seq.SingleOrNone(x => x > 4);

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(5);
        }

        [Fact]
        public void OrNone_Given_predicate_When_not_matched_Returns_None()
        {
            var seq = Enumerable.Empty<int>();

            var seqResult = seq.SingleOrNone(x => x == 20);

            seqResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void OrNone_Given_predicate_When_multiple_matched_Returns_Fail_400()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.SingleOrNone(x => x > 4);
            var listResult = list.SingleOrNone(x => x > 4);

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.BadRequest);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.BadRequest);
        }

        [Fact]
        public void OrNone_Given_predicate_When_single_matched_and_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 1);

            var seqResult = seq.SingleOrNone(x => x is null);

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }
    }

    public class For_ElementAt
    {
        [Fact]
        public void OrFail_When_element_at_matched_and_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.FirstOrFail();
            var listResult = list.FirstOrFail();

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(1);

            listResult.IsSuccess.Should().BeTrue();
            listResult._value!.Should().Be(1);
        }

        [Fact]
        public void OrFail_When_element_at_out_of_range_Returns_Fail_404()
        {
            var seq = Enumerable.Empty<int>();
            var list = seq.ToList();

            var seqResult = seq.FirstOrFail();
            var listResult = list.FirstOrFail();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.NotFound);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.NotFound);
        }

        [Fact]
        public void OrFail_When_element_at_matched_and_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);
            var list = seq.ToList();

            var seqResult = seq.FirstOrFail();
            var listResult = list.FirstOrFail();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }

        [Fact]
        public void OrNone_When_element_at_matched_and_not_null_Returns_Success()
        {
            var seq = Enumerable.Range(1, 10);
            var list = seq.ToList();

            var seqResult = seq.FirstOrNone();
            var listResult = list.FirstOrNone();

            seqResult.IsSuccess.Should().BeTrue();
            seqResult._value!.Should().Be(1);

            listResult.IsSuccess.Should().BeTrue();
            listResult._value!.Should().Be(1);
        }

        [Fact]
        public void OrNone_When_element_at_out_of_range_Returns_None()
        {
            var seq = Enumerable.Empty<int>();
            var list = seq.ToList();

            var seqResult = seq.FirstOrNone();
            var listResult = list.FirstOrNone();

            seqResult.IsNone.Should().BeTrue();
            listResult.IsNone.Should().BeTrue();
        }

        [Fact]
        public void OrNone_When_element_at_matched_and_null_Returns_Fail_410()
        {
            var seq = Enumerable.Repeat<int?>(null, 10);
            var list = seq.ToList();

            var seqResult = seq.FirstOrNone();
            var listResult = list.FirstOrNone();

            seqResult.IsFail.Should().BeTrue();
            seqResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);

            listResult.IsFail.Should().BeTrue();
            listResult.GetError().ErrorCode.Should().Be(ErrorCodes.Gone);
        }
    }
}
