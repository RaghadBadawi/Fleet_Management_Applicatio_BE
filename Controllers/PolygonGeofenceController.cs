using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using System.Web.Http;
using System.Net;
using System.Data;
using FPro;
using Npgsql;
using TProject.Models;

namespace TProject.Controllers
{
    public class PolygonGeofenceController : ApiController
    {
        GVAR gvar = new GVAR();
        string connectionString = "Host=localhost;Port=5432;Database=RaghadBadawi_FMS;Username=postgres;Password=NEETisLIFE";

        public PolygonGeofenceController()
        {
            gvar.DicOfDic["PolygonGeofence"] = new ConcurrentDictionary<string, string>();
        }

     
        public IHttpActionResult GetAllPolygonGeofences()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM PolygonGeofences";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var polygonGeofences = new List<PolygonGeofence>();
                        while (reader.Read())
                        {
                            var polygonGeofence = new PolygonGeofence
                            {
                                ID = reader.GetInt32(0),
                                GeofenceID = reader.GetInt64(1),
                                Latitude = reader.GetInt32(2),
                                Longitude = reader.GetInt32(3)
                            };
                            polygonGeofences.Add(polygonGeofence);
                        }
                        return Ok(polygonGeofences);
                    }
                }
            }
        }

        [HttpPost]
        [Route("api/polygonGeofence/add")]
        public IHttpActionResult AddPolygonGeofence([FromBody] GVAR gvar)
        {
            var geofenceID = Convert.ToInt64(gvar.DicOfDic["PolygonGeofence"]["GeofenceID"]);
            var latitude = Convert.ToInt32(gvar.DicOfDic["PolygonGeofence"]["Latitude"]);
            var longitude = Convert.ToInt32(gvar.DicOfDic["PolygonGeofence"]["Longitude"]);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "INSERT INTO PolygonGeofences (GeofenceID, Latitude, Longitude) VALUES (@GeofenceID, @Latitude, @Longitude)";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@GeofenceID", geofenceID);
                    command.Parameters.AddWithValue("@Latitude", latitude);
                    command.Parameters.AddWithValue("@Longitude", longitude);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("api/polygonGeofence/update")]
        public IHttpActionResult UpdatePolygonGeofence([FromBody] GVAR gvar)
        {
            var id = Convert.ToInt32(gvar.DicOfDic["PolygonGeofence"]["ID"]);
            var geofenceID = Convert.ToInt64(gvar.DicOfDic["PolygonGeofence"]["GeofenceID"]);
            var latitude = Convert.ToInt32(gvar.DicOfDic["PolygonGeofence"]["Latitude"]);
            var longitude = Convert.ToInt32(gvar.DicOfDic["PolygonGeofence"]["Longitude"]);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "UPDATE PolygonGeofences SET GeofenceID = @GeofenceID, Latitude = @Latitude, Longitude = @Longitude WHERE ID = @ID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@GeofenceID", geofenceID);
                    command.Parameters.AddWithValue("@Latitude", latitude);
                    command.Parameters.AddWithValue("@Longitude", longitude);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("api/polygonGeofence/delete")]
        public IHttpActionResult DeletePolygonGeofence([FromBody] GVAR gvar)
        {
            var id = Convert.ToInt32(gvar.DicOfDic["PolygonGeofence"]["ID"]);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM PolygonGeofences WHERE ID = @ID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }
    }
}