using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

namespace DalApi
{
    class DalXml : IDal
    {
        static readonly IDal instance = new DalXml();
        internal static IDal Instance { get { return instance; } }
        static DalXml() { }

        readonly string BaseStationPath = "BaseStation.xml";
        readonly string ClientPath = "Clien.xml";
        readonly string PackagePath = "Package.xml";
        readonly string SkimmerLoadingPath = "SkimmerLoading.xml";
        readonly string SkimmerPath = "Skimmer.xml";
        readonly string ConfigPsth = "Config.xml";
        DalXml()
        {
            DalObject.DataSource.Initialize();
            xml.XMLTools.SetBaseStationListFile(DalObject.DataSource.ListBaseStation, BaseStationPath);
            xml.XMLTools.SaveListToXMLSerializer(DalObject.DataSource.ListPackage, PackagePath);
            xml.XMLTools.SaveListToXMLSerializer(DalObject.DataSource.ListClient, ClientPath);
            xml.XMLTools.SaveListToXMLSerializer(DalObject.DataSource.ListQuadocopter, SkimmerPath);
            xml.XMLTools.SaveListToXMLSerializer(DalObject.DataSource.ListSkimmerLoading, SkimmerLoadingPath);
            xml.XMLTools.Config(ConfigPsth);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddBaseStation(BaseStation baseStation)
        {
            XElement rootBaseStationElemnt = xml.XMLTools.LoadListFromXmlElement(BaseStationPath);
            XElement baseStationElemnt = (from b in rootBaseStationElemnt.Elements()
                                          where Convert.ToInt32(b.Element("UniqueID").Value) == baseStation.UniqueID
                                          select b).FirstOrDefault();
            if (baseStationElemnt != null)
                throw new ExistsInSystemException("BaseStation Save to system", baseStation.UniqueID);
            else
            {
                XElement newBaseStation = new XElement("BaseStation",
                        new XElement("UniqueID", baseStation.UniqueID),
                        new XElement("StationName", baseStation.StationName),
                        new XElement("Latitude", baseStation.Latitude),
                        new XElement("Longitude", baseStation.Longitude),
                        new XElement("SeveralPositionsArgument", baseStation.SeveralPositionsArgument)
                        );
                rootBaseStationElemnt.Add(newBaseStation);
                xml.XMLTools.SaveListToXmlElement(rootBaseStationElemnt, BaseStationPath);
            }
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddClient(Client client)
        {
            List<Client> clients = xml.XMLTools.LoadListFromXMLSerializer<Client>(ClientPath);
            if (clients.Any(c => c.ID == client.ID)) throw new ExistsInSystemException($"Customer {client.ID} Save to system", Severity.Mild);
            clients.Add(client);
            xml.XMLTools.SaveListToXMLSerializer(clients, ClientPath);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddPackage(Package package)
        {
            List<Package> packages = xml.XMLTools.LoadListFromXMLSerializer<DO.Package>(PackagePath);
            if (packages.Any(p => p.ID == package.ID)) throw new ExistsInSystemException("BaseStation Save to system", package.ID);
            package.PackageCreationTime = DateTime.Now;
            XElement rootConfigElement = xml.XMLTools.LoadListFromXmlElement(ConfigPsth);
            XElement idPackage = rootConfigElement.Element("IDPackage");

            int id = Convert.ToInt32(idPackage.Value);
            idPackage.Value = (Convert.ToInt32(idPackage.Value) + 1).ToString();

            xml.XMLTools.SaveListToXmlElement(rootConfigElement, ConfigPsth);

            package.ID = id;
            packages.Add(package);
            xml.XMLTools.SaveListToXMLSerializer(packages, PackagePath);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddSkimmer(Quadocopter quadocopter)
        {
            List<Quadocopter> Quadocopters = xml.XMLTools.LoadListFromXMLSerializer<DO.Quadocopter>(SkimmerPath);
            if (Quadocopters.Any(q => q.IDNumber == quadocopter.IDNumber)) throw new ExistsInSystemException("skimmer Save to system", quadocopter.IDNumber);
            Quadocopters.Add(quadocopter);
            xml.XMLTools.SaveListToXMLSerializer(Quadocopters, SkimmerPath);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddSkimmerLoading(SkimmerLoading SL)
        {
            List<SkimmerLoading> SkimmerLoadings = xml.XMLTools.LoadListFromXMLSerializer<SkimmerLoading>(SkimmerLoadingPath);
            //if (SkimmerLoadings.Any(s => s.SkimmerID == SL.SkimmerID)) throw new ExistsInSystemException($"SkimmerLoading {SL.SkimmerID} Save to system", Severity.Mild);
            SkimmerLoadings.Add(SL);
            xml.XMLTools.SaveListToXMLSerializer(SkimmerLoadings, SkimmerLoadingPath);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AssignPackageSkimmer(int idp, int idq)
        {
            Package temp_p = GetPackage(idp);
            temp_p.IDSkimmerOperation = idq;//Change the skimmer ID of the package to the associated skimmer ID
            temp_p.TimeAssignGlider = DateTime.Now;//Update time association

            List<Package> Packages = xml.XMLTools.LoadListFromXMLSerializer<Package>(PackagePath);
            Packages.RemoveAll(packag => packag.ID == idp);
            Packages.Add(temp_p);
            xml.XMLTools.SaveListToXMLSerializer(Packages, PackagePath);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CollectionPackage(int idp)
        {
            Package temp_p = this.GetPackage(idp);
            temp_p.PackageCollectionTime = DateTime.Now;//Update the package collection time from the sender

            List<Package> Packages = xml.XMLTools.LoadListFromXMLSerializer<Package>(PackagePath);
            Packages.RemoveAll(packag => packag.ID == idp);
            Packages.Add(temp_p);
            xml.XMLTools.SaveListToXMLSerializer(Packages, PackagePath);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteBaseStation(int idb)
        {
            XElement rootBaseStationElemnt = xml.XMLTools.LoadListFromXmlElement(BaseStationPath);
            XElement baseStationElemnt = (from b in rootBaseStationElemnt.Elements()
                                          where Convert.ToInt32(b.Element("UniqueID").Value) == idb
                                          select b).FirstOrDefault();
            if (baseStationElemnt == null)
                throw new ExistsInSystemException("BaseStation dont Save to system", idb);
            else
            {
                baseStationElemnt.Remove();
                xml.XMLTools.SaveListToXmlElement(rootBaseStationElemnt, BaseStationPath);
            }
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteClient(int IDc)
        {
            var clientList = xml.XMLTools.LoadListFromXMLSerializer<Client>(ClientPath);
            if (!clientList.Any(c => c.ID == IDc))
                throw new IdDoesNotExistException($"Customer {IDc} dont Save to system", Severity.Mild);
            Client client = clientList.Find(c => c.ID == IDc);
            clientList.Remove(client);
            xml.XMLTools.SaveListToXMLSerializer(clientList, ClientPath);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeletePackage(int id)
        {
            var packages = xml.XMLTools.LoadListFromXMLSerializer<DO.Package>(PackagePath);
            if(!packages.Any(p=>p.ID==id)) throw new ExistsInSystemException("Package dont Save to system", id);
            Package package = packages.Find(p => p.ID == id);
            packages.Remove(package);
            xml.XMLTools.SaveListToXMLSerializer(packages, PackagePath);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteSkimmer(int idq)
        {
            var skimmers = xml.XMLTools.LoadListFromXMLSerializer<DO.Quadocopter>(SkimmerPath);
            if (!skimmers.Any(s => s.IDNumber == idq)) throw new ExistsInSystemException("skimmer dont Save to system", idq);
            Quadocopter skimmer = skimmers.Find(s => s.IDNumber == idq);
            skimmers.Remove(skimmer);
            xml.XMLTools.SaveListToXMLSerializer(skimmers, SkimmerPath);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteSkimmerLoading(int idsl)
        {
            var SkimmerLoadingList = xml.XMLTools.LoadListFromXMLSerializer<SkimmerLoading>(SkimmerLoadingPath);
            if (!SkimmerLoadingList.Any(s => s.SkimmerID == idsl))
                throw new ExistsInSystemException($"Skimmer {idsl} Save to system of SkimmerLoading", Severity.Mild);
            SkimmerLoading skimmerLoading = SkimmerLoadingList.Find(s => s.SkimmerID == idsl);
            SkimmerLoadingList.Remove(skimmerLoading);
            xml.XMLTools.SaveListToXMLSerializer(SkimmerLoadingList, SkimmerLoadingPath);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation GetBaseStation(int IDb)
        {
            XElement rootBaseStationElemnt = xml.XMLTools.LoadListFromXmlElement(BaseStationPath);
            XElement baseStationElemnt = (from b in rootBaseStationElemnt.Elements()
                                          where Convert.ToInt32(b.Element("UniqueID").Value) == IDb
                                          select b).FirstOrDefault();
            if (baseStationElemnt == null)
                throw new ExistsInSystemException("id does not exist!!", IDb);
            else
            {
                BaseStation newBaseStation = new BaseStation()
                {
                    UniqueID = Convert.ToInt32(baseStationElemnt.Element("UniqueID").Value),
                    StationName = baseStationElemnt.Element("StationName").Value.ToString(),
                    Latitude = Convert.ToDouble(baseStationElemnt.Element("Latitude").Value),
                    Longitude = Convert.ToDouble(baseStationElemnt.Element("Longitude").Value),
                    SeveralPositionsArgument = Convert.ToInt32(baseStationElemnt.Element("SeveralPositionsArgument").Value)
                };
                      
                return newBaseStation;
            }
        }//עובד
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> GetBaseStationList(Func<BaseStation, bool> predicate = null)
        {
            IEnumerable<BaseStation> baseStations = new List<BaseStation>();
            XElement rootBaseStationElemnt = xml.XMLTools.LoadListFromXmlElement(BaseStationPath);
            baseStations = (from b in rootBaseStationElemnt.Elements()
                            select new BaseStation()
                            {
                                UniqueID = Convert.ToInt32(b.Element("UniqueID").Value),
                                StationName = b.Element("StationName").Value.ToString(),
                                Latitude = Convert.ToDouble(b.Element("Latitude").Value),
                                Longitude = Convert.ToDouble(b.Element("Longitude").Value),
                                SeveralPositionsArgument = Convert.ToInt32(b.Element("SeveralPositionsArgument").Value)
                            }).ToList();
            if (predicate == null) return baseStations;
            return baseStations.Where(predicate);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Client GetClient(int IDc)
        {
            var clientList = xml.XMLTools.LoadListFromXMLSerializer<Client>(ClientPath);
            if (!clientList.Any(c => c.ID == IDc))
                throw new IdDoesNotExistException($"id : {IDc} does not exist!!", Severity.Mild);
            Client client = clientList.Find(c => c.ID == IDc);
            return client;
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Client> GetClientList(Func<Client, bool> predicate = null)
        {
            if (predicate == null) return xml.XMLTools.LoadListFromXMLSerializer<Client>(ClientPath);
            return xml.XMLTools.LoadListFromXMLSerializer<Client>(ClientPath).Where(predicate);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Package GetPackage(int idp)
        {
            var packages = xml.XMLTools.LoadListFromXMLSerializer<DO.Package>(PackagePath);
            if (!packages.Any(p => p.ID == idp)) throw new ExistsInSystemException("Package dont Save to system", idp);
            Package package = packages.Find(p => p.ID == idp);
            return package;
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Package> GetPackageList(Func<Package, bool> predicate = null)
        {
            if (predicate == null) return xml.XMLTools.LoadListFromXMLSerializer<DO.Package>(PackagePath);
            return xml.XMLTools.LoadListFromXMLSerializer<DO.Package>(PackagePath).Where(predicate);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Quadocopter> GetQuadocopterList(Func<Quadocopter, bool> predicate = null)
        {
            if (predicate == null) return xml.XMLTools.LoadListFromXMLSerializer<DO.Quadocopter>(SkimmerPath);
            return xml.XMLTools.LoadListFromXMLSerializer<DO.Quadocopter>(SkimmerPath).Where(predicate);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Quadocopter GetQuadrocopter(int IDq)
        {
            var skimmers = xml.XMLTools.LoadListFromXMLSerializer<DO.Quadocopter>(SkimmerPath);
            if (!skimmers.Any(s => s.IDNumber == IDq)) throw new ExistsInSystemException("Package dont Save to system", IDq);
            Quadocopter skimmer = skimmers.Find(s => s.IDNumber == IDq);
            return skimmer;
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public SkimmerLoading GetSkimmerLoading(int IDsl)
        {
            var skimmerLoadingList = xml.XMLTools.LoadListFromXMLSerializer<SkimmerLoading>(SkimmerLoadingPath);
            if (!skimmerLoadingList.Any(s => s.SkimmerID == IDsl))
                throw new IdDoesNotExistException($"id : {IDsl} does not exist!!", Severity.Mild);
            SkimmerLoading skimmerLoading = skimmerLoadingList.Find(s => s.SkimmerID == IDsl);
            return skimmerLoading;
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<SkimmerLoading> GetSkimmerLoadingList()
        {
            return xml.XMLTools.LoadListFromXMLSerializer<SkimmerLoading>(SkimmerLoadingPath);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PackageDelivery(int idp)
        {
            Package temp_p = this.GetPackage(idp);
            temp_p.TimeArrivalRecipient = DateTime.Now;//Update the time of arrival of the package to the recipient

            List<Package> Packages = xml.XMLTools.LoadListFromXMLSerializer<Package>(PackagePath);
            Packages.RemoveAll(packag => packag.ID == idp);
            Packages.Add(temp_p);
            xml.XMLTools.SaveListToXMLSerializer(Packages, PackagePath);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] PowerConsumptionRequest()
        {
            XElement rootConfigElement = xml.XMLTools.LoadListFromXmlElement(ConfigPsth);

            double[] arry = new double[5];
            arry[0] = Convert.ToDouble(rootConfigElement.Element("Free").Value);
            arry[1] = Convert.ToDouble(rootConfigElement.Element("LightWeightCarrier").Value);
            arry[2] = Convert.ToDouble(rootConfigElement.Element("MediumWeightCarrier").Value);
            arry[3] = Convert.ToDouble(rootConfigElement.Element("HeavyWeightCarrier").Value);
            arry[4] = Convert.ToDouble(rootConfigElement.Element("SkimmerLoadingRate").Value);

            return arry;
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpadteB(BaseStation baseStation)
        {
            XElement rootBaseStationElemnt = xml.XMLTools.LoadListFromXmlElement(BaseStationPath);
            XElement baseStationElemnt = (from b in rootBaseStationElemnt.Elements()
                                          where Convert.ToInt32(b.Element("UniqueID").Value) == baseStation.UniqueID
                                          select b).FirstOrDefault();
            if (baseStationElemnt == null)
                throw new ExistsInSystemException("BaseStation Not Find", baseStation.UniqueID);
            else
            {
                baseStationElemnt.Remove();
                xml.XMLTools.SaveListToXmlElement(rootBaseStationElemnt, BaseStationPath);
                AddBaseStation(baseStation);
                
            }
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpadteC(Client client)
        {
            var clientList = xml.XMLTools.LoadListFromXMLSerializer<Client>(ClientPath);
            Client newclient = clientList.Find(c => c.ID == client.ID);
            clientList.Remove(newclient);
            newclient = client;
            clientList.Add(newclient);
            xml.XMLTools.SaveListToXMLSerializer(clientList, ClientPath);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpadteP(Package package)
        {
            var packages = xml.XMLTools.LoadListFromXMLSerializer<DO.Package>(PackagePath);
            Package newpackage = packages.Find(p => p.ID == package.ID);
            packages.Remove(newpackage);
            newpackage = package;
            packages.Add(newpackage);
            xml.XMLTools.SaveListToXMLSerializer(packages, PackagePath);
        }//worke
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpadteQ(Quadocopter quadocopter)
        {
            var skimmers = xml.XMLTools.LoadListFromXMLSerializer<DO.Quadocopter>(SkimmerPath);
            Quadocopter newskimmer = skimmers.Find(s => s.IDNumber == quadocopter.IDNumber);
            skimmers.Remove(newskimmer);
            newskimmer = quadocopter;
            skimmers.Add(newskimmer);
            xml.XMLTools.SaveListToXMLSerializer(skimmers, SkimmerPath);
        }//worke
    }
}
