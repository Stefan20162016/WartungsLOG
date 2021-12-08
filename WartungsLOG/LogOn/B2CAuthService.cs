
// from "active-directort-b2c-Xamarin-native" MS Sample

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Xamarin.Forms;
using WartungsLOG.Select;

namespace WartungsLOG.LogOn
{
    public class B2CAuthService
    {
        private readonly IPublicClientApplication _pca;
        private readonly HttpClient _httpClient = new HttpClient();

        public IPublicClientApplication PCA { get => _pca;}

        public B2CAuthService(IParentWindowLocatorService windowLocatorService)
        {
            // default redirectURI; each platform specific project will have to override it with its own
            var builder = PublicClientApplicationBuilder.Create(B2CConstants.ClientID)
                .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
                .WithIosKeychainSecurityGroup(B2CConstants.IOSKeyChainGroup)
                .WithRedirectUri($"msal{B2CConstants.ClientID}://auth");
            // Android implementation is based on https://github.com/jamesmontemagno/CurrentActivityPlugin
            // iOS implementation would require to expose the current ViewControler - not currently implemented as it is not required
            // UWP does not require this

            //var windowLocatorService = DependencyService.Get<IParentWindowLocatorService>();

            if (windowLocatorService != null)
            {
                builder = builder.WithParentActivityOrWindow(() => windowLocatorService?.GetCurrentParentWindow());
            }

            _pca = builder.Build();
        }

        public async Task<UserContext> SignInAsync()
        {
            UserContext newContext;
            try
            {
                newContext = await getTokenSilent();
            }
            catch (MsalUiRequiredException)
            {
                newContext = await getTokenInteractive();
            }
            return newContext;
        }

        public async Task<UserContext> SignInOnAppearing()
        {
            UserContext newContext;
            try
            {
                newContext = await getTokenSilent();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"XXXX B2CAutService: SignInOnAppearing: {ex}");
                newContext = new UserContext();
                newContext.IsLoggedOn = false;
            }
            return newContext;
        }


        private async Task<UserContext> getTokenSilent()
        {
            IEnumerable<IAccount> accounts = await _pca.GetAccountsAsync(B2CConstants.PolicySignUpSignIn);
            AuthenticationResult authResult = await _pca.AcquireTokenSilent(B2CConstants.Scopes, accounts.FirstOrDefault())
               .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
               .ExecuteAsync();
            var newContext = UpdateUserInfo(authResult);
            return newContext;
        }

        private async Task<UserContext> getTokenInteractive()
        {
            AuthenticationResult authResult = await _pca.AcquireTokenInteractive(B2CConstants.Scopes)
                .ExecuteAsync();
            IEnumerable<IAccount> accounts = await _pca.GetAccountsAsync(B2CConstants.PolicySignUpSignIn);

            Debug.WriteLine("XXXX B2CAuthService: scopes #: " + authResult.Scopes.Count());
            foreach ( var s in authResult.Scopes)
            {
                Debug.WriteLine("XXXX B2CAuthService: scopes: " + s);

            }

            if (accounts.Any())
            {
                var newContext = UpdateUserInfo(authResult);
                return newContext;
            }
            else
            {
                Debug.WriteLine("XXXX B2CAuthService: in getTokenInteractive: DID NOT GET ACCOUNT");
                return new UserContext { IsLoggedOn = false, Name = "not logged in", Email = "noemail", UserIdentifier = "0000_id" };
            }
        }
        public UserContext UpdateUserInfo(AuthenticationResult authResult)
        {
            Debug.WriteLine("XXXX B2CAuthService: in updateuserinfo");
            var newContext = new UserContext();
            newContext.UserIdentifier = authResult.Account.HomeAccountId.ObjectId;
            newContext.Name = authResult.ClaimsPrincipal.FindFirst("name")?.Value;
            newContext.Email = authResult.ClaimsPrincipal.FindFirst("emails")?.Value;
            newContext.IsLoggedOn = true;
            return newContext;
        }

        public async Task<UserContext> SignOutAsync()
        {
            IEnumerable<IAccount> accounts = await _pca.GetAccountsAsync();
            while (accounts.Any())
            {
                await _pca.RemoveAsync(accounts.FirstOrDefault()); // .Remove auth
                accounts = await _pca.GetAccountsAsync();
            }
            var signedOutContext = new UserContext();
            signedOutContext.IsLoggedOn = false;
            return signedOutContext;
        }

    }
}
