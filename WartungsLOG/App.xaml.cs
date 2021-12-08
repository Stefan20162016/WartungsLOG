using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WartungsLOG
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new MainPage();
            var shell = Startup.ServiceProvider.GetService<AppShell>();
            MainPage = shell;

            //MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
