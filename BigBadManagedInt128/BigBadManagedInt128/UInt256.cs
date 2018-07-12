using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BigBadManagedInt128
{
    public struct UInt256
    {
        private UInt64 QWORD0;
        private UInt64 QWORD1;
        private UInt64 QWORD2;
        private UInt64 QWORD3;

        [DllImport("BigBadInt128.dll")]
        private static extern IntPtr UInt256ToString(UInt256 input, IntPtr resultBuffer);

        public override string ToString()
        {
            IntPtr ptrwszBuffer = Marshal.AllocHGlobal(180);  // minimum 162
            IntPtr lpwszString = UInt256ToString(this, ptrwszBuffer);
            string result = Marshal.PtrToStringUni(lpwszString);
            Marshal.FreeHGlobal(ptrwszBuffer);
            return result;
        }
    }
}
