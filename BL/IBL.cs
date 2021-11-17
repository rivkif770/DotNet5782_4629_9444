using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        void AddBaseStation(BaseStation newBaseStation);
        void AddCustomer(Customer newCustomer);
        void AddPackage(Package newPackage);
        void AddSkimmer(Skimmer newSkimmer, int station);
        BaseStation GetBeseStation(int id);
        Customer GetCustomer(int id);
        Customer GetPackage(int id);
        Skimmer GetSkimmer(int id);
    }
}