using Monsajem_Incs.Resources.Transactions;
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
        private static string AddThousandSprator(Int64 IntValue)
        {
            var StyleGreen = "style='color:Green;'";
            var StyleRed = "style='color:chocolate;'";
            if(IntValue<0)
            {
                StyleGreen = "style='color:chocolate;'";
                StyleRed = "style='color:Green;'";
                IntValue = IntValue * -1;
            }

            var StrValue = IntValue.ToString();
            var Result = "";
            for (int i = StrValue.Length - 1; i > 2; i -= 3)
            {
                Result = "<b "+StyleRed+ ">'</b><b " + StyleGreen + ">" + StrValue.Substring(i - 2, 3) + "</b>" + Result;
            }
            var LastPos = (StrValue.Length) % 3;
            if (LastPos == 0)
                LastPos = 3;
            Result = "<div dir='ltr' style='flex-wrap:nowrap;display:inline'><b " + StyleGreen + ">" + StrValue.Substring(0, LastPos) + "</b>" + Result + "</div>";
            return Result;
        }

        [AutoRun]
        private void Transactions()
        {
            Data.Transactions.RegisterView((c) =>
            {
                c.SetEdit<TransactionEdit_html>((i) =>
                {
                     i.FillView = (i) =>
                     {

                     };
                     i.FillValue = (i) =>
                     {
                         return i.OldValue;
                     };
                });

                c.SetView<TransactionView_html>((i) =>
                {
                    i.MakeHolder = (Table) =>
                    {
                        var MainDiv = new Monsajem_Incs.Resources.Card_html();
                        var Div = MainDiv.Card;

                        var PartTable = Table as Monsajem_Incs.Database.Base.PartOfTable<Transaction,uint>;
                        if (PartTable != null)
                        {
                            {//// show person name
                                var H1 = new Monsajem_Incs.Resources.Base.Html.h1_html().Main;
                                H1.TextContent = PartTable.HolderTable.Key.ToString();
                                Div.AppendChild(H1);
                                Div.AppendChild("<br/>");
                            }

                            {
                                var H1 = new Monsajem_Incs.Resources.Base.Html.h5_html().Main;
                                var AcountingText = "";
                                var Acounting = PartTable.Sum((c) => c.Value.Value);
                                if(Acounting==0)
                                    AcountingText = "بی حساب";
                                else if (Acounting < 0)
                                    AcountingText = "مجموع حساب بدهی ما به شخص";
                                else if(Acounting>0)
                                    AcountingText = "مجموع حساب طلب ما از شخص";
                                H1.TextContent = AcountingText;
                                Div.AppendChild(H1);
                                if (Acounting != 0)
                                {
                                    Div.AppendChild("<br/>");
                                    Div.AppendChild(AddThousandSprator(Acounting));
                                }
                            }
                        }
                        Div.AppendChild("<br/>");
                        Div.AppendChild("<br/>");

                        return MainDiv.Main;
                    };
                    i.FillView = (i) =>
                    {
                        if (IsUser)
                            i.View.ManagerArea.Remove();
                        var PersonName = i.Value.Person.Key.ToString();

                        i.View.Value.InnerHtml = AddThousandSprator(i.Value.Value);
                        if (i.Value.Value > 0)
                            i.View.TransactionType.TextContent = "طلب ما از "+ PersonName;
                        else
                            i.View.TransactionType.TextContent = "بدهی ما به "+ PersonName;

                        var TransActions = i.Value.Person.Value.Transactions.ToArray();
                        TransActions = TransActions.Where((c) => c.Value.Code <= i.Value.Code).ToArray();
                        var Acounting = TransActions.Sum((c) => c.Value.Value);
                        if(Acounting!=0)
                        {
                            i.View.AcountingValue.InnerHtml = AddThousandSprator(Acounting);
                            if (Acounting > 0)
                                i.View.AcountingType.TextContent = "مجموع حساب طلب ما از "+PersonName;
                            else if (Acounting < 0)
                                i.View.AcountingType.TextContent = "مجموع حساب بدهی ما به "+PersonName;
                        }
                        else
                            i.View.AcountingType.TextContent = "تسویه حساب شد";

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

                c.SetSelector((c) =>
                {
                    var Values = c.Values.Reverse();
                    if (c.Query != null)
                    {
                        var keys = c.Query.Split(" ");
                        keys.Where((c) => c.Trim() != "").
                            GroupBy((c) => c).ToArray();

                        var SValues = Values.Select((c) =>
                        {
                            var i = 0;
                            foreach (var Key in keys)
                            {
                                if (c.Describe.Contains(Key))
                                    i++;
                            }
                            return (Value: c, Related: i);
                        }).Where((c) => c.Related > 0).ToArray();

                        Values = SValues.OrderByDescending((c) => c.Related).
                                         Select((c) => c.Value).ToArray();

                        if (Values.Count() == 0)
                        {
                            ShowDangerMessage("نتیجه ای یافت نشد");
                            throw new Exception();
                        }
                    }
                    return Values;
                });
            });
        }
    }
}