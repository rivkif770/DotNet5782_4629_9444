using IDAL.DO;
using IBL.BO;
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
        /// Adding a customer
        /// </summary>
        /// <param name="newCustomer"></param>
        public void AddCustomer(IBL.BO.Customer newCustomer)
        {
            IDAL.DO.Client temp_c = new IDAL.DO.Client
            {
                ID = newCustomer.Id,
                Name = newCustomer.Name,
                Telephone = newCustomer.Phone,
                Latitude = newCustomer.Location.Latitude,
                Longitude = newCustomer.Location.Longitude
            };

            try
            {
                mayDal.AddClient(temp_c);
            }
            catch (ExistsInSystemException exception)
            {
                throw new ExistsInSystemException_BL($"Person {temp_c.ID} Save to system", Severity.Mild);
            }
        }
        public Customer GetCustomer(int id)
        {
            IDAL.DO.Client somoeone;
            try
            {
                somoeone = mayDal.GetClient(id);
            }
            catch (IDAL.DO.IdDoesNotExistException cex)
            {
                throw new IdDoesNotExistException_BL(cex.Message + " from dal");
            }
            List<PackageAtCustomer> sentParcels = new List<PackageAtCustomer>();
            foreach (IDAL.DO.Package item in mayDal.GetPackageList())
            {
                if (item.IDgets == somoeone.ID)
                {
                    PackageAtCustomer packageAtCustomer = new PackageAtCustomer
                    {
                        Id = item.ID,
                        WeightCategory = (Weight)WeightConversion(item),
                        priority = (Priority)PriorityConversion(item),
                        PackageMode = (ParcelStatus)ReturnsSkimmerMode(item),
                        customerInParcel = new CustomerInParcel
                        {
                            Id = item.IDgets,
                            Name = GetCustomer(item.IDgets).Name
                        }
                    };
                    sentParcels.Add(packageAtCustomer);
                }
            }
            List<PackageAtCustomer> ReceiveParcels = new List<PackageAtCustomer>();
            foreach (IDAL.DO.Package item in mayDal.GetPackageList())
            {
                if (item.IDgets == somoeone.ID)
                {
                    PackageAtCustomer packageAtCustomer = new PackageAtCustomer
                    {
                        Id = item.ID,
                        WeightCategory = (Weight)WeightConversion(item),
                        priority = (Priority)PriorityConversion(item),
                        PackageMode = (ParcelStatus)ReturnsSkimmerMode(item),
                        customerInParcel = new CustomerInParcel
                        {
                            Id = item.IDSender,
                            Name = GetCustomer(item.IDgets).Name
                        }
                    };
                    ReceiveParcels.Add(packageAtCustomer);
                }
            }
            return new Customer
            {
                Id = somoeone.ID,
                Name = somoeone.Name,
                Phone = somoeone.Telephone,
                Location = new Location { Latitude = somoeone.Latitude, Longitude = somoeone.Longitude },
                SentParcels = sentParcels,
                ReceiveParcels = ReceiveParcels,
            };
        }
        /// <summary>
        /// Update customer data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        public void UpdateCustomerData(int id, string name, string phone)
        {
            IBL.BO.Customer customer = GetCustomer(id); 
            customer.Id = id;
            //If the "Name" field is not empty, update the Name field
            if (name != "")
                customer.Name = name;
            //If the "Phone" field is not blank, update the phone field
            if (phone != "")
            {
                customer.Phone= phone;
            }
            //Adding a new customer with the new data and deleting the old customer with the out-of-date data
            AddCustomer(customer);
            mayDal.DeleteClient(id);
        }
        /// <summary>
        /// Returns skimmer mode
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int ReturnsSkimmerMode (IDAL.DO.Package p)
        {
            if (p.TimeArrivalRecipient != null)
                return 3;
            if (p.PackageCollectionTime != null)
                return 2;
            if (p.TimeAssignGlider != null)
                return 1;
            if (p.PackageCreationTime != null)
                return 0;
            return -1;
        }
        /// <summary>
        /// Weight conversion
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int WeightConversion(IDAL.DO.Package p)
        {
            if (p.Weight == WeightCategories.low)
                return 2;
            if (p.Weight == WeightCategories.middle)
                return 1;
            if (p.Weight == WeightCategories.heavy)
                return 0;
            return -1;
        }
        /// <summary>
        /// Priority conversion
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int PriorityConversion(IDAL.DO.Package p)
        {
            if (p.priority == Priorities.regular)
                return 0;
            if (p.priority == Priorities.fast)
                return 1;
            if (p.priority == Priorities.emergency)
                return 2;
            return -1;
        }
    }
}
