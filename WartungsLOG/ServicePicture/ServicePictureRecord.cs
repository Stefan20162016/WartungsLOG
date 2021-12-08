using System;
using System.Collections.Generic;
using System.Text;

namespace WartungsLOG.ServicePicture
{
    public class ServicePictureRecord
    {
        public string ServicePictureID { get; set; } 

        public string RefServiceID { get; set; } // set to ServiceRecord ID to indicate backward reference

        public string Description { get; set; }

        public byte[] ImageData { get; set; } // big picture

    }
}
