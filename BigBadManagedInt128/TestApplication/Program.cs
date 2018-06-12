using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            BigBadManagedInt128.Int128 a = BigBadManagedInt128.Int128.Parse("-200000000000000000000002");
            BigBadManagedInt128.Int128 b = BigBadManagedInt128.Int128.Parse("-100000000000000000000001");

            //BigBadManagedInt128.Int128 a = new BigBadManagedInt128.Int128(-1, -2);
            //BigBadManagedInt128.Int128 b = new BigBadManagedInt128.Int128(-1, -3);

            BigBadManagedInt128.Int128 c = a + b;

            Console.WriteLine(c);

            BigBadManagedInt128.UInt128 d = BigBadManagedInt128.UInt128.Parse("200000000000000000000002");
            BigBadManagedInt128.UInt128 e = BigBadManagedInt128.UInt128.Parse("100000000000000000000001");

            //BigBadManagedInt128.UInt128 d = new BigBadManagedInt128.UInt128(1,1);
            //BigBadManagedInt128.UInt128 e = new BigBadManagedInt128.UInt128(2,2);

            BigBadManagedInt128.UInt128 f = d + e;

            Console.WriteLine(f);

            Console.ReadLine();
        }
    }
}
