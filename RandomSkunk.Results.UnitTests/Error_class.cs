namespace RandomSkunk.Results.UnitTests;

public class Error_class
{
    public class Constructor
    {
        [Fact]
        public void When_optional_parameters_are_provided_Properties_are_set_accordingly()
        {
            var message = "my-message";
            var stackTrace = "my-stack-trace";
            var errorCode = 1;
            var identifier = "my-identifier";
            var type = "my-type";
            var innerError = new Error();

            var error = new Error(message, stackTrace, errorCode, identifier, type, innerError);

            error.Message.Should().Be(message);
            error.StackTrace.Should().Be(stackTrace);
            error.ErrorCode.Should().Be(errorCode);
            error.Identifier.Should().Be(identifier);
            error.Type.Should().Be(type);
            error.InnerError.Should().BeSameAs(innerError);
        }

        [Fact]
        public void When_optional_parameters_are_not_provided_Properties_are_set_to_null_or_default_values()
        {
            var error = new Error();

            error.Message.Should().Be(Error.DefaultMessage);
            error.StackTrace.Should().BeNull();
            error.ErrorCode.Should().BeNull();
            error.Identifier.Should().BeNull();
            error.Type.Should().Be(nameof(Error));
            error.InnerError.Should().BeNull();
        }
    }

    public class FromException
    {
        [Fact]
        public void When_optional_parameters_are_provided_Properties_are_set_accordingly()
        {
            var exception = GetException();
            var message = "my-message";
            var errorCode = 1;
            var identifier = "my-identifier";
            var type = "my-type";
            var innerError = new Error();

            var error = Error.FromException(exception, message, errorCode, identifier, type, innerError);

            error.Message.Should().Be($"{message}{Environment.NewLine}{exception.GetType().Name}: {exception.Message}");
            error.StackTrace.Should().Be(exception.StackTrace);
            error.ErrorCode.Should().Be(errorCode);
            error.Identifier.Should().Be(identifier);
            error.Type.Should().Be(type);
            error.InnerError.Should().BeSameAs(innerError);
        }

        [Fact]
        public void When_optional_parameters_are_not_provided_Properties_are_set_to_null_or_default_values()
        {
            var exception = GetException();

            var error = Error.FromException(exception);

            error.Message.Should().Be($"{exception.GetType().Name}: {exception.Message}");
            error.StackTrace.Should().Be(exception.StackTrace);
            error.ErrorCode.Should().BeNull();
            error.Identifier.Should().BeNull();
            error.Type.Should().Be(nameof(Error));
            error.InnerError.Should().BeNull();
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

    public new class Equals
    {
        [Fact]
        public void Given_same_property_values_Returns_true()
        {
            var error = new Error("a", "b", 3, "d", "e", new Error());
            var other = new Error("a", "b", 3, "d", "e", new Error());

            var actual = error.Equals(other);

            actual.Should().BeTrue();
        }

        [Fact]
        public void Given_different_property_values_Returns_true()
        {
            var error = new Error("a", "b", 3, "d", "e", new Error("X"));
            var other = new Error("a", "b", 3, "d", "e", new Error("Y"));

            var actual = error.Equals(other);

            actual.Should().BeFalse();
        }
    }

    public new class GetHashCode
    {
        [Fact]
        public void Different_results_return_different_values()
        {
            var error1 = new Error().GetHashCode();
            var error2 = new Error(message: "a").GetHashCode();
            var error3 = new Error(stackTrace: "b").GetHashCode();
            var error4 = new Error(errorCode: 3).GetHashCode();
            var error5 = new Error(identifier: "c").GetHashCode();
            var error6 = new Error(type: "d").GetHashCode();
            var error7 = new Error(innerError: new Error()).GetHashCode();
            var error8 = new Error("a", "b", 3, "d", "e", new Error("X")).GetHashCode();
            var error9 = new Error("a", "b", 3, "d", "e", new Error("Y")).GetHashCode();

            error1.Should().NotBe(error2);
            error1.Should().NotBe(error3);
            error1.Should().NotBe(error4);
            error1.Should().NotBe(error5);
            error1.Should().NotBe(error6);
            error1.Should().NotBe(error7);
            error1.Should().NotBe(error8);
            error1.Should().NotBe(error9);

            error2.Should().NotBe(error3);
            error2.Should().NotBe(error4);
            error2.Should().NotBe(error5);
            error2.Should().NotBe(error6);
            error2.Should().NotBe(error7);
            error2.Should().NotBe(error8);
            error2.Should().NotBe(error9);

            error3.Should().NotBe(error4);
            error3.Should().NotBe(error5);
            error3.Should().NotBe(error6);
            error3.Should().NotBe(error7);
            error3.Should().NotBe(error8);
            error3.Should().NotBe(error9);

            error4.Should().NotBe(error5);
            error4.Should().NotBe(error6);
            error4.Should().NotBe(error7);
            error4.Should().NotBe(error8);
            error4.Should().NotBe(error9);

            error5.Should().NotBe(error6);
            error5.Should().NotBe(error7);
            error5.Should().NotBe(error8);
            error5.Should().NotBe(error9);

            error6.Should().NotBe(error7);
            error6.Should().NotBe(error8);
            error6.Should().NotBe(error9);

            error7.Should().NotBe(error8);
            error7.Should().NotBe(error9);

            error8.Should().NotBe(error9);
        }
    }
}
