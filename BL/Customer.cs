using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        public List<PackageAtCustomer> SentParcels { get; set; }
        public List<PackageAtCustomer> ReceiveParcels { get; set; }

        public override string ToString()
        {
            String result = "";
            result += $"ID is: {Id}, \n";
            result += $"Name is: {Name}, \n";
            result += $"Telephone is: {Phone.Substring(0, 3)}-{Phone.Substring(3)}, \n";
            result += Location;
            result += $"Delivery list Packages at customer - from customer: ";
            for (int i = 0; i < SentParcels.Count(); i++)
                result += SentParcels[i];
            result += $"Delivery list of packages at the customer - to the customer: ";
            for (int i = 0; i < ReceiveParcels.Count(); i++)
                result += ReceiveParcels[i];
            return result;
        }
    }
}
