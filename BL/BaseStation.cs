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
        public Location location { get; set; }
        public int SeveralClaimPositionsVacant { get; set; }
        public List<SkimmerInCharging> ListOfSkimmersCharge { get; set; }
    }
}
