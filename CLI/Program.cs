using System;

namespace TestGenerator.CLI
{
    static class Program
    {
        private static int Square(int x) => (x * x);


        private static void Main()
        {
//            var service = new TestService();
//            service.TestGeneratingProcedure();
            Console.WriteLine(Square(7));
            Console.ReadKey();
        }
    }
}