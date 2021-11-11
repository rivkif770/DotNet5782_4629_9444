﻿using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObjectPackage
    {
        public static void AddPackage_privet(Package p)//Add a package
        {
            if (DataSource.ListPackage.Exists(item => item.ID == p.ID))//If finds an existing Package throws an error.
            {
                throw new BaseStationException($"Package {p.ID} Save to system", Severity.Mild);
            }
            p.ID = global::DalObject.DataSource.Config.IDPackage++;
            DataSource.ListPackage.Add(p);
        }
        public static Package GetPackage_privet(int idp)//Package view by appropriate ID
        {
            if (!DataSource.ListPackage.Exists(item => item.ID == idp))
            {
                throw new PackageException($"id : {idp} does not exist!!", Severity.Mild);
            }
            return DataSource.ListPackage.FirstOrDefault(p => p.ID == idp);
        }
        public static IEnumerable<Package> GetPackageList_privet()//Displays a list of Package
        {
            //return DataSource.ListPackage.ToList();
            return DataSource.ListPackage.Take(DataSource.ListPackage.Count).ToList();
        }
        public static List<Package> PackagesWithoutSkimmer_privet()//Displays a list of Packages not yet associated with the glider
        {
            List<Package> result = new List<Package>();
            for (int i = 0; i < DataSource.ListPackage.Count; i++)
            {
                if (DataSource.ListPackage[i].IDSkimmerOperation == 0)
                {
                    result.Add(DataSource.ListPackage[i]);
                }
            }
            return result;
        }
    }
}
