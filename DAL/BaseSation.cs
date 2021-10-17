﻿using System;
namespace IDAL
{
    namespace DO
    {
        public struct BaseStation
        {
            public int UniqueID { get; set; }
            public String StationName { get; set; }
            public int SeveralPositionsArgument { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public override string ToString()
            {
                String result = "";
                result += $"Unique ID number is: {UniqueID}, \n";
                result += $"The station name is: {StationName}, \n";
                result += $"Several positions of argument is: {SeveralPositionsArgument}, \n";
                result += $"Latitude is: {Latitude}, \n";
                result += $"Longitude is: {Longitude}, \n";
                return result;
            }
        }
    }
}