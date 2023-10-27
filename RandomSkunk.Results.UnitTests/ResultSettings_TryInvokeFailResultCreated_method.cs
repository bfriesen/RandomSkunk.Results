namespace RandomSkunk.Results.UnitTests;

[Collection(nameof(ResultSettings))]
public class ResultSettings_TryInvokeFailResultCreated_method
{
    [Fact]
    public void GivenFailResultCreatedIsSet_ThenItIsInvoked()
    {
        Error? capturedError = null;

        ResultSettings.FailResultCreated = error => capturedError = error;

        try
        {
            var error = new Error { Message = "Example" };

            ResultSettings.InvokeFailResultCreatedCallback(error);

            capturedError.Should().BeSameAs(error);
        }
        finally
        {
            ResultSettings.FailResultCreated = null;
        }
    }

    [Fact]
    public void GivenFailResultCreatedIsSet_WhenTryInvokeFailResultCallbackIsCalledMultipleTimesWithTheSameError_ThenCallbackIsInvokedOnlyOnce()
    {
        var invocationCount = 0;

        ResultSettings.FailResultCreated = error => ++invocationCount;

        try
        {
            var error = new Error { Message = "Example" };

            ResultSettings.InvokeFailResultCreatedCallback(error);
            ResultSettings.InvokeFailResultCreatedCallback(error);
            ResultSettings.InvokeFailResultCreatedCallback(error);

            invocationCount.Should().Be(1);
        }
        finally
        {
            ResultSettings.FailResultCreated = null;
        }
    }

    [CollectionDefinition(nameof(ResultSettings), DisableParallelization = true)]
    public class FailResultCollectionDefinition
    {
    }
}
