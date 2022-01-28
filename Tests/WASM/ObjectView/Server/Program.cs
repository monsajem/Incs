using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Serialization;
namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string DB_Dir = Environment.CurrentDirectory + "\\DB";
                Directory.Delete(DB_Dir, true);
            }catch{}

            var Data = new Monsajem_Server.MyDataBase();
            Data.Groups.GetOrInserItem((c) => c.Name = "Root");

            new Monsajem_Incs.Net.Web.Server().StartServicing(8845, (c) =>
            {
                c.RunRecivedAction();
            });

            Console.ReadKey();
        }
    }
}