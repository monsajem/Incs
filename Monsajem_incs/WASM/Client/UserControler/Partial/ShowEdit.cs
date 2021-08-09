using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.Views.Extentions.Table;
using static MonsajemData.DataBase;
using static MonsajemData.DataBaseInfo;
using static WASM_Global.Publisher;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using static Monsajem_Client.SafeRun;
using static Monsajem_Client.Network;
using MonsajemData;

namespace UserControler.Partial
{
    public static class Page<ValueType, KeyType>
        where KeyType : IComparable<KeyType>
    {
        public static Array<(string RelationName, Caption Caption)>
            Relations = new Array<(string RelationName, Caption Caption)>(4);
        public class Actions
        {
            public static Table<ValueType, KeyType> GetTable(
                string TableName)
            {
                return (Table<ValueType, KeyType>)FindDB((TableName, "")).Tbl;
            }
            private static PartOfTable<ValueType, KeyType> GetTable(
                (string TableName, string RelationName, object ItemName) Info)
            {
                return ((PartOfTable<ValueType, KeyType>)FindDB((Info.TableName, Info.RelationName)).
                    GetTblOfRelation(Info.ItemName));
            }
            public static Table<ValueType, KeyType> GetTable(
                DataBaseInfo Info)
            {
                if (Info.RelationName == "")
                    return (Table<ValueType, KeyType>)Info.Tbl;
                else
                    return (Table<ValueType, KeyType>)Info.GetTblOfRelation(Info.Key);
            }
            private static async Task Delete(
                string TableName,
                KeyType Key)
            {
                await Remote(() => GetTable(TableName).Delete(Key));
            }
            private static async Task Delete(
                (string TableName, string RelationName, object ItemName) Info,
                KeyType Key)
            {
                await Remote(() => GetTable(Info).Delete(Key));
            }
            public static async Task Delete(
                 DataBaseInfo Info,
                 KeyType Key)
            {
                if (Info.RelationName == "")
                    await Delete(Info.TableName, Key);
                else
                    await Delete((Info.TableName, Info.RelationName, Info.Key), Key);
            }
            private static async Task Insert(
                 (string TableName, string RelationName, object ItemName) Info,
                 ValueType Value)
            {
                await Remote(() => 
                    GetTable(Info).Insert(Value));
            }
            private static async Task Insert(
                 string TableName,
                 ValueType Value)
            {
                await Remote(() => GetTable(TableName).Insert(Value));
            }
            public static async Task Insert(
                 DataBaseInfo Info,
                 ValueType Value)
            {
                if (Info.RelationName == "")
                    await Insert(Info.TableName, Value);
                else
                    await Insert((Info.TableName, Info.RelationName, Info.Key), Value);
            }
            public static async Task Update(
                 string TableName,
                 KeyType Key,
                 ValueType Value)
            {
                await Remote(() =>
                {
                    var TBL = GetTable(TableName);
                    TBL.Update(Key,
                        (c) =>
                        {
                            TBL.MoveRelations(c, Value);
                            return Value;
                        });
                });
            }
            public static async Task Accept(
                (string TableName, string RelationName, object ItemName) Info,
                KeyType[] Key)
            {
                await Remote(() =>
                {
                    var TBL = GetTable(Info);
                    TBL.Accept(Key);
                });
            }
            public static async Task Ignore(
                (string TableName, string RelationName, object ItemName) Info,
                KeyType[] Key)
            {
                await Remote(() =>
                {
                    var TBL = GetTable(Info);
                    TBL.Ignore(Key);
                });
            }
            public static async Task SyncData(DataBaseInfo Info)
            {
                if (Info.RelationName == "")
                    await SyncData(Info.TableName);
                else
                    await SyncData((Info.TableName, Info.RelationName, Info.Key));
            }
            private static async Task SyncData((string TableName, string RelationName, object ItemKey) Info)
            {
                await Remote((rs) =>
                {
                    rs.SendUpdate(GetTable(Info));
                }, async (rq) =>
                {
                    await rq.GetUpdate(GetTable(Info));
                });
            }
            public static async Task SyncData(string TableName)
            {
                Console.WriteLine(typeof(ValueType).Name);
                Console.WriteLine(TableName);
                await Remote((rs) =>
                {
                    rs.SendUpdate(GetTable(TableName));
                }, async (rq) =>
                {
                    await rq.GetUpdate(GetTable(TableName));
                });
            }
        }

        public class ShowPage
        {
            public DataBaseInfo DBInfo;
            protected virtual void OnViewMade((KeyType Key, HTMLElement View) c)
            {
                c.View.OnClick += (e1, e2) =>
                {
                    var Options = Document.document.CreateElement<HTMLDivElement>();
                    {
                        var Btn = Document.document.CreateElement<HTMLButtonElement>();
                        Btn.InnerText = "حذف";
                        Btn.OnClick += async (c1, c2) =>
                            await Safe(async () =>
                            {
                                new Window().History.Back();
                                await Actions.Delete(DBInfo, c.Key);
                                Publish.HideAction();
                                Publish.ShowSuccessMessage("با موفقیت حذف شد");
                            });
                        Options.AppendChild(Btn);
                    }
                    {
                        var Btn = Document.document.CreateElement<HTMLButtonElement>();
                        Btn.InnerText = "ویرایش";
                        Btn.OnClick += async (c1, c2) =>
                        {
                            new Window().History.Back();
                            object Key = c.Key;
                            new Partial.UpdatePage().Show(DBInfo.ParentTableName, (string)Key);
                        };
                        Options.AppendChild(Btn);
                    }
                    foreach (var RLN in Relations)
                    {
                        {
                            var Btn = Document.document.CreateElement<HTMLButtonElement>();
                            Btn.InnerText = "مشاهده " + RLN.Caption.Name_Multy;
                            Btn.OnClick += (c1, c2) =>
                            {
                                new Window().History.Back();
                                new Partial.ShowPage().Show(DBInfo.ParentTableName, RLN.RelationName, c.Key);
                            };
                            Options.AppendChild(Btn);
                        }
                    }
                    Monsajem_Incs.Views.Menu.Show(Options);
                };
            }

            protected virtual void OnLoad(HTMLDivElement ReadyElement)
            {
                {
                    var TitleView = Document.document.CreateElement<HTMLDivElement>();
                    TitleView.Dir = "rtl";
                    if(DBInfo.RelationCaption!=null)
                        TitleView.InnerText = 
                            DBInfo.Caption.Name_Multy_Of + " " +
                            DBInfo.RelationCaption.Name_Single + " " + DBInfo.Key;
                    else
                        TitleView.InnerText = DBInfo.Caption.Name_Multy;
                    ReadyElement.AppendChild(TitleView);
                    {
                        var Btn = Document.document.CreateElement<HTMLButtonElement>();
                        Btn.InnerText = "ایجاد "+ DBInfo.Caption.Name_Single + " جدید";
                        Btn.OnClick += (c1, c2) =>
                        {
                            new Partial.insertPage().Show(DBInfo);
                        };
                        ReadyElement.AppendChild(Btn);
                    }
                    if(DBInfo.RelationName!="")
                    {

                        {
                            var Btn = Document.document.CreateElement<HTMLButtonElement>();
                            Btn.InnerText = "افزودن تعدادی از "+ DBInfo.Caption.Name_Multy;
                            Btn.OnClick += (c1, c2) =>
                            {
                                new Partial.SelectForAccept().Show(DBInfo.TableName, DBInfo.RelationName, DBInfo.Key);
                            };
                            ReadyElement.AppendChild(Btn);
                        }
                        {
                            var Btn = Document.document.CreateElement<HTMLButtonElement>();
                            Btn.InnerText = "برداشتن تعدادی از  " + DBInfo.Caption.Name_Multy;
                            Btn.OnClick += (c1, c2) =>
                            {
                                new Partial.SelectForIgnore().Show(DBInfo.TableName, DBInfo.RelationName, DBInfo.Key);
                            };
                            ReadyElement.AppendChild(Btn);
                        }
                    }
                }
            }

            protected virtual IEnumerable<Table<ValueType, KeyType>.ValueInfo> GetValues()
            {
                return Actions.GetTable(DBInfo);
            }

            public async virtual Task SyncData()
            {
                await Actions.SyncData(DBInfo);
            }

            public async Task<HTMLDivElement> Ready()
            {
                await SyncData();
                var ReadyElement = Document.document.CreateElement<HTMLDivElement>();
                OnLoad(ReadyElement);
                ReadyElement.AppendChild(GetValues().MakeView((c) =>OnViewMade(c)));
                return ReadyElement;
            }
        }

        public class insertPage
        {
            public DataBaseInfo DBInfo;
            public async Task<HTMLDivElement> Ready()
            {
                var ReadyElement = Document.document.CreateElement<HTMLDivElement>();
                ReadyElement.ReplaceChilds(Actions.GetTable(DBInfo).MakeInsertView(async (c) =>
                await Safe(async () =>
                {
                    await Actions.Insert(DBInfo, c);
                    Publish.ShowSuccessMessage("ثبت شد");
                    Publish.HideAction();
                    new Window().History.Back();
                })));
                return ReadyElement;
            }
        }

        public class UpdatePage
        {
            public DataBaseInfo DBInfo;
            public async Task<HTMLDivElement> Ready(object Key)
            {
                var ReadyElement = Document.document.CreateElement<HTMLDivElement>();
                ReadyElement.ReplaceChilds(Actions.GetTable(DBInfo).GetItem((KeyType)Key).MakeEditView(async (c) =>
                await Safe(async () =>
                {
                    await Actions.Update(DBInfo.TableName, c.OldKey, c.NewValue);
                    Publish.ShowSuccessMessage("تغییرات ثبت شد");
                    Publish.HideAction();
                    new Window().History.Back();
                })));
                return ReadyElement;
            }
        }

        public class SelectPageForAdd:ShowPage
        {
            private Array<KeyType> Keys = new Array<KeyType>(10);
            public override Task SyncData()
            {
                return Actions.SyncData(DBInfo.ParentTableName);
            }
            protected override void OnViewMade((KeyType Key, HTMLElement View) c)
            {
                var CheckBox = Document.document.CreateElement<HTMLInputElement>();
                CheckBox.Type = InputElementType.CheckBox;
                CheckBox.OnChange += (e1, e2) =>
                {
                    if (CheckBox.Checked)
                        Keys.BinaryInsert(c.Key);
                    else
                        Keys.BinaryDelete(c.Key);
                };
                c.View.AppendChild(CheckBox);
            }
            protected override void OnLoad(HTMLDivElement ReadyElement)
            {
                {
                    var Btn = Document.document.CreateElement<HTMLButtonElement>();
                    Btn.InnerText = "افزودن";
                    Btn.OnClick += async (c1, c2) =>
                    {
                        await Actions.Accept((DBInfo.TableName, DBInfo.RelationName, DBInfo.Key), Keys);
                        new Window().History.Back();
                    };
                    ReadyElement.AppendChild(Btn);
                }
            }
            protected override IEnumerable<Table<ValueType, KeyType>.ValueInfo> GetValues()
            {
                return Actions.GetTable(DBInfo.ParentTableName).GetElseItems(
                        Actions.GetTable(DBInfo));
            }
        }
        public class SelectPageForDrop : ShowPage
        {
            private Array<KeyType> Keys = new Array<KeyType>(10);
            protected override void OnViewMade((KeyType Key, HTMLElement View) c)
            {
                var CheckBox = Document.document.CreateElement<HTMLInputElement>();
                CheckBox.Type = InputElementType.CheckBox;
                CheckBox.OnChange += (e1, e2) =>
                {
                    if (CheckBox.Checked)
                        Keys.BinaryInsert(c.Key);
                    else
                        Keys.BinaryDelete(c.Key);
                };
                c.View.AppendChild(CheckBox);
            }
            protected override void OnLoad(HTMLDivElement ReadyElement)
            {
                {
                    var Btn = Document.document.CreateElement<HTMLButtonElement>();
                    Btn.InnerText = "برداشتن";
                    Btn.OnClick += async (c1, c2) =>
                    {
                        await Actions.Ignore(
                            (DBInfo.TableName, DBInfo.RelationName, DBInfo.Key), Keys);
                        new Window().History.Back();
                    };
                    ReadyElement.AppendChild(Btn);
                }
            }
            protected override IEnumerable<Table<ValueType, KeyType>.ValueInfo> GetValues()
            {
                return Actions.GetTable(DBInfo);
            }
        }
    }

    public class UserPermitionsShow : Page.HaveData
    {
        public override string Address => "UserPermitionsShow";

        public void Show(string UserName)
        {
           
        }

        protected async override Task Ready()
        {
            await DataBaseInfo.UpdatePermitions();
            var UserID = GetDataString();

            var Permitions = DataBaseInfo.GetPermitions(UserID);
            MainElement.InnerHtml="";
            foreach (var Permition in Permitions)
            {
                var View = new Monsajem_Incs.Resources.Base.Permitions_html();
                View.Accept.Checked = Permition.Accept;
                View.Delete.Checked = Permition.Delete;
                View.Edit.Checked = Permition.Update;
                View.Ignore.Checked = Permition.Ignore;
                View.MakeNew.Checked = Permition.Insert;
                View.Title.TextContent = Permition.TableName + Permition.RelationName;
                MainElement.AppendChild(View.Main);
            }
        }
    }

    public class ShowPage : Page.HaveData
    {
        public override string Address => "ShowItems";

        public void Show<KeyType,ValueType>(Table<ValueType,KeyType> DB)
            where KeyType:IComparable<KeyType>
        {
            base.Show(FindDB_Hash(DB.GetHashCode()));
        }
        internal void Show(string TableName, string RelationName, object ItemKey)
        {
            base.Show(TableName + "?" + RelationName + "?" + (string)ItemKey);
        }

        internal static (string TableName, string RelationName, string ItemKey)
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
            var DB = FindDB((Data.TableName, Data.RelationName));
            DB.Key = Data.ItemKey;
            MainElement.ReplaceChilds(await DB.ReadyShow());
        }
    }

    public class insertPage : Page.HaveData
    {
        public void Show<KeyType, ValueType>(Table<ValueType, KeyType> DB)
            where KeyType : IComparable<KeyType>
        {
            base.Show(FindDB_Hash(DB.GetHashCode()));
        }

        internal void Show(DataBaseInfo DB)
        {
            if (DB.RelationName == "")
                base.Show(DB.TableName);
            else
                base.Show(DB.TableName + "?" + DB.RelationName + "?" + DB.Key);
        }

        public override string Address => "insertPage";

        protected async override Task Ready()
        {
            var Data = ShowPage.GetDataFrom(GetDataString());
            var DB = FindDB((Data.TableName, Data.RelationName));
            DB.Key = Data.ItemKey;
            MainElement.ReplaceChilds(await DB.Readyinsert());
        }
    }

    public class UpdatePage : Page.HaveData
    {
        public void Show<KeyType, ValueType>(Table<ValueType, KeyType> DB, string Key)
            where KeyType : IComparable<KeyType>
        {
            base.Show(FindDB_Hash(DB.GetHashCode()) + "," + Key);
        }
        internal void Show(string TableName, string Key)
        {
            base.Show(TableName + "," + Key);
        }

        public override string Address => "UpdatePage";

        protected async override Task Ready()
        {
            var Data = GetDataString();
            var SpratorPos = Data.IndexOf(",");
            var TableName = Data.Substring(0, SpratorPos);
            var Key = Uri.UnescapeDataString(Data.Substring(SpratorPos + 1));
            MainElement.ReplaceChilds(await FindDB((TableName, "")).ReadyUpdate(Key));
        }
    }

    public class SelectForAccept : Page.HaveData
    {
        public override string Address => "SelectForAccept";

        public void Show<KeyType, ValueType>(Table<ValueType, KeyType> DB)
            where KeyType : IComparable<KeyType>
        {
            base.Show(FindDB_Hash(DB.GetHashCode()));
        }
        internal void Show(string TableName, string RelationName, object ItemKey)
        {
            base.Show(TableName + "?" + RelationName + "?" + (string)ItemKey);
        }

        protected async override Task Ready()
        {
            var Data = ShowPage.GetDataFrom(GetDataString());
            var DB = FindDB((Data.TableName, Data.RelationName));
            DB.Key = Data.ItemKey;
            MainElement.ReplaceChilds(await DB.ReadyAccept());
        }
    }

    public class SelectForIgnore : Page.HaveData
    {
        public override string Address => "SelectForIgnore";

        public void Show<KeyType, ValueType>(Table<ValueType, KeyType> DB)
            where KeyType : IComparable<KeyType>
        {
            base.Show(FindDB_Hash(DB.GetHashCode()));
        }
        internal void Show(string TableName, string RelationName, object ItemKey)
        {
            base.Show(TableName + "?" + RelationName + "?" + (string)ItemKey);
        }

        protected async override Task Ready()
        {
            var Data = ShowPage.GetDataFrom(GetDataString());
            var DB = FindDB((Data.TableName, Data.RelationName));
            DB.Key = Data.ItemKey;
            MainElement.ReplaceChilds(await DB.ReadyIgnore());
        }
    }
}