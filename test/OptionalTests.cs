using System;
using Xunit;
using System.Linq;
using System.Threading.Tasks;

// while, yes, Optional("test") == Optional(test),
// the test is for the Optional<T>#Equals override, and == / !=, as they use the Equals method.

// ReSharper disable EqualExpressionComparison

// this occurs because Optional is a struct, but can be nullchecked
// because the nullcheck tests the Optional's internal value.

// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace Gommon.Tests
{
    public class OptionalTests
    {
        [Fact]
        public void TestOptional()
        {
            Assert.True(new PossibleRef<int>().IsEmpty);

            Assert.Equal("", new Possible<string>(() => ""));
            
            // oh boy here we go
            Assert.True(new Possible<string>("")
                .OnlyIf(s => s.Any())
                .OrElseGet(() => "test string")
                .Any());

            Assert.Equal(69,
                new PossibleRef<int>().OrElseGet(() => 69)
            );

            Assert.Throws<NullReferenceException>(() => new PossibleRef<int>().OrThrow());
        }

        [Fact]
        public void TestEqualsOverride()
        {
            Assert.False(new Possible<object>() == "test string");
            Assert.False(new Possible<string>("test string") == null);
            Assert.True(new Possible<object>() == null);
            Assert.True(new Possible<string>("test string") == new Possible<string>("test string"));
        }
    }
}