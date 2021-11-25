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
                throw new ExistsInSystemExceptionBL($"Person {temp_p.ID} Save to system", Severity.Mild);
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
                throw new IdDoesNotExistExceptionBL(cex.Message + " from dal");
            }
            CustomerInParcel customerSender = new CustomerInParcel
            {
                Id = somoePackage.IDSender,
                Name = GetCustomer(somoePackage.IDSender).Name
            };
            CustomerInParcel customergets = new CustomerInParcel
            {
                Id = somoePackage.IDgets,
                Name = GetCustomer(somoePackage.IDgets).Name
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
            SkimmerToList skimmer = skimmersList.Find(item => item.Id == id);
            IBL.BO.Package package;
            package = GetPackage(skimmer.PackageNumberTransferred);
            int idc = package.SendPackage.Id;
            Location locationsend = GetCustomer(idc).Location;
            //Only a skimmer that delivers a package that has been associated with it but has not yet been collected will be able to pick it up
            if (skimmer.SkimmerStatus == SkimmerStatuses.shipping && package.AssignmentTime != null && package.CollectionTime == null)
            {
                //Update battery status according to the distance between the original location and the sender location
                double distance = Tools.Utils.GetDistance(skimmer.CurrentLocation.Longitude, skimmer.CurrentLocation.Latitude, locationsend.Longitude, locationsend.Latitude);
                skimmer.BatteryStatus = (skimmer.BatteryStatus) - (distance * Free);
                //Update location to sender location
                skimmer.CurrentLocation = locationsend;
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
            SkimmerToList skimmer = skimmersList.Find(item => item.Id == id);
            List<IDAL.DO.Package> dalPackages = new List<IDAL.DO.Package>(mayDal.GetPackageList().ToList().FindAll(x => (int)(x.Weight) <= (int)skimmer.WeightCategory && x.TimeAssignGlider == null));
            IDAL.DO.Package package = new IDAL.DO.Package();
            int priority = 2, weight = (int)skimmer.WeightCategory;
            bool flag = true;
            while(flag)
            {
                List<IDAL.DO.Package> filteredPackage = dalPackages.FindAll(p => p.priority == (IDAL.DO.Priorities)priority);
                if(filteredPackage.Count()==0)
                {
                    if(priority!=0)
                    {
                        priority--;
                        weight = (int)skimmer.WeightCategory;
                        continue;
                    }
                    else throw new SkimmerExistsInSystemException_BL($"לא נמצאה חבילה לרחפן", Severity.Mild);
                }
                filteredPackage = filteredPackage.FindAll(p => p.Weight == (IDAL.DO.WeightCategories)weight);
                if(filteredPackage.Count()==0)
                {
                    if(weight!=0)
                    {
                        weight--;
                        continue;
                    }
                    else
                    {
                        if (priority != 0)
                            priority--;
                        weight = (int)skimmer.WeightCategory;
                        continue;
                    }
                }
                package = ChecksSmallDistanceBetweenSkimmerAndPackage(skimmer);
                IDAL.DO.Client senderClient = mayDal.GetClient(package.IDSender);
                IDAL.DO.Client getClient = mayDal.GetClient(package.IDgets);
                double minBattery = MinimumPaymentToGetToThePackage(skimmer, senderClient);
                minBattery += MinimalLoadingPerformTheShipmentAndArriveForLoading(skimmer, senderClient);
                minBattery = Math.Ceiling(minBattery);
                if (minBattery + 1 > skimmer.BatteryStatus)
                    dalPackages.Remove(package);
                else flag = false;
            }
            int index = skimmersList.FindIndex(d => d.Id == skimmer.Id);
            skimmersList[index].SkimmerStatus = SkimmerStatuses.shipping;
            package.IDSkimmerOperation = skimmer.Id;
            package.TimeAssignGlider = DateTime.Now;
            mayDal.UpadteP(package);
        }
        private IDAL.DO.Package ChecksSmallDistanceBetweenSkimmerAndPackage(SkimmerToList skimmer)
        {
            IDAL.DO.Package package = default;
            double smallDistance = Double.MaxValue;

            foreach (var p in mayDal.GetPackageList())
            {
                Customer customer = GetCustomer(p.IDSender);
                double dist = Tools.Utils.GetDistance(skimmer.CurrentLocation.Longitude, skimmer.CurrentLocation.Latitude, customer.Location.Longitude, customer.Location.Latitude);
                if (dist < smallDistance)
                {
                    smallDistance = dist;
                    package = p;
                }
            }
            return mayDal.GetPackage(package.ID);
        }
        /// <summary>
        /// Delivery of a package by skimmer
        /// </summary>
        /// <param name="id"></param>
        public void DeliveryOfPackageBySkimmer(int id)
        {
            SkimmerToList skimmer = skimmersList.Find(item => item.Id == id);
            IBL.BO.Package package = GetPackage(skimmer.PackageNumberTransferred);
            Location locationGets = GetCustomer(id).Location;
            //Only a skimmer that has collected but has not yet delivered the package will be able to deliver it
            if (package.CollectionTime != null && package.SupplyTime == null)
            {
                double batteryCalculation = BatteryCalculation(skimmer.CurrentLocation, locationGets, package.WeightCategory);
                skimmer.BatteryStatus = (skimmer.BatteryStatus) - (batteryCalculation);
                //Update location to the location of the shipping destination
                skimmer.CurrentLocation = locationGets;
                skimmer.SkimmerStatus = SkimmerStatuses.free;
                IDAL.DO.Package package1=mayDal.GetPackage(package.Id);
                package.SupplyTime = DateTime.Now;
                mayDal.UpadteP(package1);
            }
            else
                throw new SkimmerExistsInSystemException_BL($"Skimmer {id}does not ship a package that has been associated with it or has already been collected", Severity.Mild);
        }
        public IEnumerable<IBL.BO.PackageToList> GetPackageList()
        {
            List<PackageToList> packageToList = new List<PackageToList>();
            foreach (IDAL.DO.Package item in mayDal.GetPackageList())
            {
                IBL.BO.Package package1 = GetPackage(item.ID);
                PackageToList package = new PackageToList
                {
                    Id = item.ID,
                    CustomerNameSends = package1.SendPackage.Name,
                    CustomerNameGets = package1.ReceivesPackage.Name,
                    WeightCategory = package1.WeightCategory,
                    priority = package1.priority,
                    PackageMode = (ParcelStatus)PackageMode(package1),
                };
                packageToList.Add(package);
            }
            return packageToList.Take(packageToList.Count).ToList();
        }
        private int PackageMode(IBL.BO.Package p)
        {
            if (p.SupplyTime != null)
                return 3;
            if (p.CollectionTime != null)
                return 2;
            if (p.AssignmentTime != null)
                return 1;
            if (p.PackageCreationTime != null)
                return 0;
            return -1;
        }
        public IEnumerable<PackageToList> GetPackagesWithoutSkimmer()
        {
            List<PackageToList> packageToList = new List<PackageToList>();
            foreach (IDAL.DO.Package item in mayDal.GetPackageList())
            {
                IBL.BO.Package package1 = GetPackage(item.ID);
                if(package1.AssignmentTime==null)
                {
                    PackageToList package = new PackageToList
                    {
                        Id = item.ID,
                        CustomerNameSends = package1.SendPackage.Name,
                        CustomerNameGets = package1.ReceivesPackage.Name,
                        WeightCategory = package1.WeightCategory,
                        priority = package1.priority,
                        PackageMode = (ParcelStatus)PackageMode(package1),
                    };
                    packageToList.Add(package);
                }               
            }
            return packageToList.Take(packageToList.Count).ToList();
        }
    }

}

