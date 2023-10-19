namespace RandomSkunk.Results.UnitTests;

public class ToFailIfNone_method
{
    [Fact]
    public void GivenSuccessResult_ReturnsSameResult()
    {
        var result = Result<int>.Success(123);
        var error = new Error { Title = "a", Message = "b", Identifier = "c", ErrorCode = 12345 };

        var actual = result.ToFailIfNone(() => error);

        actual.Should().Be(result);
    }

    [Fact]
    public void GivenFailResult_ReturnsSameResult()
    {
        var result = Result<int>.Fail(message: "1", errorCode: 54321, identifier: "2", title: "3");
        var error = new Error { Title = "a", Message = "b", Identifier = "c", ErrorCode = 12345 };

        var actual = result.ToFailIfNone(() => error);

        actual.Should().Be(result);
    }

    [Fact]
    public void GivenNoneResult_WhenGetErrorParameterIsSpecified_ReturnsFailResultWithErrorFromIt()
    {
        var result = Result<int>.None();
        var error = new Error { Title = "a", Message = "b", Identifier = "c", ErrorCode = 12345 };

        var actual = result.ToFailIfNone(() => error);

        actual.Should().NotBe(result);
        actual.IsFail.Should().BeTrue();
        actual.Error.Should().Be(error);
    }
}
