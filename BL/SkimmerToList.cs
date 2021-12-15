using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class SkimmerToList
    {
        public int Id { get; set; }
        public string SkimmerModel { get; set; }
        public Weight WeightCategory { get; set; }
        public double BatteryStatus { get; set; }
        public SkimmerStatuses SkimmerStatus { get; set; }
        public Location CurrentLocation { get; set; }
        public int PackageNumberTransferred { get; set; }
        public DateTime? EntertimeLoading { get; set; }

        public override string ToString()
        {
            String result = "";
            result += $"ID is: {Id} \n";
            result += $"Skimmer model: {SkimmerModel} \n";
            result += $"Weight category: {WeightCategory} \n";
            result += $"Battery status: {BatteryStatus} \n";
            result += $"Skimmer mode: {SkimmerStatus} \n";
            result += $"Location current: \n{CurrentLocation}";
            result += $"Package number is transferred: {PackageNumberTransferred}\n";
            return result;
        }
    }
}
