using Monsajem_Client;
using System;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var Mem_reg = new Monsajem_Incs.Database.Register.MemoryRegister<object>();
            var Data = new DataBase(Mem_reg);

            Data.Persons.Insert(new Monsajem_Client.Person()
            {
                Name = "a"
            });

            Data.Persons["a"].Value.Transactions.Insert(new Transaction() { Code = 1 });
            Data.Persons["a"].Value.Transactions.Update(0, (c) =>
            {
                c.Value = 10000;
            });
        }
    }
}
