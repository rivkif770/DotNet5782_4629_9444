using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Parcel
    {
        public int Id { get; set; }
        public Customer SendParcel { get; set; }
        public Customer ReceivesParcel { get; set; }
        public Weight WeightCategory { get; set; }
        public Priority priority { get; set; }
        public SkimmerInPackage SkimmerInParcel { get; set; }
        public DateTime ParcelCreationTime { get; set; }
        public DateTime AssignmentTime { get; set; }
        public DateTime CollectionTime { get; set; }
        public DateTime SupplyTime { get; set; }
    }
}
