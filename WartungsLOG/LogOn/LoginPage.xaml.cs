using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WartungsLOG.Select;
using Xamarin.Essentials;
using System.Runtime.CompilerServices;

namespace WartungsLOG.LogOn
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private B2CAuthService _b2cauth;

        public LoginPage()
        {
            InitializeComponent();
            _b2cauth = Startup.ServiceProvider.GetService<B2CAuthService>();
        }
        public void logMe(string log, [CallerMemberName] string cmn = "", [CallerLineNumber] int cln=0, [CallerFilePath] string cfp="")

        {
            Debug.WriteLine($"XXXX: LoginPage: callermembername: {cmn} line: {cln} file: {cfp} : LOGMESSAGE: {log}" );
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
            logMe("hier und da");

            //native.OpenToast("global Toast");

            Debug.WriteLine($"XXXX: platform:  {Xamarin.Forms.Device.RuntimePlatform}");

            if (Xamarin.Forms.Device.RuntimePlatform == "Android" || Xamarin.Forms.Device.RuntimePlatform == "UWP")
            {
                try
                {
                    if (btnSignInSignOut.Text == "Einloggen")
                    {
                        var userContext = await _b2cauth.SignInOnAppearing();
                        UpdateSignInState(userContext); // set label to "Ausloggen" if already logged in
                        UpdateUserInfo(userContext);    // set labels to show DisplayName and Emailaddress

                    }
                }
                catch (Exception ex)
                {
                    //    DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
                    Debug.WriteLine($"XXXX LoginPage.xaml.cs: Exception: {ex}");

                }
            }

        }

        private async void btnSignInSignOut_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (btnSignInSignOut.Text == "Einloggen")
                {
                    var userContext = await _b2cauth.SignInAsync();
                    UpdateSignInState(userContext);
                    UpdateUserInfo(userContext);
                }
                else
                {
                    var userContext = await _b2cauth.SignOutAsync();
                    UpdateSignInState(userContext);
                    UpdateUserInfo(userContext);
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("AADB2C90118"))
                {
                    //OnPasswordReset();
                }
                // Alert if any exception excluding user canceling sign-in dialog
                else if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                    await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }
        }

        private void UpdateSignInState(UserContext uc)
        {
            btnSignInSignOut.Text = uc.IsLoggedOn ? "Ausloggen" : "Einloggen";
        }

        public void UpdateUserInfo(UserContext uc)
        {
            lblName.Text = uc.Name;
            lblEmail.Text = uc.Email;
            //lblOID.Text = uc.UserIdentifier;
        }

        private void btnGoSelect_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"{nameof(SelectVehiclePage)}");
        }

        private async void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            try
            {
                Vibration.Vibrate();
                if (e.Value)
                {
                    await Xamarin.Essentials.Flashlight.TurnOnAsync();
                }
                else
                {
                    await Xamarin.Essentials.Flashlight.TurnOffAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("torch", $"Taschenlampenexpeption: {ex.Message}", "dismiss");
            }
        }

       
    }
}