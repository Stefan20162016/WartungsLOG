using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WartungsLOG.ServiceHistory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddServiceEntryPage : ContentPage
    {
        public AddServiceEntryPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<AddServiceEntryViewModel>();
        }
    }
}