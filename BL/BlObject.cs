using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;
using DalObject;
namespace IBL
{
    public class BlObject : IBL.IBL
    {
        private IDal mayDal;
        //static DalObject.DalObject mydal = new DalObject.DalObject();
        private List<IBL.BO.Skimmer> Skimmers;
        public BlObject()
        {
            Skimmers = new List<IBL.BO.Skimmer>();
            mayDal = new DalObject.DalObject();
        }


        public void AddBaseStation(IBL.BO.BaseStation newBaseStation)
        {
            try
            {
                mayDal.AddBaseStation(newBaseStation);
            }
            catch (Exception)
            {

                throw;
            }
            if (DataSource.ListBaseStation.Exists(item => item.UniqueID == b.UniqueID))//If finds an existing base station throws an error.
            {
                throw new BaseStationException($"Person {b.UniqueID} Save to system", Severity.Mild);
            }
            DataSource.ListBaseStation.Add(b);
            throw new NotImplementedException();
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
        public Customer GetCustomer(int id)
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
        public IBL.BO.BaseStation GetBeseStation(int id)
        {
            IDAL.DO.BaseStation somoeBaseStation;
            try
            {
                somoeBaseStation = mayDal.GetBaseStation(id);
            }
            catch (IDAL.DO.BaseStationException cex)
            {
                throw new BLBaseStationException(cex.Message + " from dal");
            }
            return new IBL.BO.BaseStation
            {
                Id = somoeBaseStation.UniqueID,
                Name = somoeBaseStation.StationName,
                location = new Location { Latitude = somoeBaseStation.Latitude, Longitude = somoeBaseStation.Longitude },
                SeveralClaimPositionsVacant = somoeBaseStation.SeveralPositionsArgument,
                ListOfSkimmersCharge = somoeBaseStation.
                };
        }
        public IBL.BO.Skimmer GetSkimmer(int id)
        {
            IDAL.DO.BaseStation somoeSkimmer;
            try
            {
                somoeSkimmer = mayDal.GetSkimmer(id);
            }
            catch (IDAL.DO.QuadocopterException cex)
            {
                throw new BLSkimmerException(cex.Message + " from dal"); ;
            }
            return new IBL.BO.Skimmer
            {
                Id = somoeSkimmer.UniqueID,
                SkimmerModel = somoeSkimmer.SkimmerModel,
                location = new Location { Latitude = somoeSkimmer.Latitude, Longitude = somoeSkimmer.Longitude },
                SeveralClaimPositionsVacant = somoeSkimmer.SeveralPositionsArgument,
                ListOfSkimmersCharge = somoeSkimmer.
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

            return new IBL.BO.Package
            {
                Id = somoePackage.ID,
                SendPackage = somoePackage.sen,
                Phone = somoePackage.Telephone,
                Location = new Location { Latitude = somoeone.Latitude, Longitude = somoeone.Longitude }
            };
        }

    }
}
