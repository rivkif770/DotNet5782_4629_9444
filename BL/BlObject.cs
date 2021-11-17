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
    public class BlObject   
    {
        internal static Random r = new Random();
        private IDal mayDal;
        //static DalObject.DalObject mydal = new DalObject.DalObject();
        private List<IBL.BO.Skimmer> Skimmers;
        public BlObject()
        {
            Skimmers = new List<IBL.BO.Skimmer>();
            mayDal = new DalObject.DalObject();
            SkimmerUpdate.SkimmerUpdates();
        }


        public void AddBaseStation(IBL.BO.BaseStation newBaseStation)
        {
            BaseStation temp_BS = new BaseStation
            {
                UniqueID = newBaseStation.Id,
                StationName = newBaseStation.Name,
                SeveralPositionsArgument = newBaseStation.SeveralClaimPositionsVacant,
                Latitude = newBaseStation.location.Latitude,
                Longitude = newBaseStation.location.Longitude
            };

            try
            {
                mayDal.AddBaseStation(temp_BS);
            }
            catch (ExistsInSystemException exception)
            {
                throw new ExistsInSystemException_BL($"Person {temp_BS.UniqueID} Save to system", Severity.Mild);
            }
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
        public void AddCustomer(IBL.BO.Customer newCustomer)
        {
            Client temp_c = new Client
            {
                ID = newCustomer.Id,
                Name = newCustomer.Name,
                Telephone = newCustomer.Phone,
                Latitude = newCustomer.Location.Latitude,
                Longitude = newCustomer.Location.Longitude
            };

            try
            {
                mayDal.AddClient(temp_c);
            }
            catch (ExistsInSystemException exception)
            {
                throw new ExistsInSystemException_BL($"Person {temp_c.ID} Save to system", Severity.Mild);
            }
        }
        public void AddPackage(IBL.BO.Package newPackage)
        {
            IDAL.DO.Package temp_p = new IDAL.DO.Package
            {
                IDSender = newPackage.SendPackage.Id,
                IDgets = newPackage.ReceivesPackage.Id,
                Weight =(WeightCategories) newPackage.WeightCategory,
                priority = (Priorities)newPackage.priority,
                PackageCreationTime = DateTime.Now,
                TimeAssignGlider = new DateTime( 0, 0, 0),
                PackageCollectionTime = new DateTime(0, 0, 0),
                TimeArrivalRecipient = new DateTime(0, 0, 0),
                IDSkimmerOperation = 0,
            };
            try
            {
                mayDal.AddPackage(temp_p);
            }
            catch (ExistsInSystemException exception)
            {
                throw new ExistsInSystemException_BL($"Person {temp_p.ID} Save to system", Severity.Mild);
            }
        }
        //public void AddCustomer(IDAL.DO.Client newCustomer)
        //{
        //    try
        //    {
        //        mayDal.AddClient(newCustomer);
        //    }
        //    catch (Exception)
        //    {

        public IBL.BO.BaseStation GetBeseStation(int id)
        {
            IDAL.DO.BaseStation somoeBaseStation;
            try
            {
                somoeBaseStation = mayDal.GetBaseStation(id);
            }
            catch (IDAL.DO.IdDoesNotExistException cex)
            {
                throw new IdDoesNotExistException_BL(cex.Message + " from dal");
            }
            return new IBL.BO.BaseStation
            {
                Id = somoeBaseStation.UniqueID,
                Name = somoeBaseStation.StationName,
                location = new Location { Latitude = somoeBaseStation.Latitude, Longitude = somoeBaseStation.Longitude },
                SeveralClaimPositionsVacant = somoeBaseStation.SeveralPositionsArgument,
                ListOfSkimmersCharge = 
                };
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
                BatteryStatus=somoeSkimmer.,
                SkimmerStatus=somoeSkimmer.,
                PackageInTransfer=somoeSkimmer.,
                Location = new Location { Latitude = somoeSkimmer.Latitude, Longitude = somoeSkimmer.Longitude },
                };
        }
        public Customer GetCustomer(int id)
        {
            IDAL.DO.Client somoeone;
            try
            {
                somoeone = mayDal.GetClient(id);
            }
            catch (IDAL.DO.IdDoesNotExistException cex)
            {
                throw new IdDoesNotExistException_BL(cex.Message + " from dal");
            }
            return new Customer
            {
                Id = somoeone.ID,
                Name = somoeone.Name,
                Phone = somoeone.Telephone,
                Location = new Location { Latitude = somoeone.Latitude, Longitude = somoeone.Longitude },
                SentParcels= somoeone.,
                ReceiveParcels= somoeone.,
            };
        }
        public Customer GetPackage(int id)
        {
            IDAL.DO.Package somoePackage;
            try
            {
                somoePackage = mayDal.GetPackage(id);
            }
            catch (IDAL.DO.PackageException cex)
            {
                throw new BLPackageException(cex.Message + " from dal");
            }

            return new IBL.BO.Package
            {
                Id = somoePackage.ID,
                SendPackage = somoePackage.sen,
                Phone = somoePackage.Telephone,
                Location = new Location { Latitude = somoeone.Latitude, Longitude = somoeone.Longitude }
            };
        }

    }
}
