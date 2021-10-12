using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Stam();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void Stam()
        {
            IDAL.DO.Client client = new IDAL.DO.Client
            {
                Name = "kuku",
                Latitude = -36.123456,
                Longitude = 29.654321,
                Telephone = "052534111",
                ID = 123
            };
            Console.WriteLine(client);
        }
    }
}
