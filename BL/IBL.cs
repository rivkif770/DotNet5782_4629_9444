﻿using IBL.BO;
using System;
using System.Collections.Generic;

namespace IBL
{
    public interface IBL
    {
        void AddBaseStation(BaseStation newBaseStation);
        public void UpdateBaseStation(int id, string name, string countOfChargingStations);
        BaseStation GetBeseStation(int id);
        IEnumerable<BaseStationToList> GetBaseStationList();
        IEnumerable<BaseStationToList> GetBaseStationFreeCharging();

        void AddCustomer(Customer newCustomer);
        Customer GetCustomer(int id);
        public void UpdateCustomerData(int id, string name, string phone);
        IEnumerable<CustomerToList> GetCustomerList();

        void AddPackage(Package newPackage);
        public void CollectingPackageBySkimmer(int id);
        public void AssigningPackageToSkimmer(int id);
        public void DeliveryOfPackageBySkimmer(int id);
        IEnumerable<PackageToList> GetPackageList();
        IEnumerable<PackageToList> GetPackagesWithoutSkimmer();

        void AddSkimmer(Skimmer newSkimmer, int station);
        public SkimmerToList GetSkimmer(int id);
        public void ReleaseSkimmerFromCharging(int id, double ChargingTime);
        public void UpdateSkimmerName(int ids, string name);
        public void SendingSkimmerForCharging(int id);
        IEnumerable<SkimmerToList> GetSkimmerList(Func<SkimmerToList, bool> predicate = null);

  
    }
}