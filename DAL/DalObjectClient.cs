using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    class DalObjectClient
    {
        public static void AddClient_private(Client c)//Adding a customer
        {
            if (DataSource.ListClient.Exists(item => item.ID == c.ID))//If finds an existing Customer throws an error.
            {
                throw new BaseStationException($"Customer {c.ID} Save to system", Severity.Mild);
            }
            DataSource.ListClient.Add(c);
        }
        public static Client GetClient_private(int IDc)//Client view by appropriate ID
        {
            if (!DataSource.ListClient.Exists(item => item.ID == IDc))
            {
                throw new ClientException($"id : {IDc} does not exist!!", Severity.Mild);
            }
            return DataSource.ListClient.FirstOrDefault(c => c.ID == IDc);
        }
        public static IEnumerable<Client> GetClientList_private()//Displays a list of Client
        {
            //return DataSource.ListClient.ToList();
            return DataSource.ListClient.Take(DataSource.ListClient.Count).ToList();
        }
    }
}
