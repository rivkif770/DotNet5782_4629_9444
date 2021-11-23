using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public static class Utils
    {
        /// <summary>
        /// https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="otherLongitude"></param>
        /// <param name="otherLatitude"></param>
        /// <returns></returns>
        public static double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            var oneDegree = Math.PI / 180.0;
            var d1 = latitude * oneDegree;
            var num1 = longitude * oneDegree;
            var d2 = otherLatitude * oneDegree;
            var num2 = otherLongitude * oneDegree - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
