using System;
using System.Collections.Generic;
using IBL.BO;
namespace ConsoleUI_BL
{
    class Program
    {

        enum Options { Exit, Add, Update, Display, ViewTheLists }
        enum InseitOption { Exit, AddBaseStation, AddSkimmer, AddClient, AddPackage }
        enum UpdateOption { Exit, Affiliation, Collection, Supply, SendLoading, ReleaseCharging }
        enum DisplayOptions { Exit, DisplayBaseStation, DisplaySkimmer, DisplayClient, DisplayPackage }
        enum OptionsListView { Exit, ViewBaseStation, ViewSkimmer, ViewClient, ViewPackage, ViewUnassignedPackages, ViewFreeBaseStation }

        static BlObject.BlObject mydal = new BlObject.BlObject();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "<Pending>")]
        private static void Menu()
        {
            Options options;
            InseitOption inseitOption;
            UpdateOption updateOption;
            OptionsListView optionsListView;
            DisplayOptions displayOptions;
            bool success;
            do
            {
                Console.WriteLine("welcome!" + "option:\n 0-Exit\n 1-Add\n 2-Update\n 3-Display\n 4-List View\n");
                options = (Options)int.Parse(Console.ReadLine());
                switch (options)
                {
                    case Options.Add:
                        Console.WriteLine("adding option:\n" +
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

                                int IDb;
                                Console.WriteLine("Station number:");
                                int.TryParse(Console.ReadLine(), out IDb);

                                string name;
                                Console.WriteLine("Station name:");
                                name = (Console.ReadLine());

                                double longitude, latitude;
                                Console.WriteLine("Enter longitude:");
                                double.TryParse(Console.ReadLine(), out longitude);

                                Console.WriteLine("Enter Latitude:");
                                double.TryParse(Console.ReadLine(), out latitude);
                                BaseStation newBaseStation = new BaseStation
                                {
                                    Id = IDb,
                                    Name = name,
                                    location = new Location { Latitude = latitude, Longitude = longitude },
                                    SeveralClaimPositionsVacant = 0,
                                    ListOfSkimmersCharge = new List<SkimmerInCharging>()
                                };
                                mydal.AddBaseStation(newBaseStation);
                                break;
                            case InseitOption.AddSkimmer:
                                //Quadocopter newQuadocopter = new Quadocopter();

                                int IDs;
                                Console.WriteLine("Serial number of the manufacturer:");
                                int.TryParse(Console.ReadLine(), out IDs);

                                string model;
                                Console.WriteLine("Skimmer model:");
                                model = (Console.ReadLine());

                                int weight;
                                Console.WriteLine("Maximum weight: 0-low,1-middle,2-heavy:");
                                int.TryParse(Console.ReadLine(), out weight);

                                int station;
                                Console.WriteLine("Station number Put the skimmer in it for initial charging");
                                int.TryParse(Console.ReadLine(), out station);

                                //newQuadocopter.Battery = 100;
                                //newQuadocopter.SkimmerMode = (DronStatuses)0;
                                Skimmer newSkimmer = new Skimmer
                                {
                                    Id = IDs,
                                    SkimmerModel = model,
                                    WeightCategory = (Weight)weight,
                                };
                                mydal.AddSkimmer(newSkimmer, station);

                            //                        //newQuadocopter.Battery = 100;
                            //                        //newQuadocopter.SkimmerMode = (DronStatuses)0;
                            //                        try
                            //                        {
                            //                            mydal.AddSkimmer(newQuadocopter);
                            //                        }
                            //                        catch (QuadocopterException exception)
                            //                        {
                            //                            Console.WriteLine(exception);
                            //                            throw;
                            //                        }

                            //                        break;
                            case InseitOption.AddClient:

                                int Idc;
                                Console.WriteLine("Enter unique ID number:");
                                int.TryParse(Console.ReadLine(), out Idc);

                                string Name;
                                Console.WriteLine("Enter the customer's name:");
                                Name = (Console.ReadLine());

                                string Phone;
                                Console.WriteLine("Enter Phone Number:");
                                Phone = (Console.ReadLine());

                                double Longitude;
                                Console.WriteLine("Enter longitude:");
                                double.TryParse(Console.ReadLine(), out Longitude);

                                double Latitude;
                                Console.WriteLine("Enter Latitude:");
                                double.TryParse(Console.ReadLine(), out Latitude);
                                Customer newCustomer = new Customer
                                {
                                    Id = Idc,
                                    Name = Name,
                                    Phone = Phone,
                                    Location = new Location { Latitude = Latitude, Longitude = Longitude },
                                    SentParcels = new List<CustomerInParcel>(),
                                    ReceiveParcels = new List<CustomerInParcel>()
                                };
                                mydal.AddCustomer(newCustomer);
                                break;
                                //try
                                //{
                                //    mydal.AddClient(newClient);
                                //}
                                //catch (ClientException exception)
                                //{
                                //    Console.WriteLine(exception);
                                //    throw;
                        }
                    case (Options)InseitOption.AddPackage:

                        int Idsc;
                        Console.WriteLine("Enter Sending customer ID:");
                        int.TryParse(Console.ReadLine(), out Idsc);

                        int Idgc;
                        Console.WriteLine("Receiving customer ID:");
                        int.TryParse(Console.ReadLine(), out Idgc);

                        int Weight;
                         Console.WriteLine("Enter Weight category 0-low,1-middle,2-heavy:");
                        int.TryParse(Console.ReadLine(), out Weight);

                        int Priorities;
                        Console.WriteLine("Enter priority 0-regular,1-fast,2-emergency:");
                        int.TryParse(Console.ReadLine(), out Priorities);
                        Package newPackage = new Package
                        {

                            SendPackage = Idsc,
                            ReceivesPackage = Idgc,
                            WeightCategory = (Weight)Weight,
                            priority = (Priority)Priorities,
                        };
                        mydal.AddPackage(newPackage);
                        break;

            //                        newPackage.IDSkimmerOperation = 0;
            //                        newPackage.PackageCreationTime = DateTime.Now;

                        //try
                        //{
                        //    mydal.AddPackage(newPackage);
                        //}
                        //catch (ClientException exception)
                        //{
                        //    Console.WriteLine(exception);
                        //    throw;
                        //}

                        break;
                }
                break;

                //            case Options.Update:
                //                Console.WriteLine("adding option:\n" +
                //                    " 0-Exit\n" +
                //                    " 1-Assign a package to a skimmer\n" +
                //                    " 2-Package collection by skimmer\n" +
                //                    " 3-ADelivery package to customer\n" +
                //                    " 4-Sending a skimmer for charging at a base station\n" +
                //                    " 5-Release skimmer from charging at base station");
                //                updateOption = (UpdateOption)int.Parse(Console.ReadLine());
                //                switch (updateOption)
                //                {
                //                    case UpdateOption.Exit:
                //                        return;
                //                    //Assign a package to a skimmer
                //                    case UpdateOption.Affiliation:
                //                        int idp, idq;
                //                        Console.WriteLine("enter ID of Package:");
                //                        idp = int.Parse(Console.ReadLine());
                //                        Console.WriteLine("enter ID of skimmers:");
                //                        idq = int.Parse(Console.ReadLine());
                //                        try
                //                        {
                //                            mydal.AssignPackageSkimmer(idp, idq);
                //                        }
                //                        catch (Exception exception)
                //                        {
                //                            Console.WriteLine(exception);
                //                            throw;
                //                        }
                //                        break;
                //                    //Package collection by skimmer
                //                    case UpdateOption.Collection:
                //                        Console.WriteLine("enter ID of Package:");
                //                        int id = int.Parse(Console.ReadLine());
                //                        try
                //                        {
                //                            mydal.CollectionPackage(id);
                //                        }
                //                        catch (Exception exception)
                //                        {
                //                            Console.WriteLine(exception);
                //                            throw;
                //                        }
                //                        break;
                //                    //Delivery package to customer
                //                    case UpdateOption.Supply:
                //                        Console.WriteLine("enter ID of Package:");
                //                        id = int.Parse(Console.ReadLine());
                //                        try
                //                        {
                //                            mydal.PackageDelivery(id);
                //                        }
                //                        catch (Exception exception)
                //                        {
                //                            Console.WriteLine(exception);
                //                            throw;
                //                        }
                //                        break;
                //                    // Sending a skimmer for charging at a base station
                //                    case UpdateOption.SendLoading:
                //                        Console.WriteLine("enter ID of skimmers:");
                //                        id = int.Parse(Console.ReadLine());
                //                        Console.WriteLine("Select a base station from the displayed stations and enter its ID number:");
                //                        foreach (var item in mydal.BaseStationFreeCharging())
                //                        {
                //                            Console.WriteLine(item);
                //                        }
                //                        int idBS = int.Parse(Console.ReadLine());
                //                        try
                //                        {
                //                            mydal.SendingSkimmerForCharging(id, idBS);
                //                        }
                //                        catch (Exception exception)
                //                        {
                //                            Console.WriteLine(exception);
                //                            throw;
                //                        }
                //                        break;
                //                    //Release skimmer from charging at base station
                //                    case UpdateOption.ReleaseCharging:
                //                        Console.WriteLine("enter ID of skimmers:");
                //                        id = int.Parse(Console.ReadLine());
                //                        Console.WriteLine("enter ID of Base Station:");
                //                        idq = int.Parse(Console.ReadLine());
                //                        try
                //                        {
                //                            mydal.SkimmerRelease(id, idq);
                //                        }
                //                        catch (Exception exception)
                //                        {
                //                            Console.WriteLine(exception);
                //                            throw;
                //                        }
                //                        break;

                //                }
                //                break;
                //            case Options.Display:
                //                Console.WriteLine("adding option:\n" +
                //                    " 0-Exit\n" +
                //                    " 1-Base Station View\n" +
                //                    " 2-Skimmer display\n" +
                //                    " 3-Customer view\n" +
                //                    " 4-Package view\n");
                //                displayOptions = (DisplayOptions)int.Parse(Console.ReadLine());
                //                switch (displayOptions)
                //                {
                //                    case DisplayOptions.Exit:
                //                        return;
                //                    case DisplayOptions.DisplayBaseStation:
                //                        int IDb;
                //                        do
                //                        {
                //                            Console.WriteLine("enter ID of BaseStation:");
                //                            success = int.TryParse(Console.ReadLine(), out IDb);

                //                        } while (success == false);
                //                        try
                //                        {
                //                            Console.WriteLine(mydal.GetBaseStation(IDb));
                //                        }
                //                        catch (BaseStationException Exception)
                //                        {
                //                            Console.WriteLine(Exception);
                //                            throw;
                //                        }

                //                        break;
                //                    case DisplayOptions.DisplaySkimmer:
                //                        int IDq;
                //                        do
                //                        {
                //                            Console.WriteLine("enter ID of Skimmer:");
                //                            success = int.TryParse(Console.ReadLine(), out IDq);
                //                        } while (success == false);
                //                        try
                //                        {
                //                            Console.WriteLine(mydal.GetQuadrocopter(IDq));
                //                        }
                //                        catch (QuadocopterException Exception)
                //                        {
                //                            Console.WriteLine(Exception);
                //                            throw;
                //                        }

                //                        break;
                //                    case DisplayOptions.DisplayClient:
                //                        int IDc;
                //                        do
                //                        {
                //                            Console.WriteLine("enter ID of Client:");
                //                            success = int.TryParse(Console.ReadLine(), out IDc);
                //                        } while (success == false);
                //                        try
                //                        {
                //                            Console.WriteLine(mydal.GetClient(IDc));
                //                        }
                //                        catch (ClientException Exception)
                //                        {
                //                            Console.WriteLine(Exception);
                //                            throw;
                //                        }

                //                        break;
                //                    case DisplayOptions.DisplayPackage:
                //                        int IDp;
                //                        do
                //                        {
                //                            Console.WriteLine("enter ID of Package:");
                //                            success = int.TryParse(Console.ReadLine(), out IDp);
                //                        } while (success == false);
                //                        try
                //                        {
                //                            Console.WriteLine(mydal.GetPackage(IDp));
                //                        }
                //                        catch (PackageException Exception)
                //                        {
                //                            Console.WriteLine(Exception);
                //                            throw;
                //                        }

                //                        break;
                //                }
                //                break;
                //            case Options.ViewTheLists:
                //                Console.WriteLine("adding option:\n" +
                //                    " 0-Exit\n" +
                //                    " 1-Displays a list of base stations\n" +
                //                    " 2-Displays the list of skimmers\n" +
                //                    " 3-View customer list\n" +
                //                    " 4-Displays the list of packages\n" +
                //                    " 5-Displays a list of packages not yet associated with the glider\n" +
                //                    " 6-Display of base stations with available charging stations");
                //                optionsListView = (OptionsListView)int.Parse(Console.ReadLine());
                //                switch (optionsListView)
                //                {
                //                    case OptionsListView.Exit:
                //                        return;
                //                    case OptionsListView.ViewBaseStation:
                //                        foreach (BaseStation item in mydal.GetBaseStationList())
                //                        {
                //                            Console.WriteLine(item);
                //                        }
                //                        break;
                //                    case OptionsListView.ViewSkimmer:
                //                        foreach (Quadocopter item in mydal.GetQuadocopterList())
                //                        {
                //                            Console.WriteLine(item);
                //                        }
                //                        break;
                //                    case OptionsListView.ViewClient:
                //                        foreach (Client item in mydal.GetClientList())
                //                        {
                //                            Console.WriteLine(item);
                //                        }
                //                        break;
                //                    case OptionsListView.ViewPackage:
                //                        foreach (Package item in mydal.GetPackageList())
                //                        {
                //                            Console.WriteLine(item);
                //                        }
                //                        break;
                //                    case OptionsListView.ViewUnassignedPackages:
                //                        foreach (var item in mydal.PackagesWithoutSkimmer())
                //                        {
                //                            Console.WriteLine(item);
                //                        }
                //                        break;
                //                    case OptionsListView.ViewFreeBaseStation:
                //                        foreach (var item in mydal.BaseStationFreeCharging())
                //                        {
                //                            Console.WriteLine(item);
                //                        }
                //                        break;
                //                }
                //                break;
                //        }
                //    }
                //    while (options != 0);
                //}


















                //static BL.BlObject mydal = new BL.BlObject();

                //static IBL.BO.IBL bl = new BL.BlObject();
                //static void Main(string[] args)
                //{
                //    Console.WriteLine("Hell O World!");


                //    Customer customer;
                //    Console.WriteLine("give me  customer id");
                //    int id = Int32.Parse(Console.ReadLine());
                //    try
                //    {
                //        customer = bl.GetCustomer(id);
                //    }
                //    catch (CustomerException exception)
                //    {
                //        Console.WriteLine(exception.Message);
                //    }
                //    Console.WriteLine();
                //}
            }
    }
}