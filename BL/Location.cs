using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Location
    {
        public double Latitude { get; init; }
        public double Longitude { get; init; }

        public override string ToString()
        {
            String result = "";
            result += $"Latitude is: {Latitude} \n";
            result += $"Longitude is: {Longitude} \n";
            return result;
        }
    }
}
