using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using WartungsLOG.Select;
using WartungsLOG.ServiceHistory;
using WartungsLOG.Service;
using WartungsLOG.ServicePicture;

namespace WartungsLOG.Data
{
    public interface IDataService
    {

        //
        // Vehicle
        //

        Task<List<Vehicle>> GetVehiclesAsync(); // get all vehicles
        Task AddVehicle(Vehicle v);
        Task DeleteVehicleEntry(Vehicle v);


        //
        // ServiceHistory
        //

        Task<List<ServiceHistoryRecord>> GetServiceHistoryRecordsAsync(string ID); // get service history for vehicle with ID
        Task AddServiceHistoryEntry(ServiceHistoryRecord shr);
        Task DeleteServiceHistoryEntry(ServiceHistoryRecord shr);

        //
        // Service
        //

        Task<List<ServiceRecord>> GetServiceRecordAsync(string serviceID); // get service record for vehicle & history-entry
        Task AddServiceEntry(ServiceRecord sr);
        Task DeleteServiceEntry(ServiceRecord sr);

        //
        // Service Picture
        //

        Task<ServicePictureRecord> GetServicePictureAsync(string picID); // get pictures which belong to history-entry

        
        
    }
}
