using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class PackageToList
    {
        public int Id { get; set; }
        public String CustomerNameSends { get; set; }
        public String CustomerNameGets { get; set; }
        public Weight WeightCategory{ get; set; }
        public Priority priority { get; set; }
        public ParcelStatus PackageMode { get; set; }
    }
}
