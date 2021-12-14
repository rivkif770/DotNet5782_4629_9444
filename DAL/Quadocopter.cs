using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
    namespace DO
    {
        public struct Quadocopter
        {
            public int IDNumber { get; set; }
            public String SkimmerModel { get; set; }
            public WeightCategories Weight { get; set; }

            public override string ToString()
            {
                String result = "";
                result += $"Unique ID number: {IDNumber}, \n";
                result += $"The skimmer model: {SkimmerModel}, \n";
                result += $"Weight category: {Weight}, \n";
               return result;
            }
        }
    }