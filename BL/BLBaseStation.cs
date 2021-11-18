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
        public void AddBaseStation(IBL.BO.BaseStation newBaseStation)
        {
            IDAL.DO.BaseStation temp_BS = new IDAL.DO.BaseStation
            {
                UniqueID = newBaseStation.Id,
                StationName = newBaseStation.Name,
                SeveralPositionsArgument = newBaseStation.SeveralClaimPositionsVacant,
                Latitude = newBaseStation.location.Latitude,
                Longitude = newBaseStation.location.Longitude
            };

            try
            {
                mayDal.AddBaseStation(temp_BS);
            }
            catch (ExistsInSystemException exception)
            {
                throw new ExistsInSystemException_BL($"Person {temp_BS.UniqueID} Save to system", Severity.Mild);
            }
        }

        public void AddCustomer(Customer newCustomer)
        {
            throw new NotImplementedException();
        }

        public IBL.BO.BaseStation GetBeseStation(int id)
        {
            IDAL.DO.BaseStation somoeBaseStation;
            try
            {
                somoeBaseStation = mayDal.GetBaseStation(id);
            }
            catch (IDAL.DO.IdDoesNotExistException cex)
            {
                throw new IdDoesNotExistException_BL(cex.Message + " from dal");
            }
            return new IBL.BO.BaseStation
            {
                Id = somoeBaseStation.UniqueID,
                Name = somoeBaseStation.StationName,
                location = new Location { Latitude = somoeBaseStation.Latitude, Longitude = somoeBaseStation.Longitude },
                SeveralClaimPositionsVacant = somoeBaseStation.SeveralPositionsArgument,
                ListOfSkimmersCharge =
                };
        }

        public Customer GetCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateBaseStation(int id, string name, string countOfChargingStations)
        {
            IBL.BO.BaseStation baseStation = GetBeseStation(id);
            mayDal.DeleteBaseStation(id);
            int TotalChargeAmount ,NumberOfSkimmersInCharge;
            baseStation.Id = id;
            if (name != "")
                baseStation.Name = name;
            if (countOfChargingStations != "")
            {
                TotalChargeAmount = int.Parse(countOfChargingStations);
                NumberOfSkimmersInCharge = baseStation.ListOfSkimmersCharge.Count();
                baseStation.SeveralClaimPositionsVacant = TotalChargeAmount - NumberOfSkimmersInCharge;
            }
            AddBaseStation(baseStation);
        }
    }
}

