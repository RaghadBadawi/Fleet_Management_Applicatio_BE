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
    public class RectangleGeofenceController : ApiController
    {
        GVAR gvar = new GVAR();
        string connectionString = "Host=localhost;Port=5432;Database=RaghadBadawi_FMS;Username=postgres;Password=NEETisLIFE";

        public RectangleGeofenceController()
        {
            gvar.DicOfDic["RectangleGeofence"] = new ConcurrentDictionary<string, string>();
        }

    
        public IHttpActionResult GetAllRectangleGeofences()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM RectangleGeofences";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var rectangleGeofences = new List<RectangleGeofence>();
                        while (reader.Read())
                        {
                            var rectangleGeofence = new RectangleGeofence
                            {
                                ID = reader.GetInt32(0),
                                GeofenceID = reader.GetInt64(1),
                                North = reader.GetInt32(2),
                                East = reader.GetInt32(3),
                                West = reader.GetInt32(4),
                                South = reader.GetInt32(5)
                            };
                            rectangleGeofences.Add(rectangleGeofence);
                        }
                        return Ok(rectangleGeofences);
                    }
                }
            }
        }

        [HttpPost]
        [Route("api/rectangleGeofence/add")]
        public IHttpActionResult AddRectangleGeofence([FromBody] GVAR gvar)
        {
            var geofenceID = Convert.ToInt64(gvar.DicOfDic["RectangleGeofence"]["GeofenceID"]);
            var north = Convert.ToInt32(gvar.DicOfDic["RectangleGeofence"]["North"]);
            var east = Convert.ToInt32(gvar.DicOfDic["RectangleGeofence"]["East"]);
            var west = Convert.ToInt32(gvar.DicOfDic["RectangleGeofence"]["West"]);
            var south = Convert.ToInt32(gvar.DicOfDic["RectangleGeofence"]["South"]);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "INSERT INTO RectangleGeofences (GeofenceID, North, East, West, South) VALUES (@GeofenceID, @North, @East, @West, @South)";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@GeofenceID", geofenceID);
                    command.Parameters.AddWithValue("@North", north);
                    command.Parameters.AddWithValue("@East", east);
                    command.Parameters.AddWithValue("@West", west);
                    command.Parameters.AddWithValue("@South", south);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("api/rectangleGeofence/update")]
        public IHttpActionResult UpdateRectangleGeofence([FromBody] GVAR gvar)
        {
            var id = Convert.ToInt32(gvar.DicOfDic["RectangleGeofence"]["ID"]);
            var geofenceID = Convert.ToInt64(gvar.DicOfDic["RectangleGeofence"]["GeofenceID"]);
            var north = Convert.ToInt32(gvar.DicOfDic["RectangleGeofence"]["North"]);
            var east = Convert.ToInt32(gvar.DicOfDic["RectangleGeofence"]["East"]);
            var west = Convert.ToInt32(gvar.DicOfDic["RectangleGeofence"]["West"]);
            var south = Convert.ToInt32(gvar.DicOfDic["RectangleGeofence"]["South"]);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "UPDATE RectangleGeofences SET GeofenceID = @GeofenceID, North = @North, East = @East, West = @West, South = @South WHERE ID = @ID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@GeofenceID", geofenceID);
                    command.Parameters.AddWithValue("@North", north);
                    command.Parameters.AddWithValue("@East", east);
                    command.Parameters.AddWithValue("@West", west);
                    command.Parameters.AddWithValue("@South", south);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("api/rectangleGeofence/delete")]
        public IHttpActionResult DeleteRectangleGeofence([FromBody] GVAR gvar)
        {
            var id = Convert.ToInt32(gvar.DicOfDic["RectangleGeofence"]["ID"]);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM RectangleGeofences WHERE ID = @ID";
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