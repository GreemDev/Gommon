using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Gommon.Tests;

public class ExtensionTests {
    public static class System {
        [Fact]
        public static void Type_AsPrettyString() {
            Assert.Equal("List<int?>", typeof(List<int?>).AsPrettyString());
        }

        [Fact]
        public static void Task_Shit() {
            Assert.Equal(0, Task.Run(() => "")
                .Then(res => res.Length)
                .Result);
                
            Assert.Equal("", Task.Run(() => 0)
                .Then(_ => "")
                .Result);
                
            Assert.True(((Task)null).OrCompleted().IsCompleted);
        }
    }

    public static class Primitive {
        [Fact]
        public static void String_Append_Prepend() {
            Assert.Equal("leftright", "right".Prepend("left"));
            Assert.Equal("leftright", "left".Append("right"));
        }

        [Fact]
        public static void Int_Coerce()
        {
            Assert.Equal(10, 100.CoerceAtMost(10));
            Assert.Equal(100, 10.CoerceAtLeast(100));
        }
            
    }
        
    public static class Collection {
        [Fact]
        public static void SafeDictionary_Indexer() {
            Assert.Null(new SafeDictionary<object, object>()["yo"]);
        }
    }
}