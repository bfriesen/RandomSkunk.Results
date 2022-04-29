namespace RandomSkunk.Results.UnitTests;

public class ResultBase_should
{
    [Fact]
    public void Have_default_behavior()
    {
        ResultBase result = new DefaultResultBase();

        result.IsSuccess.Should().BeFalse();
        result.IsFail.Should().BeFalse();
        Accessing.Error(result).Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(Exceptions.CannotAccessErrorUnlessFailMessage);
    }

    private class DefaultResultBase : ResultBase
    {
    }
}
