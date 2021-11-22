﻿using IDAL.DO;
using System.Collections.Generic;

namespace IDAL.DO
{
    public interface IDal
    {
        void AddBaseStation(BaseStation b);
        void AddClient(Client c);
        void AddPackage(Package p);
        void AddSkimmer(Quadocopter q);
        void AssignPackageSkimmer(int idp, int idq);
        List<BaseStation> BaseStationFreeCharging();
        void CollectionPackage(int idp);
        BaseStation GetBaseStation(int IDb);
        IEnumerable<BaseStation> GetBaseStationList();
        Client GetClient(int IDc);
        IEnumerable<Client> GetClientList();
        Package GetPackage(int idp);
        IEnumerable<Package> GetPackageList();
        IEnumerable<Quadocopter> GetQuadocopterList();
        IEnumerable<SkimmerLoading> GetSkimmerLoading();
        public IEnumerable<SkimmerLoading> GetSkimmerLoadingList();
        Quadocopter GetQuadrocopter(int IDq);
        void PackageDelivery(int idp);
        List<Package> PackagesWithoutSkimmer();
        void SendingSkimmerForCharging(int idq, int idBS);
        void SkimmerRelease(int idq, int IdBS);
        BaseStation GetSkimmer(int id);
        public double[] PowerConsumptionRequest();
        public void DeleteSkimmer(int idq);
        public void DeleteBaseStation(int idb);
        public void AddSkimmerLoading(SkimmerLoading SL);
        public void DeleteSkimmerLoading(int idsl);
        public void DeletePackage(int id);
        public void DeleteClient(int id);
        void DeleteBaseStation(BaseStation baseStation);
        BaseStation GetBeseStation(int b);
        void Update(Quadocopter qc);
    }
}