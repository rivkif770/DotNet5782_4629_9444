using IBL.BO;
using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        public void AddPackage(IBL.BO.Package newPackage)
        {
            IDAL.DO.Package temp_p = new IDAL.DO.Package
            {
                IDSender = newPackage.SendPackage.Id,
                IDgets = newPackage.ReceivesPackage.Id,
                Weight = (WeightCategories)newPackage.WeightCategory,
                priority = (Priorities)newPackage.priority,
                PackageCreationTime = DateTime.Now,
                TimeAssignGlider = new DateTime(0, 0, 0),
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
        public IBL.BO.Customer GetPackage(int id)
        {
            IDAL.DO.Package somoePackage;
            try
            {
                somoePackage = mayDal.GetPackage(id);
            }
            catch (IDAL.DO.IdDoesNotExistException cex)
            {
                throw new IdDoesNotExistException_BL(cex.Message + " from dal");
            }

            return new IBL.BO.Package
            {
                Id = somoePackage.ID,
                SendPackage = somoePackage.IDSender,
                ReceivesPackage = somoePackage.IDgets,
                WeightCategory = (Weight)somoePackage.Weight,
                priority = (Priority)somoePackage.priority,
                SkimmerInPackage = somoePackage.,
                PackageCreationTime = somoePackage.PackageCreationTime,
                AssignmentTime = somoePackage.TimeAssignGlider,
                CollectionTime = somoePackage.PackageCollectionTime,
                SupplyTime = somoePackage.TimeArrivalRecipient
            };
        }
    }
}
