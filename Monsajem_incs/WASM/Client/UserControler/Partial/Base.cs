using Monsajem_Incs.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.Views.Extentions.Table;
using static MonsajemData.DataBase;
using static WASM_Global.Publisher;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using static Monsajem_Client.SafeRun;

namespace UserControler
{
    public abstract class Page:IComparable<Page>
    {
        public static HTMLElement AppMainElement;
        private class _Page : Page
        {
            public string _Address;
            public override string Address => _Address;

            protected async override Task Ready()
            {
                
            }
        }
        private static Array<Page> Pages = new Array<Page>(10);
        private static string CurrentAddress;
        public abstract string Address { get; }
        public HTMLDivElement MainElement;
        public async Task Show()
        {
            await Task.Delay(1);
            await Safe(async ()=> await Route("/?" + Address));
            NavigationManager.NavigateTo("/?"+Address);
        }

        protected abstract Task Ready();

        public int CompareTo(Page other)
        {
            return Address.CompareTo(other.Address);
        }

        public static async Task Route(string Address)
        {
            if (Address.Contains('?')==false)
                Address = Address + "?";
            var FirstPos = Address.IndexOf('?');
            if (FirstPos > -1)
                Address = Address.Substring(FirstPos + 1);
            await _Route(Address);
        }
        private static async Task _Route(string Address)
        {
            if (CurrentAddress == Address)
            {
                return;
            }
            var LastAddress = CurrentAddress;
            CurrentAddress = Address;
            Address = Address.Split('?')[0];
            var Pos = Pages.BinarySearch(new _Page() { _Address = Address }).Index;
            if (Pos < 0)
            {
                Publish.ShowDangerMessage("صفحه یافت نشد");
                return;
            }
            var page = Pages[Pos];
            try
            {
                page.MainElement = Document.document.CreateElement<HTMLDivElement>();
                await page.Ready();
                AppMainElement.ReplaceChilds(page.MainElement);
            }
            catch
            {
                CurrentAddress = LastAddress;
                throw;
            }
        }

        public static void SubmitPage(params Page[] Page)
        {
            Pages.BinaryInsert(Page);
        }

        public abstract class HaveData:Page
        {
            public async Task Show(string Data)
            {
                await Task.Delay(1);
                await Safe(async () => await Route("/?" + Address + "?" + Data));
                NavigationManager.NavigateTo("/?" + Address + "?" + Data);
            }
            public async Task Show<DataType>(DataType Data)
            {
                await Show(Convert.ToBase64String(Data.Serialize()));
            }
            public async Task Show(int Data)
            {
                await Show(Data);
            }
            protected string GetDataString()
            {
                try
                {
                    return NavigationManager.ToAbsoluteUri(CurrentAddress).Query.Substring(1);
                }
                catch
                {
                    Publish.ShowDangerMessage("خطا در مقادیر ورودی");
                    NavigationManager.NavigateTo("/");
                    throw;
                }
            }
            protected DataType GetData<DataType>()
            {
                var Data = GetDataString();
                  try
                  {
                        return Convert.FromBase64String(Data).Deserialize<DataType>();
                  }
                  catch
                  {
                        Publish.ShowDangerMessage("خطا در مقادیر ورودی");
                        NavigationManager.NavigateTo("/");
                        throw;
                  }
            }
        }
    }
}