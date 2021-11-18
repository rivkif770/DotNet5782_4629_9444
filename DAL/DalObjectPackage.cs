using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public partial class DalObject: IDal
    {
        public void AddPackage(Package p)//Add a package
        {
            if (DataSource.ListPackage.Exists(item => item.ID == p.ID))//If finds an existing Package throws an error.
            {
                throw new ExistsInSystemException($"Package {p.ID} Save to system", Severity.Mild);
            }
            p.ID = global::DalObject.DataSource.Config.IDPackage++;
            DataSource.ListPackage.Add(p);
        }
        public Package GetPackage(int idp)//Package view by appropriate ID
        {
            if (!DataSource.ListPackage.Exists(item => item.ID == idp))
            {
                throw new IdDoesNotExistException($"id : {idp} does not exist!!", Severity.Mild);
            }
            return DataSource.ListPackage.FirstOrDefault(p => p.ID == idp);
        }
        public IEnumerable<Package> GetPackageList()//Displays a list of Package
        {
            //return DataSource.ListPackage.ToList();
            return DataSource.ListPackage.Take(DataSource.ListPackage.Count).ToList();
        }
        public IEnumerable<Package> PackagesWithoutSkimmer()//Displays a list of Packages not yet associated with the glider
        {
            List<Package> result = new List<Package>();
            for (int i = 0; i < DataSource.ListPackage.Count; i++)
            {
                if (DataSource.ListPackage[i].IDSkimmerOperation == 0)
                {
                    result.Add(DataSource.ListPackage[i]);
                }
            }
            return result.Take(result.Count).ToList();
        }
        public void DeletePackage(Package p)//Add a package
        {
            if (!DataSource.ListPackage.Exists(item => item.ID == p.ID))//If finds an existing Package throws an error.
            {
                throw new IdDoesNotExistException($"Package {p.ID} dont Save to system", Severity.Mild);
            }
            p.ID = global::DalObject.DataSource.Config.IDPackage--;
            DataSource.ListPackage.Remove(p);
        }
    }
}
