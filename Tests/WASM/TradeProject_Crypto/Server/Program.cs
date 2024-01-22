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

            typeof(Monsajem_Client.App).ToString();

            var DB_Dir = new DirectoryInfo(Environment.CurrentDirectory).FullName + "\\DB";

            //System.IO.File.Delete(DB_Dir);



            var Register = new Monsajem_Incs.Database.Register.MemoryRegister<object>();
            Register.Value = File.ReadAllBytes(DB_Dir).Deserialize<object>();

            var DB = new Monsajem_Client.DataBase(Register);

            var CG1 = DB.Groups["زعفران"].Value;

            DB.Groups.Update("زعفران",(c)=>c.Name="زعفرانی");

            var CG2 = DB.Groups["زعفران بسته بندی با ظرف"].Value;

            DB.Groups.Delete("زعفران بسته بندی با ظرف");

            DB.Groups[0].Value.ProductChilds.Accept("aaa");

            Register.Save();

            new Monsajem_Incs.Net.Web.Server().StartServicing(8845, (c) =>
            {
                
            });

            Console.ReadKey();
        }
    }
}