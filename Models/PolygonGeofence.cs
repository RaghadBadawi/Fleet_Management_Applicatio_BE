using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TProject.Models
{
    public class PolygonGeofence
    {
        public int ID { get; set; }
        public long GeofenceID { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
      
    }
}
