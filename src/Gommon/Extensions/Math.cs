using System.Linq;

namespace System
{
    public static partial class Math
    {
        public static int Average(params int[] numbers) 
            => numbers.Sum() / numbers.Length;
    }
}
