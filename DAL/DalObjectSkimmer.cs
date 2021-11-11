using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObjectSkimmer
    {
        public static void AddSkimmer_privet(Quadocopter q)//added a skimmer
        {
            if (DataSource.ListQuadocopter.Exists(item => item.IDNumber == q.IDNumber))//If finds an existing Skimmer throws an error.
            {
                throw new BaseStationException($"Skimmer {q.IDNumber} Save to system", Severity.Mild);
            }
            DataSource.ListQuadocopter.Add(q);
        }
        public static Quadocopter GetQuadrocopter_privet(int IDq)//Quadrocopter view by appropriate ID
        {
            if (!DataSource.ListQuadocopter.Exists(item => item.IDNumber == IDq))
            {
                throw new QuadocopterException($"id : {IDq} does not exist!!", Severity.Mild);
            }
            return DataSource.ListQuadocopter.FirstOrDefault(q => q.IDNumber == IDq);
        }
        public static IEnumerable<Quadocopter> GetQuadocopterList_privet()//Displays a list of Skimmer
        {
            //return DataSource.ListQuadocopter.ToList();
            return DataSource.ListQuadocopter.Take(DataSource.ListQuadocopter.Count).ToList();
        }
    }
    
}
