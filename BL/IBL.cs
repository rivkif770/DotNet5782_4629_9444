using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        void AddCustomer(Customer newCustomer);
        Customer GetCustomer(int id);
        public void UpdateCustomerData(int id, string name, string phone);

        public void ReleaseSkimmerFromCharging(int id, double ChargingTime);
        void AddSkimmer(Skimmer newSkimmer, int station);
        public SkimmerToList GetSkimmer(int id);
        public void UpdateSkimmerName(int ids, string name);
        public void SendingSkimmerForCharging(int id);

        void AddBaseStation(BaseStation newBaseStation);
        public void UpdateBaseStation(int id, string name, string countOfChargingStations);
        BaseStation GetBeseStation(int id);
        void AddCustomer(Customer newCustomer);
        void AddPackage(Package newPackage);
        void AddSkimmer(Skimmer newSkimmer, int station);
        Customer GetCustomer(int id);
        Customer GetPackage(int id);
        Skimmer GetSkimmer(int id);
        public void CollectingPackageBySkimmer(int id);
        public void AssigningPackageToSkimmer(int id);
        public void DeliveryOfPackageBySkimmer(int id);
    }
}