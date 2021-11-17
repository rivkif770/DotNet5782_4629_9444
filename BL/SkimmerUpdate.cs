using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlObject
{
    public partial class SkimmerUpdate
    {
        double[] vs = DalObject.DalObject.PowerConsumptionRequest();
        public double free = vs[0];
        public double LightWeightCarrier=vs[1];
        public double MediumWeightBearing;
        public double CarryingHeavyWeight;
        public double SkimmerLoadingRate;
        public static void SkimmerUpdates()
        {
            double[] arr = new double[5];
            double[] vs = DalObject.DalObject.PowerConsumptionRequest();
            //    arr = vs;
            //public double free = vs[0];
            //public double LightWeightCarrier;
            //public double MediumWeightBearing;
            //public double CarryingHeavyWeight;
            //public double SkimmerLoadingRate;
            double distanceToDestination = DistanceToDestination();
            int minBattertSkimmer = 0;
            int weight=(int)
        }
        
    }
}
