﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerInParcel
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public override string ToString()
        {
            String result = "";
            result += $"ID is: {Id} \n";
            result += $"Name is: {Name} \n";
            return result;
        }
    }
}
