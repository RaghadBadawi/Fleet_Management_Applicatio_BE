using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TProject.Models
{

    public class RouteHistory
    {
        public long RouteHistoryID { get; set; }
        public long VehicleID { get; set; }
        public int VehicleDirection { get; set; }
        public char Status { get; set; }
        public string VehicleSpeed { get; set; }
        public long   Epoch { get; set; } 
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
    

}