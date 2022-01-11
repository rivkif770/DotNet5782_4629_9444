using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace DalObject
{
    public partial class DalObject : DalApi.IDal
    {
        static readonly IDal instance = new DalObject();
        internal static IDal Instance { get { return instance; } }
        static DalObject() { }
        public DalObject()
        {
            DataSource.Initialize();
        }
        public double[] PowerConsumptionRequest()
        {
            return new double[] {
                DataSource.Config.Free,
                DataSource.Config.LightWeightCarrier,
                DataSource.Config.MediumWeightCarrier,
                DataSource.Config.HeavyWeightCarrier,
                DataSource.Config.SkimmerLoadingRate};
        }
        public void AssignPackageSkimmer(int idp, int idq)//Assign a package to a skimmer
        {
            Package temp_p = this.GetPackage(idp);
            temp_p.IDSkimmerOperation = idq;//Change the skimmer ID of the package to the associated skimmer ID
            temp_p.TimeAssignGlider = DateTime.Now;//Update time association

            DataSource.ListPackage.RemoveAll(packag => packag.ID == idp);//Deleting the old Package object
            DataSource.ListPackage.Add(temp_p);//Deleting the new Package object includes the change
        }
        public void CollectionPackage(int idp)//Update package collection by skimmer
        {
            Package temp_p = this.GetPackage(idp);
            temp_p.PackageCollectionTime = DateTime.Now;//Update the package collection time from the sender

            DataSource.ListPackage.RemoveAll(packag => packag.ID == idp);//Deleting the old Package object
            DataSource.ListPackage.Add(temp_p);//Deleting the new Package object includes the change
        }
        public void PackageDelivery(int idp)//Delivery of a package to the customer
        {
            Package temp_p = this.GetPackage(idp);
            temp_p.TimeArrivalRecipient = DateTime.Now;//Update the time of arrival of the package to the recipient

            DataSource.ListPackage.RemoveAll(packag => packag.ID == idp);//Deleting the old Package object
            DataSource.ListPackage.Add(temp_p);//Deleting the new Package object includes the change
        }
        public void SendingSkimmerForCharging(int idq, int idBS)//Sending a skimmer for charging at a base station
        {
            BaseStation temp_BS = this.GetBaseStation(idBS);
            temp_BS.SeveralPositionsArgument = temp_BS.SeveralPositionsArgument - 1;//Update the number of stations available for charging at least 1

            DataSource.ListBaseStation.RemoveAll(Base => Base.UniqueID == idBS);//Deleting the old BaseStation object
            DataSource.ListBaseStation.Add(temp_BS);//Deleting the new BaseStation object includes the change
        }
        public void SkimmerRelease(int idq, int IdBS)//Release skimmer from base charge
        {            
            BaseStation temp_BS = this.GetBaseStation(IdBS);
            temp_BS.SeveralPositionsArgument = temp_BS.SeveralPositionsArgument + 1;//Update the number of stations available for charging at more 1

            DataSource.ListBaseStation.RemoveAll(Base => Base.UniqueID == IdBS);//Deleting the old BaseStation object
            DataSource.ListBaseStation.Add(temp_BS);//Deleting the new BaseStation object includes the change
        }

    }
}
