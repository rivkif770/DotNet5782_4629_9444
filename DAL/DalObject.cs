﻿using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObject : IDal
    {
        public DalObject()
        {
            DataSource.Initialize();
        }
        //////////////////////////////////////////////////////////
        public void AddBaseStation(BaseStation b)//Adding a station
        {
            if (DataSource.ListBaseStation.Exists(item => item.UniqueID == b.UniqueID))//If finds an existing base station throws an error.
            {
                throw new BaseStationException($"Person {b.UniqueID} Save to system", Severity.Mild);
            }
            DataSource.ListBaseStation.Add(b);
        }
        public void AddSkimmer(Quadocopter q)//added a skimmer
        {
             DalObjectSkimmer.AddSkimmer_privet(q);
        }
        
        public void AddClient(Client c)//Adding a customer
        {
            if (DataSource.ListClient.Exists(item => item.ID == c.ID))//If finds an existing Customer throws an error.
            {
                throw new BaseStationException($"Customer {c.ID} Save to system", Severity.Mild);
            }
            DataSource.ListClient.Add(c);
        }
        public void AddPackage(Package p)//Add a package
        {
            DalObjectPackage.AddPackage_privet(p);
        }
        //////////////////////////////////////////////////////////
        public void AssignPackageSkimmer(int idp, int idq)//Assign a package to a skimmer
        {
            Quadocopter temp_q = this.GetQuadrocopter(idq);
            //temp_q.SkimmerMode = (DronStatuses)2;//Change the glider position to catch

            DataSource.ListQuadocopter.RemoveAll(quad => quad.IDNumber == idq);//Deleting the old skimmer object
            DataSource.ListQuadocopter.Add(temp_q);//Deleting the new skimmer object includes the change

            Package temp_p = this.GetPackage(idp);
            temp_p.IDSkimmerOperation = idq;//Change the skimmer ID of the package to the associated skimmer ID
            temp_p.TimeAssignGlider = DateTime.Now;//Update time association

            DataSource.ListPackage.RemoveAll(packag => packag.ID == idp);//Deleting the old Package object
            DataSource.ListPackage.Add(temp_p);//Deleting the new Package object includes the change
        }
        public void CollectionPackage(int idp)//Update package collection by skimmer
        {
            Package temp_p = this.GetPackage(idp);
            temp_p.PackageCollectionTime = DateTime.Now;//Update the package collection time from the sender

            DataSource.ListPackage.RemoveAll(packag => packag.ID == idp);//Deleting the old Package object
            DataSource.ListPackage.Add(temp_p);//Deleting the new Package object includes the change
        }
        public void PackageDelivery(int idp)//Delivery of a package to the customer
        {
            Package temp_p = this.GetPackage(idp);
            temp_p.TimeArrivalRecipient = DateTime.Now;//Update the time of arrival of the package to the recipient

            DataSource.ListPackage.RemoveAll(packag => packag.ID == idp);//Deleting the old Package object
            DataSource.ListPackage.Add(temp_p);//Deleting the new Package object includes the change
        }
        public void SendingSkimmerForCharging(int idq, int idBS)//Sending a skimmer for charging at a base station
        {
            Quadocopter temp_q = this.GetQuadrocopter(idq);
            //temp_q.Battery = 0;//Update the battery to 0 percent
            //temp_q.SkimmerMode = (DronStatuses)1;//Skimmer status update for maintenance

            DataSource.ListQuadocopter.RemoveAll(quad => quad.IDNumber == idq);//Deleting the old skimmer object
            DataSource.ListQuadocopter.Add(temp_q);//Deleting the new skimmer object includes the change

            BaseStation temp_BS = this.GetBaseStation(idBS);
            temp_BS.SeveralPositionsArgument = temp_BS.SeveralPositionsArgument - 1;//Update the number of stations available for charging at least 1

            DataSource.ListBaseStation.RemoveAll(Base => Base.UniqueID == idBS);//Deleting the old BaseStation object
            DataSource.ListBaseStation.Add(temp_BS);//Deleting the new BaseStation object includes the change
        }
        public void SkimmerRelease(int idq, int IdBS)//Release skimmer from base charge
        {
            Quadocopter temp_q = this.GetQuadrocopter(idq);
            //temp_q.Battery = 100;//Update the battery to 100 percent
            //temp_q.SkimmerMode = (DronStatuses)0;//Skimmer status update available

            DataSource.ListQuadocopter.RemoveAll(quad => quad.IDNumber == idq);//Deleting the old skimmer object
            DataSource.ListQuadocopter.Add(temp_q);//Deleting the new skimmer object includes the change

            BaseStation temp_BS = this.GetBaseStation(IdBS);
            temp_BS.SeveralPositionsArgument = temp_BS.SeveralPositionsArgument + 1;//Update the number of stations available for charging at more 1

            DataSource.ListBaseStation.RemoveAll(Base => Base.UniqueID == IdBS);//Deleting the old BaseStation object
            DataSource.ListBaseStation.Add(temp_BS);//Deleting the new BaseStation object includes the change
        }
        //////////////////////////////////////////////////////////
        public BaseStation GetBaseStation(int IDb)//Base station view by appropriate ID
        {
            if (!DataSource.ListBaseStation.Exists(item => item.UniqueID == IDb))
            {
                throw new BaseStationException($"id : {IDb} does not exist!!", Severity.Mild);
            }
            return DataSource.ListBaseStation.FirstOrDefault(b => b.UniqueID == IDb);
            // int result = DataSource.ListBaseStation.FindIndex(x => x.UniqueID == IDb);
            //return DataSource.ListBaseStation[result];
        }
        public Client GetClient(int IDc)//Client view by appropriate ID
        {
            if (!DataSource.ListClient.Exists(item => item.ID == IDc))
            {
                throw new ClientException($"id : {IDc} does not exist!!", Severity.Mild);
            }
            return DataSource.ListClient.FirstOrDefault(c => c.ID == IDc);
            //int result = DataSource.ListClient.FindIndex(x => x.ID == IDc);
            //DataSource.ListClient[result].ToString();
        }
        public Quadocopter GetQuadrocopter(int IDq)//Quadrocopter view by appropriate ID
        {
           return DalObjectSkimmer.GetQuadrocopter_privet(IDq);
        }     
        public Package GetPackage(int idp)//Package view by appropriate ID
        {
            return DalObjectPackage.GetPackage_privet(idp);
        }
        //////////////////////////////////////////////////////////
        public IEnumerable<BaseStation> GetBaseStationList()//return a list of base stations
        {
            return DataSource.ListBaseStation.Take(DataSource.ListBaseStation.Count).ToList();
            //return IEnumerable< DataSource.ListBaseStation.ToList();
        }
        public IEnumerable<Quadocopter> GetQuadocopterList()//Displays a list of Skimmer
        {
            return DalObjectSkimmer.GetQuadocopterList_privet();
        }
        public IEnumerable<Client> GetClientList()//Displays a list of Client
        {
            //return DataSource.ListClient.ToList();
            return DataSource.ListClient.Take(DataSource.ListClient.Count).ToList();
        }
        public IEnumerable<Package> GetPackageList()//Displays a list of Package
        {
            return DalObjectPackage.GetPackageList_privet();
        }
        public List<Package> PackagesWithoutSkimmer()//Displays a list of Packages not yet associated with the glider
        {
            return DalObjectPackage.PackagesWithoutSkimmer_privet();
        }
        public List<BaseStation> BaseStationFreeCharging()//Displays a list of Base stations with available charging stations
        {
            List<BaseStation> result = new List<BaseStation>();
            for (int i = 0; i < DataSource.ListBaseStation.Count; i++)
            {
                if (DataSource.ListBaseStation[i].SeveralPositionsArgument != 0)
                {
                    result.Add(DataSource.ListBaseStation[i]);
                }
            }
            return result;
        }
    }
}
