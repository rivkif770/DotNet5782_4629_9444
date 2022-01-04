using BO;
using System;
using System.Collections.Generic;

namespace BlApi
{
    public interface IBL
    {
        void AddBaseStation(BaseStation newBaseStation);
        public void UpdateBaseStation(int id, string name, string countOfChargingStations);
        BaseStation GetBeseStation(int id);
        public void DeleteBaseStation(int id);
        IEnumerable<BaseStationToList> GetBaseStationList();
        IEnumerable<BaseStationToList> GetBaseStationFreeCharging();
        public IEnumerable<SkimmerInCharging> GetListOfSkimmersCharge(BO.BaseStation baseStation);

        void AddCustomer(Customer newCustomer);
        Customer GetCustomer(int id);
        public void UpdateCustomerData(int id, string name, string phone);
        IEnumerable<CustomerToList> GetCustomerList();
        public void DeleteCustomer(int id);
        public BO.CustomerInParcel GetCustomerInParcel(int id);
        public IEnumerable<PackageAtCustomer> GetSentParcels(BO.Customer customer);
        public IEnumerable<PackageAtCustomer> GetReceiveParcels(BO.Customer customer);
        public Customer GetCustomerListID(int id, string name);

        void AddPackage(Package newPackage);
        public void CollectingPackageBySkimmer(int id);
        public void AssigningPackageToSkimmer(int id);
        public void DeliveryOfPackageBySkimmer(int id);
        IEnumerable<PackageToList> GetPackageList(Func<BO.PackageToList, bool> predicate = null);
        IEnumerable<PackageToList> GetPackagesWithoutSkimmer();
        public void DeletePackage(int id);
        public BO.Package GetPackage(int id);
        public int PackageMode(BO.Package p);
        public IEnumerable<PackageToList> GetListOfPackageShipped(int customerID);
        public IEnumerable<PackageToList> GetListOfPackageReceived(int customerID);

        void AddSkimmer(Skimmer newSkimmer, int station);
        public SkimmerToList GetSkimmerToList(int id);
        public Skimmer GetSkimmerr(int id);
        public void ReleaseSkimmerFromCharging(int id, double ChargingTime);
        public void UpdateSkimmerName(int ids, string name);
        public void SendingSkimmerForCharging(int id);
        IEnumerable<SkimmerToList> GetSkimmerList(Func<SkimmerToList, bool> predicate = null);

  
    }
}