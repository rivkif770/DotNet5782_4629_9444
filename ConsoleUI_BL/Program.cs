using System;
using IBL.BO;
namespace ConsoleUI_BL
{
    class Program
    {
        static IBl bl = new BL.BlObject();
        static void Main(string[] args)
        {
            Console.WriteLine("Hell O World!");


            Customer customer;
            Console.WriteLine("give me  customer id");
            int id = Int32.Parse(Console.ReadLine());
            try
            {
                customer = bl.GetCustomer(id);
            }
            catch (CustomerException exception)
            {
                Console.WriteLine(exception.Message);
            }
            Console.WriteLine();
        }
    }
}
