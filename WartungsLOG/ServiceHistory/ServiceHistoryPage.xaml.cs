using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WartungsLOG.Select;

namespace WartungsLOG.ServiceHistory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //[QueryProperty(nameof(VehicleID), "para"+nameof(VehicleID))] // propery, route-parametername
    //[QueryProperty(nameof(VehicleID), nameof(VehicleID))]
    public partial class ServiceHistoryPage : ContentPage
    {
        private readonly ServiceHistoryViewModel _vm;

        //public int VehicleID { get; set; }
        
        public ServiceHistoryPage()
        {
            InitializeComponent();
            _vm = Startup.ServiceProvider.GetService<ServiceHistoryViewModel>();
            BindingContext = _vm;
        }

        protected override async void OnAppearing()
        {
            //_vm.VehicleID = VehicleID;
            await _vm.Initialize();
            base.OnAppearing();
            
        }
    }
}