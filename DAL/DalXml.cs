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
        DalXml()
        {
            XElement a = new XElement("jojo");
            DalObject.DataSource.Initialize();
            xml.XMLTools.SetBaseStationListFile(DalObject.DataSource.ListBaseStation, BaseStationPath);
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

        public void AddPackage(Package p)
        {
            throw new NotImplementedException();
        }

        public void AddSkimmer(Quadocopter q)
        {
            throw new NotImplementedException();
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
                rootBaseStationElemnt.Remove(idb);
                xml.XMLTools.SaveListToXmlElement(rootBaseStationElemnt, BaseStationPath);
            }
        }

        public void DeleteClient(int IDc)
        {
            throw new NotImplementedException();
        }

        public void DeletePackage(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteSkimmer(int idq)
        {
            throw new NotImplementedException();
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
                return baseStationElemnt;
            }
        }

        public IEnumerable<BaseStation> GetBaseStationList(Func<BaseStation, bool> predicate = null)
        {
            List<BaseStation> baseStations = new List<BaseStation>();
            XElement rootBaseStationElemnt = xml.XMLTools.LoadListFromXmlElement(BaseStationPath);
            baseStations = (from b in rootBaseStationElemnt.Elements()
                            select new BaseStation()
                            {
                                UniqueID = Convert.ToInt32(b.Element("Id").Value),
                                StationName = b.Element("Name").Value.ToString(),
                                Latitude = Convert.ToDouble(b.Element("Latitude").Value),
                                Longitude = Convert.ToDouble(b.Element("Longitude").Value),
                                SeveralPositionsArgument = Convert.ToInt32(b.Element("SeveralPositionsArgument").Value)
                            }).ToList();
            return baseStations.FindAll(predicate);
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
            throw new NotImplementedException();
        }

        public IEnumerable<Package> GetPackageList(Func<Package, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Quadocopter> GetQuadocopterList(Func<Quadocopter, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Quadocopter GetQuadrocopter(int IDq)
        {
            throw new NotImplementedException();
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

        public void UpadteB(BaseStation b)
        {
            throw new NotImplementedException();
        }

        public void UpadteC(Client c)
        {
            throw new NotImplementedException();
        }

        public void UpadteP(Package p)
        {
            throw new NotImplementedException();
        }

        public void UpadteQ(Quadocopter qc)
        {
            throw new NotImplementedException();
        }
    }
}
