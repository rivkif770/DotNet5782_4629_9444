using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace ConsoleUI
{
    class Program
    {
        enum Options { Exit, Add, Update, Display, ViewTheLists }
        enum InseitOption { Exit, AddBaseStation, AddSkimmer, AddClient,AddPackage }
        enum UpdateOption { Exit, Affiliation, Collection, Supply, SendLoading, ReleaseCharging }
        enum OptionsListView { Exit, ViewBaseStation, ViewSkimmer, ViewClient, ViewPackage, ViewUnassignedPackages, ViewFreeBaseStation }
        enum DisplayOptions { Exit, DisplayBaseStation, DisplaySkimmer, DisplayClient, DisplayPackage }
        private static void Menu()
        {
            Options options;
            InseitOption inseitOption;
            UpdateOption updateOption;
            OptionsListView optionsListView;
            DisplayOptions displayOptions;
            do
            {
                Console.WriteLine("welcome!" + "option:\n 0-Exit\n 1-Add\n 2-Update\n 3-List View\n 4-Display\n");
                options = (Options)int.Parse(Console.ReadLine());
                int id, num, status, CustomerSending, CustomerReceiving, SkimmerOperation;
                string name, Phone;
                double longitude, Latitude;
                DronStatuses mode;
                WeightCategories Weight;
                Priorities priority;
                DateTime TimeDelivery, TimeGlider, collectionTime, TimeRecipient;
                switch (options)
                {
                    case Options.Add:
                        Console.WriteLine("adding option:\n 0-Exit\n; 1- Add a base station to the list of stations\n" +
                            " 2- Add a skimmer to the list of existing skimmers\n" +
                            "3- Admission of a new customer to the customer list\n " +
                            " 4-Receipt of package for shipment\n");
                        inseitOption = (InseitOption)int.Parse(Console.ReadLine());
                        switch (inseitOption)
                        {
                            case InseitOption.Exit:
                                break;
                            case InseitOption.AddBaseStation:
                                Console.WriteLine("Enter unique ID number:");
                                id = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter The station name:");
                                name = (Console.ReadLine());
                                Console.WriteLine("Enter Several positions of charging:");
                                num = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter longitude:");
                                longitude = double.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Latitude:");
                                Latitude = double.Parse(Console.ReadLine());
                                //DalObject.DataSource.BaseStationArry[ DataSource.B](id, name, num, longitude, Latitude);
                                break;
                            case InseitOption.AddSkimmer:
                                Console.WriteLine("Enter unique ID number:");
                                id = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Skimmer model:");
                                name = (Console.ReadLine());
                                Console.WriteLine("Enter Weight category 1-low,2-middle,3-heavy:");
                                Weight = (WeightCategories)int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Battery status:");
                                status = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Skimmer mode:");
                                mode = (DronStatuses)int.Parse(Console.ReadLine());
                                //DalObject.DataSource.BaseStationArry[ DataSource.B](id, name, num, longitude, Latitude);
                                break;
                            case InseitOption.AddClient:
                                Console.WriteLine("Enter unique ID number:");
                                id = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter the customer's name:");
                                name = (Console.ReadLine());
                                Console.WriteLine("Enter Phone Number:");
                                Phone = (Console.ReadLine());
                                Console.WriteLine("Enter longitude:");
                                longitude = double.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Latitude:");
                                Latitude = double.Parse(Console.ReadLine());
                                //DalObject.DataSource.BaseStationArry[ DataSource.B](id, name, num, longitude, Latitude);
                                break;
                            case InseitOption.AddPackage:
                                Console.WriteLine("Enter unique ID number:");
                                id = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Sending customer ID:");
                                CustomerSending = int.Parse(Console.ReadLine());
                                Console.WriteLine("Receiving customer ID:");
                                CustomerReceiving = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Weight category 1-low,2-middle,3-heavy:");
                                Weight = (WeightCategories)int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter priority:");
                                priority = (Priorities)int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Operator skimmer ID:");
                                SkimmerOperation = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Time to create a package for delivery:");
                                TimeDelivery = DateTime.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Time to assign the package to the glider:");
                                TimeGlider = DateTime.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Package collection time from the sender:");
                                collectionTime = DateTime.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Time of arrival of the package to the recipient:");
                                TimeRecipient = DateTime.Parse(Console.ReadLine());
                                //DalObject.DataSource.BaseStationArry[ DataSource.B](id, name, num, longitude, Latitude);
                                break;
                        }
                        break;

                    case Options.Update:
                        Console.WriteLine("adding option:\n 0-Exit\n; 1- Assign a package to a skimmer\n" +
                            " 2- Package collection by skimmer\n" +
                            "3- ADelivery package to customer\n " +
                            " 4-Sending a skimmer for charging at a base station\n" +
                            "5-Release skimmer from charging at base station");
                        updateOption = (UpdateOption)int.Parse(Console.ReadLine());
                        switch (updateOption)
                        {
                            case UpdateOption.Exit:
                                break;
                        }
                        break;
                }
            }
            while (true);
        }

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
