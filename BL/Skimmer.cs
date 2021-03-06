using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Skimmer
    {
        public int Id { get; set; }
        public string SkimmerModel { get; set; }
        public Weight WeightCategory { get; set; }
        public double BatteryStatus { get; set; }
        public SkimmerStatuses SkimmerStatus { get; set; }
        public PackageInTransfer PackageInTransfer { get; set; }
        public Location Location { get; set; }

        public override string ToString()
        {
            String result = "";
            result += $"ID is: {Id} \n";
            result += $"Skimmer model: {SkimmerModel} \n";
            result += $"Weight category: {WeightCategory} \n";
            result += $"Battery status: {BatteryStatus} \n";
            result += $"Skimmer mode: {SkimmerStatus} \n";
            result += $"Shipping package in transfer:\n{PackageInTransfer}";
            result += Location;
            return result;
        }
    }
}
