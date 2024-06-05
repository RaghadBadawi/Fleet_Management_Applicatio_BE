using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TProject.Models
{
    public class CircleGeofence
    {
        public int ID { get; set; }
        public long GeofenceID { get; set; }
        public long Radius { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        
    }
}
