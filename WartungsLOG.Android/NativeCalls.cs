using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WartungsLOG.Droid
{
    public class NativeCalls : INativeCalls
    {
        public void OpenToast(string text)
        {
            Toast.MakeText(Application.Context, text, ToastLength.Long).Show();
        }
    }
}