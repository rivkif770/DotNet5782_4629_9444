//efrat hilel 213089444 rivki fraiman 325194629
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
        enum InseitOption { Exit, AddBaseStation, AddSkimmer, AddClient, AddPackage }
        enum UpdateOption { Exit, Affiliation, Collection, Supply, SendLoading, ReleaseCharging }
        enum DisplayOptions { Exit, DisplayBaseStation, DisplaySkimmer, DisplayClient, DisplayPackage }
        enum OptionsListView { Exit, ViewBaseStation, ViewSkimmer, ViewClient, ViewPackage, ViewUnassignedPackages, ViewFreeBaseStation }
        private static void Menu()
        {
            Options options;
            InseitOption inseitOption;
            UpdateOption updateOption;
            OptionsListView optionsListView;
            DisplayOptions displayOptions;
            do
            {
                Console.WriteLine("welcome!" + "option:\n 0-Exit\n 1-Add\n 2-Update\n 3-Display\n 4-List View\n");
                options = (Options)int.Parse(Console.ReadLine());
                switch (options)
                {
                    case Options.Add:
                        Console.WriteLine("adding option:\n"+
                            " 0-Exit\n" +
                            " 1- Add a base station to the list of stations\n" +
                            " 2- Add a skimmer to the list of existing skimmers\n" +
                            " 3- Admission of a new customer to the customer list\n" +
                            " 4-Receipt of package for shipment\n");
                        inseitOption = (InseitOption)int.Parse(Console.ReadLine());
                        switch (inseitOption)
                        {
                            case InseitOption.Exit:
                                return;
                            case InseitOption.AddBaseStation:
                                BaseStation newBaseStation = new BaseStation();

                                Console.WriteLine("Enter unique ID number:");
                                newBaseStation.UniqueID = int.Parse(Console.ReadLine());

                                Console.WriteLine("Enter The station name:");
                                newBaseStation.StationName = (Console.ReadLine());

                                Console.WriteLine("Enter Several positions of charging:");
                                newBaseStation.SeveralPositionsArgument = int.Parse(Console.ReadLine());

                                Console.WriteLine("Enter longitude:");
                                newBaseStation.Longitude = double.Parse(Console.ReadLine());

                                Console.WriteLine("Enter Latitude:");
                                newBaseStation.Latitude = double.Parse(Console.ReadLine());

                                DalObject.dalObject.AddBaseStation(newBaseStation);
                                break;
                            case InseitOption.AddSkimmer:
                                Quadocopter newQuadocopter = new Quadocopter();

                                Console.WriteLine("Enter unique ID number:");
                                newQuadocopter.IDNumber = int.Parse(Console.ReadLine());

                                Console.WriteLine("Enter Skimmer model:");
                                newQuadocopter.SkimmerModel = Console.ReadLine();

                                Console.WriteLine("Enter Weight category 0-low,1-middle,2-heavy:");
                                newQuadocopter.Weight = (WeightCategories)int.Parse(Console.ReadLine());

                                newQuadocopter.Battery = 100;
                                newQuadocopter.SkimmerMode = (DronStatuses)0;

                                DalObject.dalObject.AddSkimmer(newQuadocopter);
                                break;
                            case InseitOption.AddClient:
                                Client newClient = new Client();

                                Console.WriteLine("Enter unique ID number:");
                                newClient.ID = int.Parse(Console.ReadLine());

                                Console.WriteLine("Enter the customer's name:");
                                newClient.Name = (Console.ReadLine());

                                Console.WriteLine("Enter Phone Number:");
                                newClient.Telephone = (Console.ReadLine());

                                Console.WriteLine("Enter longitude:");
                                newClient.Longitude = double.Parse(Console.ReadLine());

                                Console.WriteLine("Enter Latitude:");
                                newClient.Latitude = double.Parse(Console.ReadLine());

                                DalObject.dalObject.AddClient(newClient);
                                break;
                            case InseitOption.AddPackage:
                                Package newPackage = new Package();

                                newPackage.ID = 0;

                                Console.WriteLine("Enter Sending customer ID:");
                                newPackage.IDSender = int.Parse(Console.ReadLine());

                                Console.WriteLine("Receiving customer ID:");
                                newPackage.IDgets = int.Parse(Console.ReadLine());

                                Console.WriteLine("Enter Weight category 0-low,1-middle,2-heavy:");
                                newPackage.Weight = (WeightCategories)int.Parse(Console.ReadLine());

                                Console.WriteLine("Enter priority 0-regular,1-fast,2-emergency:");
                                newPackage.priority = (Priorities)int.Parse(Console.ReadLine());

                                newPackage.IDSkimmerOperation = 0;
                                newPackage.PackageCreationTime = DateTime.Now;

                                DalObject.dalObject.AddPackage(newPackage);
                                break;
                        }
                        break;

                    case Options.Update:
                        Console.WriteLine("adding option:\n" +
                            " 0-Exit\n" +
                            " 1-Assign a package to a skimmer\n" +
                            " 2-Package collection by skimmer\n" +
                            " 3-ADelivery package to customer\n" +
                            " 4-Sending a skimmer for charging at a base station\n" +
                            " 5-Release skimmer from charging at base station");
                        updateOption = (UpdateOption)int.Parse(Console.ReadLine());
                        switch (updateOption)
                        {
                            case UpdateOption.Exit:
                                return;
                            case UpdateOption.Affiliation:
                                int idp,idq;
                                Console.WriteLine("enter ID of Package:");
                                idp = int.Parse(Console.ReadLine());
                                Console.WriteLine("enter ID of skimmers:");
                                idq = int.Parse(Console.ReadLine());
                                DalObject.dalObject.AssignPackageSkimmer(idp, idq);
                                break;
                            case UpdateOption.Collection:
                                Console.WriteLine("enter ID of Package:");
                                int id = int.Parse(Console.ReadLine());
                                DalObject.dalObject.CollectionPackage(id);
                                break;
                            case UpdateOption.Supply:
                                Console.WriteLine("enter ID of Package:");
                                id = int.Parse(Console.ReadLine());
                                DalObject.dalObject.PackageDelivery(id);
                                break;
                            case UpdateOption.SendLoading:
                                Console.WriteLine("enter ID of skimmers:");
                                id = int.Parse(Console.ReadLine());
                                Console.WriteLine("Select a base station from the displayed stations and enter its ID number:");
                                DalObject.dalObject.printBaseStationFreeCharging();
                                int idBS = int.Parse(Console.ReadLine());
                                DalObject.dalObject.SendingSkimmerForCharging(id, idBS);
                                break;
                            case UpdateOption.ReleaseCharging:
                                Console.WriteLine("enter ID of skimmers:");
                                id = int.Parse(Console.ReadLine());
                                DalObject.dalObject.SkimmerRelease(id);
                                break;

                        }
                        break;
                    case Options.Display:
                        Console.WriteLine("adding option:\n" +
                            " 0-Exit\n" +
                            " 1-Base Station View\n" +
                            " 2-Skimmer display\n" +
                            " 3-Customer view\n" +
                            " 4-Package view\n");
                        displayOptions = (DisplayOptions)int.Parse(Console.ReadLine());
                        switch (displayOptions)
                        {
                            case DisplayOptions.Exit:
                                return;
                            case DisplayOptions.DisplayBaseStation:
                                Console.WriteLine("enter ID of BaseStation:");
                                int IDb;
                                IDb = int.Parse(Console.ReadLine());
                                DalObject.dalObject.DisplayBaseStation(IDb);
                                break;
                            case DisplayOptions.DisplaySkimmer:
                                Console.WriteLine("enter ID of Skimmer:");
                                int IDq;
                                IDq = int.Parse(Console.ReadLine());
                                DalObject.dalObject.DisplaySkimmer(IDq);
                                break;
                            case DisplayOptions.DisplayClient:
                                Console.WriteLine("enter ID of Client:");
                                int IDc;
                                IDc = int.Parse(Console.ReadLine());
                                DalObject.dalObject.DisplayClient(IDc);
                                break;
                            case DisplayOptions.DisplayPackage:
                                Console.WriteLine("enter ID of Package:");
                                int IDp;
                                IDp = int.Parse(Console.ReadLine());
                                DalObject.dalObject.DisplayPackage(IDp);
                                break;
                        }
                        break;
                    case Options.ViewTheLists:
                        Console.WriteLine("adding option:\n" +
                            " 0-Exit\n" +
                            " 1-Displays a list of base stations\n" +
                            " 2-Displays the list of skimmers\n" +
                            " 3-View customer list\n" +
                            " 4-Displays the list of packages\n" +
                            " 5-Displays a list of packages not yet associated with the glider\n" +
                            " 6-Display of base stations with available charging stations");
                        optionsListView = (OptionsListView)int.Parse(Console.ReadLine());
                        switch (optionsListView)
                        {
                            case OptionsListView.Exit:
                                return;
                            case OptionsListView.ViewBaseStation:
                                DalObject.dalObject.printBaseStation();
                                break;
                            case OptionsListView.ViewSkimmer:
                                DalObject.dalObject.printSkimmer();
                                break;
                            case OptionsListView.ViewClient:
                                DalObject.dalObject.printClient();
                                break;
                            case OptionsListView.ViewPackage:
                                DalObject.dalObject.printPackage();
                                break;
                            case OptionsListView.ViewUnassignedPackages:
                                DalObject.dalObject.printPackageWithoutSkimmer();
                                break;
                            case OptionsListView.ViewFreeBaseStation:
                                DalObject.dalObject.printBaseStationFreeCharging();
                                break;
                        }
                        break;
                }
            }
            while (options != 0);
        }

        static void Main(string[] args)
        {
            DalObject.DataSource.Initialize(); 
            Menu();

        }
    }

}
