﻿using Monsajem_Incs.Async;
using Monsajem_Incs.Net.Base.Socket.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Monsajem_Incs.Collection.Array.Extentions;

namespace Monsajem_Incs.Net.Base.Socket
{
    namespace Exceptions
    {
        public class SocketExeption : Exception
        {
#if DEBUG
            public (string Trace, object Info)[] DebugInfo;
#endif
            public SocketExeption() { }
            public SocketExeption(string Message) : base(Message) { }
        }
        public class SocketClosedExeption : SocketExeption
        {
            public SocketClosedExeption() { }
            public SocketClosedExeption(string Message) : base(Message) { }
        }
        public class SocketClosingExeption : SocketExeption
        {
            public SocketClosingExeption() { }
            public SocketClosingExeption(string Message) : base(Message) { }
        }
        public class ConnectionFailedExeption : SocketExeption
        {
            public ConnectionFailedExeption() { }
            public ConnectionFailedExeption(string Message) : base(Message) { }
        }
    }

    public interface IClientSocket
    {
        Task Send(byte[] Data);
        Task<int> Recive(byte[] Buffer);

        Task<byte[]> Recive(int Size);
        Task SendPacket(byte[] Data);
        Task<byte[]> RecivePacket();
        Task Disconncet();
    }

    public class ClientSocket<AddressType> :
        IClientSocket
    {
#if DEBUG
        public (string Trace, object Info)[] DebugInfo = new (string Trace, object Info)[0];
        public static int SendTimeOut = 3000;
        public static int ReciveTimeOut = 3000;

        internal void AddDebugInfo(string Trace) => AddDebugInfo((Trace, null));
        internal void AddDebugInfo(string Trace, object Info) => AddDebugInfo((Trace, Info));
        internal void AddDebugInfo((string Trace, object Info) Trace)
        {
            lock (DebugInfo)
                Insert(ref DebugInfo, Trace);
        }
#endif

        private Locker<int> Sendings = new();
        public event Action<Exception> OnError;
        private AddressType P_Address;

        public ClientSocket()
        {

        }
        public virtual AddressType Address => P_Address;

        private Locker<bool> P_IsConnected = new();
        public virtual bool IsConnected
        {
            get => P_IsConnected.LockedValue;
            internal set
            {
#if DEBUG
                AddDebugInfo("Connection Changed To " + value + " in \n" +

                        string.Concat(new System.Diagnostics.StackTrace(true).GetFrames().
                        Select((c) => "\nMethod> " + c.GetMethod().DeclaringType.ToString() + "." +
                                                  c.GetMethod().Name +
                                     "\nFile> " + c.GetFileName() +
                                     "\nLine> " + c.GetFileLineNumber())));

#endif
                if (value != P_IsConnected.Value)
                {
                    P_IsConnected.Value = value;
                }
            }
        }

        private async Task _Send(byte[] Data)
        {
            try
            {
#if TRACE_NET
                Console.WriteLine($"Net:{Address} Sending " + Data.Length);
#endif
                Task[] Tasks;
                using (P_IsConnected.Lock())
                {
                    if (P_IsConnected.Value == false)
                    {
                        var ExMSG = "socket connection Changed in _Send(byte[] Data)";
                        var Ex = new SocketClosedExeption(ExMSG);
#if DEBUG
                        AddDebugInfo(ExMSG);
                        Ex.DebugInfo = DebugInfo;
#endif
                        OnError?.Invoke(Ex);
                        throw Ex;
                    }
                    Tasks = new Task[] {
                        Inner_Send(Data),
                        P_IsConnected.WaitForChange()};
                }
                using (Sendings.Lock())
                    Sendings.Value++;
#if DEBUG
                var ResultTask = await Task.WhenAny(Tasks).TimeOut(SendTimeOut);
#else
                var ResultTask = await Task.WhenAny(Tasks);
#endif
                if (P_IsConnected.LockedValue == false)
                {
                    var ExMSG = "socket connection Changed in _Send(byte[] Data)";
                    var Ex = new SocketClosedExeption(ExMSG);
#if DEBUG
                    AddDebugInfo(ExMSG);
                    Ex.DebugInfo = DebugInfo;
#endif
                    OnError?.Invoke(Ex);
                    throw Ex;
                }
#if TRACE_NET
                Console.WriteLine($"Net:{Address} Sended " + Data.Length);
#endif
            }
#if TRACE_NET
            catch (Exception ex)
            {
                Console.WriteLine($"Net:{Address} Send Faild " + Data.Length);
                Console.WriteLine(ex.Message);
                throw;
            }
#endif
            finally
            {
                using (Sendings.Lock())
                    Sendings.Value--;
            }
        }

        public async Task Send(byte[] Data)
        {
            await _Send(Data);
        }

        public virtual async Task<int> Recive(byte[] Buffer)
        {
#if TRACE_NET
            Console.WriteLine($"Net:{Address} Reciveing ...");
#endif
            Task[] SomthingRecived = null;
            using (P_IsConnected.Lock() + RecivedBuffer.Lock())
            {
                if (RecivedBuffer.Value.Length == 0)
                {
                    if (P_IsConnected.Value == false)
                    {

                        var ExMSG = "socket connection Changed in Recive(byte[] Buffer)";
                        var Ex = new SocketClosedExeption(ExMSG);
#if DEBUG
                        AddDebugInfo(ExMSG);
                        Ex.DebugInfo = DebugInfo;
#endif
                        OnError?.Invoke(Ex);
                        throw Ex;
                    }
                    SomthingRecived = new Task[] { RecivedBuffer.WaitForChange(),
                                                   P_IsConnected.WaitForChange()};
                }
            }
            if (SomthingRecived != null)
            {
#if TRACE_NET
                Console.WriteLine($"Net:{Address} Reciveing Waiting ...");
#endif
#if DEBUG
                var ResultTask = await Task.WhenAny(SomthingRecived).TimeOut(ReciveTimeOut);
#else
                var ResultTask = await Task.WhenAny(SomthingRecived);
#endif
            }
            using (RecivedBuffer.Lock())
            {
                var R_Buffer = RecivedBuffer.Value;

#if TRACE_NET
                Console.WriteLine($"Net:{Address} Recived " + R_Buffer.Length);
#endif
                var Position = Buffer.Length > R_Buffer.Length ? R_Buffer.Length - 1 : Buffer.Length - 1;
                var Recived = PopTo(ref R_Buffer, Position);
                RecivedBuffer.Value = R_Buffer;
                Recived.CopyTo(Buffer, 0);
                return Recived.Length;
            }
        }

        public async Task Connect(AddressType Address)
        {
            if (P_IsConnected.Value == true)
            {
                var Ex = new ConnectionFailedExeption("Socket Is Connected");
#if DEBUG
                Ex.DebugInfo = DebugInfo;
#endif
                OnError?.Invoke(Ex);
                throw Ex;
            }
            P_Address = Address;
            await Inner_Connect(Address);
            P_IsConnected.Value = true;
        }

        public async Task Close()
        {
#if DEBUG
            AddDebugInfo("Close Method invoked.");
#endif
#if TRACE_NET
            Console.WriteLine($"Net:{Address} Disconnecting Close ...");
#endif
#if DEBUG
            await Inner_Disconnect().TimeOut(ReciveTimeOut);
#else
            await Inner_Disconnect();
#endif
#if TRACE_NET
            Console.WriteLine($"Net:{Address} Disconnected Close !");
#endif
            IsConnected = false;
            P_Address = default;
        }

        public async Task Disconncet()
        {
#if DEBUG
            AddDebugInfo("Disconncet Method invoked.");
#endif
#if TRACE_NET
            Console.WriteLine($"Net:{Address} Disconnceting ");
#endif
        CheckSends:
            Task WaitForSend = null;
            using (Sendings.Lock())
            {
                if (Sendings.Value > 0)
                {
#if TRACE_NET
                    Console.WriteLine($"Net:{Address} Disconnceting Waiting for Sending " + Sendings.Value);
#endif
                    WaitForSend = Sendings.WaitForChange();
                }
            }
            if (WaitForSend != null)
            {
                await WaitForSend;
                goto CheckSends;
            }

            try
            {
#if TRACE_NET
                Console.WriteLine($"Net:{Address} Last hand shaking...");
#endif
                await Task_EX.CheckAll(Send(new byte[1]), Recive(1));
#if TRACE_NET
                Console.WriteLine($"Net:{Address} Last hand shaked.");
#endif

#if TRACE_NET
                Console.WriteLine($"Net:{Address} Last hand shake 2/2");
#endif
            }
            catch
            {
#if TRACE_NET
                Console.WriteLine($"Net:{Address} Last hand shake faild but its ok.");
#endif
            }

            await Close();
#if TRACE_NET
            Console.WriteLine($"Net:{Address} Disconnceted ");
#endif
        }

        public async Task<byte[]> Recive(int Size)
        {

#if DEBUG
            if (Size < 0)
                throw new Exception("Recive size is wrong!");
#endif
#if TRACE_NET
            var OldSize = Size;
            Console.WriteLine($"Net:{Address} Reciving For Size 0/{OldSize}");
#endif
            var Result = new byte[Size];
            var ResultLen = Size;
            while (Size > 0)
            {
                var currentBuffer = new byte[Size];
                var currentBuffer_Size = await Recive(currentBuffer);
                System.Array.Copy(currentBuffer, 0, Result, ResultLen - Size,
                    currentBuffer_Size);
                Size -= currentBuffer_Size;
#if TRACE_NET

                Console.WriteLine($"Net:{Address} Reciving For Size {OldSize-Size}/{OldSize}");
#endif
            }
#if TRACE_NET
            Console.WriteLine($"Net:{Address} Recived For Size " + OldSize);
#endif
            return Result;
        }
        public async Task SendPacket(byte[] Data)
        {
#if TRACE_NET
            Console.WriteLine($"Net:{Address} Sending Packet " + Data.Length);
#endif
            await Send(BitConverter.GetBytes(Data.Length));
            await Send(Data);
#if TRACE_NET
            Console.WriteLine($"Net:{Address} Sendeded Packet " + Data.Length);
#endif
        }
        public async Task<byte[]> RecivePacket()
        {
#if TRACE_NET
            Console.WriteLine($"Net:{Address} Reciving Packet ");
#endif
            var PacketSize = BitConverter.ToInt32(await Recive(4), 0);
#if TRACE_NET
            Console.WriteLine($"Net:{Address} Reciving Packet " + PacketSize);
#endif
            var Result = await Recive(PacketSize);

#if TRACE_NET
            Console.WriteLine($"Net:{Address} Recived Packet " + PacketSize);
#endif
            return Result;
        }

        protected virtual Task Inner_Connect(AddressType Address) => throw new NotImplementedException("Inner_Connect Not Implemented");
        protected virtual Task Inner_Disconnect() => throw new NotImplementedException("Inner_Disconnect Not Implemented");
        protected virtual Task Inner_Send(byte[] Data) => throw new NotImplementedException("Inner_Send Not Implemented");

        private Locker<byte[]> RecivedBuffer = new() { Value = new byte[0] };
        protected void Recived(byte[] Buffer)
        {
#if DEBUG
            try
            {
#endif
                using (RecivedBuffer.Lock())
                {
                    var R_Buffer = RecivedBuffer.Value;
                    Insert(ref R_Buffer, Buffer);
                    RecivedBuffer.Value = R_Buffer;
                }
#if DEBUG
            }
            catch (Exception ex)
            {
                AddDebugInfo("exception thrown at Recived method.\n" + ex.Message,
                    new Exception("exception thrown at Recived method.", ex));
            }
#endif
        }
    }
}