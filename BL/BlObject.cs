//using IBL.BO;
using System;
using IBL.BO;
using System.Collections.Generic;
using IDAL.DO;
namespace BL
{
    public class BlObject /*: IBL.BO.IBL*/
    {
        private IDal mayDal;
        private List<IBL.BO.Skimmer> Skimmers;
        public BlObject()
        {
            Skimmers = new List<IBL.BO.Skimmer>();
            mayDal = new DalObject.DalObject();
        }
        public void AddCustomer(IDAL.DO.Client newCustomer)
        {
            try
            {
                mayDal.AddClient(newCustomer);
            }
            catch (Exception)
            {

                throw;
            }
            //if (DataSource.ListBaseStation.Exists(item => item.UniqueID == b.UniqueID))//If finds an existing base station throws an error.
            //{
            //    throw new BaseStationException($"Person {b.UniqueID} Save to system", Severity.Mild);
            //}
            //DataSource.ListBaseStation.Add(b);
            //throw new NotImplementedException();
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
        public Customer GetPackage(int id)
        {
            IDAL.DO.Package somoePackage;
            try
            {
                somoePackage = mayDal.GetPackage(id);
            }
            catch (IDAL.DO.PackageException cex)
            {
                throw new BLPackageException(cex.Message + " from dal");
            }
            return new Package
            {
                Id = somoePackage.ID,
                SendPackage = somoePackage.sen,
                Phone = somoePackage.Telephone,
                Location = new Location { Latitude = somoeone.Latitude, Longitude = somoeone.Longitude }
            };
        }

    }
}
