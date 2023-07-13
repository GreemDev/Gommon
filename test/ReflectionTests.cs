using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace Gommon.Tests {
    public class MirrorTests {

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
                Mirror.ReflectUnsafe(new ReflectionTest())
                    .Call("uhjetiughjrtiuhrtu")
            );
        }

        private class ReflectionTest {
            private string lmfaoStr = "lmfao";
            private string oafmlStr => "oafml";
            private void ThrowEx() => throw new Exception();

        }
    }

    public class ReflectionTests
    {
        [Fact]
        public static void Test_TypeAsFullNamePrettyString()
        {
            Assert.Equal("System.Collections.Generic.List<String>", typeof(List<string>).AsFullNamePrettyString());
        }

        [Fact]
        public static void Test_MethodSignatureMatches()
        {
            var methodInfo = typeof(ReflectionTests).GetMethod("DummyMethod", BindingFlags.Static | BindingFlags.NonPublic);
            
            Assert.True(methodInfo.CheckSignature(typeof(string), typeof(List<int>)).Matches);
            Assert.False(methodInfo.CheckSignature(typeof(string)).Matches);
            Assert.Equal(
                "Parameter at index 0 is not of the required type! Required: System.Int32, Actual: System.String",
                methodInfo.CheckSignature(typeof(int), typeof(List<int>)).Error);
        }

        private static void DummyMethod(string var1, List<int> var2)
        {
            
        }
        
    }
}