using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Gommon.Tests {
    public class OptionalTests {
        [Fact]
        public void TestOptional() {
            Assert.False(Optional.None<char>().HasValue);

            Assert.Equal("", new Optional<string>(() => ""));

            // oh boy here we go
            Assert.True(new Optional<string>("")
                .OnlyIf(s => s.Any())
                .OrElseGet(() => "test string")
                .Any());
            
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
            Assert.True(new Optional<string>("test string") == new Optional<string>("test string"));
        }
    }
}