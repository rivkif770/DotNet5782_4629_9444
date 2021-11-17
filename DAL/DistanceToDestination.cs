using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DAL
{
    public class DistanceToDestination
    {
        const double PI = Math.PI;
        const int Radius = 6371;
        public static double Calculation(double lon1,double lat1, Client wanted)
        {
            double RadiusOfLon = (lon1 - wanted.Longitude) * PI / 180;
            double RadiusOfLat = (lat1 - wanted.Longitude) * PI / 180;
            double havd = Math.Pow(Math.Sin(RadiusOfLat / 2), 2) +
                (Math.Cos(wanted.Latitude)) * (Math.Cos(lat1)) * Math.Pow(Math.Sin(RadiusOfLon), 2);
            double distance = 2 * Radius * Math.Asin(havd);
            return distance;
        }
        public static BaseStation FindClientCoordinates (int id)
        {
            BaseStation NotFoundCase = new BaseStation();
            NotFoundCase.UniqueID = 0;
            IEnumerator<BaseStation> iter = (IEnumerator<BaseStation>)DalObject.DalObjectBaseStation.GetBaseStationList_private(); 
            while(iter.MoveNext())
            {
                if (id == iter.Current.UniqueID)
                    return iter.Current;
            }
            return NotFoundCase;
        }
    }
}
