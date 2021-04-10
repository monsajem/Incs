using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Database.ArrayDb;
using Monsajem_Incs.Net.Tcp;
using Monsajem_Incs.Net.Base.Service;

namespace TestOnClient
{
    class Program
    {

        static void Main()
        {
            Console.Title = "Client";
            DB_Test.Test();
        }
    }
}
