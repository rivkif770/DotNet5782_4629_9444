using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public partial class DalObject: IDal
    {
        public void UpadteQ(Quadocopter qc)
        {
            for (int i = 0; i < DataSource.ListQuadocopter.Count; i++)
            {
                if (DataSource.ListQuadocopter[i].IDNumber== qc.IDNumber)
                {
                    DataSource.ListQuadocopter[i] = qc;
                    break;
                }
            }
        }
        public void AddSkimmer(Quadocopter q)//added a skimmer
        {
            if (DataSource.ListQuadocopter.Exists(item => item.IDNumber == q.IDNumber))//If finds an existing Skimmer throws an error.
            {
                throw new ExistsInSystemException($"Skimmer {q.IDNumber} Save to system", Severity.Mild);
            }
            DataSource.ListQuadocopter.Add(q);
        }
        public void DeleteSkimmer(int idq)//added a skimmer
        {
            if (!DataSource.ListQuadocopter.Exists(item => item.IDNumber == idq))//If finds an existing Skimmer throws an error.
            {
                throw new IdDoesNotExistException($"Skimmer {idq} dont Save to system", Severity.Mild);
            }
            DataSource.ListQuadocopter.RemoveAll(item => item.IDNumber == idq);
        }
        public Quadocopter GetQuadrocopter(int IDq)//Quadrocopter view by appropriate ID
        {
            if (!DataSource.ListQuadocopter.Exists(item => item.IDNumber == IDq))
            {
                throw new IdDoesNotExistException($"id : {IDq} does not exist!!", Severity.Mild);
            }
            return DataSource.ListQuadocopter.FirstOrDefault(q => q.IDNumber == IDq);
        }
        public IEnumerable<Quadocopter> GetQuadocopterList()//Displays a list of Skimmer
        {
            return DataSource.ListQuadocopter.Take(DataSource.ListQuadocopter.Count).ToList();
        }
        
        
        public void AddSkimmerLoading(SkimmerLoading SL)//added a skimmer
        {
            if (DataSource.ListSkimmerLoading.Exists(item => item.SkimmerID == SL.SkimmerID))//If finds an existing Skimmer throws an error.
            {
                throw new ExistsInSystemException($"Skimmer {SL.SkimmerID} Save to system of SkimmerLoading", Severity.Mild);
            }
            DataSource.ListSkimmerLoading.Add(SL);
        }
        public void DeleteSkimmerLoading(int idsl)//added a skimmer
        {
            if (!DataSource.ListSkimmerLoading.Exists(item => item.SkimmerID == idsl))//If finds an existing Skimmer throws an error.
            {
                throw new IdDoesNotExistException($"Skimmer {idsl} dont Save to system of SkimmerLoading", Severity.Mild);
            }
            DataSource.ListSkimmerLoading.RemoveAll(item => item.SkimmerID == idsl);
        }
        public IEnumerable<SkimmerLoading> GetSkimmerLoading()//Displays a list of Skimmer
        {
            return DataSource.ListSkimmerLoading.Take(DataSource.ListSkimmerLoading.Count).ToList();
            //return (IEnumerable<Quadocopter>)DataSource.ListSkimmerLoading.Take(DataSource.ListSkimmerLoading.Count).ToList();
        }
        public IEnumerable<SkimmerLoading> GetSkimmerLoadingList()//Displays a list of SkimmerLoading
        {
            return DataSource.ListSkimmerLoading.Take(DataSource.ListSkimmerLoading.Count).ToList();
        }
    }
}
