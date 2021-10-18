using System;
namespace IDAL
{
    namespace DO
    {
        public struct Quadocopter
        {
            public int IDNumber { get; set; }
            public String SkimmerModel { get; set; }
            public WeightCategories Weight { get; set; }
            public DronStatuses Battery { get; set; }
            public Double SkimmerMode { get; set; }

            public override string ToString()
            {
                String result = "";
                result += $"Unique ID number: {IDNumber}, \n";
                result += $"The skimmer model: {SkimmerModel}, \n";
                result += $"Weight category: {Weight}, \n";
                result += $"Battery status: {Battery} + '%', \n";
                result += $"Skimmer mode: {SkimmerMode}, \n";
                return result;
            }
        }
    }
}