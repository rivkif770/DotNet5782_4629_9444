using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace DalObject
{
    public partial class DalObject :DalApi.IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpadteB(BaseStation b)
        {
            for (int i = 0; i < DataSource.ListBaseStation.Count; i++)
            {
                if (DataSource.ListBaseStation[i].UniqueID == b.UniqueID)
                {
                    DataSource.ListBaseStation[i] = b;
                    break;
                }
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddBaseStation(BaseStation b)//Adding a station
        {
            if (DataSource.ListBaseStation.Exists(item => item.UniqueID == b.UniqueID))//If finds an existing base station throws an error.
            {
                throw new ExistsInSystemException($"BaseStation {b.UniqueID} Save to system", Severity.Mild);
            }
            DataSource.ListBaseStation.Add(b);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation GetBaseStation(int IDb)//Base station view by appropriate ID
        {
            if (!DataSource.ListBaseStation.Exists(item => item.UniqueID == IDb))
            {
                throw new IdDoesNotExistException($"id : {IDb} does not exist!!", Severity.Mild);
            }
            return DataSource.ListBaseStation.FirstOrDefault(b => b.UniqueID == IDb);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> GetBaseStationList(Func<BaseStation,bool> predicate = null)//return a list of base stations
        {
            if(predicate==null)
                return DataSource.ListBaseStation.Take(DataSource.ListBaseStation.Count).ToList();
            return DataSource.ListBaseStation.Where(predicate).ToList();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteBaseStation(int idb)//Adding a station
        {
            if (!DataSource.ListBaseStation.Exists(item => item.UniqueID == idb))//If finds an existing base station throws an error.
            {
                throw new IdDoesNotExistException($"BaseStation {idb} dont Save to system", Severity.Mild);
            }
            DataSource.ListBaseStation.RemoveAll(item => item.UniqueID == idb);
        }
   
    }
}
