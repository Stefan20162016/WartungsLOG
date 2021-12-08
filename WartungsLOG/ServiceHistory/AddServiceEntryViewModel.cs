using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text;
using Xamarin.Forms;

using WartungsLOG.Select;
using WartungsLOG.Core;
using WartungsLOG.Data;
using WartungsLOG.ServiceHistory;
using System.Diagnostics;

namespace WartungsLOG.ServiceHistory
{
    [QueryProperty(nameof(VehicleIDProperty), "para" + nameof(VehicleIDProperty))]
    public class AddServiceEntryViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navService;
        private string _description, _datum;
        private int _km, _vid;
        public ICommand NavigateToSaveCommand { get; }


        public int KmProperty { get => _km; set => SetProperty(ref _km, value); }
        public string DescriptionProperty { get => _description; set => SetProperty(ref _description, value); }
        public string DatumProperty { get => _datum; set => SetProperty(ref _datum, value); }

     

        public int VehicleIDProperty { get => _vid; set => SetProperty(ref _vid, value); }

        public static Vehicle _static_vehicle;

        public Vehicle StaticVehicleProperty { get => _static_vehicle; set => SetProperty(ref _static_vehicle, value); }


        public AddServiceEntryViewModel(IDataService dataSvc, INavigationService navSvc)
        {
            _dataService = dataSvc;
            _navService = navSvc;
            NavigateToSaveCommand = new Command(NavigateAddServiceHistoryEntry);
            
            Title = "Add Service Entry";

            Debug.WriteLine($"XXXX: in AddServiceEntryViewModel init: static Vehicle: ID:{StaticVehicleProperty.ID} kenn:{StaticVehicleProperty.Kennzeichen}");

            System.DateTime d = System.DateTime.Now;
            Debug.WriteLine($"XXX: Datetime doppelpunkt {d:t}");
            string d2 = $"{d.Day}.{d.Month}.{d.Year}";
            DatumProperty = d2;
            Debug.WriteLine($"XXXX: in AddServiceEntryViewModel init: date: {d} dateprop : {DatumProperty}");


        }

        private async void NavigateAddServiceHistoryEntry(object arg)
        {
            // fill in ID in dataservice

            ServiceHistoryRecord shr = new ServiceHistoryRecord { Date = DatumProperty, Description = DescriptionProperty, Kilometerstand = KmProperty, RefVehicleID = StaticVehicleProperty.ID };

            await _dataService.AddServiceHistoryEntry(shr);

            //await _navService.GoToAsync($"{nameof(ServiceHistoryPage)}");

            await _navService.GoToAsync("..");




        }
    }
}
