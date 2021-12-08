using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WartungsLOG.Service
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // property, parametername
    //[QueryProperty(nameof(VehicleID), "paraVehicleID")] // name, queryID
    //[QueryProperty(nameof(ServiceHistoryID), "paraHistoryRecordID")]
    public partial class ServicePage : ContentPage
    {
        private readonly ServiceViewModel _vm;
        public int VehicleID { get; set; }
        public int ServiceHistoryID { get; set; }
        public int SR_ID { get; set; }

        public ServicePage()
        {
            InitializeComponent();
            _vm = Startup.ServiceProvider.GetService<ServiceViewModel>();
            BindingContext = _vm;

        }
        protected override async void OnAppearing()
        {
            //_vm.VehicleIDProperty = VehicleID;
            //_vm.ServiceHistoryRecordIDProperty = ServiceHistoryID;
            //SR_ID = 4711;
            await _vm.Initialize();
            base.OnAppearing();
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {
            Debug.WriteLine("XXXX: code behind SwipeItem_Invoked");
        }
    }
}