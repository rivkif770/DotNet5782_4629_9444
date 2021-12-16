using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    public partial class BL : BlApi.IBL
    {
        /// <summary>
        /// Adding a package, all times initialized to zero time except for a creation date that will be initialized to DateTime.Now, the glider will be initialized to null
        /// </summary>
        /// <param name="newPackage"></param>
        public void AddPackage(BO.Package newPackage)
        {
            DO.Package tempP = new DO.Package
            {
                IDSender = newPackage.SendPackage.Id,
                IDgets = newPackage.ReceivesPackage.Id,
                Weight = (WeightCategories)newPackage.WeightCategory,
                priority = (Priorities)newPackage.priority,
                PackageCreationTime = DateTime.Now,
                IDSkimmerOperation = 0,
            };
            try
            {
                mayDal.AddPackage(tempP);
            }
            catch (ExistsInSystemException exception)
            {
                throw new ExistsInSystemExceptionBL(exception.Message + " from dal");
            }
        }
        /// <summary>
        /// Returns a package type entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BO.Package GetPackage(int id)
        {
            DO.Package somoePackage;
            try
            {
                somoePackage = mayDal.GetPackage(id);
            }
            catch (DO.IdDoesNotExistException cex)
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
            SkimmerInPackage skimmerPackage;
            if (somoePackage.IDSkimmerOperation != 0)
            {
                skimmerPackage = new SkimmerInPackage
                {
                    Id = somoePackage.IDSkimmerOperation,
                    BatteryStatus = GetSkimmerToList(somoePackage.IDSkimmerOperation).BatteryStatus,
                    Location = GetSkimmerToList(somoePackage.IDSkimmerOperation).CurrentLocation
                };
            }
            else
            {
                skimmerPackage = new SkimmerInPackage
                {
                    Id = somoePackage.IDSkimmerOperation,
                };
            }            
            return new BO.Package
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
            if (skimmer != null)
            {
                BO.Package package;
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
                    DO.Package package1 = mayDal.GetPackage(package.Id);
                    package1.PackageCollectionTime = DateTime.Now;
                    mayDal.UpadteP(package1);
                }
                else
                    throw new SkimmerExceptionBL($"The package was collected or not associated", Severity.Mild);
            }
            else
                throw new SkimmerExceptionBL($"The skimmer {id} is not associated with the package", Severity.Mild);
        }
        /// <summary>
        /// Assigning package to skimmer
        /// </summary>
        /// <param name="id"></param>
        public void AssigningPackageToSkimmer(int id)
        {
            SkimmerToList skimmer = skimmersList.Find(item => item.Id == id);
            //If the glider is available we will associate it, otherwise an exception will be sent.
            if (skimmer.SkimmerStatus == SkimmerStatuses.free)
            {
                //A list containing packages that are equal to or less than the weight of the skimmer and not associated.
                List<DO.Package> dalPackages = new List<DO.Package>(mayDal.GetPackageList().ToList().FindAll(x => (int)(x.Weight) <= (int)skimmer.WeightCategory && x.TimeAssignGlider == null));
                DO.Package package = new DO.Package();
                //Setting the priority to the highest priority and weight to the maximum weight of the skimmer.
                int priority = 2, weight = (int)skimmer.WeightCategory;
                bool flag = true;
                //
                while (flag)
                {
                    //A list that contains a particular priority.
                    List<DO.Package> filteredPackage = dalPackages.FindAll(p => p.priority == (DO.Priorities)priority);
                    //If the list is empty
                    if (filteredPackage.Count() == 0)
                    {
                        //If there is a possibility to go down in priority we will go down and if no exception is sent
                        if (priority != 0)
                        {
                            priority--;
                            weight = (int)skimmer.WeightCategory;
                            continue;
                        }
                        else throw new SkimmerExceptionBL($"No skimmer package found {id}", Severity.Mild);
                    }
                    //We will mark the list for packages with the maximum weight
                    filteredPackage = filteredPackage.FindAll(p => p.Weight == (DO.WeightCategories)weight);
                    //If the list is empty
                    if (filteredPackage.Count() == 0)
                    {
                        //If there is a possibility of losing weight we will lose and if we do not try to lose priority
                        if (weight != 0)
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
                    //We will look for a package with a small hovercraft distance
                    package = ChecksSmallDistanceBetweenSkimmerAndPackage(skimmer , filteredPackage);
                    DO.Client senderClient = mayDal.GetClient(package.IDSender);
                    DO.Client getClient = mayDal.GetClient(package.IDgets);
                    double minBattery = MinimumPaymentToGetToThePackage(skimmer, senderClient);
                    minBattery += MinimalLoadingPerformTheShipmentAndArriveForLoading(skimmer, senderClient, getClient);
                    minBattery = Math.Ceiling(minBattery);
                    //If sending the package consumes more battery than the glider, then we will delete the package otherwise we will associate the glider with the package.
                    if (minBattery + 1 > skimmer.BatteryStatus)
                        dalPackages.Remove(package);
                    else flag = false;
                }
                int index = skimmersList.FindIndex(d => d.Id == skimmer.Id);
                skimmersList[index].SkimmerStatus = SkimmerStatuses.shipping;
                skimmersList[index].PackageNumberTransferred = package.ID; 
                package.IDSkimmerOperation = skimmer.Id;
                package.TimeAssignGlider = DateTime.Now;
                mayDal.UpadteP(package);
            }
            else
                throw new SkimmerExceptionBL($"The skimmer {id} is not available", Severity.Mild);
        }
        /// <summary>
        /// Checks the smallest distance between the skimmer and the package, and returns the package.
        /// </summary>
        /// <param name="skimmer"></param>
        /// <returns></returns>
        private DO.Package ChecksSmallDistanceBetweenSkimmerAndPackage(SkimmerToList skimmer , List<DO.Package> filteredPackage)
        {
            DO.Package package = default;
            double smallDistance = Double.MaxValue;
            foreach (var p in filteredPackage)
            {
                Customer customer = GetCustomer(p.IDSender);
                //Checks the distance between a particular package and the glider.
                double dist = Tools.Utils.GetDistance(skimmer.CurrentLocation.Longitude, skimmer.CurrentLocation.Latitude, customer.Location.Longitude, customer.Location.Latitude);
                //Updates the package with the small distance.
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
            if (skimmer != null)
            {
                BO.Package package = GetPackage(skimmer.PackageNumberTransferred);
                Location locationGets = GetCustomer(package.ReceivesPackage.Id).Location;
                //Only a skimmer that has collected but has not yet delivered the package will be able to deliver it
                if (package.CollectionTime != null && package.SupplyTime == null)
                {
                    double batteryCalculation = BatteryCalculation(skimmer.CurrentLocation, locationGets, package.WeightCategory);
                    skimmer.BatteryStatus = (skimmer.BatteryStatus) - (batteryCalculation);
                    //Update location to the location of the shipping destination
                    skimmer.CurrentLocation = locationGets;
                    skimmer.SkimmerStatus = SkimmerStatuses.free;
                    DO.Package package1 = mayDal.GetPackage(package.Id);
                    package1.TimeArrivalRecipient = DateTime.Now;
                    mayDal.UpadteP(package1);
                }
                else
                    throw new SkimmerExceptionBL($"Skimmer {id}does not ship a package that has been associated with it or has already been collected", Severity.Mild);
            }
            else
                throw new SkimmerExceptionBL($"The skimmer {id} did not collect the package", Severity.Mild);
        }
        /// <summary>
        /// Returns a list of packages.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BO.PackageToList> GetPackageList(Func<BO.PackageToList, bool> predicate = null)
        {
            List<PackageToList> packageToList = new List<PackageToList>();
            foreach (DO.Package item in mayDal.GetPackageList())
            {
                BO.Package package1 = GetPackage(item.ID);
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
            if (predicate == null) 
                return packageToList.Take(packageToList.Count).ToList();
            return packageToList.Where((Func<PackageToList, bool>)predicate).ToList();
        }
        /// <summary>
        /// Returns the skimmer mat.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int PackageMode(BO.Package p)
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
        /// <summary>
        /// Returns a package that does not belong to a skimmer.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PackageToList> GetPackagesWithoutSkimmer()
        {
            IEnumerable<DO.Package> packageFromDal = mayDal.GetPackageList(x => x.TimeAssignGlider == null);
            List<BO.PackageToList> result = new List<BO.PackageToList>();
            foreach (var item in packageFromDal)
            {
                BO.Package package1 = GetPackage(item.ID);
                result.Add(new PackageToList
                {
                    Id = item.ID,
                    CustomerNameSends = package1.SendPackage.Name,
                    CustomerNameGets = package1.ReceivesPackage.Name,
                    WeightCategory = package1.WeightCategory,
                    priority = package1.priority,
                    PackageMode = (ParcelStatus)PackageMode(package1),
                });

            }
            return result;
        }
        /// <summary>
        /// Delete Package
        /// </summary>
        /// <param name="id"></param>
        public void DeletePackage(int id)
        {
            try
            {
                mayDal.DeletePackage(id);
            }
            catch (IdDoesNotExistException exception)
            {
                throw new ExistsInSystemExceptionBL(exception.Message + " from dal");
            }
        }

    }
}