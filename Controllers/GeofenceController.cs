using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using System.Web.Http;
using System.Net;
using System.Data;
using FPro;
using Npgsql;
using TProject.Models;
using System.Web.Http.Cors;

namespace TProject.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/geofence")]
    public class GeofenceController : ApiController
    {
        GVAR gvar = new GVAR();
        string connectionString = "Host=localhost;Port=5432;Database=RaghadBadawi_FMS;Username=postgres;Password=NEETisLIFE";

        public GeofenceController()
        {
            gvar.DicOfDic["Geofence"] = new ConcurrentDictionary<string, string>();
        }

        [HttpPost]
        [Route("getAll")]
        public IHttpActionResult GetAllGeofences([FromBody] GVAR gvar)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = "SELECT * FROM Geofences";
                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var geofences = new List<Dictionary<string, object>>();
                            while (reader.Read())
                            {
                                var geofenceData = new Dictionary<string, object>
                                {
                                         { "GeofenceID", reader.GetInt32(0) },
                                         { "GeofenceType", reader.GetString(1) },
                                         { "AddedDate", reader.GetInt64(2) },
                                         { "StrokeColor", reader.GetString(3) },
                                         { "StrokeOpacity", reader.GetDouble(4) }, 
                                         { "StrokeWeight", reader.GetDouble(5) },
                                         { "FillColor", reader.GetString(6) },
                                         { "FillOpacity", reader.GetDouble(7) } 
                                };

                                geofences.Add(geofenceData);
                            }


                            var geofencesConcurrentDict = new ConcurrentDictionary<string, string>();
                            int count = 0;
                            foreach (var geofence in geofences)
                            {
                                geofencesConcurrentDict.TryAdd($"Geofence_{count}", Newtonsoft.Json.JsonConvert.SerializeObject(geofence));
                                count++;
                            }


                            gvar.DicOfDic["Geofences"] = geofencesConcurrentDict;


                            return Ok(gvar);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                return InternalServerError(ex);
            }
        }



        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddGeofence([FromBody] GVAR gvar)
        {
            var geofenceType = gvar.DicOfDic["Geofence"]["GeofenceType"];
            var addedDate = Convert.ToInt64(gvar.DicOfDic["Geofence"]["AddedDate"]);
            var strokeColor = gvar.DicOfDic["Geofence"]["StrokeColor"];
            var strokeOpacity = Convert.ToDouble(gvar.DicOfDic["Geofence"]["StrokeOpacity"]);
            var strokeWeight = Convert.ToDouble(gvar.DicOfDic["Geofence"]["StrokeWeight"]);
            var fillColor = gvar.DicOfDic["Geofence"]["FillColor"];
            var fillOpacity = Convert.ToDouble(gvar.DicOfDic["Geofence"]["FillOpacity"]);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "INSERT INTO Geofences (GeofenceType, AddedDate, StrokeColor, StrokeOpacity, StrokeWeight, FillColor, FillOpacity) VALUES (@GeofenceType, @AddedDate, @StrokeColor, @StrokeOpacity, @StrokeWeight, @FillColor, @FillOpacity)";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@GeofenceType", geofenceType);
                    command.Parameters.AddWithValue("@AddedDate", addedDate);
                    command.Parameters.AddWithValue("@StrokeColor", strokeColor);
                    command.Parameters.AddWithValue("@StrokeOpacity", strokeOpacity);
                    command.Parameters.AddWithValue("@StrokeWeight", strokeWeight);
                    command.Parameters.AddWithValue("@FillColor", fillColor);
                    command.Parameters.AddWithValue("@FillOpacity", fillOpacity);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateGeofence([FromBody] GVAR gvar)
        {
            var geofenceID = Convert.ToInt32(gvar.DicOfDic["Geofence"]["GeofenceID"]);
            var geofenceType = gvar.DicOfDic["Geofence"]["GeofenceType"];
            var addedDate = Convert.ToInt64(gvar.DicOfDic["Geofence"]["AddedDate"]);
            var strokeColor = gvar.DicOfDic["Geofence"]["StrokeColor"];
            var strokeOpacity = Convert.ToInt32(gvar.DicOfDic["Geofence"]["StrokeOpacity"]);
            var strokeWeight = Convert.ToInt32(gvar.DicOfDic["Geofence"]["StrokeWeight"]);
            var fillColor = gvar.DicOfDic["Geofence"]["FillColor"];
            var fillOpacity = Convert.ToInt32(gvar.DicOfDic["Geofence"]["FillOpacity"]);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "UPDATE Geofences SET GeofenceType = @GeofenceType, AddedDate = @AddedDate, StrokeColor = @StrokeColor, StrokeOpacity = @StrokeOpacity, StrokeWeight = @StrokeWeight, FillColor = @FillColor, FillOpacity = @FillOpacity WHERE GeofenceID = @GeofenceID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@GeofenceID", geofenceID);
                    command.Parameters.AddWithValue("@GeofenceType", geofenceType);
                    command.Parameters.AddWithValue("@AddedDate", addedDate);
                    command.Parameters.AddWithValue("@StrokeColor", strokeColor);
                    command.Parameters.AddWithValue("@StrokeOpacity", strokeOpacity);
                    command.Parameters.AddWithValue("@StrokeWeight", strokeWeight);
                    command.Parameters.AddWithValue("@FillColor", fillColor);
                    command.Parameters.AddWithValue("@FillOpacity", fillOpacity);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("delete")]
        public IHttpActionResult DeleteGeofence([FromBody] GVAR gvar)
        {
            var geofenceID = Convert.ToInt32(gvar.DicOfDic["Geofence"]["GeofenceID"]);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM Geofences WHERE GeofenceID = @GeofenceID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@GeofenceID", geofenceID);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }
    }
}