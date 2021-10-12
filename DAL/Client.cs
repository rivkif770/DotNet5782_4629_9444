using System;
namespace IDAL
{
    namespace DO
    {
        public struct Client
        {
            public int ID { get;  set; }
            public String Name { get; set; }
            public String Telephone { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public override string ToString()
            {
                String result = "";
                result += $"ID is {ID}, \n";
                result += $"Name is {Name}, \n";
                result += $"Telephone is {Telephone.Substring(0,3)+'-'+Telephone.Substring(3)}, \n";
                result += $"Latitude is {Latitude}, \n";
                result += $"Longitude is {Longitude}, \n";
                return result;
            }
        }
    }
}