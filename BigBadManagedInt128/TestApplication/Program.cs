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
            BigBadManagedInt128.Int128 a = new BigBadManagedInt128.Int128(1);
            BigBadManagedInt128.Int128 b = new BigBadManagedInt128.Int128(2);

            BigBadManagedInt128.Int128 c = a + b;

            // right now ToString does not work
            // so printing pieces
            Console.WriteLine(c.loQWORD);
            Console.WriteLine(c.hiQWORD);
            Console.ReadLine();
        }
    }
}
