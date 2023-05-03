namespace RandomSkunk.Results.UnitTests;

[Collection(nameof(FailResult))]
public class FailResult_Handle_method
{
    [Fact]
    public void GivenErrorWithNoStackTrace_WhenOmitStackTraceParameterIsFalse_ThenStackTraceIsAddedToReplacedErrorParameter()
    {
        var error = new Error { Message = "Example" };
        var original = error;

        FailResult.Handle(ref error, false);

        error.Should().NotBeSameAs(original);
        error.StackTrace.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GivenErrorWithNoStackTrace_WhenOmitStackTraceParameterIsNullAndOmitStackTracePropertyIsFalse_ThenStackTraceIsAddedToReplacedErrorParameter()
    {
        var omitStackTrace = FailResult.OmitStackTrace;
        FailResult.OmitStackTrace = false;

        try
        {
            var error = new Error { Message = "Example" };
            var original = error;

            FailResult.Handle(ref error, null);

            error.Should().NotBeSameAs(original);
            error.StackTrace.Should().NotBeNullOrEmpty();
        }
        finally
        {
            FailResult.OmitStackTrace = omitStackTrace;
        }
    }

    [Fact]
    public void GivenErrorWithNoStackTrace_WhenOmitStackTraceParameterIsTrue_ThenErrorParameterIsNotChanged()
    {
        var error = new Error { Message = "Example" };
        var original = error;

        FailResult.Handle(ref error, true);

        error.Should().BeSameAs(original);
    }

    [Fact]
    public void GivenErrorWithNoStackTrace_WhenOmitStackTraceParameterIsNullAndOmitStackTracePropertyIsTrue_ThenErrorParameterIsNotChanged()
    {
        var omitStackTrace = FailResult.OmitStackTrace;
        FailResult.OmitStackTrace = true;

        try
        {
            var error = new Error { Message = "Example" };
            var original = error;

            FailResult.Handle(ref error, null);

            error.Should().BeSameAs(original);
        }
        finally
        {
            FailResult.OmitStackTrace = omitStackTrace;
        }
    }

    [Fact]
    public void GivenReplaceErrorFunctionSet_ThenItIsInvokedAndTheReturnedErrorReplacesTheErrorParameter()
    {
        Error? capturedError = null;

        FailResult.SetReplaceErrorFunction(error =>
        {
            capturedError = error with { Title = "Replacement Title" };
            return capturedError;
        });

        try
        {
            var error = new Error { Message = "Example" };
            var original = error;

            FailResult.Handle(ref error, true);

            error.Should().NotBeSameAs(original);
            error.Should().BeSameAs(capturedError);
        }
        finally
        {
            FailResult.SetReplaceErrorFunction(null);
        }
    }

    [Fact]
    public void GivenReplaceErrorFunctionSet_WhenHandleIsCalledMultipleTimesWithTheSameError_ThenReplaceErrorFunctionFunctionIsCalledOnlyOnce()
    {
        var invocationCount = 0;

        FailResult.SetReplaceErrorFunction(error =>
        {
            ++invocationCount;
            return error with { Title = "Replacement Title" };
        });

        try
        {
            var error = new Error { Message = "Example" };

            FailResult.Handle(ref error, true);
            FailResult.Handle(ref error, true);
            FailResult.Handle(ref error, true);

            invocationCount.Should().Be(1);
        }
        finally
        {
            FailResult.SetReplaceErrorFunction(null);
        }
    }

    [Fact]
    public void GivenCallbackFunctionSet_ThenItIsInvoked()
    {
        Error? capturedError = null;

        FailResult.SetCallbackFunction(error => capturedError = error);

        try
        {
            var error = new Error { Message = "Example" };

            FailResult.Handle(ref error, true);

            capturedError.Should().BeSameAs(error);
        }
        finally
        {
            FailResult.SetCallbackFunction(null);
        }
    }

    [Fact]
    public void GivenCallbackFunctionSet_WhenHandleIsCalledMultipleTimesWithTheSameError_ThenCallbackFunctionFunctionIsCalledOnlyOnce()
    {
        var invocationCount = 0;

        FailResult.SetCallbackFunction(error => ++invocationCount);

        try
        {
            var error = new Error { Message = "Example" };

            FailResult.Handle(ref error, true);
            FailResult.Handle(ref error, true);
            FailResult.Handle(ref error, true);

            invocationCount.Should().Be(1);
        }
        finally
        {
            FailResult.SetCallbackFunction(null);
        }
    }

    [CollectionDefinition(nameof(FailResult), DisableParallelization = true)]
    public class FailResultCollectionDefinition
    {
    }
}
