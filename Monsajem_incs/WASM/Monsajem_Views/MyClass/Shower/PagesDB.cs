using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;
using static MonsajemData.DataBase;
using Monsajem_Incs.Convertors;
using static WASM_Global.Publisher;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using static Monsajem_Client.SafeRun;
using static Monsajem_Client.Network;
using MonsajemData;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace Monsajem_Incs.Views.Shower.Database
{
    internal class ShowPage : Page.HaveData
    {
        public override string Address => "DbShow";

        public void Show<ValueType,KeyType>(PartOfTable<ValueType,KeyType> Table, KeyType Key)
            where KeyType:IComparable<KeyType>
        {
            var TableName = (string) Table.HolderTable.Table.TableName;
            var RelationName = (string)Table.HolderTable.Table.TableName;
            base.Show(TableName + "?" + RelationName + "?" + Key.ConvertToString());
        }
        public void Show<ValueType, KeyType>(Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            var TableName = Table.TableName;
            base.Show(TableName);
        }

        public static (string TableName, string RelationName, string ItemKey)
            GetDataFrom(string Info)
        {
            string TableName = "";
            string RelationName = "";
            string ItemKey = null;
            var Pos = Info.IndexOf('?');
            if (Pos < 0)
                TableName = Info;
            else
            {
                TableName = Info.Substring(0, Pos);
                Info = Info.Substring(Pos + 1);
                Pos = Info.IndexOf('?');
                RelationName = Info.Substring(0, Pos);
                ItemKey = Uri.UnescapeDataString(Info.Substring(Pos + 1));
            }
            return (TableName, RelationName, ItemKey);
        }

        protected async override Task Ready()
        {
            var Data = ShowPage.GetDataFrom(GetDataString());
            var TableInfo = TableFinder.FindTable(Data.TableName);
            if (Data.RelationName=="")
            {
                MainElement.ReplaceChilds(TableInfo.MakeShowViewForItems(
                    OnUpdate:(c)=>
                    {
                        new UpdatePage().Show(c.TableInfo.TableName, c.Key);
                    },
                    OnDelete:(c)=>
                    {
                        c.TableInfo.Delete(c.Key);
                        this.Ready();
                    }));
            }
            else
            {
                var RelationInfo = TableInfo.FindRelation(Data.RelationName);
                MainElement.ReplaceChilds(RelationInfo.MakeShowViewForItems(Data.ItemKey,
                    OnUpdate: (c) =>
                    {
                        new UpdatePage().Show(c.TableInfo.TableName, c.Key);
                    },
                    OnDelete: (c) =>
                    {
                        c.TableInfo.Delete(c.Key);
                        this.Ready();
                    }));
            }
        }
    }

    internal class insertPage : Page.HaveData
    {
        public void Show<ValueType, KeyType>(PartOfTable<ValueType, KeyType> Table, KeyType Key)
            where KeyType : IComparable<KeyType>
        {
            var TableName = (string)Table.HolderTable.Table.TableName;
            var RelationName = (string)Table.HolderTable.Table.TableName;
            base.Show(TableName + "?" + RelationName + "?" + Key.ConvertToString());
        }
        public void Show<ValueType, KeyType>(Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            var TableName = Table.TableName;
            base.Show(TableName);
        }

        public override string Address => "DbInsert";

        protected async override Task Ready()
        {
            var Data = ShowPage.GetDataFrom(GetDataString());
            var TableInfo = TableFinder.FindTable(Data.TableName);
            if (Data.RelationName == "")
            {
                MainElement.ReplaceChilds(TableInfo.MakeInsertView(()=>js.GoBack()));
            }
            else
            {
                var RelationInfo = TableInfo.FindRelation(Data.RelationName);
                MainElement.ReplaceChilds(RelationInfo.MakeInsertView(
                                Data.ItemKey,
                                ()=>js.GoBack()));
            }
        }
    }
    internal class UpdatePage : Page.HaveData
    {
        public void Show<KeyType, ValueType>(Table<ValueType, KeyType> DB, KeyType Key)
            where KeyType : IComparable<KeyType>
        {
            this.Show(DB.TableName,Key.ConvertToString());
        }
        internal void Show(string TableName, object Key)
        {
            base.Show(TableName + "," + Key.ConvertToString());
        }

        public override string Address => "DbUpdate";

        protected async override Task Ready()
        {
            var Data = GetDataString();
            var SpratorPos = Data.IndexOf(",");
            var TableName = Data.Substring(0, SpratorPos);
            var Key = Uri.UnescapeDataString(Data.Substring(SpratorPos + 1));
            var TableInfo = TableFinder.FindTable(TableName);
            MainElement.ReplaceChilds(TableInfo.MakeEditView(Key,
                ()=> js.GoBack()));
        }
    }
}