using System;
using static MonsajemData.DataBase;
using MonsajemData;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Database.KeyValue;
using Monsajem_Incs.Resources;
using Monsajem_Incs.Net.Web;
using Monsajem_Incs.Net.Base.Service;
using WebAssembly.Browser.DOM;
using System.Reflection;
using System.Threading.Tasks;
using static WASM_Global.Publisher;

namespace Monsajem_Client
{
    public class ThisException : Exception
    {
        public ThisException(string Message) : base(Message)
        { }
    }

    public class ThisVerException : ThisException
    {
        public ThisVerException(string Message) : base(Message)
        { }
    }

    public class ThisSecureException : ThisException
    {
        public ThisSecureException(string Message) : base(Message)
        { }
    }
}