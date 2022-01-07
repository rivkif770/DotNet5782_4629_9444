using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal(string type)
        {
            switch(type)
            {
                case "List":
                    return DalObject.DalObject.Instance;
                case "xml":
                    return DalXml.Instance;
                default:
                    throw new DO.IdDoesNotExistException("Idal only have List/xml type", type);
            }
        }
    }
}
