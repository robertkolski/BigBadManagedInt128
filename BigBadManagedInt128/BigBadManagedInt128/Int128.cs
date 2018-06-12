﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BigBadManagedInt128
{
    public struct Int128
    {
        public Int64 loQWORD;
        public Int64 hiQWORD;

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
        private static extern void Int128Add(IntPtr addend1, IntPtr addend2, IntPtr result);

        [DllImport("BigBadInt128.dll")]
        private static extern void Int128Sub(IntPtr input1, IntPtr input2, IntPtr result);

        [DllImport("BigBadInt128.dll")]
        private static extern void Int128Mul(IntPtr input1, IntPtr input2, IntPtr result);

        [DllImport("BigBadInt128.dll")]
        private static extern IntPtr Int128ToString(IntPtr input, IntPtr result);

        [DllImport("BigBadInt128.dll")]
        private static extern Int32 Int128Parse(IntPtr input, IntPtr result);

        public static Int128 operator+ (Int128 addend1, Int128 addend2)
        {
            IntPtr ptrAddend1 = Marshal.AllocHGlobal(16);
            IntPtr ptrAddend2 = Marshal.AllocHGlobal(16);
            IntPtr ptrResult = Marshal.AllocHGlobal(16);
            Marshal.StructureToPtr(addend1, ptrAddend1, false);
            Marshal.StructureToPtr(addend2, ptrAddend2, false);
            Int128Add(ptrAddend1, ptrAddend2, ptrResult);
            Int128 result = (Int128)Marshal.PtrToStructure(ptrResult, typeof(Int128));
            Marshal.FreeHGlobal(ptrAddend1);
            Marshal.FreeHGlobal(ptrAddend2);
            Marshal.FreeHGlobal(ptrResult);
            return result;
        }

        public static Int128 operator -(Int128 input1, Int128 input2)
        {
            IntPtr ptrInput1 = Marshal.AllocHGlobal(16);
            IntPtr ptrInput2 = Marshal.AllocHGlobal(16);
            IntPtr ptrResult = Marshal.AllocHGlobal(16);
            Marshal.StructureToPtr(input1, ptrInput1, false);
            Marshal.StructureToPtr(input2, ptrInput2, false);
            Int128Sub(ptrInput1, ptrInput2, ptrResult);
            Int128 result = (Int128)Marshal.PtrToStructure(ptrResult, typeof(Int128));
            Marshal.FreeHGlobal(ptrInput1);
            Marshal.FreeHGlobal(ptrInput2);
            Marshal.FreeHGlobal(ptrResult);
            return result;
        }

        public static Int128 operator *(Int128 input1, Int128 input2)
        {
            IntPtr ptrInput1 = Marshal.AllocHGlobal(16);
            IntPtr ptrInput2 = Marshal.AllocHGlobal(16);
            IntPtr ptrResult = Marshal.AllocHGlobal(16);
            Marshal.StructureToPtr(input1, ptrInput1, false);
            Marshal.StructureToPtr(input2, ptrInput2, false);
            Int128Mul(ptrInput1, ptrInput2, ptrResult);
            Int256 result256 = (Int256)Marshal.PtrToStructure(ptrResult, typeof(Int256));
            Marshal.FreeHGlobal(ptrInput1);
            Marshal.FreeHGlobal(ptrInput2);
            Marshal.FreeHGlobal(ptrResult);

            Int128 result = default(Int128);
            result.loQWORD = result256.QWORD0;
            result.hiQWORD = result256.QWORD1;

            if (
                (result256.QWORD2 == 0 && result256.QWORD3 == 0)
                ||
                (result256.QWORD2 == -1 && result256.QWORD3 == -1)
                )
            {
                return result;
            }
            else
            {
                throw new OverflowException();
            }
        }

        public Int256 MultiplyWith256BitResult(Int128 input)
        {
            IntPtr ptrInput1 = Marshal.AllocHGlobal(16);
            IntPtr ptrInput2 = Marshal.AllocHGlobal(16);
            IntPtr ptrResult = Marshal.AllocHGlobal(16);
            Marshal.StructureToPtr(this, ptrInput1, false);
            Marshal.StructureToPtr(input, ptrInput2, false);
            Int128Mul(ptrInput1, ptrInput2, ptrResult);
            Int256 result = (Int256)Marshal.PtrToStructure(ptrResult, typeof(Int256));
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
            IntPtr lpwszString = Int128ToString(ptrInput, ptrwszBuffer);
            string result = Marshal.PtrToStringUni(lpwszString);
            Marshal.FreeHGlobal(ptrInput);
            Marshal.FreeHGlobal(ptrwszBuffer);
            return result;
        }

        public static Int128 Parse(string input)
        {
            Int128 result = default(Int128);

            IntPtr ptrInput = Marshal.StringToHGlobalUni(input);
            IntPtr ptrResult = Marshal.AllocHGlobal(16);
            Int32 errorCode = Int128Parse(ptrInput, ptrResult);
            if (errorCode == 0)
            {
                result = (Int128)Marshal.PtrToStructure(ptrResult, typeof(Int128));
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
