using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerToList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int ParcelSentAndDelivered { get; set; }
        public int ParcelSentAndNotDelivered { get; set; }
        public int PackagesHeReceived { get; set; }
        public int PackagesOnTheWayToCustomer { get; set; }

        public override string ToString()
        {
            String result = "";
            result += $"ID is: {Id}, \n";
            result += $"Name is: {Name}, \n";
            result += $"Telephone is: {Phone.Substring(0, 3)}-{Phone.Substring(3)}, \n";
            result += $"Number of packages sent and delivered: {ParcelSentAndDelivered}, \n";
            result += $"Number of packages sent but not yet delivered: {ParcelSentAndNotDelivered}, \n";
            result += $"Number of packages he received: {PackagesHeReceived}, \n";
            result += $"Number of packages on the way to the customer: {PackagesOnTheWayToCustomer}, \n";
            return result;
        }
    }
}
