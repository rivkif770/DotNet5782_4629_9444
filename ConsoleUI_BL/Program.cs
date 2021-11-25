using System;
using System.Collections.Generic;
using IBL.BO;
namespace ConsoleUI_BL
{
    class Program
    {

        enum Options { Exit, Add, Update, Display, ViewTheLists }
        enum InseitOption { Exit, AddBaseStation, AddSkimmer, AddCustomer, AddPackage }
        enum UpdateOption { Exit, UpdateSkimmerName, UpdateBaseStation, UpdateCustomerData, SendingSkimmerForCharging, ReleaseSkimmerFromCharging, AssigningPackageToSkimmer, CollectingPackageBySkimmer, DeliveryOfPackageBySkimmer }
        enum DisplayOptions { Exit, DisplayBaseStation, DisplaySkimmer, DisplayCustomer, DisplayPackage }
        enum OptionsListView { Exit, ViewBaseStation, ViewSkimmer, ViewCustomer, ViewPackage, ViewUnassignedPackages, ViewFreeBaseStation }

        static BL.BL myBL = new BL.BL();

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
                                    Location = new Location { Latitude = latitude, Longitude = longitude },
                                    SeveralClaimPositionsVacant = 0,
                                    ListOfSkimmersCharge = new List<SkimmerInCharging>()
                                };
                                myBL.AddBaseStation(newBaseStation);
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

                                Skimmer newSkimmer = new Skimmer
                                {
                                    Id = IDs,
                                    SkimmerModel = model,
                                    WeightCategory = (Weight)weight,
                                };
                                myBL.AddSkimmer(newSkimmer, station);

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

                                break;
                            case InseitOption.AddCustomer:

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
                                };
                                myBL.AddCustomer(newCustomer);
                                break;
                            //try
                            //{
                            //    mydal.AddClient(newClient);
                            //}
                            //catch (ClientException exception)
                            //{
                            //    Console.WriteLine(exception);
                            //    throw;
                            case InseitOption.AddPackage:

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

                                    SendPackage = new CustomerInParcel
                                    {
                                        Id = Idsc
                                    },
                                    ReceivesPackage = new CustomerInParcel
                                    {
                                        Id = Idgc
                                    },
                                    WeightCategory = (Weight)Weight,
                                    priority = (Priority)Priorities,
                                };
                                myBL.AddPackage(newPackage);
                                break;

                                //newPackage.IDSkimmerOperation = 0;
                                //newPackage.PackageCreationTime = DateTime.Now;

                                //try
                                //{
                                //    mydal.AddPackage(newPackage);
                                //}
                                //catch (ClientException exception)
                                //{
                                //    Console.WriteLine(exception);
                                //    throw;
                                //}
                        }
                        break;
                    case Options.Update:
                        Console.WriteLine("adding option:\n" +
                            " 0-Exit\n" +
                            " 1-Updated skimmer name\n" +
                            " 2-Update station data\n" +
                            " 3-Update customer data\n" +
                            " 4-Sending a skimmer for charging at a base station\n" +
                            " 5-Release skimmer from charging\n" +
                            " 6-Assigning a package to a skimmer\n" +
                            " 7-Collecting a package by skimmer\n" +
                            " 8-Delivery of a package by skimmer\n");
                        updateOption = (UpdateOption)int.Parse(Console.ReadLine());
                        switch (updateOption)
                        {
                            case UpdateOption.Exit:
                                return;
                            //Update skimmer name
                            case UpdateOption.UpdateSkimmerName:
                                int ids;
                                string nameS;
                                Console.WriteLine("enter ID of skimmer:");
                                ids = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter a new name:");
                                nameS = Console.ReadLine();
                                try
                                {
                                    myBL.UpdateSkimmerName(ids, nameS);
                                }
                                catch (IdDoesNotExistExceptionBL exception)
                                {
                                    Console.WriteLine(exception);
                                }
                                break;
                            //Update station data
                            case UpdateOption.UpdateBaseStation:
                                string nameB, NumberOfChargingStations;
                                Console.WriteLine("enter ID of BaseStation:");
                                int idb = int.Parse(Console.ReadLine());
                                Console.WriteLine("Station name:");
                                nameB = Console.ReadLine();
                                Console.WriteLine("Total amount of charging stations:");
                                NumberOfChargingStations = Console.ReadLine();
                                try
                                {
                                    myBL.UpdateBaseStation(idb, nameB, NumberOfChargingStations);
                                }
                                catch (IdDoesNotExistExceptionBL exception)
                                {
                                    Console.WriteLine(exception);
                                }
                                break;
                            //Update customer data
                            case UpdateOption.UpdateCustomerData:
                                string nameC, phoneC;
                                Console.WriteLine("enter ID of Customer:");
                                int idc = int.Parse(Console.ReadLine());
                                Console.WriteLine("Customer name:");
                                nameC = Console.ReadLine();
                                Console.WriteLine("New phone number:");
                                phoneC = Console.ReadLine();
                                try
                                {
                                    myBL.UpdateCustomerData(idc, nameC, phoneC);
                                }
                                catch (IdDoesNotExistExceptionBL exception)
                                {
                                    Console.WriteLine(exception);
                                    throw;
                                }
                                break;
                            // Sending a skimmer for charging at a base station
                            case UpdateOption.SendingSkimmerForCharging:
                                Console.WriteLine("enter ID of skimmers:");
                                ids = int.Parse(Console.ReadLine());
                                try
                                {
                                    myBL.SendingSkimmerForCharging(ids);
                                }
                                catch (Exception exception)
                                {
                                    Console.WriteLine(exception);
                                    throw;
                                }
                                break;
                            // Release skimmer from charging
                            case UpdateOption.ReleaseSkimmerFromCharging:
                                double ChargingTime;
                                Console.WriteLine("enter ID of Skimmer:");
                                ids = int.Parse(Console.ReadLine());
                                Console.WriteLine("Charging time:");
                                ChargingTime = double.Parse(Console.ReadLine());
                                try
                                {
                                    myBL.ReleaseSkimmerFromCharging(ids, ChargingTime);
                                }
                                catch (IdDoesNotExistExceptionBL exception)
                                {
                                    Console.WriteLine(exception);
                                    throw;
                                }
                                break;

                            //Assigning a package to a skimmer
                            case UpdateOption.AssigningPackageToSkimmer:
                                Console.WriteLine("enter ID of skimmers:");
                                ids = int.Parse(Console.ReadLine());
                                try
                                {
                                    myBL.AssigningPackageToSkimmer(ids);
                                }
                                catch (IdDoesNotExistExceptionBL exception)
                                {
                                    Console.WriteLine(exception);
                                    throw;
                                }
                                break;
                            //Collecting a package by skimmer
                            case UpdateOption.CollectingPackageBySkimmer:
                                Console.WriteLine("enter ID of skimmers:");
                                ids = int.Parse(Console.ReadLine());
                                try
                                {
                                    myBL.CollectingPackageBySkimmer(ids);
                                }
                                catch (Exception exception)
                                {
                                    Console.WriteLine(exception);
                                    throw;
                                }
                                break;
                            //Delivery of a package by skimmer
                            case UpdateOption.DeliveryOfPackageBySkimmer:
                                Console.WriteLine("enter ID of skimmers:");
                                ids = int.Parse(Console.ReadLine());
                                try
                                {
                                    myBL.DeliveryOfPackageBySkimmer(ids);
                                }
                                catch (IdDoesNotExistExceptionBL exception)
                                {
                                    Console.WriteLine(exception);
                                    throw;
                                }
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
                                int IDb;
                                Console.WriteLine("enter ID of BaseStation:");
                                success = int.TryParse(Console.ReadLine(), out IDb);
                                try
                                {
                                    Console.WriteLine( myBL.GetBeseStation(IDb));
                                }
                                catch (BaseStationException Exception)
                                {
                                    Console.WriteLine(Exception);
                                    throw;
                                }
                                break;
                            case DisplayOptions.DisplaySkimmer:
                                int IDq;
                                 Console.WriteLine("enter ID of Skimmer:");
                                 success = int.TryParse(Console.ReadLine(), out IDq);
                                try
                                {
                                    Console.WriteLine(myBL.GetSkimmer(IDq));
                                }
                                catch (QuadocopterException Exception)
                                {
                                    Console.WriteLine(Exception);
                                    throw;
                                }
                                break;
                            case DisplayOptions.DisplayCustomer:
                                int IDc;
                                Console.WriteLine("enter ID of Client:");
                                success = int.TryParse(Console.ReadLine(), out IDc);
                                try
                                {
                                    Console.WriteLine(myBL.GetCustomer(IDc));
                                }
                                catch (ClientException Exception)
                                {
                                    Console.WriteLine(Exception);
                                    throw;
                                }
                                break;
                            case DisplayOptions.DisplayPackage:
                                int IDp;
                                Console.WriteLine("enter ID of Package:");
                                success = int.TryParse(Console.ReadLine(), out IDp);
                                try
                                {
                                    Console.WriteLine(myBL.GetPackage(IDp));
                                }
                                catch (PackageException Exception)
                                {
                                    Console.WriteLine(Exception);
                                    throw;
                                }
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
                                foreach (BaseStationToList item in myBL.GetBaseStationList())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case OptionsListView.ViewSkimmer:
                                foreach (SkimmerToList item in myBL.GetSkimmerList())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case OptionsListView.ViewCustomer:
                                foreach (CustomerToList item in myBL.GetCustomerList())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case OptionsListView.ViewPackage:
                                foreach (PackageToList item in myBL.GetPackageList())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case OptionsListView.ViewUnassignedPackages:
                                foreach (var item in myBL.GetPackagesWithoutSkimmer())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case OptionsListView.ViewFreeBaseStation:
                                foreach (var item in myBL.GetBaseStationFreeCharging())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                        }
                        break;
                }
            }
            while (options != 0);
        }


















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
        // }
    }                
}