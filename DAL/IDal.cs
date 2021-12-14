using DO;
using System;
using System.Collections.Generic;

namespace DalApi
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
        public void DeleteBaseStation(int idb);

        void UpadteC(Client c);
        void AddClient(Client c);
        Client GetClient(int IDc);
        IEnumerable<Client> GetClientList(Func<Client, bool> predicate = null);
        void DeleteClient(int IDc);

        void UpadteP(Package p);
        void AddPackage(Package p);
        Package GetPackage(int idp);
        IEnumerable<Package> GetPackageList(Func<Package, bool> predicate = null);
        public void DeletePackage(int id);

        void UpadteQ(Quadocopter qc);
        void AddSkimmer(Quadocopter q);
        public void DeleteSkimmer(int idq);
        Quadocopter GetQuadrocopter(int IDq);
        IEnumerable<Quadocopter> GetQuadocopterList(Func<Quadocopter, bool> predicate = null);

        void AddSkimmerLoading(SkimmerLoading SL);
        void DeleteSkimmerLoading(int idsl);
        IEnumerable<SkimmerLoading> GetSkimmerLoading();
        public IEnumerable<SkimmerLoading> GetSkimmerLoadingList();          
    }
}