using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace DalObject
{
    internal class DataSource
    {
        Quadocopter[] QuadocopterArry = new Quadocopter[10];
        BaseStation[] BaseStationArry = new BaseStation[5];
        Package[] PackageArry = new Package[1000];
        Client[] ClientArry = new Client[100];
        internal class Config
        {
            internal static  int Q = 0;//pointer to Quadocopter
            internal static  int B = 0;//pointer to BaseStation
            internal static  int P = 0;//pointer to Package
            internal static  int C = 0;//pointer to Client
        }
        public static void Initialize(DataSource DJ )
        {
            Random r = new Random();
            int num = r.Next();
            for (int i = Config.B; i < 2; i++)
            {
                DJ.BaseStationArry[i].UniqueID = r.Next(999,10000);
                DJ.BaseStationArry[i].SeveralPositionsArgument = r.Next(5);
                DJ.BaseStationArry[i].Latitude = r.Next(-100,100);
                DJ.BaseStationArry[i].Longitude = r.Next(-100,100);
            }
            DJ.BaseStationArry[0].StationName = "Tamar";
            DJ.BaseStationArry[1].StationName = "Rimon";
            for (int i = Config.Q ; i < 5; i++)
            {
                DJ.QuadocopterArry[i].IDNumber = r.Next(99,1000);
                DJ.QuadocopterArry[i].Battery = r.Next(101);
                DJ.QuadocopterArry[i].Weight = (WeightCategories)r.Next(3);
                DJ.QuadocopterArry[i].SkimmerMode = (DronStatuses)r.Next(3);
            }

            DJ.QuadocopterArry[0].SkimmerModel = "A20";
            DJ.QuadocopterArry[1].SkimmerModel = "A21";
            DJ.QuadocopterArry[2].SkimmerModel = "A22";
            DJ.QuadocopterArry[3].SkimmerModel = "A23";
            DJ.QuadocopterArry[4].SkimmerModel = "A24";

            for (int i = Config.C; i < 10; i++)
            {
                DJ.ClientArry[i].Latitude = r.Next(-100, 100);
                DJ.ClientArry[i].Longitude = r.Next(-100, 100);
                DJ.ClientArry[i].ID = r.Next(999999999, 1000000000);
                DJ.ClientArry[i].Telephone = $"0{r.Next(50,59)}-{r.Next(1000000,10000000)}";
            }
            DJ.ClientArry[0].Name = "David";
            DJ.ClientArry[1].Name = "Sara";
            DJ.ClientArry[2].Name = "Chaim";
            DJ.ClientArry[3].Name = "Moshe";
            DJ.ClientArry[4].Name = "Jon";
            DJ.ClientArry[5].Name = "Rivki";
            DJ.ClientArry[6].Name = "Efrat";
            DJ.ClientArry[7].Name = "Chani";
            DJ.ClientArry[8].Name = "Goldi";
            DJ.ClientArry[9].Name = "Pazit";

            for (int i = Config.P; i < 10; i++)
            {
                DJ.PackageArry[i].ID = r.Next(99999, 100000);
                DJ.PackageArry[i].IDSender =DJ.ClientArry[r.Next(6)].ID ;
                DJ.PackageArry[i].IDgets = DJ.ClientArry[r.Next(6)].ID;
                DJ.PackageArry[i].Weight = (WeightCategories)r.Next(3);
                DJ.PackageArry[i].priority = (Priorities)r.Next(3);
                DJ.PackageArry[i].PackageCreationTime = new DateTime(2021, r.Next(1, 13), r.Next(1, 30), r.Next(1, 24), r.Next(1, 61),r.Next(1, 61));
                DJ.PackageArry[i].TimeAssignGlider = new DateTime(2021, r.Next(1, 13), r.Next(1, 30), r.Next(1, 24), r.Next(1, 61), r.Next(1, 61));
                DJ.PackageArry[i].PackageCollectionTime = new DateTime(2021, r.Next(1, 13), r.Next(1, 30), r.Next(1, 24), r.Next(1, 61), r.Next(1, 61));
                DJ.PackageArry[i].TimeArrivalRecipient = new DateTime(2021, r.Next(1, 13), r.Next(1, 30), r.Next(1, 24), r.Next(1, 61), r.Next(1, 61));
            }
        }
    }
}