using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using WartungsLOG.Core;
using WartungsLOG.Data;
using WartungsLOG.Select;
using WartungsLOG.ServiceHistory;
using WartungsLOG.ServicePicture;
using Xamarin.Forms;
using System.Web;
using System.Diagnostics;


namespace WartungsLOG.Service
{
    [QueryProperty(nameof(VehicleIDProperty), "paraVehicleID")] // name, queryID
    [QueryProperty(nameof(ServiceHistoryRecordIDProperty), "paraServiceHistoryRecordID")]
    [QueryProperty(nameof(DatumProperty), "paraDatum")]
    [QueryProperty(nameof(KmProperty), "paraKm")]
    [QueryProperty(nameof(BeschreibungProperty), "paraBeschreibung")]
    
    public class ServiceViewModel : BaseViewModel       //, IQueryAttributable
    {
        /*
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            string paraVehicleID = HttpUtility.UrlDecode(query["paraVehicleID"]);
            string paraHistoryID = HttpUtility.UrlDecode(query["paraHistoryRecordID"]);
            if(int.TryParse(paraVehicleID, out int x1))
            {
                VehicleIDProperty = x1;
            }
            if (int.TryParse(paraHistoryID, out int x2))
            {
                ServiceHistoryRecordIDProperty = x2;
            }
        }
        */
        private readonly IDataService _dataService;
        private readonly INavigationService _navService;

        //private ServiceHistoryRecord _shr;
        private string _vID;
        private string _shrID;
        private ServiceRecord _sr;
        private string _datum, _description;
        private int _km;

        public ObservableCollection<ServiceRecord> ServiceCollectionProperty { get; } = new ObservableCollection<ServiceRecord>();

        public ICommand NavigateToServicePictureCommand { get; }
        public ICommand NavigateToAddEntryCommand { get; }
        public ICommand DeleteServiceCommand { get; }
        public ICommand EditServiceCommand { get; }

        public string ServiceHistoryRecordIDProperty { get => _shrID; set => SetProperty(ref _shrID, value); }
        public string VehicleIDProperty { get => _vID; set => SetProperty(ref _vID, value); }
        public string DatumProperty { get => _datum; set => SetProperty(ref _datum, value); }
        public int KmProperty { get => _km ; set => SetProperty(ref _km, value); }

        public string BeschreibungProperty { get => _description; set => SetProperty(ref _description, value); }
        
        public ServiceViewModel(IDataService data, INavigationService nav)
        {
            _dataService = data; 
            _navService = nav;
            Title = "Service Page";
            NavigateToServicePictureCommand = new Command(NavigateToServicePicture);
            NavigateToAddEntryCommand = new Command(NavigateToAddEntry);
            DeleteServiceCommand = new Command(DeleteService);
            EditServiceCommand = new Command(EditService);
        }

        private void EditService(object obj)
        {
            if (obj is ServiceRecord sr)
            {
                Debug.WriteLine($"XXXX ServiceViewModel: EditService: id: {sr.ServiceID} descr: {sr.Description} ");
            }
            else { Debug.WriteLine($"XXXX ServiceViewModel: EditService empty"); }
        }

        private async void DeleteService(object obj)
        {
            Debug.WriteLine("XXX: DeleteService");
            if (obj is ServiceRecord sr) {
                Debug.WriteLine($"XXX: DeleteService: ServiceRecord id: {sr.ServiceID} descr: {sr.Description}  ");
                await _dataService.DeleteServiceEntry(sr);

            }

            await LoadData(); // reload collectionview
            
        }

        private async void NavigateToServicePicture(object arg)
        {
            if(arg is ServiceRecord sr)
            {
                await _navService.GoToAsync($"{nameof(ServicePicturePage)}?paraServiceID={sr.ServiceID}");
            }
        }

        private async void NavigateToAddEntry(object arg)
        {
           
                await _navService.GoToAsync($"{nameof(AddDetailEntryPage)}?paraServiceHistoryIDProperty={ServiceHistoryRecordIDProperty}");
            
        }


        private async Task LoadData()
        {
            try
            {
                IsBusy = true;
                ServiceCollectionProperty.Clear();

                Debug.WriteLine("XXXX: in ServiceViewModel calling dataservice.GetServiceRecord SHR: " + ServiceHistoryRecordIDProperty);

                var x = await _dataService.GetServiceRecordAsync(ServiceHistoryRecordIDProperty);

                foreach (var v in x)
                {
                    //Debug.WriteLine($"XXXX: in ServiceViewModel add to ServiceCollection: {v.ServiceID} {v.Description}");
                    Logging.logMe($"add to ServiceCollection: {v.ServiceID} {v.Description}");
                    
                    ServiceCollectionProperty.Add(v);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"XXXX: ServiceViewModel LoadData() Exception: {ex}");
            }
            finally
            { //await Task.Delay(TimeSpan.FromMilliseconds(-1)); 
                IsBusy = false;
            }
        }

        public override async Task Initialize()
        {
            //var x = await _dataService.GetServiceRecordAsync(1);
            await LoadData();
            await base.Initialize();
        }



        
    }
}
