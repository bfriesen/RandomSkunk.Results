<Query Kind="Statements">
  <NuGetReference Prerelease="true">RandomSkunk.Results</NuGetReference>
  <Namespace>RandomSkunk.Results</Namespace>
</Query>

Result result;

// The error from a default or uninitialized result does not have a stack trace.
result = default(Result);
result.Error.Dump("Error from default result");

// An error by itself does not have a stack trace unless explicitly set.
Error error = new Error("My error message.");
error.Dump("Error by itself");

// By default, a stack trace is added to the error of a Fail result.
result = Result.Fail(error);
result.Error.Dump("Error from Fail result");

// If the error of a Fail result already has a stack trace, it is not changed.
Error errorWithStackTraceAlready = new Error { StackTrace = "Some stack trace" };
result = Result.Fail(errorWithStackTraceAlready);
result.Error.Dump("Error already with stack trace from Fail result");

// Whether to set the stack trace of fail results can be configured globally.
FailResult.SetStackTrace = false;

// Now when the result is created, its error does not have a stack trace.
result = Result.Fail(error);
result.Error.Dump("FailResult.SetStackTrace is false");

// You can override the global setting on a per-call basis. All result Fail methods have this parameter.
result = Result.Fail(error, setStackTrace: true);
result.Error.Dump("setStackTrace parameter is true");

#region Support Code

// Setting FailResult.SetStackTrace back to true isn't relevant when you run this script once. But since
// LINQPad reuses the process from a query, the next time you ran this script, you wouldn't see a stack
// trace in the "Error from result" example above as expected because the process is still running and
// globally configured to omit stack traces unless the setStackTrace parameter was provided.
FailResult.SetStackTrace = true;

#endregion