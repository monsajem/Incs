using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp_NetCore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("myApp");
            var BaseUri = new Uri(builder.HostEnvironment.BaseAddress).GetLeftPart(System.UriPartial.Authority);
            BaseUri = BaseUri + "/";
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(BaseUri) });
            await builder.Build().RunAsync();
        }
    }
}
