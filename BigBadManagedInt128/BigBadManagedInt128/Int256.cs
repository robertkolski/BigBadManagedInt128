using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BigBadManagedInt128
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Int256
    {
        private Int64 QWORD0;
        private Int64 QWORD1;
        private Int64 QWORD2;
        private Int64 QWORD3;

        [DllImport("BigBadInt128.dll")]
        private static extern IntPtr Int256ToString(Int256 input, IntPtr resultBuffer);

        public override string ToString()
        {
            IntPtr ptrwszBuffer = Marshal.AllocHGlobal(180); // 162 bytes minimum
            IntPtr lpwszString = Int256ToString(this, ptrwszBuffer);
            string result = Marshal.PtrToStringUni(lpwszString);
            Marshal.FreeHGlobal(ptrwszBuffer);
            return result;
        }
    }
}
