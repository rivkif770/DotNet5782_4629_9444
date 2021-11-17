using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DalObject;
namespace BlObject
{
    public partial class BlObject : IBL.IBL
    {
        static Random r = new Random();
        public IDal mayDal;
        public List<SkimmerToList> SkimmersList;
        public double Free;
        public double LightWeightCarrier;
        public double MediumWeightCarrier;
        public double HeavyWeightCarrier;
        public double SkimmerLoadingRate;
        public BlObject()
        {
            SkimmersList = new List<SkimmerToList>();
            mayDal = new DalObject.DalObject();

            double[] vs = mayDal.PowerConsumptionRequest();
            Free = vs[0];
            LightWeightCarrier = vs[1];
            MediumWeightCarrier = vs[2];
            HeavyWeightCarrier = vs[3];
            SkimmerLoadingRate = vs[4];

            SkimmerUpdates();
        }
        public void SkimmerUpdates()
        {

        }
        public void AddSkimmer(IBL.BO.Skimmer newSkimmer, int station)
        {
            newSkimmer.BatteryStatus = r.Next(20, 41);
            newSkimmer.SkimmerStatus = IBL.BO.SkimmerStatuses.maintenance;
            IBL.BO.BaseStation temp_BaseStation = GetBeseStation(station);
            newSkimmer.Location = temp_BaseStation.location;
            Quadocopter temp_S = new Quadocopter
            {
                IDNumber = newSkimmer.Id,
                SkimmerModel = newSkimmer.SkimmerModel,
                Weight = (WeightCategories)newSkimmer.WeightCategory
            };

            try
            {
                mayDal.AddSkimmer(temp_S);
            }
            catch (ExistsInSystemException exception)
            {
                throw new ExistsInSystemException_BL($"Person {temp_S.IDNumber} Save to system", Severity.Mild);
            }
        }
        public IBL.BO.Skimmer GetSkimmer(int id)
        {
            IDAL.DO.Quadocopter somoeSkimmer;
            try
            {
                somoeSkimmer = mayDal.GetQuadrocopter(id);
            }
            catch (IDAL.DO.IdDoesNotExistException cex)
            {
                throw new IdDoesNotExistException_BL(cex.Message + " from dal"); ;
            }
            return new IBL.BO.Skimmer
            {
                Id = somoeSkimmer.IDNumber,
                SkimmerModel = somoeSkimmer.SkimmerModel,
                WeightCategory = (Weight)somoeSkimmer.Weight,
                BatteryStatus = somoeSkimmer.,
                SkimmerStatus = somoeSkimmer.,
                PackageInTransfer = somoeSkimmer.,
                Location = new Location { Latitude = somoeSkimmer.Latitude, Longitude = somoeSkimmer.Longitude },
            };
        }        
    }
}
