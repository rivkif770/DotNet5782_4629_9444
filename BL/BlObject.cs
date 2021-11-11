using IBL.BO;
using System;
using System.Collections.Generic;

namespace BL
{
    public class BlObject : IBL.BO.IBL
    {
        internal static List<SkimmerToList> listSkimmer= new List<SkimmerToList>();

        IDAL.DO.IDal dal;
        public BlObject()
        {
            dal = new DalObject.DalObject();
        }
        public Customer GetCustomer(int id)
        {
            IDAL.DO.Client somoeone;
            try
            {
                somoeone = dal.GetClient(id);
            }
            catch (IDAL.DO.ClientException cex)
            {
                //TO DO
                throw;
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
