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
using static Monsajem_Incs.WasmClient.SafeRun;
using static Monsajem_Incs.WasmClient.Network;
using MonsajemData;
using WebAssembly.Browser.MonsajemDomHelpers;
using static Monsajem_Incs.Collection.Array.Extentions;

namespace Monsajem_Incs.Views.Shower.Database
{
    internal class ShowPage : Page.HaveData
    {
        public override string Address => "DbShow";

        private string[] ProvideUriParams<ValueType, KeyType>(
            Table<ValueType, KeyType> Table,
            string Query = null)
            where KeyType : IComparable<KeyType>
        {
            var PartTable = Table as PartOfTable<ValueType, KeyType>;
            string[] Parameters;
            if (PartTable != null)
            {
                var TableName = (string)PartTable.HolderTable.Table.TableName;
                var RelationName = (string)PartTable.TableName;
                Parameters = new string[]{TableName,
                                    RelationName,
                                    PartTable.HolderTable.Key.ConvertToString() };
            }
            else
            {
                var TableName = Table.TableName;
                Parameters = new string[] { TableName };
            }

            if (Query != null)
                Insert(ref Parameters, Query);
            return Parameters;
        }

        public string ProvideFullUri<ValueType, KeyType>(Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            return ProvideUri(ProvideUriParams(Table));
        }

        public void Show<ValueType, KeyType>(Table<ValueType, KeyType> Table,
                                             string Query = null)
            where KeyType : IComparable<KeyType>
        {
            base.Show(ProvideUriParams(Table,Query));
        }

        public static (string TableName, string RelationName, string ItemKey, string Query)
            GetDataFrom(string[] Info)
        {
            string TableName = Info[0];
            string RelationName = "";
            string ItemKey = null;
            string Query = null;

            if (Info.Length == 2)
                Query = Info[1];
            else if (Info.Length == 4)
                Query = Info[3];

            if(Info.Length>=3)
            {
                RelationName = Info[1];
                ItemKey = Info[2];
            }

            return (TableName, RelationName, ItemKey, Query);
        }

        protected async override Task Ready()
        {
            var Data = ShowPage.GetDataFrom(GetDataStringParameters());
            var TableInfo = TableFinder.FindTable(Data.TableName);
            if (Data.RelationName=="")
            {
                MainElement.ReplaceChilds(TableInfo.MakeShowViewForItems(
                    Data.Query,
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
                MainElement.ReplaceChilds(RelationInfo.MakeShowViewForItems(
                    Data.ItemKey,
                    Data.Query,
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
        public void Show()
        {
            var Data = ShowPage.GetDataFrom(GetDataStringParameters());
            if (Data.RelationName != "")
                base.Show(Data.TableName, 
                          Data.RelationName,
                          Data.ItemKey);
            else
                base.Show(Data.TableName);
        }

        public void Show<ValueType, KeyType>(Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            var PartTable = Table as PartOfTable<ValueType, KeyType>;
            if(PartTable != null)
            {
                var TableName = (string)PartTable.HolderTable.Table.TableName;
                var RelationName = PartTable.TableName;
                base.Show(TableName,
                          RelationName, 
                          PartTable.HolderTable.Key.ConvertToString());
            }
            else
            {
                var TableName = Table.TableName;
                base.Show(TableName);
            }
        }

        public override string Address => "DbInsert";

        protected async override Task Ready()
        {
            var Data = ShowPage.GetDataFrom(GetDataStringParameters());
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
            base.Show(TableName,Key.ConvertToString());
        }

        public override string Address => "DbUpdate";

        protected async override Task Ready()
        {
            var Data = GetDataString();
            var SpratorPos = Data.IndexOf(DataUrlSperator);
            var TableName = Data.Substring(0, SpratorPos);
            var Key = Uri.UnescapeDataString(Data.Substring(SpratorPos + 1));
            var TableInfo = TableFinder.FindTable(TableName);
            MainElement.ReplaceChilds(TableInfo.MakeEditView(Key,
                ()=> js.GoBack()));
        }
    }
}