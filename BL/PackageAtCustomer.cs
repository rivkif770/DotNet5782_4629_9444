using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class PackageAtCustomer
    {
        public int Id { get; set; }
        public Weight WeightCategory { get; set; }
        public Priority priority { get; set; }
        public ParcelStatus PackageMode { get; set; }
        public CustomerInParcel customerInParcel { get; set; }
    }
}
