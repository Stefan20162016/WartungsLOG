using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Logging;
using WartungsLOG.Select;
using WartungsLOG.ServiceHistory;
using WartungsLOG.Service;
using WartungsLOG.ServicePicture;

namespace WartungsLOG.Data
{
    public class DummyDataService : IDataService
    {
#pragma warning disable IDE0044 // Add readonly modifier
        ILogger<DummyDataService> logger;
#pragma warning restore IDE0044 // Add readonly modifier
        private readonly List<Vehicle> _vehicles;
        private readonly List<List<ServiceHistoryRecord>> _serviceHistoryRecord;
        private readonly List<ServiceRecord> _serviceRecordDetail;
        private readonly ServicePictureRecord _servicePictures;

        public DummyDataService(ILogger<DummyDataService> logger )
        {
            this.logger = logger;

            var stream = typeof(DummyDataService).Assembly.GetManifestResourceStream("WartungsLOG.Bilder.car_thumb.png");
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            byte[] image_data = ms.ToArray();
            
            var stream2 = typeof(DummyDataService).Assembly.GetManifestResourceStream("WartungsLOG.Bilder.car_thumb_big.png");
            var ms2 = new MemoryStream();
            stream2.CopyTo(ms2);
            
            byte[] image_data_big = ms2.ToArray();

            _vehicles = new List<Vehicle> {
                new Vehicle{ ID = "1", Kennzeichen = "TOL-XX 668 dummy", Description="BMW e36 Cabrio 318i, 115PS, 2 tuerig ",
                    ImageThumbData = image_data
                    },
                new Vehicle{ID = "2", Kennzeichen = "TOL-HU 77 dummy", Description="Ford Fiesta V 60 PS, 3 tuerig", ImageThumbData = image_data},
                new Vehicle{ ID = "3", Kennzeichen = "WOR-XX 668 dummy", Description="Ford Fiesta VI 82PS, 5 tuerig", ImageThumbData = image_data}
            };
            // latest change remove VehicleID =1 =2 =3 from initialization
            _serviceHistoryRecord = new List<List<ServiceHistoryRecord>>
            {
                new List<ServiceHistoryRecord> {
                    new ServiceHistoryRecord { ServiceHistoryID = "1", Date = "1.4.2021", Description = "Oelwechsel", Kilometerstand = 200123,  ImageThumbData=image_data },
                    new ServiceHistoryRecord { ServiceHistoryID = "2", Date = "17.5.2021", Description = "Riemenspanner", Kilometerstand = 200267,  ImageThumbData=image_data },
                    new ServiceHistoryRecord { ServiceHistoryID = "3", Date = "2.6.2021", Description = "Servobehaelter", Kilometerstand = 200301,  ImageThumbData=image_data }
                },
                new List<ServiceHistoryRecord>
                {
                    new ServiceHistoryRecord { ServiceHistoryID = "1", Date = "8.4.2021", Description = "Bremsscheiben+Belaege", Kilometerstand = 133123, ImageThumbData=image_data },
                    new ServiceHistoryRecord { ServiceHistoryID = "2", Date = "24.5.2021", Description = "Radlager links", Kilometerstand = 134567, ImageThumbData=image_data },
                    new ServiceHistoryRecord { ServiceHistoryID = "3", Date = "11.11.2021", Description = "Radlager rechts", Kilometerstand = 149999, ImageThumbData=image_data }
                },
                new List<ServiceHistoryRecord>
                {
                    new ServiceHistoryRecord { ServiceHistoryID = "1", Date = "11.11.2020", Description = "Federn hinten", Kilometerstand = 57123, },
                    new ServiceHistoryRecord { ServiceHistoryID = "2", Date = "1.2.2021", Description = "Bremscheiben + Belaege", Kilometerstand = 62400}
                }

            };

            var tmpList = new List<byte[]> { image_data, image_data };

            _serviceRecordDetail = new List<ServiceRecord> {
                new ServiceRecord { ServiceID = "123", Description = "more radlager",  ImageThumbData = image_data } ,
                new ServiceRecord { ServiceID = "555", Description = "Windschutzscheibe", ImageThumbData = image_data } ,


            };

            _servicePictures = new ServicePictureRecord { ServicePictureID = "1", Description="this belongs to ID1 with big pic", ImageData=image_data_big };

        }
        public Task<List<Vehicle>> GetVehiclesAsync()
        {
            logger.LogCritical("XXXX: DI logger in DummyDataService.GetVehiclesAsync();");
            return Task.FromResult(_vehicles);
        }

        public Task<List<ServiceHistoryRecord>> GetServiceHistoryRecordsAsync(string vehicleID)
        {
            logger.LogInformation("XXXX: DI logger in DummyDataSerivce.GetServiceHistoryRecordsAsync();");
            int intvehicleID;
            int.TryParse(vehicleID, out intvehicleID);
            var tmp = _serviceHistoryRecord[intvehicleID-1]; // return List of ServiceHistoryRecords for vehicleID

            /*var tmp = new List<ServiceHistoryRecord> {
                    new ServiceHistoryRecord { ID = 1, Date = "1.4.2021", Description = "Oelwechsel", Kilometerstand = 200123, VehicleID = 1 },
                    new ServiceHistoryRecord { ID = 2, Date = "17.5.2021", Description = "Riemenspanner", Kilometerstand = 200267, VehicleID = 1 },
                    new ServiceHistoryRecord { ID = 2, Date = "2.6.2021", Description = "Servobehaelter", Kilometerstand = 200301, VehicleID = 1 }
                };*/
            return Task.FromResult(tmp);
        }
         
        public Task<List<ServiceRecord>> GetServiceRecordAsync(string serviceHistoryID)
        {
            //int intserviceID;
            //int.TryParse(serviceHistoryID, out intserviceID);

            logger.LogInformation("XXXX: DI logger in DummyDataSerivce.GetServiceRecordAsync();");

            return Task.FromResult(_serviceRecordDetail);
        }

        public Task<ServicePictureRecord> GetServicePictureAsync(string picID)
        {
            logger.LogInformation("XXXX: DI logger in DummyDataSerivce.GetServicePicturesAsync for ID: " + picID);
            return Task.FromResult(_servicePictures);
        }

        public Task AddVehicle(Vehicle v)
        {
            throw new NotImplementedException();
        }

        public Task AddServiceHistoryEntry(ServiceHistoryRecord shr)
        {
            throw new NotImplementedException();
        }

        public Task AddServiceEntry(ServiceRecord sr)
        {
            throw new NotImplementedException();
        }
        public Task DeleteServiceEntry(ServiceRecord sr)
        {
            throw new NotImplementedException();
        }

        public Task DeleteServiceHistoryEntry(ServiceHistoryRecord shr)
        {
            throw new NotImplementedException();
        }

        public Task DeleteVehicleEntry(Vehicle v)
        {
            throw new NotImplementedException();
        }
    }
}

