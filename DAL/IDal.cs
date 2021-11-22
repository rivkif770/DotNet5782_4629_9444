using IDAL.DO;
using System.Collections.Generic;

namespace IDAL.DO
{
    public interface IDal
    {
        void AddBaseStation(BaseStation b);
        BaseStation GetBaseStation(int IDb);
        BaseStation GetBeseStation(int b);
        void DeleteBaseStation(BaseStation baseStation);
        IEnumerable<BaseStation> GetBaseStationList();
        public void DeleteBaseStation(int idb);
        void AddClient(Client c);
        Client GetClient(int IDc);
        public void DeleteClient(int id);
        IEnumerable<Client> GetClientList();
        void AddPackage(Package p);
        Package GetPackage(int idp);
        IEnumerable<Package> GetPackageList();
        public void DeletePackage(int id);
        void PackageDelivery(int idp);
        void CollectionPackage(int idp);
        List<Package> PackagesWithoutSkimmer();
        void AddSkimmer(Quadocopter q);
        IEnumerable<Quadocopter> GetQuadocopterList();
        IEnumerable<SkimmerLoading> GetSkimmerLoading();
        public IEnumerable<SkimmerLoading> GetSkimmerLoadingList();
        Quadocopter GetQuadrocopter(int IDq);
        BaseStation GetSkimmer(int id);
        public void AddSkimmerLoading(SkimmerLoading SL);
        public void DeleteSkimmer(int idq);
        void Update(Quadocopter qc);
        public void DeleteSkimmerLoading(int idsl);
        void SendingSkimmerForCharging(int idq, int idBS);
        void SkimmerRelease(int idq, int IdBS);
        void AssignPackageSkimmer(int idp, int idq);
        List<BaseStation> BaseStationFreeCharging();
       
        public double[] PowerConsumptionRequest();
     
    }
}