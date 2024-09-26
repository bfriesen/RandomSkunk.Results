using System.Collections.Generic;

namespace RandomSkunk.Results.UnitTests;

public class AggregateError_record_class
{
    public class Constructor
    {
        [Fact]
        public void GivenErrorsParameterHasTwoOrMoreItems_ReturnsAggregateError()
        {
            var error1 = new Error { Message = "Error 1" };
            var error2 = new Error { Message = "Error 2" };
            var innerErrors = new[] { error1, error2 };

            var aggregateError = new AggregateError(innerErrors, "My message details.") { ErrorCode = 123, Identifier = "test_identifier" };

            aggregateError.InnerErrors.Should().Equal(innerErrors);
            aggregateError.Message.Should().Be($"Two errors occurred. My message details.");
            aggregateError.ErrorCode.Should().Be(123);
            aggregateError.Identifier.Should().Be("test_identifier");
        }

        [Fact]
        public void GivenErrorsParameterIsNull_ThrowsException()
        {
            IEnumerable<Error> innerErrors = null!;

            var act = () => new AggregateError(innerErrors, "My message details.") { ErrorCode = 123, Identifier = "test_identifier" };

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*errors*");
        }

        [Fact]
        public void GivenErrorsParameterContainsFewerThanTwoItems_ThrowsException()
        {
            var error1 = new Error { Message = "Error 1" };
            var innerErrors = new[] { error1 };

            var act = () => new AggregateError(innerErrors, "My message details.") { ErrorCode = 123, Identifier = "test_identifier" };

            act.Should().ThrowExactly<ArgumentException>().WithMessage("*Sequence must contain at least two errors.*");
        }
    }

    public class CreateOrGetSingle_method
    {
        [Fact]
        public void GivenErrorsParameterHasOneItem_ReturnsItem()
        {
            var error1 = new Error { Message = "Error 1" };
            var innerErrors = new[] { error1 };

            var error = AggregateError.CreateOrGetSingle(innerErrors);
            error.Should().BeSameAs(error1);
        }

        [Fact]
        public void GivenErrorsParameterHasTwoOrMoreItems_ReturnsAggregateError()
        {
            var error1 = new Error { Message = "Error 1" };
            var error2 = new Error { Message = "Error 2" };
            var innerErrors = new[] { error1, error2 };

            var error = AggregateError.CreateOrGetSingle(innerErrors);

            var aggregateError = error.Should().BeOfType<AggregateError>().Subject;
            aggregateError.InnerErrors.Should().Equal(innerErrors);
            aggregateError.Message.Should().Be($"Two errors occurred.");
        }

        [Fact]
        public void GivenErrorsParameterIsNull_ThrowsException()
        {
            IEnumerable<Error> innerErrors = null!;

            var act = () => AggregateError.CreateOrGetSingle(innerErrors);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*errors*");
        }

        [Fact]
        public void GivenErrorsParameterContainsZeroItems_ThrowsException()
        {
            var innerErrors = Array.Empty<Error>();

            var act = () => AggregateError.CreateOrGetSingle(innerErrors);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("*Sequence must contain at least one error.*");
        }
    }
}
