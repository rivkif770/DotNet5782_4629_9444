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
        /// Adding a skimmer
        /// </summary>
        /// <param name="newBaseStation"></param>
        public void AddBaseStation(IBL.BO.BaseStation newBaseStation)
        {
            IDAL.DO.BaseStation temp_BS = new IDAL.DO.BaseStation
            {
                UniqueID = newBaseStation.Id,
                StationName = newBaseStation.Name,
                SeveralPositionsArgument = newBaseStation.SeveralClaimPositionsVacant,
                Latitude = newBaseStation.Location.Latitude,
                Longitude = newBaseStation.Location.Longitude
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
                Location = new Location { Latitude = somoeBaseStation.Latitude, Longitude = somoeBaseStation.Longitude },
                SeveralClaimPositionsVacant = somoeBaseStation.SeveralPositionsArgument,
                ListOfSkimmersCharge =
                };
        }
        /// <summary>
        /// Update station data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="countOfChargingStations"></param>
        public void UpdateBaseStation(int id, string name, string countOfChargingStations)
        {
            IBL.BO.BaseStation baseStation = GetBeseStation(id);
            mayDal.DeleteBaseStation(id);
            int TotalChargeAmount ,NumberOfSkimmersInCharge;
            baseStation.Id = id;
            //If the input in the "Name" field is not blank, update the name field
            if (name != "")
                baseStation.Name = name;
            //If the input in the "Total amount of charging stations" field is not empty, update the charging stations field by calculating the number of skimmers in charging and the number of available charging stations
            if (countOfChargingStations != "")
            {
                TotalChargeAmount = int.Parse(countOfChargingStations);
                NumberOfSkimmersInCharge = baseStation.ListOfSkimmersCharge.Count();
                baseStation.SeveralClaimPositionsVacant = TotalChargeAmount - NumberOfSkimmersInCharge;
                int help = int.Parse(countOfChargingStations);
                GetBeseStation(id).Name = name;
            }
            AddBaseStation(baseStation);
        }

    }
}

