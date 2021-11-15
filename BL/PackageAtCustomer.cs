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
        public override string ToString()
        {
            String result = "";
            result += $"Unique ID number is: {Id}, \n";
            result += $"Weight category is: {WeightCategory}, \n";
            result += $"Priority is: {priority}, \n";
            result += $"Package mode is: {PackageMode}, \n";
            result += $"customer In Parcel is: {customerInParcel}, \n";
            return result;
        }
    }
}
