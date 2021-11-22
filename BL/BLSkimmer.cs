using IDAL.DO;
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
        public List<SkimmerToList> SkimmersList;
        public double Free;
        public double LightWeightCarrier;
        public double MediumWeightCarrier;
        public double HeavyWeightCarrier;
        public double SkimmerLoadingRate;
        public BL()
        {
            SkimmersList = new List<SkimmerToList>();
            mayDal = new DalObject.DalObject();
            //Array with power consumption data by weight and by charge
            double[] vs = mayDal.PowerConsumptionRequest();
            Free = vs[0];
            LightWeightCarrier = vs[1];
            MediumWeightCarrier = vs[2];
            HeavyWeightCarrier = vs[3];
            SkimmerLoadingRate = vs[4];

            SkimmerUpdates();
        }
        /// <summary>
        /// Skimmer Updates
        /// </summary>
        public void SkimmerUpdates()
        {
            //For each skimmer from the skimmer list
            foreach (Quadocopter item in mayDal.GetQuadocopterList())
            {
                DateTime help = new DateTime(0, 0, 0);
                SkimmerToList UpdatedSkimmer = new SkimmerToList
                {
                    Id = item.IDNumber,
                    SkimmerModel=item.SkimmerModel,
                    WeightCategory=(Weight)item.Weight,                 
                };
                //Finding a glider-related package
                IDAL.DO.Package PackageAssociatedWithSkimmer = FindingPackageAssociatedWithGlider(item);
                //If there is a package that has not yet been delivered but the skimmer is already associated
                if (PackageAssociatedWithSkimmer.ID==item.IDNumber && PackageAssociatedWithSkimmer.TimeArrivalRecipient== help)
                {
                    //Update skimmer status to shipping status
                    UpdatedSkimmer.SkimmerStatus = SkimmerStatuses.shipping;
                    //If the package was associated but not collected
                    if (PackageAssociatedWithSkimmer.PackageCollectionTime == help)
                    {
                        //Location will be at the station closest to the sender
                        Location location = new Location
                        {
                            //Updates skimmer location to the station closest to the sender.
                            Latitude = ChecksSmallDistanceBetweenCustomerAndBaseStation(FindingClientSender(PackageAssociatedWithSkimmer)).Latitude,
                            Longitude = ChecksSmallDistanceBetweenCustomerAndBaseStation(FindingClientSender(PackageAssociatedWithSkimmer)).Longitude
                        };
                        UpdatedSkimmer.CurrentLocation = location;
                    }
                    //The position of the skimmer will be at the position of the sender
                    else
                    {
                        //Updates skimmer location to package shipper location.
                        UpdatedSkimmer.CurrentLocation.Latitude = FindingClientSender(PackageAssociatedWithSkimmer).Latitude;
                        UpdatedSkimmer.CurrentLocation.Longitude = FindingClientSender(PackageAssociatedWithSkimmer).Longitude;
                    }
                }

                //If the glider does not ship, its condition will be raffled off between maintenance and disposal
                if (PackageAssociatedWithSkimmer.ID != item.IDNumber)
                {
                    UpdatedSkimmer.SkimmerStatus = (SkimmerStatuses)(r.Next(2));
                }
                //If the skimmer is in maintenance, Its location will be raffled between the existing stations and Battery status will be raffled between 0% and 20%
                if (UpdatedSkimmer.SkimmerStatus==SkimmerStatuses.maintenance)
                {
                    int counts = mayDal.GetBaseStationList().Count()+1;
                    List<IDAL.DO.BaseStation> B= (List<IDAL.DO.BaseStation>)mayDal.GetBaseStationList();
                    UpdatedSkimmer.CurrentLocation.Latitude = B[r.Next(counts)].Latitude;
                    UpdatedSkimmer.CurrentLocation.Longitude = B[r.Next(counts)].Longitude;
                    UpdatedSkimmer.BatteryStatus = r.Next(21);
                }
                // If the skimmer is available
                if (UpdatedSkimmer.SkimmerStatus == SkimmerStatuses.free)
                {
                    UpdatedSkimmer.CurrentLocation = SkimmerLocationAvailable();
                }

            }

        }
        /// <summary>
        /// ○ Release skimmer from charging
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ChargingTime"></param>
        public void ReleaseSkimmerFromCharging(int id, double ChargingTime)
        {
            Skimmer s= GetSkimmer(id);
            // Only a skimmer in maintenance will be able to be released from charging
            if (s.SkimmerStatus!= SkimmerStatuses.maintenance)
            {
                throw new maintenanceExistsInSystemException_BL($"Skimmer {id} Not in maintenance", Severity.Mild);
            }
            //Battery status will be updated according to the time it was charging
            s.BatteryStatus = ChargingTime * SkimmerLoadingRate;
            //The skimmer mode will change to free
            s.SkimmerStatus = SkimmerStatuses.free;
            mayDal.BaseStationFreeCharging();
            int b;
            //Search v skimmer by id
            foreach (SkimmerLoading item in mayDal.GetSkimmerLoading())
            {
                if(item.SkimmerID==s.Id)
                {
                    b=item.StationID;
                    break;
                }
            }
            //Search for the base station where the skimmer was charging
            foreach (IDAL.DO.BaseStation item in mayDal.GetBaseStationList())
            {
                if(item.UniqueID==b)
                {
                    IDAL.DO.BaseStation baseStation = mayDal.GetBeseStation(b);
                    baseStation.SeveralPositionsArgument++;
                    //Delete the base station with the old data and add a new base station with the new data
                    mayDal.DeleteBaseStation(baseStation.UniqueID);
                    mayDal.AddBaseStation(baseStation);
                    break;
                }
            }
            IDAL.DO.Quadocopter quadocopter = mayDal.GetQuadrocopter(s.Id);
            IBL.BO.BaseStation.SkimmerInCyStatus = 0;
            IBL.BO.SkimmerInChargings.BatteryStatus = 0;
            IBL.BO.SkimmerInCharging.id = 0;
            ddSkimmer(s);
            mayDal.DeleteClient(s.Id);
        }

        //public double AmountOfElectricity(Skimmer skimmer , Weight weight)
        //{
        //    int weight1=(int)cu
        //    switch (weight)
        //    {
        //        case (int)(Weight)IBL.BO.Weight.Light:
        //    }
        //}
        //Finding a sending customer
        public Client FindingClientSender(IDAL.DO.Package p)
        {
            return mayDal.GetClient(p.IDSender);
        }
        //Finding a glider-related package
        public IDAL.DO.Package FindingPackageAssociatedWithGlider(Quadocopter q)
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
            newSkimmer.Location = temp_BaseStation.location;
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
                throw new ExistsInSystemException_BL($"Person {temp_S.IDNumber} Save to system", Severity.Mild);
            }
        }
        public IBL.BO.Skimmer GetSkimmer(int id)
        {
            IDAL.DO.Quadocopter somoeSkimmer;
            try
            {
                somoeSkimmer = mayDal.GetQuadrocopter(id);
            }
            catch (IDAL.DO.IdDoesNotExistException cex)
            {
                throw new IdDoesNotExistException_BL(cex.Message + " from dal"); ;
            }
            return new IBL.BO.Skimmer
            {
                Id = somoeSkimmer.IDNumber,
                SkimmerModel = somoeSkimmer.SkimmerModel,
                WeightCategory = (Weight)somoeSkimmer.Weight,
                BatteryStatus = somoeSkimmer.,
                SkimmerStatus = somoeSkimmer.,
                PackageInTransfer = somoeSkimmer.,
                Location = new Location { Latitude = somoeSkimmer.Latitude, Longitude = somoeSkimmer.Longitude },
            };
        }
        ///Update skimmer name
        public void UpdateSkimmerName(int ids, string name)
        {
            Skimmer skimmer = GetSkimmer(ids);
            //Deleting the skimmer with the old data and adding a new skimmer with the updated data
            mayDal.DeleteSkimmer(ids);
            skimmer.SkimmerModel = name;
            AddSkimmer(skimmer);
        }
        /// <summary>
        /// Sends skimmer for charging
        /// </summary>
        /// <param name="id"></param>
        public void SendingSkimmerForCharging(int id)
        {
            IBL.BO.Skimmer skimmer = GetSkimmer(id);
            double battery = 0;
            //Only a free skimmer can be sent for charging
            if (skimmer.SkimmerStatus == SkimmerStatuses.free)
            {
                //Find a very small distance between a skimmer and a base station
                IDAL.DO.BaseStation baseStation = ChecksSmallDistanceBetweenSkimmerAndBaseStation(skimmer);
                //If there are free charging stations
                if (baseStation.SeveralPositionsArgument != 0)
                {
                    //Check if there is enough battery
                    if (skimmer.BatteryStatus >= battery)
                    {
                        skimmer.BatteryStatus =;
                        //The glider condition will be changed for maintenance
                        skimmer.SkimmerStatus = SkimmerStatuses.maintenance;
                        ////Deleting the skimmer with the old data and adding a new skimmer with the updated data
                        mayDal.DeleteSkimmer(skimmer.Id);
                        //Lowering the number of available charging stations by 1
                        AddSkimmer(skimmer, baseStation.UniqueID);
                        baseStation.SeveralPositionsArgument--;
                        ////Deleting the BaseStation with the old data and adding a new BaseStation with the updated data
                        mayDal.DeleteBaseStation(baseStation.UniqueID);
                        mayDal.AddBaseStation(baseStation);
                        IDAL.DO.SkimmerLoading skimmerLoading = new SkimmerLoading();
                        skimmerLoading.SkimmerID = skimmer.Id;
                        skimmerLoading.StationID = baseStation.UniqueID;
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
        public IDAL.DO.BaseStation ChecksSmallDistanceBetweenSkimmerAndBaseStation(Skimmer s)
        {
            IDAL.DO.BaseStation minDistance;
            int distance1, distance2 = 100000;
            foreach (IDAL.DO.BaseStation item in mayDal.GetBaseStationList())
            {
                distance1 = DistanceToDestination.Calculation(s.Location.Longitude, s.Location.Latitude, item.Longitude, item.Latitude);
                if (distance1 < distance2)
                {
                    minDistance = item;
                    distance2 = distance1;
                }
            }
            return minDistance;
        }
        /// <summary>
        /// Checks a small distance between Customer and base station
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public IDAL.DO.BaseStation ChecksSmallDistanceBetweenCustomerAndBaseStation(Client c)
        {
            IDAL.DO.BaseStation minDistance;
            int distance1, distance2 = 100000;
            foreach (IDAL.DO.BaseStation item in mayDal.GetBaseStationList())
            {
                distance1 = DistanceToDestination.Calculation(c.Longitude, c.Latitude, item.Longitude, item.Latitude);
                if (distance1 < distance2)
                {
                    minDistance = item;
                    distance2 = distance1;
                }
            }
            return minDistance;
        }
        /// <summary>
        /// Calculation of battery by distance and weight of package
        /// </summary>
        /// <param name="location1"></param>
        /// <param name="location2"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public double BatteryCalculation(Location location1,Location location2, WeightCategories weight)
        {
            //distance calculation
            double distance = DistanceToDestination.Calculation(location1.Longitude, location1.Latitude, location2.Longitude, location2.Latitude);
            double Battery;
            if (weight == WeightCategories.heavy)
                Battery = HeavyWeightCarrier * distance;
            if (weight == WeightCategories.middle)
                Battery = MediumWeightCarrier * distance;
            if (weight == WeightCategories.low)
                Battery = LightWeightCarrier * distance;
            return Battery;
        }
        /// <summary>
        /// ○ Available skimmer location will be raffled between customers who have packages provided to them
        /// </summary>
        /// <returns></returns>
        public Location SkimmerLocationAvailable()
        {
            List<Client> CustomersWhoReceivedPackages;
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
            clientRandom = CustomersWhoReceivedPackages(r.Next(count));
            Location location = new Location
            {
                Latitude = clientRandom.Latitude,
                Longitude = clientRandom.Longitude
            };
            return location;
        }
    }
}
