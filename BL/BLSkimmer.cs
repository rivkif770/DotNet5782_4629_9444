using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;
using System.Runtime.CompilerServices;

namespace BL
{
    public partial class BL : BlApi.IBL
    {
        static readonly IBL instance = new BL();
        internal static IBL Instance { get { return instance; } }
        static BL() { }

        static Random r ;
        internal DalApi.IDal mayDal;
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
            mayDal = DalApi.DalFactory.GetDal("xml");
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
            lock (mayDal)
            {
                foreach (Quadocopter item in mayDal.GetQuadocopterList())
                {
                    SkimmerToList updatedSkimmer = new SkimmerToList
                    {
                        Id = item.IDNumber,
                        SkimmerModel = item.SkimmerModel,
                        WeightCategory = (Weight)item.Weight
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
                            DO.BaseStation baseStation = ChecksSmallDistanceBetweenCustomerAndBaseStation(GetCustomer(PackageAssociatedWithSkimmer.IDSender));
                            updatedSkimmer.CurrentLocation = new Location { Latitude = baseStation.Latitude, Longitude = baseStation.Longitude };
                        }
                        //The position of the skimmer will be at the position of the sender
                        if (PackageAssociatedWithSkimmer.PackageCollectionTime != null && PackageAssociatedWithSkimmer.TimeArrivalRecipient == null)
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
                    else
                    {
                        if (mayDal.GetSkimmerLoadingList().Count() == 0)
                        {
                            //If the glider does not ship, its condition will be raffled off between maintenance and disposal
                            if (PackageAssociatedWithSkimmer.ID == 0)
                            {
                                updatedSkimmer.SkimmerStatus = (SkimmerStatuses)(r.Next(2));
                            }
                            //If the skimmer is in maintenance, Its location will be raffled between the existing stations and Battery status will be raffled between 0% and 20%
                            if (updatedSkimmer.SkimmerStatus == SkimmerStatuses.maintenance)
                            {
                                updatedSkimmer.EntertimeLoading = DateTime.Now;
                                int counts = mayDal.GetBaseStationList().Count();
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
                        }
                        else
                        {

                            //If the skimmer is in maintenance, Its location will be raffled between the existing stations and Battery status will be raffled between 0% and 20%

                            if (mayDal.GetSkimmerLoadingList().Any(s => s.SkimmerID == updatedSkimmer.Id))
                            {
                                updatedSkimmer.SkimmerStatus = SkimmerStatuses.maintenance;
                                updatedSkimmer.EntertimeLoading = DateTime.Now;
                                int counts = mayDal.GetBaseStationList().Count();
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


                                updatedSkimmer.BatteryStatus = r.Next(21);
                            }
                            else
                            // If the skimmer is available
                            {
                                updatedSkimmer.SkimmerStatus = SkimmerStatuses.free;
                                updatedSkimmer.CurrentLocation = SkimmerLocationAvailable();
                                double minBattery = MinimalChargeToGetToTheNearestStation(updatedSkimmer);
                                updatedSkimmer.BatteryStatus = (double)r.Next((int)minBattery, 100);

                            }
                        }
                    }

                    lst.Add(updatedSkimmer);
                }
            }
        }
        /// <summary>
        /// Calculate a minimum charge that will allow him to reach the nearest station
        /// </summary>
        /// <param name="updatedSkimmer"></param>
        /// <returns></returns>
        private double MinimalChargeToGetToTheNearestStation(SkimmerToList updatedSkimmer)
        {             
            DO.BaseStation baseStation = ChecksSmallDistanceBetweenSkimmerAndBaseStation(updatedSkimmer);
            double distance =Tools.Utils.GetDistance(baseStation.Longitude, baseStation.Latitude, updatedSkimmer.CurrentLocation.Longitude, updatedSkimmer.CurrentLocation.Latitude);
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
            DO.BaseStation baseStation = ChecksSmallDistanceBetweenSkimmerAndBaseStation(updatedSkimmer);
            double distance1 = Tools.Utils.GetDistance(baseStation.Longitude, baseStation.Latitude, locationSend.Longitude, locationSend.Latitude);
            double minimalCharge = (distance * Free)+ Battery;
            return minimalCharge;
        }
        /// <summary>
        /// Release skimmer from charging
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ChargingTime"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseSkimmerFromCharging(int id)
        {
            SkimmerToList skimmer= skimmersList.Find(item => item.Id == id);
            // Only a skimmer in maintenance will be able to be released from charging
            if (skimmer.SkimmerStatus!= SkimmerStatuses.maintenance)
            {
                throw new SkimmerExceptionBL($"Skimmer {id} Not in maintenance", Severity.Mild);
            }
            DateTime? now = DateTime.Now;
            DateTime? Loading = skimmer.EntertimeLoading;

            //Battery status will be updated according to the time it was charging
            
            //The skimmer mode will change to free
            skimmer.SkimmerStatus = SkimmerStatuses.free;
            //Search v skimmer by id
            SkimmerLoading skimmerLoading = new SkimmerLoading();
            lock (mayDal)
            {
                foreach (SkimmerLoading item in mayDal.GetSkimmerLoadingList())
                {
                    if (item.SkimmerID == skimmer.Id)
                    {
                        skimmerLoading = item;
                        break;
                    }
                }
            }
            DateTime dt = skimmerLoading.EnteredLoading;
            TimeSpan timeSpan = DateTime.Now - dt;
            int time = (int)timeSpan.TotalSeconds;
            skimmer.BatteryStatus += (time * SkimmerLoadingRate);
            if (skimmer.BatteryStatus >= 100)
                skimmer.BatteryStatus = 100;
            DO.BaseStation station = mayDal.GetBaseStation(skimmerLoading.StationID);
            station.SeveralPositionsArgument++;
            mayDal.UpadteB(station);
            mayDal.DeleteSkimmerLoading(skimmerLoading.SkimmerID);
            //foreach (SkimmerLoading item in mayDal.GetSkimmerLoadingList())
            //{
            //    if (item.SkimmerID == skimmer.Id)
            //    {
            //        mayDal.DeleteSkimmerLoading(skimmer.Id);
            //        break;
            //    }
            //}
        }
        /// <summary>
        /// Finding a glider-related package
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Client FindingClientSender(DO.Package p)
        {
            lock (mayDal)
            {
                return mayDal.GetClient(p.IDSender);
            }
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
            lock (mayDal)
            {
                foreach (DO.Package item in mayDal.GetPackageList())
                {
                    if (item.IDSkimmerOperation == q.IDNumber)
                    {
                        return item;
                    }
                }
            }
            return X;
        }
        /// <summary>
        /// Add Skimmer
        /// </summary>
        /// <param name="newSkimmer"></param>
        /// <param name="station"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public SkimmerToList GetSkimmerToList(int id)
        {
            return skimmersList.FirstOrDefault(x => x.Id == id);
        }
        /// <summary>
        /// Returns an entity of the Skimmer list type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
                lock (mayDal)
                {
                    DO.Package package = mayDal.GetPackageList().First(x => x.IDSkimmerOperation == skimmer.Id && x.TimeArrivalRecipient == null);
                    skimmer.PackageInTransfer.Id = package.ID;
                    skimmer.PackageInTransfer.priority = (Priority)(package.priority);
                    skimmer.PackageInTransfer.WeightCategory = (Weight)(package.Weight);
                    if (package.PackageCollectionTime == null)
                        skimmer.PackageInTransfer.PackageMode = ParcelStatus.Assignment;

                    else
                        skimmer.PackageInTransfer.PackageMode = ParcelStatus.Collection;
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
                    if (skimmer.PackageInTransfer.PackageMode == ParcelStatus.Assignment)
                        skimmer.PackageInTransfer.TransportDistance = Tools.Utils.GetDistance(skimmer.Location.Longitude, skimmer.Location.Latitude, collectLocation.Longitude, collectLocation.Latitude);
                    if (skimmer.PackageInTransfer.PackageMode == ParcelStatus.Collection)
                        skimmer.PackageInTransfer.TransportDistance = Tools.Utils.GetDistance(skimmer.Location.Longitude, skimmer.Location.Latitude, destinationLocation.Longitude, destinationLocation.Latitude);
                }
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateSkimmerName(int ids, string name)
        {
            SkimmerToList toUpdate = skimmersList.Find(item => item.Id == ids);
            if (toUpdate== null)
            {
                throw new IdDoesNotExistExceptionBL("cannot update name");
            }
            toUpdate.SkimmerModel = name;

            //update dal
            lock (mayDal)
            {
                DO.Quadocopter quadocopter = mayDal.GetQuadrocopter(ids);
                quadocopter.SkimmerModel = name;
                mayDal.UpadteQ(quadocopter);
            }
        }
        /// <summary>
        /// Sends skimmer for charging
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
                DO.BaseStation dalBaseStation = ChecksSmallDistanceBetweenSkimmerAndBaseStation(skimmer);
                //If there are free charging stations
                BO.BaseStation baseStation = GetBeseStation(dalBaseStation.UniqueID);
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
                        skimmer.EntertimeLoading = DateTime.Now;
                        DO.BaseStation station =mayDal.GetBaseStation(baseStation.Id);
                        station.SeveralPositionsArgument--;
                        mayDal.UpadteB(station);
                        SkimmerLoading skimmerLoading = new SkimmerLoading();
                        skimmerLoading.SkimmerID = skimmer.Id;
                        skimmerLoading.StationID = baseStation.Id;
                        skimmerLoading.EnteredLoading = DateTime.Now;
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
        public DO.BaseStation ChecksSmallDistanceBetweenSkimmerAndBaseStation(SkimmerToList s)
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
            lock (mayDal)
            {
                return mayDal.GetBaseStation(minDistance.UniqueID);
            }
        }
        /// <summary>
        /// Checks a small distance between Customer and base station
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private DO.BaseStation ChecksSmallDistanceBetweenCustomerAndBaseStation(Customer c)
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
            lock (mayDal)
            {
                return mayDal.GetBaseStation(station.UniqueID);
            }
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
        /// Calculation of battery utilization by weight and distance
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public double BatteryCalculation2(double distance, int weight)
        {
            double Battery = 0;
            if (weight == (int)Weight.Heavy)
                Battery = HeavyWeightCarrier * distance;
            if (weight == (int)Weight.Medium)
                Battery = MediumWeightCarrier * distance;
            if (weight == (int)Weight.Light)
                Battery = LightWeightCarrier * distance;
            if(weight == -1)
                Battery = Free * distance;
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
            lock (mayDal)
            {
                foreach (Client item in mayDal.GetClientList())
                {
                    if (GetCustomer(item.ID).ReceiveParcels != null)
                    {
                        Location location = new Location
                        {
                            Latitude = item.Latitude,
                            Longitude = item.Longitude
                        };
                        CustomersWhoReceivedPackages.Add(location);
                    }
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<SkimmerToList> GetSkimmerList(Func<SkimmerToList, bool> predicate = null)
        {
            if (predicate == null)
                return skimmersList.Take(skimmersList.Count).ToList();
            return skimmersList.Where(predicate).ToList();
        }
        /// <summary>
        /// Running the simulator
        /// </summary>
        /// <param name="id"></param>
        /// <param name="act"></param>
        /// <param name="func"></param>
        public void SimulatorActive(int id, Action act, Func<bool> func)
        {
            new Simulator(this, id, act, func);
        }
        /// <summary>
        /// Adds battery by charging time (simulator)
        /// </summary>
        /// <param name="skimmer"></param>
        public void UploadCharge(Skimmer skimmer)
        {
            skimmersList.Find(s => s.Id == skimmer.Id).BatteryStatus += SkimmerLoadingRate;
            skimmer.BatteryStatus += SkimmerLoadingRate;
            if (skimmer.BatteryStatus > 100) skimmersList.Find(s => s.Id == skimmer.Id).BatteryStatus = 100;
        }
        /// <summary>
        /// Lowers battery by distance and weight (simulator)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lessBattry"></param>
        public void UploadLessBattry(int id ,double lessBattry)
        {
            skimmersList.Find(s => s.Id == id).BatteryStatus -= lessBattry;
        }
        /// <summary>
        /// Updates the distance left to the glider according to its location
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lonPlus"></param>
        /// <param name="logPlus"></param>
        public void UploadLocation(int id,double lonPlus,double logPlus)
        {
            skimmersList.Find(s => s.Id == id).CurrentLocation.Latitude += lonPlus;
            skimmersList.Find(s => s.Id == id).CurrentLocation.Longitude += logPlus;
        }
    }       
}
