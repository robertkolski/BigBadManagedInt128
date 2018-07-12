using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BigBadManagedInt128
{
    [StructLayout(LayoutKind.Sequential)]
    public struct UInt128
    {
        public UInt64 loQWORD;
        public UInt64 hiQWORD;

        public UInt128(UInt64 initialValue)
        {
            hiQWORD = 0;
            loQWORD = initialValue;
        }

        public UInt128(UInt64 loQWORD, UInt64 hiQWORD)
        {
            this.loQWORD = loQWORD;
            this.hiQWORD = hiQWORD;
        }

        [DllImport("BigBadInt128.dll")]
        private static extern UInt128 UInt128Add(UInt128 addend1, UInt128 addend2);

        [DllImport("BigBadInt128.dll")]
        private static extern UInt128 UInt128Sub(UInt128 input1, UInt128 input2);

        [DllImport("BigBadInt128.dll")]
        private static extern UInt256 UInt128Mul(UInt128 input1, UInt128 input2);

        [DllImport("BigBadInt128.dll")]
        private static extern IntPtr UInt128ToString(UInt128 input, IntPtr resultBuffer);

        [DllImport("BigBadInt128.dll")]
        private static extern Int32 UInt128Parse(
            [MarshalAs(UnmanagedType.LPWStr)]
            string input, 
            out UInt128 result);

        public static UInt128 operator +(UInt128 addend1, UInt128 addend2)
        {
            return UInt128Add(addend1, addend2);
        }

        public static UInt128 operator -(UInt128 input1, UInt128 input2)
        {
            return UInt128Sub(input1, input2);
        }

        public static UInt256 operator *(UInt128 input1, UInt128 input2)
        {
            return UInt128Mul(input1, input2);
        }

        public override string ToString()
        {
            IntPtr ptrwszBuffer = Marshal.AllocHGlobal(128);
            IntPtr lpwszString = UInt128ToString(this, ptrwszBuffer);
            string result = Marshal.PtrToStringUni(lpwszString);
            Marshal.FreeHGlobal(ptrwszBuffer);
            return result;
        }

        public static UInt128 Parse(string input)
        {
            UInt128 result = default(UInt128);
            Int32 errorCode = UInt128Parse(input, out result);
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
