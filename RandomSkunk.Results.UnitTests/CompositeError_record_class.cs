using System.Collections.Generic;

namespace RandomSkunk.Results.UnitTests;

public class CompositeError_record_class
{
    public class Create_method
    {
        [Fact]
        public void GivenErrorsParameterHasTwoOrMoreItems_ReturnsCompositeError()
        {
            var error1 = new Error { Message = "Error 1" };
            var error2 = new Error { Message = "Error 2" };
            var errors = new[] { error1, error2 };

            var compositeError = CompositeError.Create(errors, "My message details.", 123, "test_identifier");

            compositeError.Errors.Should().Equal(errors);
            compositeError.Message.Should().Be($"Two errors occurred. See 'Errors' item under Extensions property for details. My message details.");
            compositeError.ErrorCode.Should().Be(123);
            compositeError.Identifier.Should().Be("test_identifier");
        }

        [Fact]
        public void GivenErrorsParameterIsNull_ThrowsException()
        {
            IEnumerable<Error> errors = null!;

            var act = () => CompositeError.Create(errors, "My message details.", 123, "test_identifier");

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*errors*");
        }

        [Fact]
        public void GivenErrorsParameterContainsFewerThanTwoItems_ThrowsException()
        {
            var error1 = new Error { Message = "Error 1" };
            var errors = new[] { error1 };

            var act = () => CompositeError.Create(errors, "My message details.", 123, "test_identifier");

            act.Should().ThrowExactly<ArgumentException>().WithMessage("*Sequence must contain at least two errors.*");
        }
    }

    public class CreateOrGetSingle_method
    {
        [Fact]
        public void GivenErrorsParameterHasOneItem_ReturnsItem()
        {
            var error1 = new Error { Message = "Error 1" };
            var errors = new[] { error1 };

            var error = CompositeError.CreateOrGetSingle(errors);
            error.Should().BeSameAs(error1);
        }

        [Fact]
        public void GivenErrorsParameterHasTwoOrMoreItems_ReturnsCompositeError()
        {
            var error1 = new Error { Message = "Error 1" };
            var error2 = new Error { Message = "Error 2" };
            var errors = new[] { error1, error2 };

            var error = CompositeError.CreateOrGetSingle(errors);

            var compositeError = error.Should().BeOfType<CompositeError>().Subject;
            compositeError.Errors.Should().Equal(errors);
            compositeError.Message.Should().Be($"Two errors occurred. See 'Errors' item under Extensions property for details.");
        }

        [Fact]
        public void GivenErrorsParameterIsNull_ThrowsException()
        {
            IEnumerable<Error> errors = null!;

            var act = () => CompositeError.CreateOrGetSingle(errors);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*errors*");
        }

        [Fact]
        public void GivenErrorsParameterContainsZeroItems_ThrowsException()
        {
            var errors = Array.Empty<Error>();

            var act = () => CompositeError.CreateOrGetSingle(errors);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("*Sequence must contain at least one error.*");
        }
    }
}
