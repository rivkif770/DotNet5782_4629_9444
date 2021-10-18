using System;
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
            DJ.BaseStationArry[0].StationName = "Jojo1";
            DJ.BaseStationArry[1].StationName = "Jojo2";
            for (int i = Config.Q ; i < 5; i++)
            {
                DJ.QuadocopterArry[i].IDNumber = r.Next(99,1000);
                DJ.QuadocopterArry[i].Battery = r.Next(101);
            }
           
            DJ.QuadocopterArry[0].SkimmerModel = "A20";
            DJ.QuadocopterArry[0].Weight = WeightCategories.low;
            DJ.QuadocopterArry[0].SkimmerMode = DronStatuses.free;

            DJ.QuadocopterArry[1].SkimmerModel = "A21";
            DJ.QuadocopterArry[1].Weight = WeightCategories.heavy;
            DJ.QuadocopterArry[1].SkimmerMode = DronStatuses.shipping;

            DJ.QuadocopterArry[2].SkimmerModel = "A22";
            DJ.QuadocopterArry[2].Weight = WeightCategories.middle;
            DJ.QuadocopterArry[2].SkimmerMode = DronStatuses.shipping;

            DJ.QuadocopterArry[3].SkimmerModel = "A23";
            DJ.QuadocopterArry[3].Weight = WeightCategories.middle;
            DJ.QuadocopterArry[3].SkimmerMode = DronStatuses.maintenance;


            DJ.QuadocopterArry[4].SkimmerModel = "A24";
            DJ.QuadocopterArry[4].Weight = WeightCategories.low;
            DJ.QuadocopterArry[4].SkimmerMode = DronStatuses.maintenance;

            for (int i = Config.C; i < 10; i++)
            {
                DJ.ClientArry[i].Latitude = r.Next(-100, 100);
                DJ.ClientArry[i].Longitude = r.Next(-100, 100);
                DJ.ClientArry[i].ID = r.Next(999999999, 1000000000);
            }
            DJ.ClientArry[0].Name = "David";
            DJ.ClientArry[0].Telephone = "052-4578107";
            DJ.ClientArry[1].Name = "Sara";
            DJ.ClientArry[0].Telephone = "053-5223655";
            DJ.ClientArry[2].Name = "Chaim";
            DJ.ClientArry[0].Telephone = "052-3222781";
            DJ.ClientArry[3].Name = "Moshe";
            DJ.ClientArry[0].Telephone = "054-67703211";
            DJ.ClientArry[4].Name = "Jon";
            DJ.ClientArry[0].Telephone = "052-8826402";
            DJ.ClientArry[5].Name = "Rivki";
            DJ.ClientArry[0].Telephone = "055-0399517";
            DJ.ClientArry[6].Name = "Efrat";
            DJ.ClientArry[0].Telephone = "058-1770441";
            DJ.ClientArry[7].Name = "Chani";
            DJ.ClientArry[0].Telephone = "053-5722946";
            DJ.ClientArry[8].Name = "Goldi";
            DJ.ClientArry[0].Telephone = "052-2044763";
            DJ.ClientArry[9].Name = "Pazit";
            DJ.ClientArry[0].Telephone = "055-2584600";

            for (int i = Config.P; i < 10; i++)
            {
                DJ.PackageArry[i].ID = r.Next(99999, 100000);
                DJ.PackageArry[i].IDSender =DJ.ClientArry[r.Next(6)].ID ;
                DJ.PackageArry[i].IDgets = DJ.ClientArry[r.Next(6)].ID;
            }

            DJ.PackageArry[0].Weight = WeightCategories.heavy;
            DJ.PackageArry[0].priority = Priorities.emergency;
            DJ.PackageArry[0].IDSkimmerOperation = DronStatuses.shipping;

            DJ.PackageArry[1].Weight = WeightCategories.middle;
            DJ.PackageArry[1].priority = Priorities.emergency;
            DJ.PackageArry[1].IDSkimmerOperation = DronStatuses.free;

            DJ.PackageArry[2].Weight = WeightCategories.low;
            DJ.PackageArry[2].priority = Priorities.regular;
            DJ.PackageArry[2].IDSkimmerOperation = DronStatuses.maintenance;

            DJ.PackageArry[3].Weight = WeightCategories.low;
            DJ.PackageArry[3].priority = Priorities.regular;
            DJ.PackageArry[3].IDSkimmerOperation = DronStatuses.free;

            DJ.PackageArry[4].Weight = WeightCategories.heavy;
            DJ.PackageArry[4].priority = Priorities.emergency;
            DJ.PackageArry[4].IDSkimmerOperation = DronStatuses.shipping;

            DJ.PackageArry[5].Weight = WeightCategories.middle;
            DJ.PackageArry[5].priority = Priorities.fast;
            DJ.PackageArry[5].IDSkimmerOperation = DronStatuses.free;

            DJ.PackageArry[6].Weight = WeightCategories.low;
            DJ.PackageArry[6].priority = Priorities.regular;
            DJ.PackageArry[6].IDSkimmerOperation = DronStatuses.maintenance;

            DJ.PackageArry[7].Weight = WeightCategories.low;
            DJ.PackageArry[7].priority = Priorities.emergency;
            DJ.PackageArry[7].IDSkimmerOperation = DronStatuses.shipping;

            DJ.PackageArry[8].Weight = WeightCategories.heavy;
            DJ.PackageArry[8].priority = Priorities.regular;
            DJ.PackageArry[8].IDSkimmerOperation = DronStatuses.shipping;

            DJ.PackageArry[9].Weight = WeightCategories.heavy;
            DJ.PackageArry[9].priority = Priorities.fast;
            DJ.PackageArry[9].IDSkimmerOperation = DronStatuses.maintenance;
        }
    }
}