using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            String result = "";
            result += $"Latitude is: {Latitude} \n";
            result += $"Longitude is: {Longitude} \n";
            return result;
        }
    }
}
