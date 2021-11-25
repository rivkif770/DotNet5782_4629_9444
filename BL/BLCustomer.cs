﻿using IDAL.DO;
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
            IDAL.DO.Client tempC = new IDAL.DO.Client
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
                throw new ExistsInSystemExceptionBL($"Person {tempC.ID} Save to system", Severity.Mild);
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
                throw new IdDoesNotExistExceptionBL(cex.Message + " from dal");
            }
            List<PackageAtCustomer> sentParcels = new List<PackageAtCustomer>();
            foreach (IDAL.DO.Package item in mayDal.GetPackageList())
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
                            Id = ReturnsCustomerContrary(id,item).Id,
                            Name = ReturnsCustomerContrary(id, item).Name
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
                        WeightCategory = (Weight)(int)item.Weight,
                        priority = (Priority)(int)item.priority,
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
        private Customer ReturnsCustomerContrary(int id,IDAL.DO.Package package)
        {
            if (id == package.IDSender)
                return GetCustomer(package.IDgets);
            return GetCustomer(package.IDSender);
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
                customer.Phone = phone;
            //Adding a new customer with the new data and deleting the old customer with the out-of-date data
            Client client = new Client
            {
                ID = id,
                Name = name,
                Telephone = phone
            };
            mayDal.UpadteC(client);
        }
        /// <summary>
        /// Returns skimmer mode
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int ReturnsSkimmerMode(IDAL.DO.Package p)
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
        public IEnumerable<CustomerToList> GetCustomerList()
        {
            List<CustomerToList> customerToList= new List<CustomerToList>();
            foreach (IDAL.DO.Client item in mayDal.GetClientList())
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
    }
}
