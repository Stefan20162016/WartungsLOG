using System;
using System.Collections.Generic;
using System.Text;

namespace WartungsLOG.Service
{
    public class ServiceRecord
    {
        public string ServiceID { get; set; } // service entry ID

        public string RefServiceHistoryID { get; set; }

        public string Description { get; set; } // description for service entry
        public int HowManyPics { get; set; }
        public string DescriptionForPics { get; set; } // description for pictures
        
        
        public byte[] ImageThumbData { get; set; } // small thumbnail (big pic in ServicePictureRecord.cs)

    }
}
