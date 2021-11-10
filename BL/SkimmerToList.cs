using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class SkimmerToList
    {
        public int Id { get; set; }
        public int SkimmerModl { get; set; }
        public Weight WeightCategory { get; set; }
        public int BatteryStatus { get; set; }
        public SkimmerStatuses SkimmerStatus { get; set; }
        public Location CurrentLocation { get; set; }
        public int PackageNumberTransferred { get; set; }
    }
}
