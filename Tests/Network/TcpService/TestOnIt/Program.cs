using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Database.ArrayDb;
using Monsajem_Incs.Net.Tcp;
using Monsajem_Incs.Net.Base.Service;

namespace TestOnServer
{

    class Program
    {

        static void Main()
        {
            Console.Title = "Server";
            DB_Test.Test();
            //RemoteObjTest.Test();

        }       
    }
}
