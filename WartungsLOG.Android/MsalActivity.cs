﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WartungsLOG.LogOn;

namespace WartungsLOG.Droid
{
    [Activity]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
        DataHost = "auth",                                        // ://auth
        DataScheme = "msal04809bbd-20ab-43b4-acd8-c7c3df586222")] // msald...://auth
        //DataScheme = $"msal{B2CConstants.ClientID}")] maybe in c#10
    public class MsalActivity : BrowserTabActivity
    {
    }
}