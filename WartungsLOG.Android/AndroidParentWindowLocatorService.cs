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
using WartungsLOG.LogOn;
using Xamarin.Essentials;

namespace WartungsLOG.Droid
{
    class AndroidParentWindowLocatorService : IParentWindowLocatorService
    {
        public object GetCurrentParentWindow()
        {
            return Platform.CurrentActivity;
        }
    }
}