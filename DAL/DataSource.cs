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
            for (int i = Config.Q ; i < 5; i++)
            {
                DJ.QuadocopterArry[i].IDNumber = r.Next(1000);
                //DJ.QuadocopterArry[i].SkimmerModel = (r.Next(3) * i) % 1000;
                //DJ.QuadocopterArry[i].WeightCategories = r.Next(3);
                //DJ.QuadocopterArry[i].DronStatuses = r.Next(3);
                DJ.QuadocopterArry[i].SkimmerMode = r.Next(3);
            }
        }
    }
}