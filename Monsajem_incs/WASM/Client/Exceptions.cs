using System;

namespace Monsajem_Incs.WasmClient
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