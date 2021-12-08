
// from "active-directort-b2c-Xamarin-native" MS Sample

using System;
using System.Collections.Generic;
using System.Text;

namespace WartungsLOG.LogOn
{
    public static class B2CConstants
    {
        // Azure AD B2C Coordinates

        public static string Tenant = "wartungslog.onmicrosoft.com";
        public static string AzureADB2CHostname = "wartungslog.b2clogin.com";
        public const string ClientID = "04809bbd-20ab-43b4-acd8-c7c3df586222";

        public static string PolicySignUpSignIn = "B2C_1_signupandin";
        public static string PolicyEditProfile = "B2C_1_profileEditingFlow";
        public static string PolicyResetPassword = "b2c_1_reset";

        public static string ID_WebAPI_APP_Registration = "0d2db8a3-dafd-4c08-9382-df7e42ccd9a6";

        public static string[] Scopes = { "offline_access", $"https://{Tenant}/{ID_WebAPI_APP_Registration}/zugriffFuerBenutzer" };

       

        public static string AuthorityBase = $"https://{AzureADB2CHostname}/tfp/{Tenant}/";

          

        public static string AuthoritySignInSignUp = $"{AuthorityBase}{PolicySignUpSignIn}";
        public static string AuthorityEditProfile = $"{AuthorityBase}{PolicyEditProfile}";
        public static string AuthorityPasswordReset = $"{AuthorityBase}{PolicyResetPassword}";

        // for iOS
        public static string IOSKeyChainGroup = "com.companyname.WartungsLOG"; // check $(AppIdentifierPrefix)com.companyname.WartungsLOG in entitlment.plist
    }
}
