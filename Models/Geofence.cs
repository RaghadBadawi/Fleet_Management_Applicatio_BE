using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TProject.Models
{
    public class Geofence
    {
        public int GeofenceID { get; set; }
        public string GeofenceType { get; set; }
        public long AddedDate { get; set; } 
        public string StrokeColor { get; set; }
        public double StrokeOpacity { get; set; }
        public double StrokeWeight { get; set; }
        public string FillColor { get; set; }
        public double FillOpacity { get; set; }
        
    }
}
