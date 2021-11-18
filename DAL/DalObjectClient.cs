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
        public void AddClient(Client c)//Adding a customer
        {
            if (DataSource.ListClient.Exists(item => item.ID == c.ID))//If finds an existing Customer throws an error.
            {
                throw new ExistsInSystemException($"Customer {c.ID} Save to system", Severity.Mild);
            }
            DataSource.ListClient.Add(c);
        }
        public Client GetClient(int IDc)//Client view by appropriate ID
        {
            if (!DataSource.ListClient.Exists(item => item.ID == IDc))
            {
                throw new IdDoesNotExistException($"id : {IDc} does not exist!!", Severity.Mild);
            }
            return DataSource.ListClient.FirstOrDefault(c => c.ID == IDc);
        }
        public IEnumerable<Client> GetClientList()//Displays a list of Client
        {
            //return DataSource.ListClient.ToList();
            return DataSource.ListClient.Take(DataSource.ListClient.Count).ToList();
        }
        public void DeleteClient(Client c)//Adding a customer
        {
            if (!DataSource.ListClient.Exists(item => item.ID == c.ID))//If finds an existing Customer throws an error.
            {
                throw new IdDoesNotExistException($"Customer {c.ID} dont Save to system", Severity.Mild);
            }
            DataSource.ListClient.Remove(c);
        }
    }
}
