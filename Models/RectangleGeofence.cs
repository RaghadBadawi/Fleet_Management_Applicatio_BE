using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TProject.Models
{
    public class RectangleGeofence
    {
        public int ID { get; set; }
        public long GeofenceID { get; set; }
        public int North { get; set; }
        public int East { get; set; }
        public int West { get; set; }
        public int South { get; set; }
        
    }
}
