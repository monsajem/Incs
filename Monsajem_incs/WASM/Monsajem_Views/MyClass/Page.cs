using Monsajem_Incs.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;
using static MonsajemData.DataBase;
using static WASM_Global.Publisher;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using static Monsajem_Incs.WasmClient.SafeRun;
using Monsajem_Incs.UserControler;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace Monsajem_Incs.Views
{
    public abstract class Page : IComparable<Page>
    {
        public static Microsoft.AspNetCore.Components.NavigationManager NavigationManager { get => WASM_Global.Publisher.NavigationManager; }
        public static readonly string DataUrlSperator = "&";
        public static event Action<(Page Page,string CurrentUrl)> LoadingPage;
        public static bool IsLocalUrl {
            get
            {
                var Url = new System.Uri(NavigationManager.BaseUri);
                Console.WriteLine(Url.Host);
                return Url.Host == "localhost";
            }
        }

        private static HTMLElement AppMainElement;
        private class _Page : Page
        {
            public string _Address;
            public override string Address => _Address;

            protected async override Task Ready()
            {

            }
        }
        private static Array<Page> Pages = new Array<Page>(10);
        public static string CurrentAddress { get; private set; }
        public static Page CurrentPage { get; private set; }
        public abstract string Address { get; }
        public HTMLDivElement MainElement;
        public async Task Show(bool NewStateIfIsInPage=false)
        {
            await Task.Delay(1);
            var LastAddress = CurrentAddress;
            await Safe(async () => await Route("/?" + Address));
            if (NewStateIfIsInPage == false)
            {
                if (CurrentAddress != LastAddress)
                    NavigationManager.NavigateTo("/?" + Address);
            }
            else
                NavigationManager.NavigateTo("/?" + Address);
        }

        protected abstract Task Ready();

        public int CompareTo(Page other)
        {
            return Address.CompareTo(other.Address);
        }

        public static async Task Route(string Address)
        {
            if (Address.Contains('?') == false)
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
            var LastPage = CurrentPage;
            CurrentAddress = Address;
            Address = Address.Split('?')[0];
            Address = Address.Split(DataUrlSperator)[0];
            var Pos = Pages.BinarySearch(new _Page() { _Address = Address }).Index;
            if (Pos < 0)
            {
                Publish.ShowDangerMessage("صفحه یافت نشد");
                return;
            }
            var page = Pages[Pos];
            CurrentPage = page;
            try
            {
                await Replay();
            }
            catch
            {
                CurrentPage = LastPage;
                CurrentAddress = LastAddress;
                throw;
            }
        }

        public static async Task Replay()
        {
            LoadingPage?.Invoke((CurrentPage, CurrentAddress));
            CurrentPage.MainElement = js.Document.CreateElement<HTMLDivElement>();
            await CurrentPage.Ready();
            AppMainElement.ReplaceChilds(CurrentPage.MainElement);
        }

        private static bool DB_PagesLoaded;
        public static void SubmitPage(HTMLElement MainElement, params Page[] Page)
        {
            if (DB_PagesLoaded == false)
            {
                Pages.BinaryInsert(
                    new Shower.Database.ShowPage(),
                    new Shower.Database.insertPage(),
                    new Shower.Database.UpdatePage());
                DB_PagesLoaded = true;
            }
            Pages.BinaryInsert(Page);
            AppMainElement = MainElement;
        }

        public abstract class HaveData : Page
        {
            protected string ProvideUri(params string[] Datas)
            {
                var NewUri = "/?" + Address;
                foreach (var Data in Datas)
                    NewUri = NewUri + DataUrlSperator + Uri.EscapeDataString(Data);
                return NewUri;
            }

            public async Task Show(params string[] Datas)
            {
                await Task.Delay(1);
                await Safe(async () => await Route(ProvideUri(Datas)));
                NavigationManager.NavigateTo(ProvideUri(Datas));
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
                    var Pos = CurrentAddress.IndexOf(DataUrlSperator);
                    if (Pos < 0)
                        throw new Exception("Data in url not found!");
                    var QString = CurrentAddress.Substring(Pos + 1);
                    return QString;
                }
                catch
                {
                    Console.WriteLine(Address);
                    Console.WriteLine(CurrentAddress);
                    throw;
                    Publish.ShowDangerMessage("خطا در مقادیر ورودی");
                    NavigationManager.NavigateTo("/");
                    throw;
                }
            }

            protected string[] GetDataStringParameters()
            {
                var QString = GetDataString();
                var Inputs = QString.Split(DataUrlSperator)
                                    .Select((c) => Uri.UnescapeDataString(c))
                                    .ToArray();
                return Inputs;
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