//using IBL.BO;
using System;
using DalObject;
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
        public void AddBaseStation(IBL.BO.BaseStation newBaseStation)
        {
            try
            {
                mayDal.AddBaseStation(newBaseStation)
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
        public IBL.BO.BaseStation GetBeseStation(int id)
        {
            IDAL.DO.BaseStation somoeBaseStation;
            try
            {
                somoeBaseStation = mayDal.GetBaseStation(id);
            }
            catch (IDAL.DO.BaseStationException cex)
            {
                throw new BLBaseStationException(cex.Message+" from dal");
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
        public IBL.BO.BaseStation GetSkimmer(int id)
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
            return new IBL.BO.BaseStation
            {
                Id = somoeSkimmer.UniqueID,
                SkimmerModel = somoeSkimmer.SkimmerModel,
                location = new Location { Latitude = somoeSkimmer.Latitude, Longitude = somoeSkimmer.Longitude },
                SeveralClaimPositionsVacant = somoeSkimmer.SeveralPositionsArgument,
                ListOfSkimmersCharge = somoeSkimmer.
            };
        }
    }
}
