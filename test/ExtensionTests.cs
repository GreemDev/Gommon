using System;
using System.Collections.Generic;
using Xunit;

namespace Gommon.Tests
{
    public class ExtensionTests
    {
        public static class System
        {
            [Fact]
            public static void Type_AsPrettyString()
            {
                Assert.Equal("List<int?>", typeof(List<int?>).AsPrettyString());
            } 
        }

        public static class Primitive
        {
            [Fact]
            public static void String_Append_Prepend()
            {
                Assert.Equal("leftright", "right".Prepend("left"));
                Assert.Equal("leftright", "left".Append("right"));
            }
        }
    }
}