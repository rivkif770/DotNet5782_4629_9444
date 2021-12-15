﻿using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;

namespace BL
{
    public partial class BL : BlApi.IBL
    {
        static readonly IBL instance = new BL();
        internal static IBL Instance { get { return instance; } }
        static BL() { }

        static Random r ;
        public DalApi.IDal mayDal;
        private List<SkimmerToList> skimmersList;
        public double Free;
        public double LightWeightCarrier;
        public double MediumWeightCarrier;
        public double HeavyWeightCarrier;
        public double SkimmerLoadingRate;
        
        public BL()
        {
            r = new Random();
            skimmersList = new List<SkimmerToList>();
            mayDal = DalApi.DalFactory.GetDal("List");
            //Array with power consumption data by weight and by charge
            double[] vs = mayDal.PowerConsumptionRequest();
            Free = vs[0];
            LightWeightCarrier = vs[1];
            MediumWeightCarrier = vs[2];
            HeavyWeightCarrier = vs[3];
            SkimmerLoadingRate = vs[4];

            SkimmerUpdates(skimmersList);
        }
        /// <summary>
        /// Skimmer Updates
        /// </summary>
        private void SkimmerUpdates(List<SkimmerToList>  lst)
        {
            //For each skimmer from the skimmer list
            foreach (Quadocopter item in mayDal.GetQuadocopterList())
            {
                SkimmerToList updatedSkimmer = new SkimmerToList
                {
                    Id = item.IDNumber,
                    SkimmerModel=item.SkimmerModel,
                    WeightCategory=(Weight)item.Weight                
                };
                //Finding a glider-related package
                DO.Package PackageAssociatedWithSkimmer = FindingPackageAssociatedWithGlider(item);
                //If there is a package that has not yet been delivered but the skimmer is already associated
                if (PackageAssociatedWithSkimmer.TimeArrivalRecipient == null && PackageAssociatedWithSkimmer.ID != 0)
                {
                    //Update skimmer status to shipping status
                    updatedSkimmer.SkimmerStatus = SkimmerStatuses.shipping;
                    updatedSkimmer.PackageNumberTransferred = PackageAssociatedWithSkimmer.ID;
                    //If the package was associated but not collected 	ConsoleUI_BL.dll!ConsoleUI_BL.Program.Menu() Line 69	C#

                    if (PackageAssociatedWithSkimmer.PackageCollectionTime == null)
                    {
                        //Location will be at the station closest to the sender
                        updatedSkimmer.CurrentLocation = ChecksSmallDistanceBetweenCustomerAndBaseStation(GetCustomer(PackageAssociatedWithSkimmer.IDSender)).Location;
                    }
                    //The position of the skimmer will be at the position of the sender
                    if(PackageAssociatedWithSkimmer.PackageCollectionTime!=null && PackageAssociatedWithSkimmer.TimeArrivalRecipient==null)
                    {
                        //Updates skimmer location to package shipper location.
                        updatedSkimmer.CurrentLocation = GetCustomer(PackageAssociatedWithSkimmer.IDSender).Location;
                    }
                    //DO.Client sendCustomer= mayDal.GetClient(PackageAssociatedWithSkimmer.IDSender);
                    //DO.Client getCustomer = mayDal.GetClient(PackageAssociatedWithSkimmer.IDgets);
                    BO.Customer sendCustomer = GetCustomer(PackageAssociatedWithSkimmer.IDSender);
                    DO.Client tempS = new DO.Client
                    {
                        ID = sendCustomer.Id,
                        Name = sendCustomer.Name,
                        Telephone = sendCustomer.Phone,
                        Latitude = sendCustomer.Location.Latitude,
                        Longitude = sendCustomer.Location.Longitude
                    };
                    BO.Customer getCustomer = GetCustomer(PackageAssociatedWithSkimmer.IDgets);
                    DO.Client tempG = new DO.Client
                    {
                        ID = getCustomer.Id,
                        Name = getCustomer.Name,
                        Telephone = getCustomer.Phone,
                        Latitude = getCustomer.Location.Latitude,
                        Longitude = getCustomer.Location.Longitude
                    };
                    double minBattery = MinimalLoadingPerformTheShipmentAndArriveForLoading(updatedSkimmer, tempS, tempG);
                    updatedSkimmer.BatteryStatus = (double)r.Next((int)minBattery, 100);
                }

                //If the glider does not ship, its condition will be raffled off between maintenance and disposal
                if (PackageAssociatedWithSkimmer.ID == 0)
                {
                    updatedSkimmer.SkimmerStatus = (SkimmerStatuses)(r.Next(2));
                }
                //If the skimmer is in maintenance, Its location will be raffled between the existing stations and Battery status will be raffled between 0% and 20%
                if (updatedSkimmer.SkimmerStatus == SkimmerStatuses.maintenance)
                {
                    int counts = GetBaseStationList().Count();
                    List<DO.BaseStation> B = new List<DO.BaseStation>();
                    foreach (DO.BaseStation item1 in mayDal.GetBaseStationList())
                    {
                        B.Add(new DO.BaseStation
                        {
                            UniqueID = item1.UniqueID,
                            StationName = item1.StationName,
                            SeveralPositionsArgument = item1.SeveralPositionsArgument,
                            Latitude = item1.Latitude,
                            Longitude = item1.Longitude
                        });
                    }
                    DO.BaseStation baseStation = B[r.Next(counts)];
                    updatedSkimmer.CurrentLocation = new Location { Latitude = baseStation.Latitude, Longitude = baseStation.Longitude };
                    baseStation.SeveralPositionsArgument = baseStation.SeveralPositionsArgument--;
                    mayDal.UpadteB(baseStation);
                    SkimmerLoading skimmerLoading = new SkimmerLoading { SkimmerID = updatedSkimmer.Id, StationID = baseStation.UniqueID };
                    mayDal.AddSkimmerLoading(skimmerLoading);
                    updatedSkimmer.BatteryStatus = r.Next(21);
                }
                // If the skimmer is available
                if (updatedSkimmer.SkimmerStatus == SkimmerStatuses.free)
                {
                    updatedSkimmer.CurrentLocation = SkimmerLocationAvailable();
                    double minBattery = MinimalChargeToGetToTheNearestStation(updatedSkimmer);
                    updatedSkimmer.BatteryStatus = (double)r.Next((int)minBattery, 100);
                    
                }
                lst.Add(updatedSkimmer);
            }

        }
        /// <summary>
        /// Calculate a minimum charge that will allow him to reach the nearest station
        /// </summary>
        /// <param name="updatedSkimmer"></param>
        /// <returns></returns>
        private double MinimalChargeToGetToTheNearestStation(SkimmerToList updatedSkimmer)
        {             
            BO.BaseStation baseStation = ChecksSmallDistanceBetweenSkimmerAndBaseStation(updatedSkimmer);
            double distance =Tools.Utils.GetDistance(baseStation.Location.Longitude, baseStation.Location.Latitude, updatedSkimmer.CurrentLocation.Longitude, updatedSkimmer.CurrentLocation.Latitude);
            double minimalCharge = (distance * Free);
            return minimalCharge;
        }
        /// <summary>
        /// Minimum Charge  to get to the package
        /// </summary>
        /// <param name="updatedSkimmer"></param>
        /// <param name="senderClient"></param>
        /// <returns></returns>
        private double MinimumPaymentToGetToThePackage(SkimmerToList updatedSkimmer, DO.Client senderClient)
        {
            double distance = Tools.Utils.GetDistance(senderClient.Longitude, senderClient.Latitude, updatedSkimmer.CurrentLocation.Longitude, updatedSkimmer.CurrentLocation.Latitude);
            double minimalCharge = (distance * Free);
            return minimalCharge;
        }
        /// <summary>
        /// Calculation of the minimum charge that will allow the glider to make the shipment and arrive at the station closest to the destination of the shipment
        /// </summary>
        /// <param name="updatedSkimmer"></param>
        /// <returns></returns>
        private double MinimalLoadingPerformTheShipmentAndArriveForLoading (SkimmerToList updatedSkimmer, DO.Client senderClient, DO.Client getClient)
        {
            Customer customerGet = GetCustomer(getClient.ID);
            Location locationSend = customerGet.Location;
            double distance = Tools.Utils.GetDistance(senderClient.Longitude, senderClient.Latitude, customerGet.Location.Longitude, customerGet.Location.Latitude);
            double Battery = 0;
            if (updatedSkimmer.WeightCategory == Weight.Heavy)
                Battery = distance * HeavyWeightCarrier;
            if (updatedSkimmer.WeightCategory == Weight.Medium)
                Battery = distance * MediumWeightCarrier;
            if (updatedSkimmer.WeightCategory == Weight.Light)
                Battery = distance * LightWeightCarrier;
            BO.BaseStation baseStation = ChecksSmallDistanceBetweenSkimmerAndBaseStation(updatedSkimmer);
            double distance1 = Tools.Utils.GetDistance(baseStation.Location.Longitude, baseStation.Location.Latitude, locationSend.Longitude, locationSend.Latitude);
            double minimalCharge = (distance * Free)+ Battery;
            return minimalCharge;
        }
        /// <summary>
        /// Release skimmer from charging
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ChargingTime"></param>
        public void ReleaseSkimmerFromCharging(int id, double ChargingTime)
        {
            SkimmerToList skimmer= skimmersList.Find(item => item.Id == id);
            // Only a skimmer in maintenance will be able to be released from charging
            if (skimmer.SkimmerStatus!= SkimmerStatuses.maintenance)
            {
                throw new SkimmerExceptionBL($"Skimmer {id} Not in maintenance", Severity.Mild);
            }
            //Battery status will be updated according to the time it was charging
            skimmer.BatteryStatus = (ChargingTime * SkimmerLoadingRate) % 100;
            //The skimmer mode will change to free
            skimmer.SkimmerStatus = SkimmerStatuses.free;
            int IDb=0;
            //Search v skimmer by id
            foreach (SkimmerLoading item in mayDal.GetSkimmerLoading())
            {
                if(item.SkimmerID== skimmer.Id)
                {
                    IDb = item.StationID;
                    break;
                }
            }
            DO.BaseStation station = mayDal.GetBaseStation(IDb);
            station.SeveralPositionsArgument++;
            mayDal.UpadteB(station);
            foreach(SkimmerLoading item in mayDal.GetSkimmerLoading())
            {
                if (item.SkimmerID == skimmer.Id)
                {
                    mayDal.DeleteSkimmerLoading(skimmer.Id);
                    break;
                }
            }
        }
        /// <summary>
        /// Finding a glider-related package
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Client FindingClientSender(DO.Package p)
        {
            return mayDal.GetClient(p.IDSender);
        }
        /// <summary>
        /// Finding a glider-related package
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private DO.Package FindingPackageAssociatedWithGlider(Quadocopter q)
        {
            DO.Package X = new DO.Package
            {
                ID = 0
            };

            foreach (DO.Package item in mayDal.GetPackageList())
            {
                if(item.IDSkimmerOperation == q.IDNumber)
                {
                    return item;
                }
            }
            return X;
        }
        /// <summary>
        /// Add Skimmer
        /// </summary>
        /// <param name="newSkimmer"></param>
        /// <param name="station"></param>
        public void AddSkimmer(BO.Skimmer newSkimmer, int station)
        {
            // Battery status will be raffled off between 20 % and 40 %
             newSkimmer.BatteryStatus = r.Next(20, 41);
            // Joseph as being in maintenance
            newSkimmer.SkimmerStatus = BO.SkimmerStatuses.maintenance;
            BO.BaseStation tempBaseStation = GetBeseStation(station);
            // The glider location will be the same as the station location
            newSkimmer.Location = tempBaseStation.Location;
            Quadocopter tempS = new Quadocopter
            {
                IDNumber = newSkimmer.Id,
                SkimmerModel = newSkimmer.SkimmerModel,
                Weight = (WeightCategories)newSkimmer.WeightCategory
            };
            SkimmerToList skimmerToList;
            if (newSkimmer.PackageInTransfer == null)
            {
                skimmerToList = new SkimmerToList
                {
                    Id = newSkimmer.Id,
                    SkimmerModel = newSkimmer.SkimmerModel,
                    WeightCategory = newSkimmer.WeightCategory,
                    BatteryStatus = newSkimmer.BatteryStatus,
                    SkimmerStatus = newSkimmer.SkimmerStatus,
                    CurrentLocation = newSkimmer.Location,
                };
            }
            else
            {
                skimmerToList = new SkimmerToList
                {
                    Id = newSkimmer.Id,
                    SkimmerModel = newSkimmer.SkimmerModel,
                    WeightCategory = newSkimmer.WeightCategory,
                    BatteryStatus = newSkimmer.BatteryStatus,
                    SkimmerStatus = newSkimmer.SkimmerStatus,
                    CurrentLocation = newSkimmer.Location,
                    PackageNumberTransferred = newSkimmer.PackageInTransfer.Id
                };
            }
            //Add a skimmer to a list of skimmers in charge
            SkimmerLoading skimmerLoading = new SkimmerLoading { SkimmerID = newSkimmer.Id, StationID = tempBaseStation.Id };
            mayDal.AddSkimmerLoading(skimmerLoading);
            //Update base station, lower charging position.
            DO.BaseStation baseStation = mayDal.GetBaseStation(tempBaseStation.Id);
            baseStation.SeveralPositionsArgument = tempBaseStation.SeveralClaimPositionsVacant--;
            mayDal.UpadteB(baseStation);
            if (skimmersList.Exists(item => item.Id == skimmerToList.Id))//If finds an existing skimmer throws an error.
            {
                throw new ExistsInSystemExceptionBL($"skimmer {skimmerToList.Id} Save to system", Severity.Mild);
            }
            skimmersList.Add(skimmerToList);
            try
            {
                mayDal.AddSkimmer(tempS);
            }
            catch (ExistsInSystemException exception)
            {
                throw new ExistsInSystemExceptionBL(exception.Message + " from dal");
            }
        }
        /// <summary>
        /// Returns an entity of the SkimmerToList list type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SkimmerToList GetSkimmerToList(int id)
        {
            return skimmersList.FirstOrDefault(x => x.Id == id);
        }
        /// <summary>
        /// Returns an entity of the Skimmer list type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Skimmer GetSkimmerr(int id)
        {
            if (!skimmersList.Any(d => d.Id == id))
                throw new IdDoesNotExistExceptionBL("Skimmer {id} not found");
            SkimmerToList skimmerToList = skimmersList.Find(d => d.Id == id);
            if (skimmersList == null)
                throw new IdDoesNotExistExceptionBL("Skimmer {id} not found");
            Skimmer skimmer = new Skimmer();
            skimmer.PackageInTransfer = new PackageInTransfer();
            skimmer.Id = skimmerToList.Id;
            skimmer.BatteryStatus = skimmerToList.BatteryStatus;
            skimmer.Location = skimmerToList.CurrentLocation;
            skimmer.WeightCategory = skimmerToList.WeightCategory;
            skimmer.SkimmerModel = skimmerToList.SkimmerModel;
            skimmer.SkimmerStatus = skimmerToList.SkimmerStatus;
            if (skimmer.SkimmerStatus == SkimmerStatuses.shipping)
            {
                DO.Package package = mayDal.GetPackageList().First(x => x.IDSkimmerOperation == skimmer.Id && x.TimeArrivalRecipient == null);
                skimmer.PackageInTransfer.Id = package.ID;
                skimmer.PackageInTransfer.priority = (Priority)(package.priority);
                skimmer.PackageInTransfer.WeightCategory = (Weight)(package.Weight);
                if (package.PackageCollectionTime == null)
                    skimmer.PackageInTransfer.PackageMode = ParcelStatus.Waiting;
                else
                    skimmer.PackageInTransfer.PackageMode = ParcelStatus.OnGoing;

                CustomerInParcel sender = new CustomerInParcel();
                CustomerInParcel receiver = new CustomerInParcel();
                Location collectLocation = new Location();
                Location destinationLocation = new Location();

                sender.Id = package.IDSender;
                sender.Name = mayDal.GetClient(sender.Id).Name;
                receiver.Id = package.IDgets;
                receiver.Name = mayDal.GetClient(receiver.Id).Name;
                skimmer.PackageInTransfer.CustomerSends = sender;
                skimmer.PackageInTransfer.CustomerReceives = receiver;

                collectLocation.Latitude = mayDal.GetClient(sender.Id).Latitude;
                collectLocation.Longitude = mayDal.GetClient(sender.Id).Longitude;
                destinationLocation.Latitude = mayDal.GetClient(receiver.Id).Latitude;
                destinationLocation.Longitude = mayDal.GetClient(receiver.Id).Longitude;

                skimmer.PackageInTransfer.CollectionLocation = collectLocation;
                skimmer.PackageInTransfer.DeliveryDestinationLocation = destinationLocation;
                skimmer.PackageInTransfer.TransportDistance = Tools.Utils.GetDistance(collectLocation.Longitude, collectLocation.Latitude, destinationLocation.Longitude, destinationLocation.Latitude);
            }
            else
                skimmer.PackageInTransfer = null;
            return skimmer;
    }
        /// <summary>
        /// Update skimmer name
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="name"></param>
        public void UpdateSkimmerName(int ids, string name)
        {
            SkimmerToList toUpdate = skimmersList.Find(item => item.Id == ids);
            if (toUpdate== null)
            {
                throw new IdDoesNotExistExceptionBL("cannot update name");
            }
            toUpdate.SkimmerModel = name;

            //update dal
            DO.Quadocopter quadocopter = mayDal.GetQuadrocopter(ids);
            quadocopter.SkimmerModel = name;
            mayDal.UpadteQ(quadocopter);
        }
        /// <summary>
        /// Sends skimmer for charging
        /// </summary>
        /// <param name="id"></param>
        public void SendingSkimmerForCharging(int id)
        {
            SkimmerToList skimmer = skimmersList.Find(item => item.Id == id);
            double battery = 0;
            //Only a free skimmer can be sent for charging
            if (skimmer.SkimmerStatus == SkimmerStatuses.maintenance || skimmer.SkimmerStatus == SkimmerStatuses.shipping)
            {
                throw new SkimmerExceptionBL("the skimmer is not free");
            }
            if (skimmer.SkimmerStatus == SkimmerStatuses.free)
            {
                //Find a very small distance between a skimmer and a base station
                BO.BaseStation baseStation = ChecksSmallDistanceBetweenSkimmerAndBaseStation(skimmer);
                //If there are free charging stations
                if (baseStation.SeveralClaimPositionsVacant != 0)
                {
                    //Check if there is enough battery
                    battery = MinimalChargeToGetToTheNearestStation(skimmer);
                    if (skimmer.BatteryStatus >= battery)
                    {
                        skimmer.BatteryStatus = skimmer.BatteryStatus-battery;
                        skimmer.CurrentLocation = baseStation.Location;
                        //The glider condition will be changed for maintenance
                        skimmer.SkimmerStatus = SkimmerStatuses.maintenance;
                        DO.BaseStation station =mayDal.GetBaseStation(baseStation.Id);
                        station.SeveralPositionsArgument--;
                        mayDal.UpadteB(station);
                        SkimmerLoading skimmerLoading = new SkimmerLoading();
                        skimmerLoading.SkimmerID = skimmer.Id;
                        skimmerLoading.StationID = baseStation.Id;
                        mayDal.AddSkimmerLoading(skimmerLoading);
                    }
                    else
                    {
                        throw new SkimmerExceptionBL("There is not enough battery in the skimmer");
                    }
                }
                else
                {
                    throw new SkimmerExceptionBL("There are no free charging stations");
                }
            }
           
        }
        /// <summary>
        /// Checks a small distance between skimmer and base station
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private BO.BaseStation ChecksSmallDistanceBetweenSkimmerAndBaseStation(SkimmerToList s)
        {
            DO.BaseStation minDistance = default;
            double smallDistance = Double.MaxValue;
            foreach (DO.BaseStation bs in mayDal.GetBaseStationList())
            {
                double dist = Tools.Utils.GetDistance(s.CurrentLocation.Longitude, s.CurrentLocation.Latitude, bs.Longitude, bs.Latitude);
                if (dist < smallDistance)
                {
                    smallDistance = dist;
                    minDistance = bs;
                }
            }          
            return GetBeseStation(minDistance.UniqueID);
        }
        /// <summary>
        /// Checks a small distance between Customer and base station
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private BO.BaseStation ChecksSmallDistanceBetweenCustomerAndBaseStation(Customer c)
        {
            DO.BaseStation station = default;
            double smallDistance = Double.MaxValue;
                       
            foreach(var bs in  mayDal.GetBaseStationList())
            {
                double dist = Tools.Utils.GetDistance(c.Location.Longitude, c.Location.Latitude, bs.Longitude, bs.Latitude);
                if (dist < smallDistance)
                {
                    smallDistance = dist;
                    station = bs;
                }
            }
            return GetBeseStation(station.UniqueID);
        }
        /// <summary>
        /// Calculation of battery by distance and weight of package
        /// </summary>
        /// <param name="location1"></param>
        /// <param name="location2"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        private double BatteryCalculation( Location location1,Location location2, Weight weight)
        {
            //distance calculation
            double distance = Tools.Utils.GetDistance(location1.Longitude, location1.Latitude, location2.Longitude, location2.Latitude);
            double Battery=0;
            if (weight == Weight.Heavy)
                Battery = HeavyWeightCarrier * distance;
            if (weight == Weight.Medium)
                Battery = MediumWeightCarrier * distance;
            if (weight == Weight.Light)
                Battery = LightWeightCarrier * distance;
            return Battery;
        }
        /// <summary>
        ///Available skimmer location will be raffled between customers who have packages provided to them
        /// </summary>
        /// <returns></returns>
        private Location SkimmerLocationAvailable()
        {
            List<Location> CustomersWhoReceivedPackages = new List<Location>();
            Location locationRandom;
            int count;
            foreach (Client item in mayDal.GetClientList())
            {
                if (GetCustomer(item.ID).ReceiveParcels != null)
                {
                    Location location = new Location
                    {
                        Latitude = item.Latitude,
                        Longitude=item.Longitude
                    };
                    CustomersWhoReceivedPackages.Add(location);
                }
            }
            count = CustomersWhoReceivedPackages.Count();
            locationRandom = CustomersWhoReceivedPackages[r.Next(count)];
            return locationRandom;
        }
        /// <summary>
        /// Returns an entity of the Skimmer list type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SkimmerToList> GetSkimmerList(Func<SkimmerToList, bool> predicate = null)
        {
            if (predicate == null)
                return skimmersList.Take(skimmersList.Count).ToList();
            return skimmersList.Where(predicate).ToList();
        }
    }       
}
