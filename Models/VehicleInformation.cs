using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TProject.Models
{
    public class VehicleInformation
    {
        public long ID { get; set; }
        public long VehicleID { get; set; }
        public long DriverID { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public long PurchaseDate { get; set; } 
        
    }
}
