using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WartungsLOG.ServiceHistory;

namespace WartungsLOG.Select
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class SelectVehiclePage : ContentPage
    {
        private readonly SelectViewModel _vm;
        public SelectVehiclePage()
        {
            InitializeComponent();
            _vm = Startup.ServiceProvider.GetService<SelectViewModel>();
            BindingContext = _vm;

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _vm.Initialize();
        }

    }
}