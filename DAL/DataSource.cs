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

        
        internal static Random r = new Random();
        
        internal class Config
        {
            public static int IDPackage = 1000;
            public static double free=5;
            public static double  LightWeightCarrier = 10;
            public static double  MediumWeightBearing = 20;
            public static double CarryingHeavyWeight = 25;
            public static double SkimmerLoadingRate = 50;
        }

        public static void Initialize()
        {
            string[] clientsNames = { "Mahesh", "Jeff", "Dave", "Allen", "Monica", "Henry", "Raj", "Mark", "Rose", "Mike" };
            for (int i = 0; i < 2; i++)
            {
                ListBaseStation.Add(new BaseStation()
                {
                    UniqueID = r.Next(999, 10000),
                    StationName = $"BaseStation{i}",
                    SeveralPositionsArgument = r.Next(5),
                    Longitude = r.Next(-100, 100),
                    Latitude = r.Next(-100, 100)
                });
            }

            for (int i = 0; i < 5; i++)
            {
                Quadocopter newQ = new Quadocopter();
                newQ.IDNumber = r.Next(99, 1000);
                newQ.SkimmerModel = $"A2{i}";
                //newQ.Battery = r.Next(101);
                newQ.Weight = (WeightCategories)r.Next(3);
                //newQ.SkimmerMode = (DronStatuses)r.Next(3);
                ListQuadocopter.Add(newQ);
            }

            for (int i = 0; i < 10; i++)
            {
                ListClient.Add(new Client()
                {
                    Latitude = r.Next(-100, 100),
                    Longitude = r.Next(-100, 100),
                    ID = r.Next(99999999, 1000000000),
                    Telephone = $"0{r.Next(50, 59)}{r.Next(1000000, 10000000)}",
                    Name = clientsNames[r.Next(clientsNames.Length)]
                });
            }

            for (int i = 0; i < 10; i++)
            {
                Package newP = new Package();
                newP.ID = global::DalObject.DataSource.Config.IDPackage++;
                newP.IDSender = ListClient[r.Next(6)].ID;
                newP.IDgets = ListClient[r.Next(6)].ID;
                newP.IDSkimmerOperation = ListQuadocopter[r.Next(5)].IDNumber;
                newP.Weight = (WeightCategories)r.Next(3);
                newP.priority = (Priorities)r.Next(3);
                newP.PackageCreationTime = DateTime.Now;
                ListPackage.Add(newP);
            }
        }
    }
  
}