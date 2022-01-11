using DO;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BL
{
    public partial class BL : BlApi.IBL
    {
        /// <summary>
        /// Adding a customer
        /// </summary>
        /// <param name="newCustomer"></param>
        public void AddCustomer(BO.Customer newCustomer)
        {
            DO.Client tempC = new DO.Client
            {
                ID = newCustomer.Id,
                Name = newCustomer.Name,
                Telephone = newCustomer.Phone,
                Latitude = newCustomer.Location.Latitude,
                Longitude = newCustomer.Location.Longitude
            };

            try
            {
                mayDal.AddClient(tempC);
            }
            catch (ExistsInSystemException exception)
            {
                throw new ExistsInSystemExceptionBL(exception.Message +" from dal");
            }
        }
        /// <summary>
        /// Returns a Customer type entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Customer GetCustomer(int id)
        {
            DO.Client somoeone;
            try
            {
                somoeone = mayDal.GetClient(id);
            }
            catch (DO.IdDoesNotExistException cex)
            {
                throw new IdDoesNotExistExceptionBL(cex.Message + " from dal");
            }
            List<PackageAtCustomer> sentParcels = new List<PackageAtCustomer>();
            foreach (DO.Package item in mayDal.GetPackageList())
            {
                if (item.IDSender == somoeone.ID)
                {
                    PackageAtCustomer packageAtCustomer = new PackageAtCustomer
                    {
                        Id = item.ID,
                        WeightCategory = (Weight)(int)item.Weight,
                        priority = (Priority)(int)item.priority,
                        PackageMode = (ParcelStatus)ReturnsSkimmerMode(item),
                        customerInParcel = new CustomerInParcel
                        {
                            Id = ReturnsCustomerContrary(id,item).ID,
                            Name = ReturnsCustomerContrary(id, item).Name
                        }
                    };
                    sentParcels.Add(packageAtCustomer);
                }
            }
            List<PackageAtCustomer> ReceiveParcels = new List<PackageAtCustomer>();
            foreach (DO.Package item in mayDal.GetPackageList())
            {
                if (item.IDgets == somoeone.ID)
                {
                    PackageAtCustomer packageAtCustomer = new PackageAtCustomer
                    {
                        Id = item.ID,
                        WeightCategory = (Weight)(int)item.Weight,
                        priority = (Priority)(int)item.priority,
                        PackageMode = (ParcelStatus)ReturnsSkimmerMode(item),
                        customerInParcel = new CustomerInParcel
                        {
                            Id = item.IDSender,
                            Name = mayDal.GetClient(item.IDgets).Name
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
        /// Returns to customer in contrast (the other side of the package - the recipient for the sender and the sender for the recipient)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        private Client ReturnsCustomerContrary(int id, DO.Package package)
        {
            if (id == package.IDSender)
                return mayDal.GetClient(package.IDgets);
            return mayDal.GetClient(package.IDSender);
        }
        /// <summary>
        /// Update customer data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        public void UpdateCustomerData(int id, string name, string phone)
        {
            BO.Customer customer = GetCustomer(id); 
            customer.Id = id;
            //If the "Name" field is not empty, update the Name field
            if (name != "")
                customer.Name = name;
            //If the "Phone" field is not blank, update the phone field
            if (phone != "")
                customer.Phone = phone;
            //Adding a new customer with the new data and deleting the old customer with the out-of-date data
            Client client = new Client
            {
                ID = customer.Id,
                Name = customer.Name,
                Telephone = customer.Phone,
                Latitude = customer.Location.Latitude,
                Longitude = customer.Location.Longitude
            };
            mayDal.UpadteC(client);
        }
        /// <summary>
        /// Returns skimmer mode
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int ReturnsSkimmerMode(DO.Package p)
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
        /// Returns an entity of the Customer list type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerToList> GetCustomerList()
        {
            List<CustomerToList> customerToList= new List<CustomerToList>();
            foreach (DO.Client item in mayDal.GetClientList())
            {
                CustomerToList customer = new CustomerToList
                {
                    Id = item.ID,
                    Name = item.Name,
                    Phone = item.Telephone,
                    ParcelSentAndDelivered = mayDal.GetPackageList().Count(x => x.IDSender == item.ID && x.PackageCreationTime != null && x.TimeAssignGlider != null),
                    ParcelSentAndNotDelivered = mayDal.GetPackageList().Count(x => x.IDSender == item.ID && x.PackageCreationTime != null && x.TimeAssignGlider == null),
                    PackagesHeReceived = GetCustomer(item.ID).ReceiveParcels.Count(),
                    PackagesOnTheWayToCustomer =mayDal.GetPackageList().Count(x => x.IDSender == item.ID && x.PackageCollectionTime != null && x.TimeAssignGlider == null)
                };
                customerToList.Add(customer);
            }
            return customerToList.Take(customerToList.Count).ToList();
        }
        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCustomer(int id)
        {
            try
            {
                mayDal.DeleteClient(id);
            }
            catch (IdDoesNotExistException exception)
            {
                throw new ExistsInSystemExceptionBL(exception.Message + " from dal");
            }
        }
        /// <summary>
        /// Get Customer In Parcel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BO.CustomerInParcel GetCustomerInParcel(int id)
        {
            try
            {
                CustomerInParcel customerInParcel = new CustomerInParcel
                {
                    Id = id,
                    Name = GetCustomer(id).Name
                };
                return customerInParcel;
            }
            catch (Exception cex)
            {

                throw new IdDoesNotExistExceptionBL(cex.Message + " from dal"); ;
            }


        }
        public IEnumerable<PackageAtCustomer> GetSentParcels(BO.Customer customer)
        {
            List<PackageAtCustomer> result = new List<PackageAtCustomer>();
            result = customer.SentParcels;
            return result;
        }
        public IEnumerable<PackageAtCustomer> GetReceiveParcels(Customer customer)
        {
            List<PackageAtCustomer> result = new List<PackageAtCustomer>();
            result = customer.ReceiveParcels;
            return result;
        }
        public Customer GetCustomerListID(int id, string name)
        {
            try
            {
                foreach (DO.Client item in mayDal.GetClientList())
                {
                    if (id == item.ID)
                    {
                        if (item.Name == name) return GetCustomer(id);
                    }
                }
            }
            catch (Exception )
            {

                throw new IdDoesNotExistExceptionBL($"{ id }does exist");
            }
            return GetCustomer(id);
        }
    }

}
