using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WartungsLOG.Service
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDetailEntryPage : ContentPage
    {
        public AddDetailEntryPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<AddDetailEntryViewModel>();
        }
    }
}