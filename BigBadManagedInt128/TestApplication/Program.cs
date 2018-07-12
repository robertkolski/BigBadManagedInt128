using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BigBadManagedInt128;

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

            foreach (MethodInfo method in typeof(BigBadManagedInt128.Int128).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
            {
                Console.WriteLine(method.Name);
            }

            Console.ReadLine();

            Console.WriteLine("What type do you want to work with?");
            string requestedType = Console.ReadLine();
            Type type = GetType(requestedType);
            object input1 = null;
            object input2 = null;
            object result = null;
            Console.Write("Input 1:");
            string input1String = Console.ReadLine();
            input1 = CallParse(type, input1String);
            Console.Write("Operation add, subtract, or multiply:");
            string operation = Console.ReadLine();
            string operationMethod = GetOperationMethod(operation);
            Console.Write("Input 2:");
            string input2String = Console.ReadLine();
            input2 = CallParse(type, input2String);
            result = type.GetMethod(operationMethod, BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { input1, input2 });
            Console.WriteLine("Result: {0}", result);
            Console.ReadLine();
        }
        
        public static Type GetType(string requestedType)
        {
            switch (requestedType)
            {
                case "Int32":
                    return typeof(Int32);
                case "Int64":
                    return typeof(Int64);
                case "Int128":
                    return typeof(Int128);
                case "UInt32":
                    return typeof(UInt32);
                case "UInt64":
                    return typeof(UInt64);
                case "UInt128":
                    return typeof(UInt128);
            }

            return Type.GetType(requestedType);
        }

        public static string GetOperationMethod(string input)
        {
            switch (input)
            {
                case "add":
                    return "op_Addition";
                case "subtract":
                    return "op_Subtraction";
                case "multiply":
                    return "op_Multiply";
            }
            throw new InvalidOperationException();
        }

        public static object CallParse(Type type, string input)
        {
            MethodInfo method = type.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
            return method.Invoke(null, new object[] { input });
        }
    }
}
