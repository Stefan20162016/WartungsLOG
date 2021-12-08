using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WartungsLOG.LogOn;
using Microsoft.Identity.Client;
using Android.Content;

namespace WartungsLOG.Droid
{
    [Activity(Label = "WartungsLOG", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            //LoadApplication(new App());
            //LoadApplication(Startup.Init(ConfigureLocalServices));
            Startup.Init(ConfigureLocalServices);
            LoadApplication(new App());    
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void ConfigureLocalServices(HostBuilderContext ctx, IServiceCollection services) {
            services.AddSingleton<INativeCalls, NativeCalls>();
            services.AddSingleton<IParentWindowLocatorService, AndroidParentWindowLocatorService>();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
        }

    }
}