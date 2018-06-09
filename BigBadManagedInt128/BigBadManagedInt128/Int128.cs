using System;
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

        [DllImport("BigBadInt128.dll")]
        private static extern void Int128Add(IntPtr addend1, IntPtr addend2, IntPtr result);

        [DllImport("BigBadInt128.dll")]
        private static extern void Int128ToString(IntPtr input, IntPtr result);

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

        public override string ToString()
        {
            IntPtr ptrInput = Marshal.AllocHGlobal(16);
            IntPtr ptrwszBuffer = Marshal.AllocHGlobal(128);
            Marshal.StructureToPtr(this, ptrInput, false);
            Int128ToString(ptrInput, ptrwszBuffer);
            string result = Marshal.PtrToStringUni(ptrwszBuffer);
            Marshal.FreeHGlobal(ptrInput);
            Marshal.FreeHGlobal(ptrwszBuffer);
            return result;
        }
    }
}
