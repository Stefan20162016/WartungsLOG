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
using WartungsLOG.Service;
using Xamarin.Forms;
using System.Diagnostics;
using Xamarin.Essentials;

namespace WartungsLOG.ServiceHistory
{
    [QueryProperty(nameof(VehicleID), nameof(VehicleID))]
    [QueryProperty(nameof(VehicleKennzeichen), nameof(VehicleKennzeichen))]
    [QueryProperty(nameof(VehicleDescription), nameof(VehicleDescription))]

    public class ServiceHistoryViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navService;
        private Vehicle _vehicle;
        private string _vehicle_kennzeichen, _vehicle_description;
        private string _vehicleID;

        public static Vehicle _vehicle_static;

        public ObservableCollection<ServiceHistoryRecord> ServiceHistoryRecordCollection { get; } = new ObservableCollection<ServiceHistoryRecord>();

        public ICommand NavigateToServiceRecordDetailCommand { get; }
        public ICommand NavigateToAddEntryCommand { get; }        
        public ICommand DeleteServiceHistoryCommand { get; }

        public Vehicle StaticVehicleProperty { get => _vehicle_static; set => SetProperty( ref _vehicle_static, value);}

        public Vehicle VehicleProperty { get => _vehicle; set => SetProperty(ref _vehicle, value); }

        public string VehicleKennzeichen { get => _vehicle_kennzeichen; set => _vehicle_kennzeichen = value; }
        public string VehicleDescription { get => _vehicle_description; set => _vehicle_description = value; }
        public string VehicleID { get => _vehicleID; set => SetProperty(ref _vehicleID, value); }

        public ServiceHistoryViewModel(IDataService dataSvc, INavigationService navSvc)
        {
            _dataService = dataSvc;
            _navService = navSvc;
            Title = "Service History Records";
            NavigateToServiceRecordDetailCommand = new Command(NavigateToServiceRecordDetail);
            NavigateToAddEntryCommand = new Command(NavigateToAddEntry);
            DeleteServiceHistoryCommand = new Command(DeleteServiceHistory);
        }

        private async void DeleteServiceHistory(object obj)
        {
            
            Debug.WriteLine("XXX: DeleteServiceHistory");
            if (obj is ServiceHistoryRecord shr)
            {

                var result = await Shell.Current.DisplayAlert(
            "Delete History",
            $"alle Untereinträge werden gelöscht? ",
            "Okay", "Nein!!!!");
                if (result)
                {
                    Debug.WriteLine($"XXX: DeleteServiceHistory: _dataService.DeleteServiceHistoryEntry(ServiceHistoryRecord id: {shr.ServiceHistoryID} descr: {shr.Description}  ) ");
                    await _dataService.DeleteServiceHistoryEntry(shr);
                }
            }

            await LoadDataAsync(); // reload collectionview
        }

        private async void NavigateToServiceRecordDetail(object arg)
        {
            if(arg is ServiceHistoryRecord shr)
            {
                await _navService.GoToAsync($"{nameof(ServicePage)}?paraServiceHistoryRecordID={shr.ServiceHistoryID}&paraVehicleID={VehicleID}&paraDatum={shr.Date}&paraKm={shr.Kilometerstand}&paraBeschreibung={shr.Description}");
            }
        }
        private async void NavigateToAddEntry(object arg)
        {
            Debug.WriteLine("XXXX ServiceHistoryViewModel: in navtoAddEnry SHVM:" + StaticVehicleProperty.ID);
            //await _navService.GoToAsync($"{nameof(AddServiceEntryPage)}?paraVehicleIDProperty={StaticVehicleProperty.ID}");   

            AddServiceEntryViewModel._static_vehicle = StaticVehicleProperty;

            await _navService.GoToAsync($"{nameof(AddServiceEntryPage)}");
        }
        
        private async Task LoadDataAsync()
        {
            // CHANGE THIS HERE !!!! use routing parameters

            //VehicleProperty = (await _dataService.GetVehiclesAsync()).First(c => c.ID == VehicleID); // delete and pass Kennzeichen in SelectViewModel
            //VehicleProperty = new Vehicle { Description = VehicleDescription, Kennzeichen = VehicleKennzeichen };

            try
            {
                IsBusy = true;
                //await Task.Delay(9000);

                ServiceHistoryRecordCollection.Clear();
                Debug.WriteLine("XXXX ServiceHistoryViewModel: calling dataservice.GetServiceHistoryRecords with VID: " + VehicleID);
                var shrList_for_vehicleID = await _dataService.GetServiceHistoryRecordsAsync(VehicleID);

                foreach (var shr in shrList_for_vehicleID)
                {
                    ServiceHistoryRecordCollection.Add(shr);
                    Debug.WriteLine($"XXXX: filling ServiceHistoryRecordCollection: {shr.ServiceHistoryID} {shr.Description}");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"XXXX: ServiceHistoryViewModel LoadDataAsync: ex: {ex}");
            }
            finally { IsBusy = false; }
        }

        public override async Task Initialize()
        {
            await LoadDataAsync();

            //await base.Initialize();

            Debug.WriteLine($"XXXX ServiceHistoryViewModel: ServiceHistoryViewModel StaticVehicleProperty: {StaticVehicleProperty.ID} {StaticVehicleProperty.Description}  {StaticVehicleProperty.Kennzeichen}");

        }
        
    }
}
