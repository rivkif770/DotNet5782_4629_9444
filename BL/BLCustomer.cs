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
        public void AddClint(IBL.BO.Customer newCustomer)
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
        public Customer GetClint(int id)
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
            return new Customer
            {
                Id = somoeone.ID,
                Name = somoeone.Name,
                Phone = somoeone.Telephone,
                Location = new Location { Latitude = somoeone.Latitude, Longitude = somoeone.Longitude },
                SentParcels = somoeone.,
                ReceiveParcels = somoeone.,
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
    }
}
