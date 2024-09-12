using System.Collections.Generic;

namespace RandomSkunk.Results.UnitTests;

public class CompositeError_record_class
{
    public class Constructor
    {
        [Fact]
        public void GivenErrorsParameterHasTwoOrMoreItems_ReturnsCompositeError()
        {
            var error1 = new Error { Message = "Error 1" };
            var error2 = new Error { Message = "Error 2" };
            var innerErrors = new[] { error1, error2 };

            var compositeError = new CompositeError(innerErrors, "My message details.") { ErrorCode = 123, Identifier = "test_identifier" };

            compositeError.InnerErrors.Should().Equal(innerErrors);
            compositeError.Message.Should().Be($"Two errors occurred. My message details.");
            compositeError.ErrorCode.Should().Be(123);
            compositeError.Identifier.Should().Be("test_identifier");
        }

        [Fact]
        public void GivenErrorsParameterIsNull_ThrowsException()
        {
            IEnumerable<Error> innerErrors = null!;

            var act = () => new CompositeError(innerErrors, "My message details.") { ErrorCode = 123, Identifier = "test_identifier" };

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*errors*");
        }

        [Fact]
        public void GivenErrorsParameterContainsFewerThanTwoItems_ThrowsException()
        {
            var error1 = new Error { Message = "Error 1" };
            var innerErrors = new[] { error1 };

            var act = () => new CompositeError(innerErrors, "My message details.") { ErrorCode = 123, Identifier = "test_identifier" };

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

            var error = CompositeError.CreateOrGetSingle(innerErrors);
            error.Should().BeSameAs(error1);
        }

        [Fact]
        public void GivenErrorsParameterHasTwoOrMoreItems_ReturnsCompositeError()
        {
            var error1 = new Error { Message = "Error 1" };
            var error2 = new Error { Message = "Error 2" };
            var innerErrors = new[] { error1, error2 };

            var error = CompositeError.CreateOrGetSingle(innerErrors);

            var compositeError = error.Should().BeOfType<CompositeError>().Subject;
            compositeError.InnerErrors.Should().Equal(innerErrors);
            compositeError.Message.Should().Be($"Two errors occurred.");
        }

        [Fact]
        public void GivenErrorsParameterIsNull_ThrowsException()
        {
            IEnumerable<Error> innerErrors = null!;

            var act = () => CompositeError.CreateOrGetSingle(innerErrors);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*errors*");
        }

        [Fact]
        public void GivenErrorsParameterContainsZeroItems_ThrowsException()
        {
            var innerErrors = Array.Empty<Error>();

            var act = () => CompositeError.CreateOrGetSingle(innerErrors);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("*Sequence must contain at least one error.*");
        }
    }
}
