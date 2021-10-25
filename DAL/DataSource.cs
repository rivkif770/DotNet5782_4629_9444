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
    }


}