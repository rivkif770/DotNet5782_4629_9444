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
    }
}
