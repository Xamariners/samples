using FlightFinder.Client.Services;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FlightFinder.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            // register services
            var serviceProvider = new BrowserServiceProvider(services =>
            {
                services.AddSingleton<AppState>();
                services.AddSingleton<HistoryService>();
                services.AddSingleton<AirlineService>();                
            });

            // set browser renderer services, entrypoint, and  rendewr tree root
            new BrowserRenderer(serviceProvider).AddComponent<Main>("body");
        }
    }
}
