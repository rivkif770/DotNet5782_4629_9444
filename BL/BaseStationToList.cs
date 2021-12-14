using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BaseStationToList
    {
        public int Id { get; set; }
        public string StationName { get; set; }
        public int FreeChargingstations { get; set; }
        public int CatchChargingstations { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"Unique ID number is: {Id} \n";
            result += $"The station name is: {StationName} \n";;
            result += $"Free Chargingstations is: {FreeChargingstations} \n";
            result += $"Catch Chargingstations is: {CatchChargingstations} \n";
            return result;
        }
    }
}
