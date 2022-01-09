using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DalApi
{
    class DalXml : IDal
    {
        static readonly IDal instance = new DalXml();
        internal static IDal Instance { get { return instance; } }
        static DalXml() { }
        
        private string BaseStationPath = "BaseStation.xml";
        private string ClientPath = "Clien.xml";
        private string PackagePath = "Package.xml";
        private string SkimmerLoadingPath = "SkimmerLoading.xml";
        private string SkimmerPath = "Skimmer.xml";
        DalXml()
        {
            DalObject.DataSource.Initialize();
            //xml.XMLTools.SetBaseStationListFile(DalObject.DataSource.ListBaseStation, BaseStationPath);
            //xml.XMLTools.SaveListToXMLSerializer(DalObject.DataSource.ListPackage, PackagePath);
            //xml.XMLTools.SaveListToXMLSerializer(DalObject.DataSource.ListClient, ClientPath);
            //xml.XMLTools.SaveListToXMLSerializer(DalObject.DataSource.ListQuadocopter, SkimmerPath);
            //xml.XMLTools.SaveListToXMLSerializer(DalObject.DataSource.ListSkimmerLoading, SkimmerLoadingPath);
        }

        public void AddBaseStation(BaseStation baseStation)
        {
            XElement rootBaseStationElemnt = xml.XMLTools.LoadListFromXmlElement(BaseStationPath);
            XElement baseStationElemnt = (from b in rootBaseStationElemnt.Elements()
                                          where Convert.ToInt32(b.Element("Id").Value) == baseStation.UniqueID
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
        }

        public void AddClient(Client c)
        {
            throw new NotImplementedException();
        }

        public void AddPackage(Package package)
        {
            List<Package> packages = xml.XMLTools.LoadListFromXMLSerializer<DO.Package>(PackagePath);
            if(packages.Any(p=>p.ID ==package.ID)) throw new ExistsInSystemException("BaseStation Save to system", package.ID);
            package.PackageCreationTime = DateTime.Now;
            //package.ID = GetPackage();
            packages.Add(package);
            xml.XMLTools.SaveListToXMLSerializer(packages, PackagePath);
           // return package.ID;
        }

        public void AddSkimmer(Quadocopter quadocopter)
        {
            List<Quadocopter> Quadocopters = xml.XMLTools.LoadListFromXMLSerializer<DO.Quadocopter>(SkimmerPath);
            if (Quadocopters.Any(q => q.IDNumber == quadocopter.IDNumber)) throw new ExistsInSystemException("skimmer Save to system", quadocopter.IDNumber);
            Quadocopters.Add(quadocopter);
            xml.XMLTools.SaveListToXMLSerializer(Quadocopters, SkimmerPath);
        }

        public void AddSkimmerLoading(SkimmerLoading SL)
        {
            throw new NotImplementedException();
        }

        public void AssignPackageSkimmer(int idp, int idq)
        {
            throw new NotImplementedException();
        }

        public void CollectionPackage(int idp)
        {
            throw new NotImplementedException();
        }

        public void DeleteBaseStation(int idb)
        {
            XElement rootBaseStationElemnt = xml.XMLTools.LoadListFromXmlElement(BaseStationPath);
            XElement baseStationElemnt = (from b in rootBaseStationElemnt.Elements()
                                          where Convert.ToInt32(b.Element("Id").Value) == idb
                                          select b).FirstOrDefault();
            if (baseStationElemnt == null)
                throw new ExistsInSystemException("BaseStation dont Save to system", idb);
            else
            {
                baseStationElemnt.Remove();
                xml.XMLTools.SaveListToXmlElement(rootBaseStationElemnt, BaseStationPath);
            }
        }

        public void DeleteClient(int IDc)
        {
            throw new NotImplementedException();
        }

        public void DeletePackage(int id)
        {
            var packages = xml.XMLTools.LoadListFromXMLSerializer<DO.Package>(PackagePath);
            if(!packages.Any(p=>p.ID==id)) throw new ExistsInSystemException("Package dont Save to system", id);
            Package package = packages.Find(p => p.ID == id);
            packages.Remove(package);
            xml.XMLTools.SaveListToXMLSerializer(packages, PackagePath);
        }

        public void DeleteSkimmer(int idq)
        {
            var skimmers = xml.XMLTools.LoadListFromXMLSerializer<DO.Quadocopter>(SkimmerPath);
            if (!skimmers.Any(s => s.IDNumber == idq)) throw new ExistsInSystemException("skimmer dont Save to system", idq);
            Quadocopter skimmer = skimmers.Find(s => s.IDNumber == idq);
            skimmers.Remove(skimmer);
            xml.XMLTools.SaveListToXMLSerializer(skimmers, SkimmerPath);
        }

        public void DeleteSkimmerLoading(int idsl)
        {
            throw new NotImplementedException();
        }

        public BaseStation GetBaseStation(int IDb)
        {
            XElement rootBaseStationElemnt = xml.XMLTools.LoadListFromXmlElement(BaseStationPath);
            XElement baseStationElemnt = (from b in rootBaseStationElemnt.Elements()
                                          where Convert.ToInt32(b.Element("Id").Value) == IDb
                                          select b).FirstOrDefault();
            if (baseStationElemnt == null)
                throw new ExistsInSystemException("id does not exist!!", IDb);
            else
            {
                BaseStation newBaseStation = new BaseStation()
                {
                    UniqueID = Convert.ToInt32(baseStationElemnt.Element("Id").Value),
                    StationName = baseStationElemnt.Element("Name").Value.ToString(),
                    Latitude = Convert.ToDouble(baseStationElemnt.Element("Latitude").Value),
                    Longitude = Convert.ToDouble(baseStationElemnt.Element("Longitude").Value),
                    SeveralPositionsArgument = Convert.ToInt32(baseStationElemnt.Element("SeveralPositionsArgument").Value)
                };
                      
                return newBaseStation;
            }
        }

        public IEnumerable<BaseStation> GetBaseStationList(Func<BaseStation, bool> predicate = null)
        {
            IEnumerable<BaseStation> baseStations = new List<BaseStation>();
            XElement rootBaseStationElemnt = xml.XMLTools.LoadListFromXmlElement(BaseStationPath);
            baseStations = (from b in rootBaseStationElemnt.Elements()
                            select new BaseStation()
                            {
                                UniqueID = Convert.ToInt32(b.Element("Id").Value),
                                StationName = b.Element("Name").Value.ToString(),
                                Latitude = Convert.ToDouble(b.Element("Latitude").Value),
                                Longitude = Convert.ToDouble(b.Element("Longitude").Value),
                                SeveralPositionsArgument = Convert.ToInt32(b.Element("SeveralPositionsArgument").Value)
                            });
            if (predicate == null) return baseStations;
            return baseStations.Where(predicate);
        }

        public Client GetClient(int IDc)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Client> GetClientList(Func<Client, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Package GetPackage(int idp)
        {
            var packages = xml.XMLTools.LoadListFromXMLSerializer<DO.Package>(PackagePath);
            if (!packages.Any(p => p.ID == idp)) throw new ExistsInSystemException("Package dont Save to system", idp);
            Package package = packages.Find(p => p.ID == idp);
            return package;
        }

        public IEnumerable<Package> GetPackageList(Func<Package, bool> predicate = null)
        {
            if (predicate == null) return xml.XMLTools.LoadListFromXMLSerializer<DO.Package>(PackagePath);
            return xml.XMLTools.LoadListFromXMLSerializer<DO.Package>(PackagePath).Where(predicate);
        }

        public IEnumerable<Quadocopter> GetQuadocopterList(Func<Quadocopter, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Quadocopter GetQuadrocopter(int IDq)
        {
            var skimmers = xml.XMLTools.LoadListFromXMLSerializer<DO.Quadocopter>(SkimmerPath);
            if (!skimmers.Any(s => s.IDNumber == IDq)) throw new ExistsInSystemException("Package dont Save to system", IDq);
            Quadocopter skimmer = skimmers.Find(s => s.IDNumber == IDq);
            return skimmer;
        }

        public IEnumerable<SkimmerLoading> GetSkimmerLoading()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SkimmerLoading> GetSkimmerLoadingList()
        {
            throw new NotImplementedException();
        }

        public void PackageDelivery(int idp)
        {
            throw new NotImplementedException();
        }

        public double[] PowerConsumptionRequest()
        {
            throw new NotImplementedException();
        }

        public void UpadteB(BaseStation baseStation)
        {
            XElement rootBaseStationElemnt = xml.XMLTools.LoadListFromXmlElement(BaseStationPath);
            XElement baseStationElemnt = (from b in rootBaseStationElemnt.Elements()
                                          where Convert.ToInt32(b.Element("Id").Value) == baseStation.UniqueID
                                          select b).FirstOrDefault();
            if (baseStationElemnt != null)
                throw new ExistsInSystemException("BaseStation Not Find", baseStation.UniqueID);
            else
            {
                baseStationElemnt.Remove();
                BaseStation newBaseStation = new BaseStation()
                {
                    UniqueID = baseStation.UniqueID,
                    StationName = baseStation.StationName,
                    Latitude = baseStation.Latitude,
                    Longitude = baseStation.Longitude,
                    SeveralPositionsArgument = baseStation.SeveralPositionsArgument
                };
                rootBaseStationElemnt.Add(newBaseStation);
                xml.XMLTools.SaveListToXmlElement(rootBaseStationElemnt, BaseStationPath);
            }
        }

        public void UpadteC(Client c)
        {
            throw new NotImplementedException();
        }

        public void UpadteP(Package package)
        {
            var packages = xml.XMLTools.LoadListFromXMLSerializer<DO.Package>(PackagePath);
            Package newpackage = packages.Find(p => p.ID == package.ID);
            packages.Remove(newpackage);
            newpackage = package;
            packages.Add(newpackage);
            xml.XMLTools.SaveListToXMLSerializer(packages, PackagePath);
        }

        public void UpadteQ(Quadocopter qc)
        {
            throw new NotImplementedException();
        }
    }
}
