using System;
using System.Collections.Generic;
using System.Text;

namespace WartungsLOG.ServiceHistory
{
    public class ServiceHistoryRecord
    {
        public string ServiceHistoryID { get; set; }
        public string RefVehicleID { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public int Kilometerstand { get; set; } // service done at Kilomterstand
        //public int VehicleID { get; set; }
         
        public byte[] ImageThumbData { get; set; } // thumbnail for one Service History Entry
    }
}
