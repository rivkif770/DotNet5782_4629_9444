using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class PackageInTransfer
    {
        public int Id { get; set; }
        public ParcelStatus PackageMode { get; set; }
        public Priority priority { get; set; }
        public Weight WeightCategory { get; set; }
        public CustomerInParcel CustomerSends { get; set; }
        public CustomerInParcel CustomerReceives { get; set; }
        public Location CollectionLocation { get; set; }
        public Location DeliveryDestinationLocation { get; set; }
        public double TransportDistance { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"Unique ID number is: {Id}, \n";
            result += $"Package mode is: {PackageMode}, \n";
            result += $"Priority is: {priority}, \n";
            result += $"Weight category is: {WeightCategory}, \n";
            result += $"Customer Sends is: {CustomerSends}, \n";
            result += $"Customer Receives is: {CustomerReceives}, \n";
            result += $"Collection Location is: {CollectionLocation}, \n";
            result += $"Delivery Destination Location is: {DeliveryDestinationLocation}, \n";
            result += $"Transport Distance is: {TransportDistance}, \n";
            return result;
        }
    }
}
