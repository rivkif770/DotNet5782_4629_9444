//using IBL.BO;
using System;
using IBL.BO;
//using System.Collections.Generic;
using IDAL.DO;
namespace BL
{
    public class BlObject /*: IBL.BO.IBL*/
    {
        IDAL.DO.IDal mayDal;
        public BlObject()
        {
            mayDal = new DalObject.DalObject();
        }
        public  Customer GetCustomer(int id)
        {
            IDAL.DO.Client somoeone;
            try
            {
                somoeone = mayDal.GetClient(id);
            }
            catch (IDAL.DO.ClientException cex)
            {
                Console.WriteLine(cex);
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

        public void AddBaseStation(IBL.BO.BaseStation newBaseStation)
        {
            if (DataSource.ListBaseStation.Exists(item => item.UniqueID == b.UniqueID))//If finds an existing base station throws an error.
            {
                throw new BaseStationException($"Person {b.UniqueID} Save to system", Severity.Mild);
            }
            DataSource.ListBaseStation.Add(b);
            throw new NotImplementedException();
        }
    }
}
