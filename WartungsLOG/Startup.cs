using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;

using WartungsLOG.Core;
using WartungsLOG.Data;
using WartungsLOG.Select;
using WartungsLOG.ServiceHistory;
using WartungsLOG.Service;
using WartungsLOG.ServicePicture;
using WartungsLOG.LogOn;

namespace WartungsLOG
{
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void Init(Action<HostBuilderContext, IServiceCollection> configPlatformServices)
        {

            var a = Assembly.GetExecutingAssembly();
            var host = new HostBuilder().ConfigureHostConfiguration(c =>
               {
                   c.AddCommandLine(new[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                   c.AddJsonFile(new EmbeddedFileProvider(typeof(Startup).Assembly, typeof(Startup).Namespace), "startup_appsettings.json", false, false);
               })
                .ConfigureServices((c, x) =>
               {
                   configPlatformServices(c, x); // Parameter to Init: i.e. Init(configPlatformSpecificServices) 
                   ConfigureGeneralServices(c, x); // below
               })
                .ConfigureLogging(l => l.AddConsole(o =>
                {
                    //setup a console logger and disable colors since they don't have any colors in VS
                    o.DisableColors = true;
                }))
                .Build();
            ServiceProvider = host.Services;
            //return new App();
        }

        static void ConfigureGeneralServices(HostBuilderContext ctx, IServiceCollection service_collection)
        {
            if (ctx.HostingEnvironment.IsDevelopment())
            {
                // ie mock data services
            }
            else
            {
                
            }
            service_collection.AddTransient<AppShell>();

            service_collection.AddSingleton<B2CAuthService>();
            service_collection.AddSingleton<INavigationService, NavigationService>();
            
            //service_collection.AddSingleton<IDataService, DummyDataService>();
            service_collection.AddSingleton<IDataService, RealDataService>();
            
            service_collection.AddTransient<SelectViewModel>();
            service_collection.AddSingleton<ServiceHistoryViewModel>();
            service_collection.AddSingleton<ServiceViewModel>();

            
            service_collection.AddTransient<ServicePictureViewModel>(); // helps with repainting: Singleton shows old pic

            service_collection.AddSingleton<AddVehicleViewModel>();
            service_collection.AddSingleton<AddServiceEntryViewModel>();
            service_collection.AddSingleton<AddDetailEntryViewModel>();

            

        }
    }
}
