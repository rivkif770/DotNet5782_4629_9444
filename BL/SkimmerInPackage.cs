using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class SkimmerInPackage
    {
        public int Id { get; set; }
        public double BatteryStatus { get; set; }
        public Location Location { get; set; }

        public override string ToString()
        {
            String result = "";
            result += $"ID is: {Id} \n";
            result += $"Battery status: {BatteryStatus} \n";
            result += $"Location current: {Location}";
            return result;
        }
    }
}
