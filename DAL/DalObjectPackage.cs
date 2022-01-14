using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace DalObject
{
    public partial class DalObject: DalApi.IDal
    {
        internal static Random r = new Random();
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpadteP(Package p)
        {
            for (int i = 0; i < DataSource.ListPackage.Count; i++)
            {
                if (DataSource.ListPackage[i].ID == p.ID)
                {
                    DataSource.ListPackage[i] = p;
                    break;
                }
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddPackage(Package p)//Add a package
        {
            if (DataSource.ListPackage.Exists(item => item.ID == p.ID))//If finds an existing Package throws an error.
            {
                throw new ExistsInSystemException($"Package {p.ID} Save to system", Severity.Mild);
            }
            p.ID = global::DalObject.DataSource.Config.IDPackage++;
            DataSource.ListPackage.Add(p);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Package GetPackage(int idp)//Package view by appropriate ID
        {
            if (!DataSource.ListPackage.Exists(item => item.ID == idp))
            {
                throw new IdDoesNotExistException($"id : {idp} does not exist!!", Severity.Mild);
            }
            return DataSource.ListPackage.FirstOrDefault(p => p.ID == idp);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Package> GetPackageList(Func<Package, bool> predicate = null)//Displays a list of Package
        {
            if (predicate == null)
                return DataSource.ListPackage.Take(DataSource.ListPackage.Count).ToList();
            return DataSource.ListPackage.Where(predicate).ToList();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeletePackage(int IDp)//Add a package
        {
            if (!DataSource.ListPackage.Exists(item => item.ID == IDp))//If finds an existing Package throws an error.
            {
                throw new IdDoesNotExistException($"Package {IDp} dont Save to system", Severity.Mild);
            }
            DataSource.ListPackage.RemoveAll(item => item.ID == IDp);
        }
    }
}
