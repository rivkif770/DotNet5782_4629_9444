using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStationToList
    {
        public int Id { get; set; }
        public string StationName { get; set; }
        public int FreeChargingstations { get; set; }
        public int CatchChargingstations { get; set; }
    }
}
