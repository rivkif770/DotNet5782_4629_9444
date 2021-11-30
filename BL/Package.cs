using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Package
    {
        public int Id { get; set; }
        public CustomerInParcel SendPackage { get; set; }
        public CustomerInParcel ReceivesPackage { get; set; }
        public Weight WeightCategory { get; set; }
        public Priority priority { get; set; }
        public SkimmerInPackage SkimmerInPackage { get; set; }
        public DateTime? PackageCreationTime { get; set; }
        public DateTime? AssignmentTime { get; set; }
        public DateTime? CollectionTime { get; set; }
        public DateTime? SupplyTime { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID is: {Id} \n";
            result += $"Sending customer {SendPackage} \n";
            result += $"Receiving customer {ReceivesPackage} \n";
            result += $"Weight category is: {WeightCategory} \n";
            result += $"Priority is: {priority} \n";
            result += $"Skimmer In Package is: \n{SkimmerInPackage} \n";
            result += $"Package creation time is: {PackageCreationTime} \n";
            result += $"Time to assign the package to the glider is: {AssignmentTime} \n";
            result += $"Package collection time from the sender is: {CollectionTime} \n";
            result += $"Time of arrival of the package to the recipient is: {SupplyTime} \n";
            return result;
        }
    }
}
