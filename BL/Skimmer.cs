﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Skimmer
    {
        public int Id { get; set; }
        public int SkimmerModel { get; set; }
        public Weight WeightCategory { get; set; }
        public int BatteryStatus { get; set; }
        public SkimmerStatuses SkimmerStatus { get; set; }
        public Parcel PackageInTransfer { get; set; }
        public Location Location { get; set; }   
    }
}
