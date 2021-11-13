using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class BlObjectCustomer
    {
        public Customer GetCustomer_Privet(int id)
        {
            IDAL.DO.Client somoeone;
            try
            {
                somoeone =  mayDal.GetClient(id);
            }
            catch (IDAL.DO.ClientException cex)
            {
                throw new BLClientException(cex.Message + " from dal");
            }
            return new Customer
            {
                Id = somoeone.ID,
                Name = somoeone.Name,
                Phone = somoeone.Telephone,
                Location = new Location { Latitude = somoeone.Latitude, Longitude = somoeone.Longitude }
            };
        }
    }
}
