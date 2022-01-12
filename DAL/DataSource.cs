using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using System.Runtime.CompilerServices;
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
            public static double Free = 0.001;
            public static double LightWeightCarrier = 0.003;
            public static double MediumWeightCarrier = 0.004;
            public static double HeavyWeightCarrier = 0.006;
            public static double SkimmerLoadingRate = 5;
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
                    SeveralPositionsArgument = r.Next(3, 10),
                    Longitude = r.Next(-50, 50),
                    Latitude = r.Next(-50, 50)
                }); ;
            }

            for (int i = 0; i < 5; i++)
            {
                Quadocopter newQ = new Quadocopter();
                newQ.IDNumber = r.Next(99, 1000);
                newQ.SkimmerModel = $"A2{i}";
                newQ.Weight = (WeightCategories)r.Next(3);
                ListQuadocopter.Add(newQ);
            }

            for (int i = 0; i < 10; i++)
            {
                ListClient.Add(new Client()
                {
                    Latitude = r.Next(-50, 50),
                    Longitude = r.Next(-50, 50),
                    ID = r.Next(99999999, 1000000000),
                    Telephone = $"0{r.Next(50, 59)}{r.Next(1000000, 10000000)}",
                    Name = clientsNames[r.Next(clientsNames.Length)]
                });
            }
            for (int i = 0; i < 7; i++)
            {
                Package newP = new Package();
                newP.ID = global::DalObject.DataSource.Config.IDPackage++;
                newP.IDSender = ListClient[(i + 1) % 5].ID;
                newP.IDgets = ListClient[i % 5].ID;
                newP.IDSkimmerOperation = 0;
                newP.Weight = (WeightCategories)r.Next(3);
                newP.priority = (Priorities)r.Next(3);
                newP.PackageCreationTime = DateTime.Now;
                ListPackage.Add(newP);
            }
            for (int i = 0; i < 2; i++)
            {
                Package newP = new Package();
                newP.ID = global::DalObject.DataSource.Config.IDPackage++;
                newP.IDSender = ListClient[(i + 1) % 5].ID;
                newP.IDgets = ListClient[i % 5].ID;
                newP.IDSkimmerOperation = ListQuadocopter[i].IDNumber;
                newP.Weight = (WeightCategories)r.Next(3);
                newP.priority = (Priorities)r.Next(3);
                newP.PackageCreationTime = DateTime.UtcNow;
                newP.TimeAssignGlider = DateTime.UtcNow;
                newP.PackageCollectionTime = DateTime.Now;
                ListPackage.Add(newP);
            }
            for (int i = 0; i < 1; i++)
            {
                Package newP = new Package();
                newP.ID = global::DalObject.DataSource.Config.IDPackage++;
                newP.IDSender = ListClient[(i + 1) % 5].ID;
                newP.IDgets = ListClient[i % 5].ID;
                newP.IDSkimmerOperation = ListQuadocopter[4].IDNumber;
                newP.Weight = (WeightCategories)r.Next(3);
                newP.priority = (Priorities)r.Next(3);
                newP.PackageCreationTime = DateTime.UtcNow;
                newP.TimeAssignGlider = DateTime.Now;
                ListPackage.Add(newP);
            }
            
        }
    }
  
}