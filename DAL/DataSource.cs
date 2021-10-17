﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO; ;
namespace DalObject
{
    internal class DataSource
    {
        Quadocopter[] qu=new  Quadocopter[10];
        BaseStation[] ba = new BaseStation[5];
        Package[] pa = new Package[1000];
        Client[] cl = new Client[100];
        internal class Config
        {
            internal static int q = 0;
            internal static int b = 0;
            internal static int p = 0;
            internal static int c = 0;
        }
        
    }
}
