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
        void AddPackage(Package newPackage);
        
        BaseStation GetBeseStation(int id);
        Customer GetPackage(int id);
    }
}