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
        public struct Package
        {
            public int ID { get; set; }
            public int IDSender { get; set; }
            public int IDgets { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities priority { get; set; }
            public int IDSkimmerOperation { get; set; }
            public DateTime PackageCreationTime { get; set; }
            public DateTime TimeAssignGlider { get; set; }
            public DateTime PackageCollectionTime { get; set; }
            public DateTime TimeArrivalRecipient { get; set; }
            public override string ToString()
            {
                String result = "";
                result += $"ID is: {ID}, \n";
                result += $"Sending customer ID is: {IDSender}, \n";
                result += $"Receiving customer ID is: {IDgets}, \n";
                result += $"Weight category is: {Weight}, \n";
                result += $"Priority is: {priority}, \n";
                result += $"ID skimmer operation is: {IDSkimmerOperation}, \n";
                result += $"Package creation time is: {PackageCreationTime}, \n";
                result += $"Time to assign the package to the glider is: {TimeAssignGlider}, \n";
                result += $"Package collection time from the sender is: {PackageCollectionTime}, \n";
                result += $"Time of arrival of the package to the recipient is: {TimeArrivalRecipient}, \n";
                return result;
            }
        }
    }
}
