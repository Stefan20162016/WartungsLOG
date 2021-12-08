using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WartungsLOG.Select
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddVehiclePage : ContentPage
    {
        public AddVehiclePage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<AddVehicleViewModel>();
        }
    }
}