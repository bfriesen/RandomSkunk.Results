namespace RandomSkunk.Results.UnitTests;

public class Error_record_class
{
    public class FromException
    {
        [Fact]
        public void When_optional_parameters_are_provided_Properties_are_set_accordingly()
        {
            var exception = GetException();
            var message = "my-message";
            var errorCode = 1;
            var identifier = "my-identifier";
            var title = "my-title";

            var error = Error.FromException(exception, message, errorCode, identifier, title);

            error.Message.Should().Be(message);
            error.ErrorCode.Should().Be(errorCode);
            error.Identifier.Should().Be(identifier);
            error.Title.Should().Be(title);

            error.InnerError.Should().NotBeNull();
            error.InnerError!.Message.Should().Be(exception.Message);
            error.InnerError.StackTrace.Should().Be(exception.StackTrace);
            error.InnerError.Title.Should().Be(exception.GetType().Name);
        }

        [Fact]
        public void When_optional_parameters_are_not_provided_Properties_are_set_to_null_or_default_values()
        {
            var exception = GetException();

            var error = Error.FromException(exception);

            error.Message.Should().Be(Error._defaultExceptionFailMessage);
            error.ErrorCode.Should().BeNull();
            error.Identifier.Should().BeNull();
            error.Title.Should().Be(nameof(Error));

            error.InnerError.Should().NotBeNull();
            error.InnerError!.Message.Should().Be(exception.Message);
            error.InnerError.StackTrace.Should().Be(exception.StackTrace);
            error.InnerError.Title.Should().Be(exception.GetType().Name);
        }

        [Fact]
        public void Given_null_exception_parameter_Throws_ArgumentNullException()
        {
            Action act = () => Error.FromException(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        private static Exception GetException()
        {
            Exception exception = null!;
            try
            {
                int i = 0;
                int j = 1 / i;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return exception;
        }
    }
}
