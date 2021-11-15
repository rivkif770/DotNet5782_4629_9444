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
        public override string ToString()
        {
            String result = "";
            result += $"Unique ID number is: {Id}, \n";
            result += $"Customer Name Sends is: {CustomerNameSends}, \n";
            result += $"Customer Name Gets is: {CustomerNameGets}, \n";
            result += $"Weight category is: {WeightCategory}, \n";
            result += $"Priority is: {priority}, \n";
            result += $"Package mode is: {PackageMode}, \n";
            return result;
        }
    }
}
