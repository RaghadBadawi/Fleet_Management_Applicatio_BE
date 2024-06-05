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
    public class CircleGeofenceController : ApiController
    {
        GVAR gvar = new GVAR();
        string connectionString = "Host=localhost;Port=5432;Database=RaghadBadawi_FMS;Username=postgres;Password=NEETisLIFE";

        public CircleGeofenceController()
        {
            gvar.DicOfDic["CircleGeofence"] = new ConcurrentDictionary<string, string>();
        }

        [HttpGet]
        [Route("api/circleGeofence/getAll")]
        public IHttpActionResult GetAllCircleGeofences()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM CircleGeofences";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var circleGeofences = new List<CircleGeofence>();
                        while (reader.Read())
                        {
                            var circleGeofence = new CircleGeofence
                            {
                                ID = reader.GetInt32(0),
                                GeofenceID = reader.GetInt64(1),
                                Radius = reader.GetInt64(2),
                                Latitude = reader.GetInt32(3),
                                Longitude = reader.GetInt32(4)
                            };
                            circleGeofences.Add(circleGeofence);
                        }
                        return Ok(circleGeofences);
                    }
                }
            }
        }

        [HttpPost]
        [Route("api/circleGeofence/add")]
        public IHttpActionResult AddCircleGeofence([FromBody] GVAR gvar)
        {
            var geofenceID = Convert.ToInt64(gvar.DicOfDic["CircleGeofence"]["GeofenceID"]);
            var radius = Convert.ToInt64(gvar.DicOfDic["CircleGeofence"]["Radius"]);
            var latitude = Convert.ToInt32(gvar.DicOfDic["CircleGeofence"]["Latitude"]);
            var longitude = Convert.ToInt32(gvar.DicOfDic["CircleGeofence"]["Longitude"]);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "INSERT INTO CircleGeofences (GeofenceID, Radius, Latitude, Longitude) VALUES (@GeofenceID, @Radius, @Latitude, @Longitude)";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@GeofenceID", geofenceID);
                    command.Parameters.AddWithValue("@Radius", radius);
                    command.Parameters.AddWithValue("@Latitude", latitude);
                    command.Parameters.AddWithValue("@Longitude", longitude);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("api/circleGeofence/update")]
        public IHttpActionResult UpdateCircleGeofence([FromBody] GVAR gvar)
        {
            var id = Convert.ToInt32(gvar.DicOfDic["CircleGeofence"]["ID"]);
            var geofenceID = Convert.ToInt64(gvar.DicOfDic["CircleGeofence"]["GeofenceID"]);
            var radius = Convert.ToInt64(gvar.DicOfDic["CircleGeofence"]["Radius"]);
            var latitude = Convert.ToInt32(gvar.DicOfDic["CircleGeofence"]["Latitude"]);
            var longitude = Convert.ToInt32(gvar.DicOfDic["CircleGeofence"]["Longitude"]);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "UPDATE CircleGeofences SET GeofenceID = @GeofenceID, Radius = @Radius, Latitude = @Latitude, Longitude = @Longitude WHERE ID = @ID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@GeofenceID", geofenceID);
                    command.Parameters.AddWithValue("@Radius", radius);
                    command.Parameters.AddWithValue("@Latitude", latitude);
                    command.Parameters.AddWithValue("@Longitude", longitude);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("api/circleGeofence/delete")]
        public IHttpActionResult DeleteCircleGeofence([FromBody] GVAR gvar)
        {
            var id = Convert.ToInt32(gvar.DicOfDic["CircleGeofence"]["ID"]);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM CircleGeofences WHERE ID = @ID";
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
