﻿using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DalObject;
namespace BL
{
    public partial class BL : IBL.IBL
    {
        static Random r = new Random();
        public IDal mayDal;
        private List<SkimmerToList> skimmersList;
        public double Free;
        public double LightWeightCarrier;
        public double MediumWeightCarrier;
        public double HeavyWeightCarrier;
        public double SkimmerLoadingRate;

        public BL()
        {
            skimmersList = new List<SkimmerToList>();
            mayDal = new DalObject.DalObject();
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
                    WeightCategory=(Weight)item.Weight,                 
                };
                //Finding a glider-related package
                IDAL.DO.Package PackageAssociatedWithSkimmer = FindingPackageAssociatedWithGlider(item);
                //If there is a package that has not yet been delivered but the skimmer is already associated
                if (PackageAssociatedWithSkimmer.ID == item.IDNumber && PackageAssociatedWithSkimmer.TimeArrivalRecipient == null)
                {
                    //Update skimmer status to shipping status
                    updatedSkimmer.SkimmerStatus = SkimmerStatuses.shipping;
                    //If the package was associated but not collected
                    if (PackageAssociatedWithSkimmer.PackageCollectionTime == null)
                    {
                        //Location will be at the station closest to the sender
                        updatedSkimmer.CurrentLocation = ChecksSmallDistanceBetweenCustomerAndBaseStation(GetCustomer(PackageAssociatedWithSkimmer.IDSender)).Location;
                    }
                    //The position of the skimmer will be at the position of the sender
                    else
                    {
                        //Updates skimmer location to package shipper location.
                        updatedSkimmer.CurrentLocation = GetCustomer(PackageAssociatedWithSkimmer.IDSender).Location;
                    }
                    IBL.BO.Customer sendcustomer= GetCustomer(PackageAssociatedWithSkimmer.IDSender);
                    IDAL.DO.Client tempC = new IDAL.DO.Client
                    {
                        ID = sendcustomer.Id,
                        Name = sendcustomer.Name,
                        Telephone = sendcustomer.Phone,
                        Latitude = sendcustomer.Location.Latitude,
                        Longitude = sendcustomer.Location.Longitude
                    };
                    double minBattery = MinimalLoadingPerformTheShipmentAndArriveForLoading(updatedSkimmer, tempC);
                    updatedSkimmer.BatteryStatus = (double)r.Next((int)minBattery, 100);
                }

                //If the glider does not ship, its condition will be raffled off between maintenance and disposal
                if (PackageAssociatedWithSkimmer.ID != item.IDNumber)
                {
                    updatedSkimmer.SkimmerStatus = (SkimmerStatuses)(r.Next(2));
                }
                //If the skimmer is in maintenance, Its location will be raffled between the existing stations and Battery status will be raffled between 0% and 20%
                if (updatedSkimmer.SkimmerStatus == SkimmerStatuses.maintenance)
                {
                    int counts = GetBaseStationList().Count();
                    List<IBL.BO.BaseStation> B = (List<IBL.BO.BaseStation>)GetBaseStationList();
                    updatedSkimmer.CurrentLocation = B[r.Next(counts)].Location;
                    updatedSkimmer.BatteryStatus = r.Next(21);
                }
                // If the skimmer is available
                if (updatedSkimmer.SkimmerStatus == SkimmerStatuses.free)
                {
                    double minBattery = MinimalChargeToGetToTheNearestStation(updatedSkimmer);
                    updatedSkimmer.BatteryStatus = (double)r.Next((int)minBattery, 100);
                    updatedSkimmer.CurrentLocation = SkimmerLocationAvailable();
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
            IBL.BO.BaseStation baseStation = ChecksSmallDistanceBetweenSkimmerAndBaseStation(updatedSkimmer);
            double distance =Tools.Utils.GetDistance(baseStation.Location.Longitude, baseStation.Location.Latitude, updatedSkimmer.CurrentLocation.Longitude, updatedSkimmer.CurrentLocation.Latitude);
            double minimalCharge = (distance * Free);
            return minimalCharge;
        }
        private double MinimumPaymentToGetToThePackage(SkimmerToList updatedSkimmer, IDAL.DO.Client senderClient)
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
        private double MinimalLoadingPerformTheShipmentAndArriveForLoading (SkimmerToList updatedSkimmer, IDAL.DO.Client senderClient)
        {            
            Customer customerGet = GetCustomer(GetPackage(updatedSkimmer.PackageNumberTransferred).ReceivesPackage.Id);
            Location locationSend = customerGet.Location;
            double distance = Tools.Utils.GetDistance(senderClient.Longitude, senderClient.Latitude, customerGet.Location.Longitude, customerGet.Location.Latitude);
            double Battery = 0;
            if (updatedSkimmer.WeightCategory == Weight.Heavy)
                Battery = distance * HeavyWeightCarrier;
            if (updatedSkimmer.WeightCategory == Weight.Medium)
                Battery = distance * MediumWeightCarrier;
            if (updatedSkimmer.WeightCategory == Weight.Light)
                Battery = distance * LightWeightCarrier;
            IBL.BO.BaseStation baseStation = ChecksSmallDistanceBetweenSkimmerAndBaseStation(updatedSkimmer);
            double distance1 = Tools.Utils.GetDistance(baseStation.Location.Longitude, baseStation.Location.Latitude, locationSend.Longitude, locationSend.Latitude);
            double minimalCharge = (distance * Free)+ Battery;
            return minimalCharge;
        }
        /// <summary>
        /// ○ Release skimmer from charging
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ChargingTime"></param>
        public void ReleaseSkimmerFromCharging(int id, double ChargingTime)
        {
            SkimmerToList skimmer= skimmersList.Find(item => item.Id == id);
            // Only a skimmer in maintenance will be able to be released from charging
            if (skimmer.SkimmerStatus!= SkimmerStatuses.maintenance)
            {
                throw new maintenanceExistsInSystemException_BL($"Skimmer {id} Not in maintenance", Severity.Mild);
            }
            //Battery status will be updated according to the time it was charging
            skimmer.BatteryStatus = ChargingTime * SkimmerLoadingRate;
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
            IDAL.DO.BaseStation station = mayDal.GetBaseStation(IDb);
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
        private Client FindingClientSender(IDAL.DO.Package p)
        {
            return mayDal.GetClient(p.IDSender);
        }
        //Finding a glider-related package
        private IDAL.DO.Package FindingPackageAssociatedWithGlider(Quadocopter q)
        {
            IDAL.DO.Package X = new IDAL.DO.Package
            {
                ID = 0
            };

            foreach (IDAL.DO.Package item in mayDal.GetPackageList())
            {
                if(item.IDSkimmerOperation==q.IDNumber)
                {
                    return item;
                }
            }
            return X;
        }
        //Add Skimmer
        public void AddSkimmer(IBL.BO.Skimmer newSkimmer, int station)
        {
            // Battery status will be raffled off between 20 % and 40 %
             newSkimmer.BatteryStatus = r.Next(20, 41);
            // Joseph as being in maintenance
            newSkimmer.SkimmerStatus = IBL.BO.SkimmerStatuses.maintenance;
            IBL.BO.BaseStation temp_BaseStation = GetBeseStation(station);
            // The glider location will be the same as the station location
            newSkimmer.Location = temp_BaseStation.Location;
            Quadocopter temp_S = new Quadocopter
            {
                IDNumber = newSkimmer.Id,
                SkimmerModel = newSkimmer.SkimmerModel,
                Weight = (WeightCategories)newSkimmer.WeightCategory
            };

            try
            {
                mayDal.AddSkimmer(temp_S);
            }
            catch (ExistsInSystemException exception)
            {
                throw new ExistsInSystemExceptionBL($"Person {temp_S.IDNumber} Save to system", Severity.Mild);
            }
        }
        public SkimmerToList GetSkimmer(int id)
        {
            return skimmersList.FirstOrDefault(x => x.Id == id);
        }
        ///Update skimmer name
        public void UpdateSkimmerName(int ids, string name)
        {
            SkimmerToList toUpdate = skimmersList.Find(item => item.Id == ids);
            if (toUpdate== null)
            {
                throw new IdDoesNotExistExceptionBL("cannot update name");
            }
            toUpdate.SkimmerModel = name;

            //update dal
            IDAL.DO.Quadocopter quadocopter = mayDal.GetQuadrocopter(ids);
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
            if (skimmer.SkimmerStatus == SkimmerStatuses.free)
            {
                //Find a very small distance between a skimmer and a base station
                IBL.BO.BaseStation baseStation = ChecksSmallDistanceBetweenSkimmerAndBaseStation(skimmer);
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
                        IDAL.DO.BaseStation station =mayDal.GetBaseStation(baseStation.Id);
                        station.SeveralPositionsArgument--;
                        mayDal.UpadteB(station);
                        IDAL.DO.SkimmerLoading skimmerLoading = new SkimmerLoading();
                        skimmerLoading.SkimmerID = skimmer.Id;
                        skimmerLoading.StationID = baseStation.Id;
                        mayDal.AddSkimmerLoading(skimmerLoading);
                    }
                    else
                    {
                        //throw new ExistsInSystemException($"Skimmer {SL.SkimmerID} Save to system of SkimmerLoading", Severity.Mild);
                        throw new Exception "אין מספיק בטריה"
                    }
                }
                else
                {
                    throw new Exception "אין עמדות טעינה פנויות"
                }
            }
        }
        /// <summary>
        /// Checks a small distance between skimmer and base station
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private IBL.BO.BaseStation ChecksSmallDistanceBetweenSkimmerAndBaseStation(SkimmerToList s)
        {
            IDAL.DO.BaseStation minDistance = default;
            double smallDistance = Double.MaxValue;
            foreach (IDAL.DO.BaseStation bs in mayDal.GetBaseStationList())
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
        private IBL.BO.BaseStation ChecksSmallDistanceBetweenCustomerAndBaseStation(Customer c)
        {
            IDAL.DO.BaseStation station = default;
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
        /// ○ Available skimmer location will be raffled between customers who have packages provided to them
        /// </summary>
        /// <returns></returns>
        private Location SkimmerLocationAvailable()
        {
            List<Client> CustomersWhoReceivedPackages = new List<Client>();
            Client clientRandom;
            int count;
            foreach (Client item in mayDal.GetClientList())
            {
                if (GetCustomer(item.ID).ReceiveParcels != null)
                {
                    Client client = new Client
                    {
                        ID = item.ID
                    };
                    CustomersWhoReceivedPackages.Add(client);
                }
            }
            count = CustomersWhoReceivedPackages.Count();
            clientRandom = CustomersWhoReceivedPackages[r.Next(count)];
            Location location = new Location
            {
                Latitude = clientRandom.Latitude,
                Longitude = clientRandom.Longitude
            };
            return location;
        }
        public IEnumerable<SkimmerToList> GetSkimmerList()
        {
            return skimmersList.Take(skimmersList.Count).ToList();
        }
    }       
}
