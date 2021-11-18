using IBL.BO;
using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        public void AddPackage(IBL.BO.Package newPackage)
        {
            IDAL.DO.Package temp_p = new IDAL.DO.Package
            {
                IDSender = newPackage.SendPackage.Id,
                IDgets = newPackage.ReceivesPackage.Id,
                Weight = (WeightCategories)newPackage.WeightCategory,
                priority = (Priorities)newPackage.priority,
                PackageCreationTime = DateTime.Now,
                TimeAssignGlider = new DateTime(0, 0, 0),
                PackageCollectionTime = new DateTime(0, 0, 0),
                TimeArrivalRecipient = new DateTime(0, 0, 0),
                IDSkimmerOperation = 0,
            };
            try
            {
                mayDal.AddPackage(temp_p);
            }
            catch (ExistsInSystemException exception)
            {
                throw new ExistsInSystemException_BL($"Person {temp_p.ID} Save to system", Severity.Mild);
            }
        }
        public IBL.BO.Customer GetPackage(int id)
        {
            IDAL.DO.Package somoePackage;
            try
            {
                somoePackage = mayDal.GetPackage(id);
            }
            catch (IDAL.DO.IdDoesNotExistException cex)
            {
                throw new IdDoesNotExistException_BL(cex.Message + " from dal");
            }

            return new IBL.BO.Package
            {
                Id = somoePackage.ID,
                SendPackage = somoePackage.IDSender,
                ReceivesPackage = somoePackage.IDgets,
                WeightCategory = (Weight)somoePackage.Weight,
                priority = (Priority)somoePackage.priority,
                SkimmerInPackage = somoePackage.,
                PackageCreationTime = somoePackage.PackageCreationTime,
                AssignmentTime = somoePackage.TimeAssignGlider,
                CollectionTime = somoePackage.PackageCollectionTime,
                SupplyTime = somoePackage.TimeArrivalRecipient
            };
        }
        public void CollectingPackageBySkimmer(int id)
        {
            Skimmer s = GetSkimmer(id);
            IBL.BO.Package package;;
            DateTime date = new DateTime(0, 0, 0);
            package = s.PackageInTransfer;
            int idc = package.SendPackage.Id;
            Location locationsend = GetCustomer(idc).Location;
            if (s.SkimmerStatus == SkimmerStatuses.shipping && package.AssignmentTime != date && package.CollectionTime == date)
            {
                double distance = DalObject.DistanceToDestination.Calculation(s.Location.Longitude, s.Location.Latitude, locationsend.Longitude, locationsend.Latitude);
                s.BatteryStatus = (s.BatteryStatus) - (distance * Free);
                s.Location = locationsend;
                package.CollectionTime = DateTime.Now;

            }
            else
                throw new SkimmerExistsInSystemException_BL($"Skimmer {id}does not ship a package that has been associated with it or has already been collected", Severity.Mild);
        }
        public void AssigningPackageToSkimmer(int id)
        {
            Skimmer skimmer = GetSkimmer(id);
            if (skimmer.SkimmerStatus == SkimmerStatuses.maintenance)
            {
                foreach (IDAL.DO.Package item in mayDal.GetPackageList())
                {
                    if (item.priority == Priorities.emergency)
                    {

                    }
                }
            }
        }
        public void DeliveryOfPackageBySkimmer(int id)
        {
            Skimmer skimmer = GetSkimmer(id);
            DateTime ResetTime = new DateTime(0, 0, 0);
            IDAL.DO.Package package;
            foreach (IDAL.DO.Package item in mayDal.GetPackageList())//Looking for a package that the glider is associated with
            {
                if (item.IDSkimmerOperation == id)
                {
                    package = new IDAL.DO.Package
                    {
                        ID = item.ID,
                        IDSender = item.IDSender,
                        IDgets = item.IDgets,
                        Weight = item.Weight,
                        priority = item.priority,
                        IDSkimmerOperation = item.IDSkimmerOperation,
                        PackageCreationTime = item.PackageCreationTime,
                        TimeAssignGlider = item.TimeAssignGlider,
                        PackageCollectionTime = item.PackageCollectionTime,
                        TimeArrivalRecipient = item.TimeArrivalRecipient
                    };
                }
            }
            if (package.PackageCollectionTime == ResetTime && package.TimeArrivalRecipient != ResetTime)
            {
                Customer customer = GetClint(package.IDgets);
                
                IDAL.DO.BaseStation baseStation = ChecksSmallDistanceBetweenSkimmerAndBaseStation(skimmer);
                Location locationBaseStation = new Location
                {
                    Longitude = baseStation.Longitude,
                    Latitude = baseStation.Latitude
                };
                double battery = BatteryCalculation(skimmer.Location, customer.Location, package.Weight) + BatteryCalculation(customer.Location, locationBaseStation, package.Weight);
                if (battery <= skimmer.BatteryStatus)
                {
                    skimmer.BatteryStatus = skimmer.BatteryStatus - battery;
                    skimmer.Location = customer.Location;
                    skimmer.SkimmerStatus = SkimmerStatuses.free;
                    mayDal.DeleteSkimmer(skimmer.Id);
                    AddSkimmer(skimmer);
                    package.TimeArrivalRecipient = DateTime.Now;
                    mayDal.DeletePackage(package.ID);
                    mayDal.AddPackage(package);
                }
            }
        }
    }
}
//int idc = package.SendPackage.Id;
//Location locationsend = GetCustomer(idc).Location;
//double distance = DalObject.DistanceToDestination.Calculation(s.Location.Longitude, s.Location.Latitude, locationsend.Longitude, locationsend.Latitude);
//double weight;
//if (package.WeightCategory == Weight.Heavy)
//    weight = HeavyWeightCarrier;
//if (package.WeightCategory == Weight.Medium)
//    weight = MediumWeightCarrier;
//if (package.WeightCategory == Weight.Light)
//    weight = LightWeightCarrier;
