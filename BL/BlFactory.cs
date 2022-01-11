using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BlApi
{
    public static class BlFactory
    {
        public static IBL GetBL()
        {
            return BL.BL.Instance;
        }
    }
}
