using System;
namespace IDAL
{
    namespace DO
    {
        public struct Package
        {
            public int ID { get; set; }
            public int IDSender { get; set; }
            public int IDgets { get; set; }
            public string Weight { get; set; }
            public string priority { get; set; }
            public string IDSkimmerOperation { get; set; }
            public string PackageCreationTime { get; set; }
            public string TimeAssignGlider { get; set; }
            public string PackageCollectionTime { get; set; }
            public string TimeArrivalRecipient { get; set; }
            public override string ToString()
            {
                String result = "";
                result += $"ID is: {ID}, \n";
                result += $"Sending customer ID is: {IDSender}, \n";
                result += $"Receiving customer ID is: {IDgets}, \n";
                result += $"Weight category is: {Weight}, \n";
                result += $"Priority is: {priority}, \n";
                result += $"ID skimmer operation is: {IDSkimmerOperation}, \n";
                result += $"Package creation time is: {IDSkimmerOperation.Substring(0, 2) + ':' + IDSkimmerOperation.Substring(2)}, \n";
                result += $"Time to assign the package to the glider is: {TimeAssignGlider.Substring(0, 2) + ':' + TimeAssignGlider.Substring(2)}, \n";
                result += $"Package collection time from the sender is: {PackageCollectionTime.Substring(0, 2) + ':' + PackageCollectionTime.Substring(2)}, \n";
                result += $"Time of arrival of the package to the recipient is: {TimeArrivalRecipient.Substring(0, 2) + ':' + TimeArrivalRecipient.Substring(2)}, \n";
                return result;
            }
        }
    }
}
