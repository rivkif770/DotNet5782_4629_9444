using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace IDAL
{
    namespace DO
    {
        public struct Client
        {
            public int ID { get;  set; }
            public String Name { get; set; }
            public String Telephone { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public override string ToString()
            {
                String result = "";
                result += $"ID is: {ID}, \n";
                result += $"Name is: {Name}, \n";
                result += $"Telephone is: {Telephone.Substring(0,3)+Telephone.Substring(3)}, \n";
                result += $"Latitude is: {Latitude}, \n";
                result += $"Longitude is: {Longitude}, \n";
                Console.WriteLine(result);
                return result;
            }
        }
    }
}
/*      program:
 
       case Options.Display:
                        Console.WriteLine("adding option:\n 0-Exit\n; 1- Base Station View\n" +
                            " 2- Skimmer display\n" +
                            "3- Customer view\n " +
                            " 4-Package view\n");
                        displayOptions = (DisplayOptions)int.Parse(Console.ReadLine());
                        switch (displayOptions)
                        {
                             case DisplayOptions.Exit:
                                break;
                            case DisplayOptions.DisplayBaseStation:
                                Console.WriteLine("enter ID of BaseStation:");
                                int IDb;
                                IDb = int.Parse(Console.ReadLine());
                                DalObject.dalObject.DisplayBaseStation(IDb);
                                break;
                            case DisplayOptions.DisplaySkimmer:
                                Console.WriteLine("enter ID of Skimmer:");
                                int IDs;
                                IDs = int.Parse(Console.ReadLine());
                                DalObject.dalObject.DisplaySkimmer(IDs);
                                break;
                            case DisplayOptions.DisplayClient:
                                Console.WriteLine("enter ID of Client:");
                                int IDc;
                                IDc = int.Parse(Console.ReadLine());
                                DalObject.dalObject.DisplayClient(IDc);
                                break;
                            case DisplayOptions.DisplayPackage:
                                Console.WriteLine("enter ID of Package:");
                                int IDp;
                                IDp = int.Parse(Console.ReadLine());
                                DalObject.dalObject.DisplayPackage(IDp);
                                break;
                        }
                        break;
datasource:
 public static void DisplayBaseStation(BaseStation b)
        {
            int size=DataSource.ListBaseStation.count;
            for(int i=0; i<size;i++)
            {
                if(DataSource.ListBaseStation[i].UniqueID==iDb)
                   {
                        DataSource.ListBaseStation[i].ToString();
                    }
            }
        }
 public static void DisplaySkimmer(Client c)
        {
            int size=DataSource.ListClient.count;
            for(int i=0; i<size;i++)
            {
                if(DataSource.ListClient[i].IDNumber==iDc)
                   {
                        DataSource.ListClient[i].ToString();
                    }
            }
        }
 public static void DisplayClient(Skimmer s)
        {
            int size=DataSource.ListQuadocopter.count;
            for(int i=0; i<size;i++)
            {
                if(DataSource.ListQuadocopter[i].ID==iDs)
                   {
                        DataSource.ListQuadocopter[i].ToString();
                    }
            }
        }
 public static void DisplayPackage(Package p)
        {
            int size=DataSource.ListPackage.count;
            for(int i=0; i<size;i++)
            {
                if(DataSource.ListPackage[i].ID==iDp)
                   {
                        DataSource.ListPackage[i].ToString();
                    }
            }
        }
*/
