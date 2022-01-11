using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;

namespace BL
{
    internal class Simulator
    {
        public Simulator(BL bL, int id, Action act, Func<bool> func)
        {
        }
        public int StepTimer { get; set; }
        public int SkimmerSpeed { get; set; }  

    }
}
