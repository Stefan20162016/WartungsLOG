using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WartungsLOG.Select;
using WartungsLOG.Service;
using WartungsLOG.ServiceHistory;
using WartungsLOG.ServicePicture;
using WartungsLOG.LogOn;
using System.IO;
using System.Linq;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace WartungsLOG.Data
{
    public class RealDataService : IDataService
    {
        private readonly B2CAuthService _b2c;
        private readonly HttpClient _httpClient = new HttpClient();
        //private readonly string ApiBaseAddress = "https://todolistserviceappservice27.azurewebsites.net/";
        private readonly string ApiBaseAddress = "https://wartungslogwebapi.azurewebsites.net/";

        private readonly List<Vehicle> _vehicles;
        public RealDataService(B2CAuthService b2c)
        {
            _b2c = b2c;        
        }

        //
        //
        //          V E H I C L E S
        //
        //
        public async Task AddVehicle(Vehicle v)
        {
            string apiTailAddress = "api/vehicles";
            var accounts = await _b2c.PCA.GetAccountsAsync();
            if (!accounts.Any())
            {
                return;
            }

            AuthenticationResult result = null;
            try
            {
                result = await _b2c.PCA.AcquireTokenSilent(B2CConstants.Scopes, accounts.FirstOrDefault())
                    .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
                    .ExecuteAsync()
                    .ConfigureAwait(false);
            }
            catch (MsalException ex)
            {
                // An unexpected error occurred.
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                    Debug.WriteLine("XXXX Realdata: AddVehicle MsalException: " + message);
                }
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            string json = JsonConvert.SerializeObject(v);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(ApiBaseAddress + apiTailAddress, content);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("XXXX: POST Vehicles was successful: " + v.Kennzeichen + " descr: " + v.Description);
            }
            else
            {
                Debug.WriteLine("XXXX: POST Vehicles failed: " + response);
            }
        }


        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            string apiTailAddress = "api/vehicles";
            var emptyVehicleList = new List<Vehicle> { new Vehicle { ID = "null", Kennzeichen = "no Vehicles", Description = "Log-In or WebAPI call failed" } };
            var accounts = await _b2c.PCA.GetAccountsAsync();
            if (!accounts.Any())
            {
                return emptyVehicleList;
            }

            AuthenticationResult result = null;
            try
            {
                result = await _b2c.PCA.AcquireTokenSilent(B2CConstants.Scopes, accounts.FirstOrDefault())
                    .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
                    .ExecuteAsync()
                    .ConfigureAwait(false);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

                
                HttpResponseMessage response = await _httpClient.GetAsync(ApiBaseAddress + apiTailAddress);

                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    List<Vehicle> vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(s);

                    return vehicles;
                    
                }
                else
                {
                    Debug.WriteLine("XXXX Realdata: GetVehicle Error: " + response);
                    return emptyVehicleList;
                }
                //return await Task.FromResult(_vehicles);
            }
            catch (MsalUiRequiredException)
            {
                Debug.WriteLine("XXXX Please sign in to view list");
                // MAYBE CALL Interactive Login from here
            }
            catch (MsalException ex)
            {
                // An unexpected error occurred.
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                }
                Debug.WriteLine("XXXX Realdata: GetVehiclesAsync MsalException: " + message);
            }

            return emptyVehicleList;
        }

        public async Task DeleteVehicleEntry(Vehicle veh)
        {
            string apiTailAddress = "api/vehicles";
            var accounts = await _b2c.PCA.GetAccountsAsync();
            if (!accounts.Any())
            {
                return;
            }

            AuthenticationResult result = null;
            try
            {
                result = await _b2c.PCA.AcquireTokenSilent(B2CConstants.Scopes, accounts.FirstOrDefault())
                    .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
                    .ExecuteAsync()
                    .ConfigureAwait(false);
            }
            catch (MsalException ex)
            {
                // An unexpected error occurred.
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                    Debug.WriteLine("XXXX Realdata: DeleteVehicleEntry MsalException: " + message);
                }
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            //string json = JsonConvert.SerializeObject(sr);
            //StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            //HttpResponseMessage response = await _httpClient.DeleteAsync(ApiBaseAddress + apiTailAddress + "?sid=" + sr.ServiceID);
            HttpResponseMessage response = await _httpClient.DeleteAsync(ApiBaseAddress + apiTailAddress + "/" + veh.ID);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("XXXX: DELETE DeleteVehicleEntry was successful ref: " + veh.ID + " descr: " + veh.Description);
            }
            else
            {
                Debug.WriteLine("XXXX: DELETE DeleteVehicleEntry failed ref: " + veh.ID + " descr: " + veh.Description + response);
            }

        }


        //
        //
        //          S E R V I C E   H I S T O R Y    
        //
        //

        public async Task AddServiceHistoryEntry(ServiceHistoryRecord shr)
        {
            string apiTailAddress = "api/servicehistory";
            var accounts = await _b2c.PCA.GetAccountsAsync();
            if (!accounts.Any())
            {
                return;
            }

            AuthenticationResult result = null;
            try
            {
                result = await _b2c.PCA.AcquireTokenSilent(B2CConstants.Scopes, accounts.FirstOrDefault())
                    .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
                    .ExecuteAsync()
                    .ConfigureAwait(false);
            }
            catch (MsalException ex)
            {
                // An unexpected error occurred.
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                    Debug.WriteLine("XXXX Realdata: AddServiceHistoryEntry MsalException: " + message);
                }
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            string json = JsonConvert.SerializeObject(shr);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(ApiBaseAddress + apiTailAddress, content);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("XXXX: POST ServiceHistoryRecord was successful ref: " + shr.RefVehicleID+ " descr: " + shr.Description);
            }
            else
            {
                Debug.WriteLine("XXXX: POST ServiceHistoryRecord failed ref: " + shr.RefVehicleID + " descr: " + shr.Description + response);
            }

        }

        public async Task<List<ServiceHistoryRecord>> GetServiceHistoryRecordsAsync(string ID)
        {
            var emptySHRList = new List<ServiceHistoryRecord> { new ServiceHistoryRecord { Date = "0.0.0000", Description = "ERROR or empty", Kilometerstand = -1 } };

            string apiTailAddress = "api/servicehistory";

            var accounts = await _b2c.PCA.GetAccountsAsync();
            if (!accounts.Any())
            {
                return emptySHRList;
            }

            AuthenticationResult result = null;
            try
            {
                result = await _b2c.PCA.AcquireTokenSilent(B2CConstants.Scopes, accounts.FirstOrDefault())
                    .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
                    .ExecuteAsync()
                    .ConfigureAwait(false);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);


                HttpResponseMessage response = await _httpClient.GetAsync(ApiBaseAddress + apiTailAddress + "?vid=" + ID);

                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    List<ServiceHistoryRecord> shrlist = JsonConvert.DeserializeObject<List<ServiceHistoryRecord>>(s);

                    return shrlist;

                }
                else
                {
                    Debug.WriteLine("XXXX Realdata: GetVehicle Error: " + response);
                    return emptySHRList;
                }
                //return await Task.FromResult(_vehicles);
            }
            catch (MsalUiRequiredException)
            {
                Debug.WriteLine("XXXX Please sign in to view your To-Do list");
                // MAYBE CALL Interactive Login from here
            }
            catch (MsalException ex)
            {
                // An unexpected error occurred.
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                }
                Debug.WriteLine("XXXX Realdata: GetServiceHistoryRecordAsync MsalException: " + message);
            }
            return emptySHRList;
        }


        public async Task DeleteServiceHistoryEntry(ServiceHistoryRecord shr)
        {
            string apiTailAddress = "api/servicehistory";
            var accounts = await _b2c.PCA.GetAccountsAsync();
            if (!accounts.Any())
            {
                return;
            }

            AuthenticationResult result = null;
            try
            {
                result = await _b2c.PCA.AcquireTokenSilent(B2CConstants.Scopes, accounts.FirstOrDefault())
                    .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
                    .ExecuteAsync()
                    .ConfigureAwait(false);
            }
            catch (MsalException ex)
            {
                // An unexpected error occurred.
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                    Debug.WriteLine("XXXX Realdata: DeleteServiceHistoryEntry MsalException: " + message);
                }
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            //string json = JsonConvert.SerializeObject(sr);
            //StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            //HttpResponseMessage response = await _httpClient.DeleteAsync(ApiBaseAddress + apiTailAddress + "?sid=" + sr.ServiceID);
            HttpResponseMessage response = await _httpClient.DeleteAsync(ApiBaseAddress + apiTailAddress + "/" + shr.ServiceHistoryID);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("XXXX: DELETE DeleteServiceHistoryEntry was successful ref: " + shr.ServiceHistoryID + " descr: " + shr.Description);
            }
            else
            {
                Debug.WriteLine("XXXX: DELETE DeleteServiceHistoryEntry failed ref: " + shr.ServiceHistoryID + " descr: " + shr.Description + response);
            }
        }

        //
        // 
        //          S E R V I C E
        //
        //

        public async Task<List<ServiceRecord>> GetServiceRecordAsync(string serviceID)
        {
            string apiTailAddress = "api/service";
            var emptyVehicleList = new List<ServiceRecord> { new ServiceRecord { ServiceID = "null", Description = "ERROR: log-in or WebAPI call failed" } };

            var accounts = await _b2c.PCA.GetAccountsAsync();
            if (!accounts.Any())
            {
                return emptyVehicleList;
            }

            AuthenticationResult result = null;
            try
            {
                result = await _b2c.PCA.AcquireTokenSilent(B2CConstants.Scopes, accounts.FirstOrDefault())
                    .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
                    .ExecuteAsync()
                    .ConfigureAwait(false);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);


                HttpResponseMessage response = await _httpClient.GetAsync(ApiBaseAddress + apiTailAddress + "?sid=" + serviceID);

                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    List<ServiceRecord> services = JsonConvert.DeserializeObject<List<ServiceRecord>>(s);

                    return services; //return await Task.FromResult(services);

                }
                else
                {
                    Debug.WriteLine("XXXX Realdata: GetServiceRecord Error: " + response + " " + response.StatusCode);
                    return emptyVehicleList;
                }

            }
            catch (MsalUiRequiredException)
            {
                Debug.WriteLine("XXXX Please sign in to view list");
                // MAYBE CALL Interactive Login from here
            }
            catch (MsalException ex)
            {
                // An unexpected error occurred.
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                }
                Debug.WriteLine("XXXX: Realdata: MsalException GetServiceRecordAsync" + message);
                //return null;
            }

            //return new List<Vehicle> { new Vehicle { ID = "null", Kennzeichen = "no Vehicles", Description = "empty or Log-In" } };
            return emptyVehicleList;
        }

        public async Task AddServiceEntry(ServiceRecord sr)
        {
            string apiTailAddress = "api/service";
            var accounts = await _b2c.PCA.GetAccountsAsync();
            if (!accounts.Any())
            {
                return;
            }

            AuthenticationResult result = null;
            try
            {
                result = await _b2c.PCA.AcquireTokenSilent(B2CConstants.Scopes, accounts.FirstOrDefault())
                    .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
                    .ExecuteAsync()
                    .ConfigureAwait(false);
            }
            catch (MsalException ex)
            {
                // An unexpected error occurred.
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                    Debug.WriteLine("XXXX Realdata: AddServiceEntry MsalException: " + message);
                }
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            string json = JsonConvert.SerializeObject(sr);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(ApiBaseAddress + apiTailAddress, content);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("XXXX: POST ServiceRecord was successful ref: " + sr.RefServiceHistoryID + " descr: " + sr.Description);
            }
            else
            {
                Debug.WriteLine("XXXX: POST ServiceyRecord failed ref: " + sr.RefServiceHistoryID + " descr: " + sr.Description + response);
            }
        }

        public async Task DeleteServiceEntry(ServiceRecord sr)
        {
            string apiTailAddress = "api/service";
            var accounts = await _b2c.PCA.GetAccountsAsync();
            if (!accounts.Any())
            {
                return;
            }

            AuthenticationResult result = null;
            try
            {
                result = await _b2c.PCA.AcquireTokenSilent(B2CConstants.Scopes, accounts.FirstOrDefault())
                    .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
                    .ExecuteAsync()
                    .ConfigureAwait(false);
            }
            catch (MsalException ex)
            {
                // An unexpected error occurred.
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                    Debug.WriteLine("XXXX Realdata: DeleteServiceEntry MsalException: " + message);
                }
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            //HttpResponseMessage response = await _httpClient.DeleteAsync(ApiBaseAddress + apiTailAddress + "?sid=" + sr.ServiceID);

            HttpResponseMessage response = await _httpClient.DeleteAsync(ApiBaseAddress + apiTailAddress + "/" + sr.ServiceID); // the WebAPI deletes everything

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("XXXX: DELETE DeleteServiceEntry was successful ref: " + sr.ServiceID + " descr: " + sr.Description);
            }
            else
            {
                Debug.WriteLine("XXXX: DELETE DeleteServiceEntry failed ref: " + sr.ServiceID + " descr: " + sr.Description + response);
            }
        }

        //
        //
        //          S E R V I C E  P I C T U R E 
        //
        //

        public async Task<ServicePictureRecord> GetServicePictureAsync(string picID)
        {
            //var x = new ServicePictureRecord  { ServicePictureID = "hey"};
            //return Task.FromResult(x);
            string apiTailAddress = "api/servicepicture";
            var emptySPRecord = new ServicePictureRecord { Description = "ERROR: Log-In or WebAPI call failed" };

            var accounts = await _b2c.PCA.GetAccountsAsync();
            if (!accounts.Any())
            {
                return emptySPRecord;
            }

            AuthenticationResult result = null;
            try
            {
                result = await _b2c.PCA.AcquireTokenSilent(B2CConstants.Scopes, accounts.FirstOrDefault())
                    .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
                    .ExecuteAsync()
                    .ConfigureAwait(false);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);


                HttpResponseMessage response = await _httpClient.GetAsync(ApiBaseAddress + apiTailAddress + "?picID=" + picID);

                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServicePictureRecord spr = JsonConvert.DeserializeObject<ServicePictureRecord>(s);
                    return spr; 
                }
                else
                {
                    Debug.WriteLine("XXXX Realdata: GetServicePicture Error: " + response + " " + response.StatusCode);
                    return emptySPRecord;
                }

            }
            catch (MsalUiRequiredException)
            {
                Debug.WriteLine("XXXX Please sign in to view picture");
                // MAYBE CALL Interactive Login from here
            }
            catch (MsalException ex)
            {
                // An unexpected error occurred.
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                }
                Debug.WriteLine("XXXX: Realdata: MsalException GetServicePicutre" + message);
                //return null;
            }

            //return new List<Vehicle> { new Vehicle { ID = "null", Kennzeichen = "no Vehicles", Description = "empty or Log-In" } };
            return emptySPRecord;

        }


        

        
    }
}
