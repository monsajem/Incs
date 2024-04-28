using Monsajem_Incs.Resources.Person;
using Monsajem_Incs.Views.Maker.Database;
using Monsajem_Incs.Views.Shower.Database;
using static Monsajem_Incs.UserControler.Publish;
using System;
using WebAssembly.Browser.MonsajemDomHelpers;
using static Monsajem_Client.App;
using Monsajem_Incs.DynamicAssembly;
using System.Linq;

namespace Monsajem_Client
{
    public partial class OnLoadApp
    {
        [AutoRun]
        private void Persons()
        {
            Data.Persons.RegisterView((c) =>
            {
                c.SetEdit<PersonEdit_html>((i) =>
                {
                     i.FillView = (i) =>
                     {

                     };
                     i.FillValue = (i) =>
                     {
                         return i.OldValue;
                     };
                });

                c.SetView<PersonView_html>((i) =>
                {
                    i.FillView = (i) =>
                    {
                        if(IsUser)
                            i.View.ManagerArea.Remove();

                        var PartTable = i.Value.Transactions as Monsajem_Incs.Database.Base.PartOfTable<Transaction, uint>;
                        if (PartTable != null)
                        {
                            {
                                var H1 = new Monsajem_Incs.Resources.Base.Html.h5_html().Main;
                                var AcountingText = "";
                                var Acounting = PartTable.Sum((c) => c.Value.Value);
                                if (Acounting == 0)
                                    AcountingText = "بی حساب";
                                else if (Acounting < 0)
                                    AcountingText = "مجموع حساب بدهی ما به شخص";
                                else if (Acounting > 0)
                                    AcountingText = "مجموع حساب طلب ما از شخص";
                                H1.TextContent = AcountingText;
                                i.View.Acounting.AppendChild(H1);
                                if (Acounting != 0)
                                {
                                    i.View.Acounting.AppendChild("<br/>");
                                    i.View.Acounting.AppendChild(AddThousandSprator(Acounting));
                                }
                            }
                        }
                        i.View.Image.SetAttribute("src", "/PersonsPic/" + i.Value.Name + App.CachedUri);
                        i.View.Image.SetAttribute("alt", i.Value.Name);
                        if(IsUser)
                            i.View.Main.OnClick += (c1, c2) =>
                                i.Value.Transactions.ShowItems();
                        else
                            i.View.btn_showTransactions.OnClick += (c1, c2) => 
                                i.Value.Transactions.ShowItems();
                    };
                    i.GetMain = (i) => i.Main;
                    i.RegisterEdit = (i) =>
                    {
                        i.View.btn_edit.OnClick += (c1, c2) => i.Edit();
                    };
                    i.RegisterDelete = (i) =>
                    {
                        i.View.btn_delete.OnClick += (c1, c2) => i.Delete();
                    };
                });
            });
        }
    }
}