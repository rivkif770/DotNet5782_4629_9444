using IBL.BO;
using System.Collections.Generic;

namespace IBL
{
    public interface IBL
    {
        Customer GetCustomer(int id);
        BaseStation GetBaseStation(int id);
        Package GetPackage(int id);
        Skimmer GetSkimmer(int id);
    }
}