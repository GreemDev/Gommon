using System;
using System.Linq;
using Xunit;

namespace Gommon.Tests;

public class OptionalTests {
    [Fact]
    public void TestOptional() {
        Assert.False(Optional<char>.None.HasValue);

        Assert.Equal("", new Optional<string>(() => ""));

        // oh boy here we go
        Assert.True(new Optional<string>("")
            .OnlyIf(s => s.Length != 0)
            .OrElseGet(() => "test string")
            .Length > 0);
            
        Assert.True(Optional.Of("").Check(string.IsNullOrEmpty));

        Assert.Equal("hello", Optional.None<string>()
            .Convert(async _ => "")
            .OrElseGet(() => "hello").Result);

        Assert.Equal(69,
            new Optional<int>().OrElseGet(() => 69)
        );

        Assert.Throws<NullReferenceException>(() => new Optional<int>().OrThrow<NullReferenceException>());
    }

    [Fact]
    public void TestEqualsOverride() {
        Assert.False(new Optional<string>() == "test string");
        Assert.False(new Optional<string>("test string") == null);
        Assert.True(new Optional<object>() == null);
        Assert.True(Optional.Of("test string") == new Optional<string>("test string"));
    }
}