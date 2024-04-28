using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monsajem_Incs.Net.Base.Service
{
    internal class Service<AddressType>
        where AddressType : IComparable<AddressType>
    {
        private SortedDictionary<AddressType, Func<Task>> Services =
                [];
        private IAsyncOprations Link;

        public Service(IAsyncOprations link)
        {
            Link = link;
        }

        public void AddService(
            AddressType ServiceAddress,
            Func<Task> Service)
        {
            Services.Add(ServiceAddress, Service);
        }

        public async Task Response(AddressType EndResponse)
        {
            while (true)
            {
                var ServiceAddress = await Link.GetData<AddressType>();
                if (ServiceAddress.Equals(EndResponse))
                    return;
                await Services[ServiceAddress]();
            }
        }

        public async Task Request(AddressType ServiceAddress)
        {
            _ = await Link.SendData(ServiceAddress);
        }

        public async Task EndService(AddressType EndResponse)
        {
            _ = await Link.SendData(EndResponse);
        }
    }
}