using System;

namespace ConsoleUI
{
    enum Options { add , update , display , ViewTheLists , exit }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine( "Choose one of the following:\n" +
                "add: Insert options\n" +
                "update: Update options\n" +
                "display: Display options\n" +
                "ViewTheLists: List view options\n" +
                "exit: Output\n");
            string ans = "";
            ans = Console.ReadLine();
            switch (ans)
            {
                
                    
            }
            Stam();
            Console.WriteLine("Pr" +
                "ess any key to continue...");
            Console.ReadKey();
        }
        public void AddStation()
        {

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
