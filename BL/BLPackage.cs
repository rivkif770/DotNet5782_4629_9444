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
        /// <summary>
        /// Adding a package, all times initialized to zero time except for a creation date that will be initialized to DateTime.Now, the glider will be initialized to null
        /// </summary>
        /// <param name="newPackage"></param>
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
        public IBL.BO.Package GetPackage(int id)
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
            CustomerInParcel customerSender = new CustomerInParcel
            {
                Id = somoePackage.IDSender,
                Name = GetPackage(somoePackage.ID).Name
            };
            CustomerInParcel customergets = new CustomerInParcel
            {
                Id = somoePackage.IDgets,
                Name = GetPackage(somoePackage.ID).Name
            };
            SkimmerInPackage skimmerPackage = new SkimmerInPackage
            {
                Id = somoePackage.IDSkimmerOperation,
                BatteryStatus = GetSkimmer(somoePackage.IDSkimmerOperation).BatteryStatus,
                Location = GetSkimmer(somoePackage.IDSkimmerOperation).CurrentLocation
            };
            return new IBL.BO.Package
            {
                Id = somoePackage.ID,
                SendPackage = customerSender,
                ReceivesPackage = customergets,
                WeightCategory = (Weight)somoePackage.Weight,
                priority = (Priority)somoePackage.priority,
                SkimmerInPackage = skimmerPackage,
                PackageCreationTime = somoePackage.PackageCreationTime,
                AssignmentTime = somoePackage.TimeAssignGlider,
                CollectionTime = somoePackage.PackageCollectionTime,
                SupplyTime = somoePackage.TimeArrivalRecipient
            };
        }
        /// <summary>
        /// Collecting a package by skimmer
        /// </summary>
        /// <param name="id"></param>
        public void CollectingPackageBySkimmer(int id)
        {
            Skimmer s = GetSkimmer(id);
            IBL.BO.Package package;;
            DateTime date = new DateTime(0, 0, 0);
            package = s.PackageInTransfer;
            int idc = package.SendPackage.Id;
            Location locationsend = GetCustomer(idc).Location;
            //Only a skimmer that delivers a package that has been associated with it but has not yet been collected will be able to pick it up
            if (s.SkimmerStatus == SkimmerStatuses.shipping && package.AssignmentTime != date && package.CollectionTime == date)
            {
                //Update battery status according to the distance between the original location and the sender location
                double distance = Tools.Utils.GetDistance(s.Location.Longitude, s.Location.Latitude, locationsend.Longitude, locationsend.Latitude);
                s.BatteryStatus = (s.BatteryStatus) - (distance * Free);
                //Update location to sender location
                s.Location = locationsend;
                //Update package pickup time
                package.CollectionTime = DateTime.Now;

            }
            else
                throw new SkimmerExistsInSystemException_BL($"Skimmer {id}does not ship a package that has been associated with it or has already been collected", Severity.Mild);
        }
        /// <summary>
        /// AssigningPackageToSkimmer
        /// </summary>
        /// <param name="id"></param>
        public void AssigningPackageToSkimmer(int id)
        {
            Skimmer skimmer = GetSkimmer(id);
            //Check that the glider is in maintenance
            if (skimmer.SkimmerStatus == SkimmerStatuses.maintenance)
            {
                //Go through the entire list of packages
                foreach (IDAL.DO.Package item in mayDal.GetPackageList())
                {
                    if (item.priority == Priorities.emergency)
                    {

                    }
                }
            }
        }
        /// <summary>
        /// Delivery of a package by skimmer
        /// </summary>
        /// <param name="id"></param>
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
            //Only a skimmer that has collected but has not yet delivered the package will be able to deliver it
            if (package.PackageCollectionTime == ResetTime && package.TimeArrivalRecipient != ResetTime)
            {
                Customer customer = GetClint(package.IDgets);
                //ChecksSmall Distance Between Skimmer And BaseStation
                IDAL.DO.BaseStation baseStation = ChecksSmallDistanceBetweenSkimmerAndBaseStation(skimmer);
                //Update location to the location of the shipping destination
                Location locationBaseStation = new Location
                {
                    Longitude = baseStation.Longitude,
                    Latitude = baseStation.Latitude
                };
                //Update battery status according to the distance between the original location and the location of the shipping destination
                //,Change skimmer status to available, Update delivery time
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

