<Query Kind="Program">
  <NuGetReference Prerelease="true">RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

void Main()
{
    // The Error record class represents what went wrong in a failed operation. It is essentially a
    // replacement for the Exception class, providing much of the same information. Once created, an
    // instance of Error cannot be changed - it is immutable.
    
    // All Error instances have non-null values for their Message, Title, and Extensions properties,
    // even if none are specified during construction. All other properties initialize with null/default
    // values when not specified.
    Error error = new Error();
    error.Dump("No values specified");
    
    // While an Error cannot be modified, a copy of the error with modified properties can be created
    // using the "with" keyword.
    error = error with { Message = "Value cannot be null.", Title = "Argument Null" };
    error.Dump("Modified copy");
    
    // The ErrorCode of an Error could be any integer value that identifies the type of error, but it is
    // recommended to use an HTTP status code or a value with the last three digits ending in an HTTP
    // status code, e.g. 10404.
    error = error with { ErrorCode = 404 };
    
    // The Identifier of an Error is meant to uniquely identify the call site of a Fail result. The
    // string value of a new Guid is a good value to assign to it.
    error = error with { Identifier = "40c60c79-09ec-4ae1-a969-2e58aa649f8c" };
    
    // The Extensions property holds any additional data associated with the Fail result.
    Dictionary<string, object> extensions = new() { ["ParamName"] = "someParameter" };
    error = error with { Extensions = extensions };
    
    // The StackTrace property means the same as Exception.StackTrace. The only difference is that
    // you can set it youself.
    error = error with { StackTrace = GetStackTrace() };
    
    // InnerError means the same thing as Exception.InnerException - it's the error that cause the
    // current error.
    error = error with { InnerError = GetInnerError() };
    
    // We've set all but one of the properties on Error, let's see what it looks like.
    error.Dump("All but one properties set");
    
    // Error.ToString() returns a value similar to Exception.ToString() - all the details about the
    // error are included.
    error.ToString().Dump("ToString()");

    // If we set IsSensitive to true, we get a very short representation of the Error - only the Title,
    // ErrorCode (if present), and Identifier (if present) are included. Since it is recommended to not
    // show detailed error information to end users, the IsSensitive flag exists to make displaying
    // *non-detailed* error information to users very easy - just get a sensitive version of an error
    // and call ToString() on it.
    error = error with { IsSensitive = true };
    error.ToString().Dump("ToString() when IsSensitive is true");
}

#region Support Code

private static string GetStackTrace(int skipFrames = 1) =>
    new StackTrace(new StackFrame(skipFrames, true)).ToString();

private static Error GetInnerError() =>
    new Error
    {
        Message = "Server did not respond.",
        Title = "Problem",
        ErrorCode = 504,
        StackTrace = GetStackTrace(2),
    };

#endregion
