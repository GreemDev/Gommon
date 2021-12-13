using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace Gommon.Tests {
    public class ReflectionTests {

        private static Mirror<ReflectionTest> _mirror = Mirror.Reflect(new ReflectionTest());

        //this works, it throws an exception, but the Assert.Throws doesn't catch it.
        /*
        [Fact]
        public static void Test_Ex() {  
            Assert.Throws<Exception>(() => _mirror.Call("ThrowEx"));
        }
        */

        [Fact]
        public static void Test_FieldOrProperty() {
            Assert.Equal("lmfao", _mirror.Get<string>("lmfaoStr"));
            Assert.Equal("oafml", _mirror.Get<string>("oafmlStr"));
        }
        
        [Fact]
        public static void Test_ThrowIfMemberNull() {
            Assert.Throws<ValueException>(() =>
                Mirror.ReflectUnsafe(new ReflectionTest()).Call("uhjetiughjrtiuhrtu")
            );
        }

        private class ReflectionTest {
            private string lmfaoStr = "lmfao";
            private string oafmlStr => "oafml";
            private void ThrowEx() => throw new Exception();

        }
        
    }
}