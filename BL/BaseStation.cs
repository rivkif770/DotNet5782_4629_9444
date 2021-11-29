using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStation
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public Location Location { get; set; }
        public int SeveralClaimPositionsVacant { get; set; }
        public List<SkimmerInCharging> ListOfSkimmersCharge { get; set; }
   
        public override string ToString()
        {
            String result = "";
            result += $"Unique ID number is: {Id} \n";
            result += $"The station name is: {Name} \n";
            result += $"The Location is:\n {Location}";
            result += $"Several Claim Positions Vacant is: {SeveralClaimPositionsVacant} \n";
            result += $"List Of Skimmers Charge is:"; 
            foreach (SkimmerInCharging Item in ListOfSkimmersCharge)
            {
                result +=$"{Item} ";
                result += Item;
            }
            return result;
        }
    }
}
