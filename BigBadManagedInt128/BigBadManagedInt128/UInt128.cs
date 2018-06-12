using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BigBadManagedInt128
{
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
        private static extern void Int128Add(IntPtr addend1, IntPtr addend2, IntPtr result);

        [DllImport("BigBadInt128.dll")]
        private static extern void Int128Sub(IntPtr input1, IntPtr input2, IntPtr result);

        [DllImport("BigBadInt128.dll")]
        private static extern void UInt128Mul(IntPtr input1, IntPtr input2, IntPtr result);

        [DllImport("BigBadInt128.dll")]
        private static extern IntPtr UInt128ToString(IntPtr input, IntPtr result);

        [DllImport("BigBadInt128.dll")]
        private static extern Int32 UInt128Parse(IntPtr input, IntPtr result);

        public static UInt128 operator +(UInt128 addend1, UInt128 addend2)
        {
            IntPtr ptrAddend1 = Marshal.AllocHGlobal(16);
            IntPtr ptrAddend2 = Marshal.AllocHGlobal(16);
            IntPtr ptrResult = Marshal.AllocHGlobal(16);
            Marshal.StructureToPtr(addend1, ptrAddend1, false);
            Marshal.StructureToPtr(addend2, ptrAddend2, false);
            Int128Add(ptrAddend1, ptrAddend2, ptrResult);
            UInt128 result = (UInt128)Marshal.PtrToStructure(ptrResult, typeof(UInt128));
            Marshal.FreeHGlobal(ptrAddend1);
            Marshal.FreeHGlobal(ptrAddend2);
            Marshal.FreeHGlobal(ptrResult);
            return result;
        }

        public static UInt128 operator -(UInt128 input1, UInt128 input2)
        {
            IntPtr ptrInput1 = Marshal.AllocHGlobal(16);
            IntPtr ptrInput2 = Marshal.AllocHGlobal(16);
            IntPtr ptrResult = Marshal.AllocHGlobal(16);
            Marshal.StructureToPtr(input1, ptrInput1, false);
            Marshal.StructureToPtr(input2, ptrInput2, false);
            Int128Sub(ptrInput1, ptrInput2, ptrResult);
            UInt128 result = (UInt128)Marshal.PtrToStructure(ptrResult, typeof(UInt128));
            Marshal.FreeHGlobal(ptrInput1);
            Marshal.FreeHGlobal(ptrInput2);
            Marshal.FreeHGlobal(ptrResult);
            return result;
        }

        public static UInt128 operator *(UInt128 input1, UInt128 input2)
        {
            IntPtr ptrInput1 = Marshal.AllocHGlobal(16);
            IntPtr ptrInput2 = Marshal.AllocHGlobal(16);
            IntPtr ptrResult = Marshal.AllocHGlobal(16);
            Marshal.StructureToPtr(input1, ptrInput1, false);
            Marshal.StructureToPtr(input2, ptrInput2, false);
            UInt128Mul(ptrInput1, ptrInput2, ptrResult);
            UInt256 result256 = (UInt256)Marshal.PtrToStructure(ptrResult, typeof(UInt256));
            Marshal.FreeHGlobal(ptrInput1);
            Marshal.FreeHGlobal(ptrInput2);
            Marshal.FreeHGlobal(ptrResult);

            UInt128 result = default(UInt128);
            result.loQWORD = result256.QWORD0;
            result.hiQWORD = result256.QWORD1;

            if (result256.QWORD2 == 0 && result256.QWORD3 == 0)
            {
                return result;
            }
            else
            {
                throw new OverflowException();
            }
        }

        public UInt256 MultiplyWith256BitResult(UInt128 input)
        {
            IntPtr ptrInput1 = Marshal.AllocHGlobal(16);
            IntPtr ptrInput2 = Marshal.AllocHGlobal(16);
            IntPtr ptrResult = Marshal.AllocHGlobal(16);
            Marshal.StructureToPtr(this, ptrInput1, false);
            Marshal.StructureToPtr(input, ptrInput2, false);
            UInt128Mul(ptrInput1, ptrInput2, ptrResult);
            UInt256 result = (UInt256)Marshal.PtrToStructure(ptrResult, typeof(UInt256));
            Marshal.FreeHGlobal(ptrInput1);
            Marshal.FreeHGlobal(ptrInput2);
            Marshal.FreeHGlobal(ptrResult);
            return result;
        }

        public override string ToString()
        {
            IntPtr ptrInput = Marshal.AllocHGlobal(16);
            IntPtr ptrwszBuffer = Marshal.AllocHGlobal(128);
            Marshal.StructureToPtr(this, ptrInput, false);
            IntPtr lpwszString = UInt128ToString(ptrInput, ptrwszBuffer);
            string result = Marshal.PtrToStringUni(lpwszString);
            Marshal.FreeHGlobal(ptrInput);
            Marshal.FreeHGlobal(ptrwszBuffer);
            return result;
        }

        public static UInt128 Parse(string input)
        {
            UInt128 result = default(UInt128);

            IntPtr ptrInput = Marshal.StringToHGlobalUni(input);
            IntPtr ptrResult = Marshal.AllocHGlobal(16);
            Int32 errorCode = UInt128Parse(ptrInput, ptrResult);
            if (errorCode == 0)
            {
                result = (UInt128)Marshal.PtrToStructure(ptrResult, typeof(UInt128));
            }
            Marshal.FreeHGlobal(ptrInput);
            Marshal.FreeHGlobal(ptrResult);
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
