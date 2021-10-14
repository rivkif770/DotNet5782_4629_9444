using System;
namespace IDAL
{
    namespace DO
    {
        public struct SkimmerLoading
        {
            public int StationID { get; set; }
            public int SkimmerID { get; set; }

            public override string ToString()
            {
                String result = "";
                result += $"Base station ID is: {StationID}, \n";
                result += $"Skimmer ID is: {SkimmerID}, \n";
                return result;
            }
        }
    }
}