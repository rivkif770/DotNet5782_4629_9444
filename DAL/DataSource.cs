using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace DalObject
{
    public class DataSource
    {
        internal static List<Quadocopter> ListQuadocopter = new List<Quadocopter>();
        internal static List<BaseStation> ListBaseStation = new List<BaseStation>();
        internal static List<Package> ListPackage = new List<Package>();
        internal static List<Client> ListClient = new List<Client>();
        internal static List<SkimmerLoading> ListSkimmerLoading = new List<SkimmerLoading>();
        internal class Config
        {
            public static int IDPackage = 1000;
        }
        public static void Initialize(DataSource DJ)
        {
            Random r = new Random();
            int num = r.Next();
            for(int i = 0; i < 2; i++)
            {
                BaseStation newB = new BaseStation();
                newB.UniqueID = r.Next(999, 10000);
                newB.StationName = $"tamar{i}";
                newB.SeveralPositionsArgument = r.Next(5);
                newB.Longitude = r.Next(-100, 100);
                newB.Latitude = r.Next(-100, 100);
                ListBaseStation.Add(newB);
            }
            
            for(int i = 0; i < 5; i++)
            {
                Quadocopter newQ = new Quadocopter();
                newQ.IDNumber = r.Next(99, 1000);
                newQ.SkimmerModel = $"A2{i}";
                newQ.Battery = r.Next(101);
                newQ.Weight = (WeightCategories)r.Next(3);
                newQ.SkimmerMode = (DronStatuses)r.Next(3);
                ListQuadocopter.Add(newQ);
            }
            
            for (int i = 0; i < 10; i++)
            {
                Client newC = new Client();
                newC.Latitude = r.Next(-100, 100);
                newC.Longitude = r.Next(-100, 100);
                newC.ID = r.Next(999999999, 1000000000);
                newC.Telephone = $"0{r.Next(50, 59)}-{r.Next(1000000, 10000000)}";
                newC.Name = $"David{i}";
                ListClient.Add(newC);
            }

            for (int i = 0; i < 10; i++)
            {
                Package newP = new Package();
                newP.ID = DalObject.DataSource.Config.IDPackage++;
                newP.IDSender = ListClient[r.Next(6)].ID;
                newP.IDgets = ListClient[r.Next(6)].ID;
                newP.IDSkimmerOperation = ListQuadocopter[r.Next(6)].IDNumber;
                newP.Weight = (WeightCategories)r.Next(3);
                newP.priority = (Priorities)r.Next(3);
                newP.PackageCreationTime = new DateTime(2021, r.Next(1, 13), r.Next(1, 30), r.Next(1, 24), r.Next(1, 61), r.Next(1, 61));
                newP.TimeAssignGlider = new DateTime(2021, r.Next(1, 13), r.Next(1, 30), r.Next(1, 24), r.Next(1, 61), r.Next(1, 61));
                newP.PackageCollectionTime = new DateTime(2021, r.Next(1, 13), r.Next(1, 30), r.Next(1, 24), r.Next(1, 61), r.Next(1, 61));
                newP.TimeArrivalRecipient = new DateTime(2021, r.Next(1, 13), r.Next(1, 30), r.Next(1, 24), r.Next(1, 61), r.Next(1, 61));
                ListPackage.Add(newP);
            }
        }
    }
    public class dalObject
    {
        public static void AddBaseStation(BaseStation b)
        {
            DataSource.ListBaseStation.Add(b);
        }
        public static void AddSkimmer(Quadocopter q)
        {
            DataSource.ListQuadocopter.Add(q);
        }
        public static void AddClient(Client c)
        {
            DataSource.ListClient.Add(c);
        }
        public static void AddPackage(Package p)
        {
            p.ID = DalObject.DataSource.Config.IDPackage++;
            DataSource.ListPackage.Add(p);
        }
        //////////////////////////////////////////////////////////
        public static void AssignPackageSkimmer(int idp,int idq)
        {
            Quadocopter temp_q = new Quadocopter();
            Package temp_p = new Package();
            int result = DataSource.ListQuadocopter.FindIndex(x => x.IDNumber == idq);
            temp_q.IDNumber = DataSource.ListQuadocopter[result].IDNumber;
            temp_q.SkimmerModel = DataSource.ListQuadocopter[result].SkimmerModel;
            temp_q.Weight = DataSource.ListQuadocopter[result].Weight;
            temp_q.Battery = DataSource.ListQuadocopter[result].Battery;
            temp_q.SkimmerMode = (DronStatuses)2;
            DataSource.ListQuadocopter.RemoveAt(result);
            DataSource.ListQuadocopter.Add(temp_q);

            int result1 = DataSource.ListPackage.FindIndex(x => x.ID == idp);
            temp_p.ID = DataSource.ListPackage[result1].ID;
            temp_p.IDSender = DataSource.ListPackage[result1].IDSender;
            temp_p.IDgets = DataSource.ListPackage[result1].IDgets;
            temp_p.Weight = DataSource.ListPackage[result1].Weight;
            temp_p.priority = DataSource.ListPackage[result1].priority;
            temp_p.IDSkimmerOperation = idq;
            temp_p.PackageCreationTime = DataSource.ListPackage[result1].PackageCreationTime;
            temp_p.TimeAssignGlider = DateTime.Now;
            temp_p.PackageCollectionTime = DataSource.ListPackage[result1].PackageCollectionTime;
            temp_p.TimeArrivalRecipient = DataSource.ListPackage[result1].TimeArrivalRecipient;
            DataSource.ListPackage.RemoveAt(result1);
            DataSource.ListPackage.Add(temp_p);
        }
        public static void CollectionPackage(int id)
        {
            Package temp_p = new Package();
            int result = DataSource.ListPackage.FindIndex(x => x.ID == id);
            temp_p.ID = DataSource.ListPackage[result].ID;
            temp_p.IDSender = DataSource.ListPackage[result].IDSender;
            temp_p.IDgets = DataSource.ListPackage[result].IDgets;
            temp_p.Weight = DataSource.ListPackage[result].Weight;
            temp_p.priority = DataSource.ListPackage[result].priority;
            temp_p.IDSkimmerOperation = DataSource.ListPackage[result].IDSkimmerOperation;
            temp_p.PackageCreationTime = DataSource.ListPackage[result].PackageCreationTime;
            temp_p.TimeAssignGlider = DataSource.ListPackage[result].TimeAssignGlider;
            temp_p.PackageCollectionTime = DateTime.Now;
            temp_p.TimeArrivalRecipient = DataSource.ListPackage[result].TimeArrivalRecipient;
            DataSource.ListPackage.RemoveAt(result);
            DataSource.ListPackage.Add(temp_p);
        }
        public static void PackageDelivery(int id)
        {
            Package temp_p = new Package();
            int result = DataSource.ListPackage.FindIndex(x => x.ID == id);
            temp_p.ID = DataSource.ListPackage[result].ID;
            temp_p.IDSender = DataSource.ListPackage[result].IDSender;
            temp_p.IDgets = DataSource.ListPackage[result].IDgets;
            temp_p.Weight = DataSource.ListPackage[result].Weight;
            temp_p.priority = DataSource.ListPackage[result].priority;
            temp_p.IDSkimmerOperation = DataSource.ListPackage[result].IDSkimmerOperation;
            temp_p.PackageCreationTime = DataSource.ListPackage[result].PackageCreationTime;
            temp_p.TimeAssignGlider = DataSource.ListPackage[result].TimeAssignGlider;
            temp_p.PackageCollectionTime = DataSource.ListPackage[result].PackageCollectionTime;
            temp_p.TimeArrivalRecipient = DateTime.Now;
            DataSource.ListPackage.RemoveAt(result);
            DataSource.ListPackage.Add(temp_p);
        }
        public static void SendingSkimmerForCharging(int id,int idBS)
        {
            int result = DataSource.ListQuadocopter.FindIndex(x => x.IDNumber == id);
            Quadocopter temp_q = new Quadocopter();
            temp_q.IDNumber = DataSource.ListQuadocopter[result].IDNumber;
            temp_q.SkimmerModel = DataSource.ListQuadocopter[result].SkimmerModel;
            temp_q.Weight = DataSource.ListQuadocopter[result].Weight;
            temp_q.Battery = 0;
            temp_q.SkimmerMode = (DronStatuses)1;
            DataSource.ListQuadocopter.RemoveAt(result);
            DataSource.ListQuadocopter.Add(temp_q);

            int result1 = DataSource.ListBaseStation.FindIndex(x => x.UniqueID == idBS);
            BaseStation temp_b = new BaseStation();
            temp_b.UniqueID = DataSource.ListBaseStation[result1].UniqueID;
            temp_b.StationName = DataSource.ListBaseStation[result1].StationName;
            temp_b.SeveralPositionsArgument = DataSource.ListBaseStation[result1].SeveralPositionsArgument-1;
            temp_b.Latitude = DataSource.ListBaseStation[result1].Latitude;
            temp_b.Longitude = DataSource.ListBaseStation[result1].Longitude;
            DataSource.ListBaseStation.RemoveAt(result1);
            DataSource.ListBaseStation.Add(temp_b);

            SkimmerLoading newSkimmerLoading = new SkimmerLoading();
            newSkimmerLoading.SkimmerID = id;
            newSkimmerLoading.StationID = idBS;
            DataSource.ListSkimmerLoading.Add(newSkimmerLoading);
        }
        public static void SkimmerRelease(int id)
        {
            int result = DataSource.ListQuadocopter.FindIndex(x => x.IDNumber == id);
            Quadocopter temp_q = new Quadocopter();
            temp_q.IDNumber = DataSource.ListQuadocopter[result].IDNumber;
            temp_q.SkimmerModel = DataSource.ListQuadocopter[result].SkimmerModel;
            temp_q.Weight = DataSource.ListQuadocopter[result].Weight;
            temp_q.Battery = 100;
            temp_q.SkimmerMode = (DronStatuses)0;
            DataSource.ListQuadocopter.RemoveAt(result);
            DataSource.ListQuadocopter.Add(temp_q);

            int idBS;
            int result1 = DataSource.ListSkimmerLoading.FindIndex(x => x.SkimmerID == id);
            idBS = DataSource.ListSkimmerLoading[result1].StationID;
            int result2 = DataSource.ListBaseStation.FindIndex(x => x.UniqueID == idBS);
            BaseStation temp_b = new BaseStation();
            temp_b.UniqueID = DataSource.ListBaseStation[result2].UniqueID;
            temp_b.StationName = DataSource.ListBaseStation[result2].StationName;
            temp_b.SeveralPositionsArgument = DataSource.ListBaseStation[result2].SeveralPositionsArgument + 1;
            temp_b.Latitude = DataSource.ListBaseStation[result2].Latitude;
            temp_b.Longitude = DataSource.ListBaseStation[result2].Longitude;
            DataSource.ListBaseStation.RemoveAt(result2);
            DataSource.ListBaseStation.Add(temp_b);
        }
        //////////////////////////////////////////////////////////
        public static void DisplayBaseStation(int IDb)
        {
            int result = DataSource.ListBaseStation.FindIndex(x => x.UniqueID == IDb);
            DataSource.ListBaseStation[result].ToString();
        }
        public static void DisplayClient(int IDc)
        {
            int result = DataSource.ListClient.FindIndex(x => x.ID == IDc);
            DataSource.ListClient[result].ToString();
        }
        public static void DisplaySkimmer(int iDq)
        {
            int result = DataSource.ListQuadocopter.FindIndex(x => x.IDNumber == iDq);
            DataSource.ListQuadocopter[result].ToString();
        }
        public static void DisplayPackage(int iDp)
        {
            int result = DataSource.ListPackage.FindIndex(x => x.ID == iDp);
            DataSource.ListPackage[result].ToString();
        }
        //////////////////////////////////////////////////////////
        public static void printBaseStation()
        {
            DataSource.ListBaseStation.ForEach(x => x.ToString()); 
        }
        public static void printSkimmer()
        {
            DataSource.ListQuadocopter.ForEach(x => x.ToString());
        }
        public static void printClient()
        {
            DataSource.ListClient.ForEach(x => x.ToString());
        }
        public static void printPackage()
        {
            DataSource.ListPackage.ForEach(x => x.ToString());
        }
        public static void printPackageWithoutSkimmer()
        {

            for (int i = 0; i < DataSource.ListPackage.Count; i++)
            {
                if(DataSource.ListPackage[i].IDSkimmerOperation == 0)
                {
                    DataSource.ListPackage[i].ToString();
                }
            }
        }
        public static void printBaseStationFreeCharging()
        {
            for (int i = 0; i < DataSource.ListBaseStation.Count; i++)
            {
                if(DataSource.ListBaseStation[i].SeveralPositionsArgument != 0)
                {
                    DataSource.ListBaseStation[i].ToString();
                }
            }
        }
        
    }
}