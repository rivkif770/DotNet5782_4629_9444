using DO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;

namespace xml
{
    class XMLTools
    {
        private static string dirPath = @"..\..\..\..\Data\";

        static XMLTools()
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
        }


        public static void SaveListToXmlElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(dirPath + filePath);
            }
            catch (Exception ex)
            {
                throw new DO.ExistsInSystemException.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        internal static void SetBaseStationListFile(IEnumerable<DO.BaseStation> listbasestation, string basestationpath)
        {
            XElement rootElementBaseStation;
            var v = from BaseStation in listbasestation
                    select new XElement("listbasestation",
                       new XElement("UniqueID", BaseStation.UniqueID),
                    new XElement("StationName", BaseStation.StationName),
                    new XElement("Latitude", BaseStation.Latitude),
                    new XElement("Longitude", BaseStation.Longitude),
                    new XElement("SeveralPositionsArgument", BaseStation.SeveralPositionsArgument)
                    );
            rootElementBaseStation = new XElement("listbasestation", v);
            rootElementBaseStation.Save(dirPath + basestationpath);
        }

        public static void Config(string configPath)
        {
            XElement rootId = new XElement("Config",
               new XElement("IDPackage", 1010),
               new XElement("Free", 0.001),
               new XElement("LightWeightCarrier", 0.003),
               new XElement("MediumWeightCarrier", 0.004),
               new XElement("HeavyWeightCarrier", 0.006),
               new XElement("SkimmerLoadingRate", 20)
               );
            rootId.Save(dirPath + configPath);
        }
        public static XElement LoadListFromXmlElement(string filePath)
        {
            try
            {
                if (File.Exists(dirPath + filePath))
                {
                    return XElement.Load(dirPath + filePath);
                }
                else
                {
                    XElement rootElem = new XElement(dirPath + filePath);
                    rootElem.Save(dirPath + filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new DO.ExistsInSystemException.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(dirPath + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.ExistsInSystemException.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }


        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(dirPath + filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(dirPath + filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                throw new DO.ExistsInSystemException.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
    }
}
