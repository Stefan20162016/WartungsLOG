using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text;
using Xamarin.Forms;

using WartungsLOG.Core;
using WartungsLOG.Data;
using WartungsLOG.ServiceHistory;
using System.Diagnostics;
using Xamarin.Essentials;

namespace WartungsLOG.Select
{
    public class SelectViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navService;
        public ICommand NavigateToVehicleCommand { get; }
        public ICommand NavigateToAddVehicleCommand { get; }
        public ICommand DeleteVehicleCommand { get; }

        public ObservableCollection<Vehicle> VehiclesCollection { get; } = new ObservableCollection<Vehicle>();

        public SelectViewModel(IDataService dataSvc, INavigationService navSvc)
        {
            _dataService = dataSvc;
            _navService = navSvc;
            NavigateToVehicleCommand = new Command(NavigateToVehicle);
            NavigateToAddVehicleCommand = new Command(NavigateToAddVehicle);
            DeleteVehicleCommand = new Command(DeleteVehicle);
            Title = "Select Vehicle";
        }

        private async void DeleteVehicle(object obj)
        {
            Debug.WriteLine("XXX: DeleteVehicle");
            if (obj is Vehicle veh)
            {

                var result = await Shell.Current.DisplayAlert(
            "Delete Vehicle",
            $"alle Untereinträge werden gelöscht!",
            "Okay", "Nein!!!!");
                if (result)
                {

                    Debug.WriteLine($"XXX: DeleteVehicle: _dataService.DeleteVehicleEntry(Vehicle id: {veh.ID} descr: {veh.Description}  ) ");
                    await _dataService.DeleteVehicleEntry(veh);
                }

            }

            await LoadData(); // reload collectionview
        }

        private async void NavigateToVehicle(object arg)
        {
            Debug.WriteLine("XXXX SelectVieModel: in navi to vehicle");
            if(arg is Vehicle v)
            {
                ServiceHistoryViewModel._vehicle_static = v;
                await _navService.GoToAsync($"{nameof(ServiceHistoryPage)}?VehicleID={v.ID}&VehicleDescription={v.Description}&VehicleKennzeichen={v.Kennzeichen}"); // pass Kennzeichen
            }
        }

        private async void NavigateToAddVehicle(object arg)
        {
            Debug.WriteLine("XXXX SelectVieModel: in navi to vehicleAdd");
            await _navService.GoToAsync($"{nameof(AddVehiclePage)}");
        }

        private async Task LoadData()
        {
            try
            {
                IsBusy = true;
                VehiclesCollection.Clear();
                var tmp_vehicles = await _dataService.GetVehiclesAsync();
                foreach(var v in tmp_vehicles)
                {
                    Debug.WriteLine($"XXXX SelectVieModel: in SelectViewModel: got vehicles: ID: {v.ID} Kenn: {v.Kennzeichen} Descr: {v.Description} ");
                    VehiclesCollection.Add(v);
                }
            }
            finally
            {
                //await Task.Delay(TimeSpan.FromMilliseconds(-1));
                IsBusy = false;
            }
        }

        public override async Task Initialize()
        {
            await LoadData();
        }


       
    }
}
