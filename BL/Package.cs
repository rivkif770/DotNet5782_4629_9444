using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Package
    {
        public int Id { get; set; }
        public Customer SendPackage { get; set; }
        public Customer ReceivesPackage { get; set; }
        public Weight WeightCategory { get; set; }
        public Priority priority { get; set; }
        public SkimmerInPackage SkimmerInPackage { get; set; }
        public DateTime PackageCreationTime { get; set; }
        public DateTime AssignmentTime { get; set; }
        public DateTime CollectionTime { get; set; }
        public DateTime SupplyTime { get; set; }
    }
}
