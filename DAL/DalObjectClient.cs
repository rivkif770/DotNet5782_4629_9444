using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace DalObject
{
    public partial class DalObject: DalApi.IDal
    {
        public void UpadteC(Client c)
        {
            for (int i = 0; i < DataSource.ListClient.Count; i++)
            {
                if (DataSource.ListClient[i].ID == c.ID)
                {
                    DataSource.ListClient[i] = c;
                    break;
                }
            }
        }
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
            return DataSource.ListClient.Find(c => c.ID == IDc);
        }
        public IEnumerable<Client> GetClientList(Func<Client, bool> predicate = null)//Displays a list of Client
        {
            if (predicate == null)
                return DataSource.ListClient.Take(DataSource.ListClient.Count).ToList();
            return DataSource.ListClient.Where(predicate).ToList();
        }
        
        public void DeleteClient(int IDc)//Adding a customer
        {
            if (!DataSource.ListClient.Exists(item => item.ID == IDc))//If finds an existing Customer throws an error.
            {
                throw new IdDoesNotExistException($"Customer {IDc} dont Save to system", Severity.Mild);
            }
            DataSource.ListClient.RemoveAll(item => item.ID == IDc);
        }
    }
}
