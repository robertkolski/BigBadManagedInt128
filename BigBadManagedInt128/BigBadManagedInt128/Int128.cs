using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BigBadManagedInt128
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Int128
    {
        private Int64 loQWORD;
        private Int64 hiQWORD;

        public Int128(Int64 initialValue)
        {
            if (initialValue < 0)
            {
                hiQWORD = -1;
            }
            else
            {
                hiQWORD = 0;
            }
            loQWORD = initialValue;
        }

        public Int128(Int64 loQWORD, Int64 hiQWORD)
        {
            this.loQWORD = loQWORD;
            this.hiQWORD = hiQWORD;
        }


        [DllImport("BigBadInt128.dll")]
        private static extern Int128 Int128Add(Int128 input1, Int128 input2);

        [DllImport("BigBadInt128.dll")]
        private static extern Int128 Int128Sub(Int128 input1, Int128 input2);

        [DllImport("BigBadInt128.dll")]
        private static extern Int256 Int128Mul(Int128 input1, Int128 input2);

        [DllImport("BigBadInt128.dll")]
        private static extern IntPtr Int128ToString(Int128 input, IntPtr resultBuffer);

        [DllImport("BigBadInt128.dll")]
        private static extern Int32 Int128Parse(
            [MarshalAs(UnmanagedType.LPWStr)]
            string input, 
            out Int128 result);

        public static Int128 operator+ (Int128 input1, Int128 input2)
        {
            return Int128Add(input1, input2);
        }

        public static Int128 operator -(Int128 input1, Int128 input2)
        {
            return Int128Sub(input1, input2);
        }

        public static Int256 operator *(Int128 input1, Int128 input2)
        {
            return Int128Mul(input1, input2);
        }

        public override string ToString()
        {
            IntPtr ptrwszBuffer = Marshal.AllocHGlobal(128);
            IntPtr lpwszString = Int128ToString(this, ptrwszBuffer);
            string result = Marshal.PtrToStringUni(lpwszString);
            Marshal.FreeHGlobal(ptrwszBuffer);
            return result;
        }

        public static Int128 Parse(string input)
        {
            Int128 result = default(Int128);
            Int32 errorCode = Int128Parse(input, out result);
            if (errorCode == 1)
            {
                throw new OverflowException();
            }
            if (errorCode == 2)
            {
                throw new FormatException();
            }
            return result;
        }
    }
}
