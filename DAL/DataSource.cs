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
                newP.PackageCreationTime = DateTime.Now;
                ListPackage.Add(newP);
            }
        }
    }
    public class dalObject
    {
        public static void AddBaseStation(BaseStation b)//Adding a station
        {
            DataSource.ListBaseStation.Add(b);
        }
        public static void AddSkimmer(Quadocopter q)//added a skimmer
        {
            DataSource.ListQuadocopter.Add(q);
        }
        public static void AddClient(Client c)//Adding a customer
        {
            DataSource.ListClient.Add(c);
        }
        public static void AddPackage(Package p)//Add a package
        {
            p.ID = DalObject.DataSource.Config.IDPackage++;
            DataSource.ListPackage.Add(p);
        }
        //////////////////////////////////////////////////////////
        public static void AssignPackageSkimmer(int idp,int idq)//Assign a package to a skimmer
        {
            Quadocopter temp_q = new Quadocopter();//Create a new skimmer object
            Package temp_p = new Package();//Create a new package type object
            int result = DataSource.ListQuadocopter.FindIndex(x => x.IDNumber == idq);//Search for the location of the skimmer
            temp_q.IDNumber = DataSource.ListQuadocopter[result].IDNumber;
            temp_q.SkimmerModel = DataSource.ListQuadocopter[result].SkimmerModel;
            temp_q.Weight = DataSource.ListQuadocopter[result].Weight;
            temp_q.Battery = DataSource.ListQuadocopter[result].Battery;
            temp_q.SkimmerMode = (DronStatuses)2;//Change the glider position to catch
            DataSource.ListQuadocopter.RemoveAt(result);//Deleting the old skimmer object
            DataSource.ListQuadocopter.Add(temp_q);//Deleting the new skimmer object includes the change

            int result1 = DataSource.ListPackage.FindIndex(x => x.ID == idp);//Search for the location of the package
            temp_p.ID = DataSource.ListPackage[result1].ID;
            temp_p.IDSender = DataSource.ListPackage[result1].IDSender;
            temp_p.IDgets = DataSource.ListPackage[result1].IDgets;
            temp_p.Weight = DataSource.ListPackage[result1].Weight;
            temp_p.priority = DataSource.ListPackage[result1].priority;
            temp_p.IDSkimmerOperation = idq;//Change the skimmer ID of the package to the associated skimmer ID
            temp_p.PackageCreationTime = DataSource.ListPackage[result1].PackageCreationTime;
            temp_p.TimeAssignGlider = DateTime.Now;//Update time association
            temp_p.PackageCollectionTime = DataSource.ListPackage[result1].PackageCollectionTime;
            temp_p.TimeArrivalRecipient = DataSource.ListPackage[result1].TimeArrivalRecipient;
            DataSource.ListPackage.RemoveAt(result1);//Deleting the old Package object
            DataSource.ListPackage.Add(temp_p);//Deleting the new Package object includes the change
        }
        public static void CollectionPackage(int id)//Update package collection by skimmer
        {
            Package temp_p = new Package();//Create a new package type object
            int result = DataSource.ListPackage.FindIndex(x => x.ID == id);//Search for the location of the package
            temp_p.ID = DataSource.ListPackage[result].ID;
            temp_p.IDSender = DataSource.ListPackage[result].IDSender;
            temp_p.IDgets = DataSource.ListPackage[result].IDgets;
            temp_p.Weight = DataSource.ListPackage[result].Weight;
            temp_p.priority = DataSource.ListPackage[result].priority;
            temp_p.IDSkimmerOperation = DataSource.ListPackage[result].IDSkimmerOperation;
            temp_p.PackageCreationTime = DataSource.ListPackage[result].PackageCreationTime;
            temp_p.TimeAssignGlider = DataSource.ListPackage[result].TimeAssignGlider;
            temp_p.PackageCollectionTime = DateTime.Now;//Update the package collection time from the sender
            temp_p.TimeArrivalRecipient = DataSource.ListPackage[result].TimeArrivalRecipient;
            DataSource.ListPackage.RemoveAt(result);//Deleting the old Package object
            DataSource.ListPackage.Add(temp_p);//Deleting the new Package object includes the change
        }
        public static void PackageDelivery(int id)//Delivery of a package to the customer
        {
            Package temp_p = new Package();//Create a new package type object
            int result = DataSource.ListPackage.FindIndex(x => x.ID == id);//Search for the location of the package
            temp_p.ID = DataSource.ListPackage[result].ID;
            temp_p.IDSender = DataSource.ListPackage[result].IDSender;
            temp_p.IDgets = DataSource.ListPackage[result].IDgets;
            temp_p.Weight = DataSource.ListPackage[result].Weight;
            temp_p.priority = DataSource.ListPackage[result].priority;
            temp_p.IDSkimmerOperation = DataSource.ListPackage[result].IDSkimmerOperation;
            temp_p.PackageCreationTime = DataSource.ListPackage[result].PackageCreationTime;
            temp_p.TimeAssignGlider = DataSource.ListPackage[result].TimeAssignGlider;
            temp_p.PackageCollectionTime = DataSource.ListPackage[result].PackageCollectionTime;
            temp_p.TimeArrivalRecipient = DateTime.Now;//Update the time of arrival of the package to the recipient
            DataSource.ListPackage.RemoveAt(result);///Deleting the old Package object
            DataSource.ListPackage.Add(temp_p);//Deleting the new Package object includes the change
        }
        public static void SendingSkimmerForCharging(int id,int idBS)//Sending a skimmer for charging at a base station
        {
            int result = DataSource.ListQuadocopter.FindIndex(x => x.IDNumber == id);//Search for the location of the Quadocopter
            Quadocopter temp_q = new Quadocopter();//Create a new Quadocopter type object
            temp_q.IDNumber = DataSource.ListQuadocopter[result].IDNumber;
            temp_q.SkimmerModel = DataSource.ListQuadocopter[result].SkimmerModel;
            temp_q.Weight = DataSource.ListQuadocopter[result].Weight;
            temp_q.Battery = 0;//Update the battery to 0 percent
            temp_q.SkimmerMode = (DronStatuses)1;//Skimmer status update for maintenance
            DataSource.ListQuadocopter.RemoveAt(result);//Deleting the old Quadocopter object
            DataSource.ListQuadocopter.Add(temp_q);//Deleting the new Quadocopter object includes the change

            int result1 = DataSource.ListBaseStation.FindIndex(x => x.UniqueID == idBS);//Search for the location of the BaseStation
            BaseStation temp_b = new BaseStation();//Create a new BaseStation type object
            temp_b.UniqueID = DataSource.ListBaseStation[result1].UniqueID;
            temp_b.StationName = DataSource.ListBaseStation[result1].StationName;
            temp_b.SeveralPositionsArgument = DataSource.ListBaseStation[result1].SeveralPositionsArgument-1;//Update the number of stations available for charging at least 1
            temp_b.Latitude = DataSource.ListBaseStation[result1].Latitude;
            temp_b.Longitude = DataSource.ListBaseStation[result1].Longitude;
            DataSource.ListBaseStation.RemoveAt(result1);//Deleting the old BaseStation object
            DataSource.ListBaseStation.Add(temp_b);//Deleting the new BaseStation object includes the change

            SkimmerLoading newSkimmerLoading = new SkimmerLoading();//Create a new SkimmerLoading type object
            newSkimmerLoading.SkimmerID = id;//Inserting a skimmer ID into a skimmer pavement charge
            newSkimmerLoading.StationID = idBS;//Inserting a skimmer ID into a Base Station pavement charge
            DataSource.ListSkimmerLoading.Add(newSkimmerLoading);//Deleting the new SkimmerLoading object includes the change
        }
        public static void SkimmerRelease(int id)//Release skimmer from base charge
        {
            int result = DataSource.ListQuadocopter.FindIndex(x => x.IDNumber == id);//Search for the location of the Quadocopter
            Quadocopter temp_q = new Quadocopter();//Create a new BaseStation type object
            temp_q.IDNumber = DataSource.ListQuadocopter[result].IDNumber;
            temp_q.SkimmerModel = DataSource.ListQuadocopter[result].SkimmerModel;
            temp_q.Weight = DataSource.ListQuadocopter[result].Weight;
            temp_q.Battery = 100;//Update the battery to 100 percent
            temp_q.SkimmerMode = (DronStatuses)0;//Skimmer status update available
            DataSource.ListQuadocopter.RemoveAt(result);//Deleting the old Quadocopter object
            DataSource.ListQuadocopter.Add(temp_q);//Deleting the new Quadocopter object includes the change

            int idBS;
            int result1 = DataSource.ListSkimmerLoading.FindIndex(x => x.SkimmerID == id);//Search for the location of the SkimmerLoading
            idBS = DataSource.ListSkimmerLoading[result1].StationID;//Check which base station the skimmer is loaded on
            int result2 = DataSource.ListBaseStation.FindIndex(x => x.UniqueID == idBS);//Search for the location of the BaseStation
            BaseStation temp_b = new BaseStation();//Create a new BaseStation type object
            temp_b.UniqueID = DataSource.ListBaseStation[result2].UniqueID;
            temp_b.StationName = DataSource.ListBaseStation[result2].StationName;
            temp_b.SeveralPositionsArgument = DataSource.ListBaseStation[result2].SeveralPositionsArgument + 1;//Update the number of stations available for charging at least 1
            temp_b.Latitude = DataSource.ListBaseStation[result2].Latitude;
            temp_b.Longitude = DataSource.ListBaseStation[result2].Longitude;
            DataSource.ListBaseStation.RemoveAt(result2);//Deleting the old BaseStation object
            DataSource.ListBaseStation.Add(temp_b);//Deleting the new BaseStation object includes the change

        }
        //////////////////////////////////////////////////////////
        public static void DisplayBaseStation(int IDb)//Base station view by appropriate ID
        {
            int result = DataSource.ListBaseStation.FindIndex(x => x.UniqueID == IDb);
            DataSource.ListBaseStation[result].ToString();
        }
        public static void DisplayClient(int IDc)//Client view by appropriate ID
        {
            int result = DataSource.ListClient.FindIndex(x => x.ID == IDc);
            DataSource.ListClient[result].ToString();
        }
        public static void DisplaySkimmer(int iDq)//Skimmer view by appropriate ID
        {
            int result = DataSource.ListQuadocopter.FindIndex(x => x.IDNumber == iDq);
            DataSource.ListQuadocopter[result].ToString();
        }
        public static void DisplayPackage(int iDp)//Package view by appropriate ID
        {
            int result = DataSource.ListPackage.FindIndex(x => x.ID == iDp);
            DataSource.ListPackage[result].ToString();
        }
        //////////////////////////////////////////////////////////
        public static void printBaseStation()//Displays a list of base stations
        {
            DataSource.ListBaseStation.ForEach(x => x.ToString()); 
        }
        public static void printSkimmer()//Displays a list of Skimmer
        {
            DataSource.ListQuadocopter.ForEach(x => x.ToString());
        }
        public static void printClient()//Displays a list of Client
        {
            DataSource.ListClient.ForEach(x => x.ToString());
        }
        public static void printPackage()//Displays a list of Package
        {
            DataSource.ListPackage.ForEach(x => x.ToString());
        }
        public static void printPackageWithoutSkimmer()//Displays a list of Packages not yet associated with the glider
        {

            for (int i = 0; i < DataSource.ListPackage.Count; i++)
            {
                if(DataSource.ListPackage[i].IDSkimmerOperation == 0)
                {
                    DataSource.ListPackage[i].ToString();
                }
            }
        }
        public static void printBaseStationFreeCharging()//Displays a list of Base stations with available charging stations
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