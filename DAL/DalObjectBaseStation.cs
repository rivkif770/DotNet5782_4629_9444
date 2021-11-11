using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObjectBaseStation
    {
        public static void AddBaseStation_private(BaseStation b)//Adding a station
        {
            if (DataSource.ListBaseStation.Exists(item => item.UniqueID == b.UniqueID))//If finds an existing base station throws an error.
            {
                throw new BaseStationException($"Person {b.UniqueID} Save to system", Severity.Mild);
            }
            DataSource.ListBaseStation.Add(b);
        }
        public static BaseStation GetBaseStation_private(int IDb)//Base station view by appropriate ID
        {
            if (!DataSource.ListBaseStation.Exists(item => item.UniqueID == IDb))
            {
                throw new BaseStationException($"id : {IDb} does not exist!!", Severity.Mild);
            }
            return DataSource.ListBaseStation.FirstOrDefault(b => b.UniqueID == IDb);
        }
        public static IEnumerable<BaseStation> GetBaseStationList_private()//return a list of base stations
        {
            return DataSource.ListBaseStation.Take(DataSource.ListBaseStation.Count).ToList();
            //return IEnumerable< DataSource.ListBaseStation.ToList();
        }
    }
}
