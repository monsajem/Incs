using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Database.DirectoryTable;
using System.IO;

namespace Monsajem_Server
{

    internal class MyDataBase : Monsajem_Client.DataBase
    {
        public MyDataBase() : base(new Maker(), DB_Dir + "\\Files\\") {}
        public override bool ISUpdateAble => true;
        public override uint Ver => 1;
        public override uint LastVer
        {
            get => 1;
            set{}
        }

        private static string DB_Dir = Environment.CurrentDirectory+"\\DB";

        public override void ClearData()
        {
            Directory.Delete(DB_Dir,true);
        }

        protected class Maker : MonsajemData.TableMaker
        {
            public override Monsajem_Incs.Database.Base.Table<ValueType, KeyType> MakeDB<ValueType, KeyType>(string Name, Func<ValueType, KeyType> GetKey)
            {
                return new Monsajem_Incs.Database.KeyValue.DirBased.Table<ValueType, KeyType>(DB_Dir+"\\"+Name, GetKey, true);
            }
        }
    }
}
