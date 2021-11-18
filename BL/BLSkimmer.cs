using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DalObject;
namespace BL
{
    public partial class BL : IBL.IBL
    {
        static Random r = new Random();
        public IDal mayDal;
        public List<SkimmerToList> SkimmersList;
        public double Free;
        public double LightWeightCarrier;
        public double MediumWeightCarrier;
        public double HeavyWeightCarrier;
        public double SkimmerLoadingRate;
        public BL()
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
            foreach (Quadocopter item in mayDal.GetQuadocopterList())
            {
                DateTime help = new DateTime(0, 0, 0);
                SkimmerToList UpdatedSkimmer = new SkimmerToList
                {
                    Id = item.IDNumber,
                    SkimmerModel=item.SkimmerModel,
                    WeightCategory=(Weight)item.Weight,                 
                };
                IDAL.DO.Package PackageAssociatedWithSkimmer = FindingPackageAssociatedWithGlider(item);
                if (PackageAssociatedWithSkimmer.ID==item.IDNumber && PackageAssociatedWithSkimmer.TimeArrivalRecipient== help)
                {
                    UpdatedSkimmer.SkimmerStatus = SkimmerStatuses.shipping;
                    if (PackageAssociatedWithSkimmer.PackageCollectionTime == help)
                    {
                        UpdatedSkimmer.CurrentLocation =
                    }
                    else
                    {
                        UpdatedSkimmer.CurrentLocation.Latitude = FindingClientSender(PackageAssociatedWithSkimmer).Latitude;
                        UpdatedSkimmer.CurrentLocation.Longitude = FindingClientSender(PackageAssociatedWithSkimmer).Longitude;
                    }
                }
                if(PackageAssociatedWithSkimmer.ID != item.IDNumber)
                {
                    UpdatedSkimmer.SkimmerStatus = (SkimmerStatuses)(r.Next(2));
                }
                if(UpdatedSkimmer.SkimmerStatus==SkimmerStatuses.maintenance)
                {
                    int counts = mayDal.GetBaseStationList().Count()+1;
                    List<IDAL.DO.BaseStation> B= (List<IDAL.DO.BaseStation>)mayDal.GetBaseStationList();
                    UpdatedSkimmer.CurrentLocation.Latitude() = B[r.Next(counts)].Latitude();
                    UpdatedSkimmer.CurrentLocation.Longitude() = B[r.Next(counts)].Longitude();
                    UpdatedSkimmer.BatteryStatus = r.Next(21);
                }
                if (UpdatedSkimmer.SkimmerStatus == SkimmerStatuses.free)
                {
                    UpdatedSkimmer.CurrentLocation=
                }

            }
        }
        //public double AmountOfElectricity(Skimmer skimmer , Weight weight)
        //{
        //    int weight1=(int)cu
        //    switch (weight)
        //    {
        //        case (int)(Weight)IBL.BO.Weight.Light:
        //    }
        //}
        public Client FindingClientSender(IDAL.DO.Package p)
        {
            return mayDal.GetClient(p.IDSender);
        }
        public IDAL.DO.Package FindingPackageAssociatedWithGlider(Quadocopter q)
        {
            IDAL.DO.Package X = new IDAL.DO.Package
            {
                ID = 0
            };

            foreach (IDAL.DO.Package item in mayDal.GetPackageList())
            {
                if(item.IDSkimmerOperation==q.IDNumber)
                {
                    return item;
                }
            }
            return X;
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

        public void UpdateSkimmerName(int ids, string name)
        {
            GetSkimmer(ids).SkimmerModel = name;
        }
    }
}
