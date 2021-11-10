using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class PackageInTransfer
    {
        public int Id { get; set; }
        public ParcelStatus PackageMode { get; set; }
        public Priority priority { get; set; }
        public Weight WeightCategory { get; set; }
        public CustomerInParcel CustomerSends { get; set; }
        public CustomerInParcel CustomerReceives { get; set; }
        public Location CollectionLocation { get; set; }
        public Location DeliveryDestinationLocation { get; set; }
        public double TransportDistance { get; set; }
    }
}
