using System;
using Xunit;

namespace Gommon.Tests;

public class ResultTests
{
    [Fact]
    public static void Test_Result()
    {
        Assert.Throws<Exception>(() => AlwaysErrors().Unwrap());
        Assert.Equal("Hello", AlwaysReturns().Unwrap());
        
        Assert.Equal(0, 
            Return<int>.Failure(new NullError()).UnwrapOptional(out _).OrDefault()
            );
        
        Assert.True(AlwaysSucceeds().IsSuccess);
        Assert.False(AlwaysSucceeds().IsError);
        
        Assert.False(AlwaysFails().IsSuccess);
        Assert.True(AlwaysFails().IsError);
    }
    
    
    private static Return<string> AlwaysErrors() => Return<string>.Failure(new MessageError("Failed"));
    private static Return<string> AlwaysReturns() => "Hello";
    
    private static Result AlwaysFails() => Result.Failure(new NullError());
    private static Result AlwaysSucceeds() => Result.Success;
}