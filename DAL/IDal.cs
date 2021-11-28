using IDAL.DO;
using System;
using System.Collections.Generic;

namespace IDAL.DO
{
    public interface IDal
    {
        public double[] PowerConsumptionRequest();
        void AssignPackageSkimmer(int idp, int idq);
        void CollectionPackage(int idp);
        void PackageDelivery(int idp);

        public void UpadteB(BaseStation b);
        void AddBaseStation(BaseStation b);
        BaseStation GetBaseStation(int IDb);
        IEnumerable<BaseStation> GetBaseStationList(Func<BaseStation,bool> predicate =  null);
       // IEnumerable<BaseStation> BaseStationFreeCharging();
        public void DeleteBaseStation(int idb);

        void UpadteC(Client c);
        void AddClient(Client c);
        Client GetClient(int IDc);
        IEnumerable<Client> GetClientList();
        void DeleteClient(int IDc);

        void UpadteP(Package p);
        void AddPackage(Package p);
        Package GetPackage(int idp);
        IEnumerable<Package> GetPackageList();
        IEnumerable<Package> PackagesWithoutSkimmer();
        public void DeletePackage(int id);

        void UpadteQ(Quadocopter qc);
        void AddSkimmer(Quadocopter q);
        public void DeleteSkimmer(int idq);
        Quadocopter GetQuadrocopter(int IDq);
        IEnumerable<Quadocopter> GetQuadocopterList();

        void AddSkimmerLoading(SkimmerLoading SL);
        void DeleteSkimmerLoading(int idsl);
        IEnumerable<SkimmerLoading> GetSkimmerLoading();
        public IEnumerable<SkimmerLoading> GetSkimmerLoadingList();
          
        //void SendingSkimmerForCharging(int idq, int idBS);
        //void SkimmerRelease(int idq, int IdBS);        
    }
}