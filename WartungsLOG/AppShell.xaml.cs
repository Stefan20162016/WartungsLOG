using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using WartungsLOG.Select;
using WartungsLOG.ServiceHistory;
using WartungsLOG.Service;
using WartungsLOG.ServicePicture;


namespace WartungsLOG
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SelectVehiclePage), typeof(SelectVehiclePage));
            Routing.RegisterRoute(nameof(ServiceHistoryPage), typeof(ServiceHistoryPage));
            Routing.RegisterRoute(nameof(ServicePage), typeof(ServicePage));
            Routing.RegisterRoute(nameof(ServicePicturePage), typeof(ServicePicturePage));
            Routing.RegisterRoute(nameof(AddVehiclePage), typeof(AddVehiclePage));
            Routing.RegisterRoute(nameof(AddServiceEntryPage), typeof(AddServiceEntryPage));
            Routing.RegisterRoute(nameof(AddDetailEntryPage), typeof(AddDetailEntryPage));
        }
    }
}