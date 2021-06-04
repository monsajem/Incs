using System;
using Monsajem_Incs.Net.Base.Socket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monsajem_Incs.Serialization;
using System.Reflection;
using static Monsajem_Incs.Collection.Array.Extentions;
using static System.Runtime.Serialization.FormatterServices;

namespace Monsajem_Incs.Net.Base.Service
{
    internal class Service<AddressType>
        where AddressType:IComparable<AddressType>
    {
        private SortedDictionary<AddressType, Func<Task>> Services =
                new SortedDictionary<AddressType, Func<Task>>();
        private IAsyncOprations Link;

        public Service(IAsyncOprations link)
        {
            this.Link = link;
        }

        public void AddService(
            AddressType ServiceAddress,
            Func<Task> Service)
        {
            Services.Add(ServiceAddress,Service);
        }

        public async Task Response()
        {
            while (await Link.GetData<bool>())
            {
                var ServiceName = await Link.GetData<AddressType>();
                await Services[ServiceName]();
            }
        }

        public async Task Request(AddressType ServiceAddress)
        {
            await Link.SendData(true);
            await Link.SendData(ServiceAddress);
        }

        public async Task EndService()
        {
            await Link.SendData(false);
        }
    }
}