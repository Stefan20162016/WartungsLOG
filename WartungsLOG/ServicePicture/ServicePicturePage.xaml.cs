using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WartungsLOG.Fonts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WartungsLOG.ServicePicture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServicePicturePage : ContentPage
    {
        private readonly ServicePictureViewModel _vm;
        public ServicePicturePage()
        {
            InitializeComponent();
            _vm = Startup.ServiceProvider.GetService<ServicePictureViewModel>();
            BindingContext = _vm;
        }
        protected override async void OnAppearing()
        {
            if (Xamarin.Forms.Device.RuntimePlatform == Device.iOS || Xamarin.Forms.Device.RuntimePlatform == Device.UWP )
            {
                toolbarfontimage.Glyph = Material.Ios_share;
            }

            Console.WriteLine("XXXX: servicepicturepage onappearing1");
            await _vm.Initialize();
            
            base.OnAppearing();
            Console.WriteLine("XXXX: servicepicturepage onappearing2");
        }

    }
}