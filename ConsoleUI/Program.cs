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
                                //DalObject.dalObject.AddBaseStation();
                                break;
                            case InseitOption.AddSkimmer:
                                //AddSkimmer();
                                break;
                            case InseitOption.AddClient:
                                //AddClient();
                                break;
                            case InseitOption.AddPackage:
                                //AddPackage;
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
                            case UpdateOption.Affiliation:
                                break;
                            case UpdateOption.Collection:
                                break;
                            case UpdateOption.Supply:
                                break;
                            case UpdateOption.SendLoading:
                                break;
                            case UpdateOption.ReleaseCharging:
                                break;

                        }
                        break;
                    case Options.Display:
                        Console.WriteLine("adding option:\n 0-Exit\n; 1- Base Station View\n" +
                            " 2- Skimmer display\n" +
                            "3- Customer view\n " +
                            " 4-Package view\n");
                        optionsListView = (OptionsListView)int.Parse(Console.ReadLine());
                        switch (optionsListView) 
                        {
                            case OptionsListView.Exit:
                                break;
                            case OptionsListView.ViewBaseStation:
                                break;
                            case OptionsListView.ViewSkimmer:
                                break;
                            case OptionsListView.ViewClient:
                                break;
                            case OptionsListView.ViewPackage:
                                break;
                            case OptionsListView.ViewUnassignedPackages:
                                break;
                            case OptionsListView.ViewFreeBaseStation:
                                break;
                        }
                        break;
                    case Options.ViewTheLists:
                        Console.WriteLine("adding option:\n 0-Exit\n; 1- Displays a list of base stations\n" +
                            " 2- Displays the list of skimmers\n" +
                            "3- View customer list\n " +
                            " 4-Displays the list of packages\n" +
                            "5-Displays a list of packages not yet associated with the glider\n" +
                            "6-Display of base stations with available charging stations");
                        displayOptions = (DisplayOptions)int.Parse(Console.ReadLine());
                        switch(displayOptions)
                        {
                            case DisplayOptions.Exit:
                                break;
                            case DisplayOptions.DisplayBaseStation:
                                break;
                            case DisplayOptions.DisplaySkimmer:
                                break;
                            case DisplayOptions.DisplayClient:
                                break;
                            case DisplayOptions.DisplayPackage:
                                break;
                        }
                        break;
                }
            }
            while (true);
        }

        static void Main(string[] args)
        {
            Menu();
            //Console.WriteLine( "Choose one of the following:\n" +
            //    "add: Insert options\n" +
            //    "update: Update options\n" +
            //    "display: Display options\n" +
            //    "ViewTheLists: List view options\n" +
            //    "exit: Output\n");
            //string ans = "";
            //ans = Console.ReadLine();
            //switch (ans)
            //{
                
                    
            //}
            //Stam();
            //Console.WriteLine("Pr" +
            //    "ess any key to continue...");
            //Console.ReadKey();
        }
        //public void AddStation()
        //{

        //}

        //private static void Stam()
        //{
        //    IDAL.DO.Client client = new IDAL.DO.Client
        //    {
        //        Name = "kuku",
        //        Latitude = -36.123456,
        //        Longitude = 29.654321,
        //        Telephone = "052534111",
        //        ID = 123
        //    };
        //    Console.WriteLine(client);
        //}
    }
}
