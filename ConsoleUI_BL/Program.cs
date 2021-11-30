﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;

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
                try
                {
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
                                    do
                                    {
                                        Console.WriteLine("Station number:");
                                        success = int.TryParse(Console.ReadLine(), out IDb);
                                    } while (!success || IDb < 999 || IDb > 10000);

                                    string name;
                                    Console.WriteLine("Station name:");
                                    name = (Console.ReadLine());

                                    double longitude, latitude;
                                    do
                                    {
                                        Console.WriteLine("Enter longitude:");
                                        success = double.TryParse(Console.ReadLine(), out longitude);
                                    } while (!success || longitude < -50 || longitude > 50);

                                    do
                                    {
                                        Console.WriteLine("Enter Latitude:");
                                        success = double.TryParse(Console.ReadLine(), out latitude);
                                    } while (!success || latitude < -50 || latitude > 50);

                                    int num;
                                    do
                                    {
                                        Console.WriteLine("Number of charging stations:");
                                        success = int.TryParse(Console.ReadLine(), out num);
                                    } while (!success || num > 1000 || num < 0);
;
                                    IBL.BO.BaseStation newBaseStation = new IBL.BO.BaseStation
                                    {
                                        Id = IDb,
                                        Name = name,
                                        Location = new Location { Latitude = latitude, Longitude = longitude },
                                        SeveralClaimPositionsVacant = num,

                                        ListOfSkimmersCharge = new List<SkimmerInCharging>()
                                    };
                                    myBL.AddBaseStation(newBaseStation);
                                    //try
                                    //{
                                    //    myBL.AddBaseStation(newBaseStation);
                                    //}
                                    //catch (ExistsInSystemExceptionBL Exception)
                                    //{
                                    //    Console.WriteLine(Exception.Message);
                                    //}
                                    break;
                                case InseitOption.AddSkimmer:
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
                                    //try
                                    //{
                                    //    myBL.AddSkimmer(newSkimmer, station);
                                    //}
                                    //catch (ExistsInSystemExceptionBL exception)
                                    //{
                                    //    Console.WriteLine(exception.Message);
                                    //}

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
                                    //try
                                    //{
                                    //    myBL.AddCustomer(newCustomer);
                                    //}
                                    //catch (ExistsInSystemExceptionBL exception)
                                    //{
                                    //    Console.WriteLine(exception);
                                    //}
                                    break;
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
                                    IBL.BO.Package newPackage = new IBL.BO.Package
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
                                    //try
                                    //{
                                    //    myBL.AddPackage(newPackage);
                                    //}
                                    //catch (ExistsInSystemExceptionBL exception)
                                    //{
                                    //    Console.WriteLine(exception);
                                    //}
                                    break;
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
                                    do
                                    {
                                        Console.WriteLine("enter ID of skimmers:");
                                        success = int.TryParse(Console.ReadLine(), out ids);
                                    } while (!success || ids < 99 || ids > 1000);
                                    Console.WriteLine("Enter a new name:");
                                    nameS = Console.ReadLine();
                                    myBL.UpdateSkimmerName(ids, nameS);
                                    //try
                                    //{
                                    //    myBL.UpdateSkimmerName(ids, nameS);
                                    //}
                                    //catch (IdDoesNotExistExceptionBL exception)
                                    //{
                                    //    Console.WriteLine(exception);
                                    //}
                                    break;
                                //Update station data
                                case UpdateOption.UpdateBaseStation:
                                    string nameB, NumberOfChargingStations;
                                    int idb, NumberOfChargingStations1 = 0;
                                    do
                                    {
                                        Console.WriteLine("enter ID of BaseStation:");
                                        success = int.TryParse(Console.ReadLine(), out idb);
                                    } while (!success || idb < 999 || idb > 10000);
                                    
                                    Console.WriteLine("Station name:");
                                    nameB = Console.ReadLine();

                                    do
                                    {
                                        Console.WriteLine("Total amount of charging stations:");
                                        NumberOfChargingStations = Console.ReadLine();
                                        if (NumberOfChargingStations == "")
                                            success = true;
                                        else
                                            success = int.TryParse(Console.ReadLine(), out NumberOfChargingStations1);
                                    } while (!success || NumberOfChargingStations != "" && NumberOfChargingStations1 <= 0);
                                    myBL.UpdateBaseStation(idb, nameB, NumberOfChargingStations);
                                    //try
                                    //{
                                    //    myBL.UpdateBaseStation(idb, nameB, NumberOfChargingStations);
                                    //}
                                    //catch (IdDoesNotExistExceptionBL exception)
                                    //{
                                    //    Console.WriteLine(exception);
                                    //}
                                    break;
                                //Update customer data
                                case UpdateOption.UpdateCustomerData:
                                    string nameC, phoneC;
                                    int idc, numPhoneC;
                                    do
                                    {
                                        Console.WriteLine("enter ID of Customer:");
                                        success = int.TryParse(Console.ReadLine(), out idc);
                                    } while (!success || idc < 99999999 || idc > 1000000000);
                                    
                                    Console.WriteLine("Customer name:");
                                    nameC = Console.ReadLine();
                                    do
                                    {
                                        Console.WriteLine("New phone number:");
                                        phoneC = Console.ReadLine();
                                        numPhoneC = phoneC.Count();
                                    } while (numPhoneC < 10);
                                    myBL.UpdateCustomerData(idc, nameC, phoneC);
                                    //try
                                    //{
                                    //    myBL.UpdateCustomerData(idc, nameC, phoneC);
                                    //}
                                    //catch (IdDoesNotExistExceptionBL exception)
                                    //{
                                    //    Console.WriteLine(exception);
                                    //    throw;
                                    //}
                                    break;
                                // Sending a skimmer for charging at a base station
                                case UpdateOption.SendingSkimmerForCharging:
                                    do
                                    {
                                        Console.WriteLine("enter ID of skimmers:");
                                        success = int.TryParse(Console.ReadLine(), out ids);
                                    } while (!success || ids < 99 || ids > 1000);
                                    myBL.SendingSkimmerForCharging(ids);
                                    //try
                                    //{
                                    //    myBL.SendingSkimmerForCharging(ids);
                                    //}
                                    //catch (IdDoesNotExistExceptionBL exception)
                                    //{
                                    //    Console.WriteLine(exception);
                                    //    throw;
                                    //}
                                    break;
                                // Release skimmer from charging
                                case UpdateOption.ReleaseSkimmerFromCharging:
                                    double ChargingTime;
                                    do
                                    {
                                        Console.WriteLine("enter ID of skimmers:");
                                        success = int.TryParse(Console.ReadLine(), out ids);
                                    } while (!success || ids < 99 || ids > 1000);
                                    do
                                    {
                                        Console.WriteLine("Charging time:");
                                        success = double.TryParse(Console.ReadLine(), out ChargingTime);
                                    } while (!success || ChargingTime < 0);
                                    myBL.ReleaseSkimmerFromCharging(ids, ChargingTime);
                                    //try
                                    //{
                                    //    myBL.ReleaseSkimmerFromCharging(ids, ChargingTime);
                                    //}
                                    //catch (IdDoesNotExistExceptionBL exception)
                                    //{
                                    //    Console.WriteLine(exception);
                                    //}
                                    break;

                                //Assigning a package to a skimmer
                                case UpdateOption.AssigningPackageToSkimmer:
                                    do
                                    {
                                        Console.WriteLine("enter ID of skimmers:");
                                        success = int.TryParse(Console.ReadLine(), out ids);
                                    } while (!success || ids < 99 || ids > 1000);
                                    myBL.AssigningPackageToSkimmer(ids);
                                    //try
                                    //{
                                    //    myBL.AssigningPackageToSkimmer(ids);
                                    //}
                                    //catch (Exception exception)
                                    //{
                                    //    Console.WriteLine(exception);
                                    //}
                                    break;
                                //Collecting a package by skimmer
                                case UpdateOption.CollectingPackageBySkimmer:
                                    do
                                    {
                                        Console.WriteLine("enter ID of skimmers:");
                                        success = int.TryParse(Console.ReadLine(), out ids);
                                    } while (!success || ids < 99 || ids > 1000);
                                    myBL.CollectingPackageBySkimmer(ids);
                                    //try
                                    //{
                                    //    myBL.CollectingPackageBySkimmer(ids);
                                    //}
                                    //catch (Exception exception)
                                    //{
                                    //    Console.WriteLine(exception);
                                    //    throw;
                                    //}
                                    break;
                                //Delivery of a package by skimmer
                                case UpdateOption.DeliveryOfPackageBySkimmer:
                                    do
                                    {
                                        Console.WriteLine("enter ID of skimmers:");
                                        success = int.TryParse(Console.ReadLine(), out ids);
                                    } while (!success || ids < 99 || ids > 1000);
                                    myBL.DeliveryOfPackageBySkimmer(ids);
                                    //try
                                    //{
                                    //    myBL.DeliveryOfPackageBySkimmer(ids);
                                    //}
                                    //catch (Exception exception)
                                    //{
                                    //    Console.WriteLine(exception);
                                    //}
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
                                    do
                                    {
                                        Console.WriteLine("enter ID of BaseStation:");
                                        success = int.TryParse(Console.ReadLine(), out IDb);
                                    } while (!success || IDb < 999 || IDb > 10000);
                                    Console.WriteLine(myBL.GetBeseStation(IDb));
                                    //try
                                    //{
                                    //    Console.WriteLine(myBL.GetBeseStation(IDb));
                                    //}
                                    //catch (IdDoesNotExistExceptionBL Exception)
                                    //{
                                    //    Console.WriteLine(Exception);
                                    //}
                                    break;
                                case DisplayOptions.DisplaySkimmer:
                                    int IDq;
                                    do
                                    {
                                        Console.WriteLine("enter ID of Skimmer:");
                                        success = int.TryParse(Console.ReadLine(), out IDq);
                                    } while (!success || IDq < 99 || IDq > 1000);
                                    Console.WriteLine(myBL.GetSkimmer(IDq));
                                    //try
                                    //{
                                    //    Console.WriteLine(myBL.GetSkimmer(IDq));
                                    //}
                                    //catch (IdDoesNotExistExceptionBL Exception)
                                    //{
                                    //    Console.WriteLine(Exception);
                                    //}
                                    break;
                                case DisplayOptions.DisplayCustomer:
                                    int IDc;
                                    do
                                    {
                                        Console.WriteLine("enter ID of Client:");
                                        success = int.TryParse(Console.ReadLine(), out IDc);
                                    } while (!success || IDc < 99999999 || IDc > 1000000000);
                                    Console.WriteLine(myBL.GetCustomer(IDc));
                                    //try
                                    //{
                                    //    Console.WriteLine(myBL.GetCustomer(IDc));
                                    //}
                                    //catch (IdDoesNotExistExceptionBL Exception)
                                    //{
                                    //    Console.WriteLine(Exception);
                                    //}
                                    break;
                                case DisplayOptions.DisplayPackage:
                                    int IDp;
                                    do
                                    {
                                        Console.WriteLine("enter ID of Package:");
                                        success = int.TryParse(Console.ReadLine(), out IDp);
                                    } while (!success || IDp < 1000);
                                    Console.WriteLine(myBL.GetPackage(IDp));
                                    //try
                                    //{
                                    //    Console.WriteLine(myBL.GetPackage(IDp));
                                    //}
                                    //catch (IdDoesNotExistExceptionBL Exception)
                                    //{
                                    //    Console.WriteLine(Exception);
                                    //}
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
                catch (SkimmerExceptionBL Exception)
                {
                    Console.WriteLine(Exception.Message);
                }
                catch (ExistsInSystemExceptionBL Exception)
                {
                    Console.WriteLine(Exception.Message);
                }
                catch (IdDoesNotExistExceptionBL Exception)
                {
                    Console.WriteLine(Exception.Message);
                }

            }
            while (options != 0);
        }
        static void Main(string[] args)
        {
            Menu();
        }
    }                
}