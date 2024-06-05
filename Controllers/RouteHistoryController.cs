using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using System.Web.Http;
using FPro;
using Npgsql;
using TProject.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace TProject.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/routeHistory")]
    public class RouteHistoryController : ApiController
    {
        GVAR gvar = new GVAR();
        string connectionString = "Host = localhost; Port = 5432; Database = RaghadBadawi_FMS; Username = postgres; Password = NEETisLIFE";

        public RouteHistoryController()
        {
            gvar.DicOfDic["RouteHistory"] = new ConcurrentDictionary<string, string>();
        }

        [HttpPost]
        [Route("getAll")]
        public IHttpActionResult GetAllRouteHistory([FromBody] GVAR gvar)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = "SELECT RouteHistoryID, VehicleID, VehicleDirection, Status, VehicleSpeed, Epoch, Address, Latitude, Longitude FROM RouteHistory";
                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var routeHistories = new List<Dictionary<string, object>>();
                            while (reader.Read())
                            {
                                var routeHistoryData = new Dictionary<string, object>
                                {
                                    { "RouteHistoryID", reader.GetInt64(0) },
                                    { "VehicleID", reader.GetInt64(1) },
                                    { "VehicleDirection", reader.GetInt32(2) },
                                    { "Status", reader.GetChar(3) },
                                    { "VehicleSpeed", reader.GetString(4) },
                                    { "Epoch", reader.GetInt64(5) },
                                    { "Address", reader.GetString(6) },
                                    { "Latitude", reader.GetDouble(7) },
                                    { "Longitude", reader.GetDouble(8) }
                                };
                                routeHistories.Add(routeHistoryData);
                            }

                            var routeHistoriesConcurrentDict = new ConcurrentDictionary<string, string>();
                            int count = 0;
                            foreach (var routeHistory in routeHistories)
                            {
                                routeHistoriesConcurrentDict.TryAdd($"RouteHistory_{count}", JsonConvert.SerializeObject(routeHistory));
                                count++;
                            }

                            gvar.DicOfDic["RouteHistories"] = routeHistoriesConcurrentDict;
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
        [Route("getByVehicleId")]
        public IHttpActionResult GetRouteHistoryByVehicleId([FromBody] GVAR gvar)
        {
            try
            {
                var vehicleID = Convert.ToInt64(gvar.DicOfDic["RouteHistory"]["VehicleID"]);
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = "SELECT RouteHistoryID, VehicleID, VehicleDirection, Status, VehicleSpeed, Epoch, Address, Latitude, Longitude FROM RouteHistory WHERE VehicleID = @VehicleID";
                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@VehicleID", vehicleID);

                        using (var reader = command.ExecuteReader())
                        {
                            var routeHistories = new List<Dictionary<string, object>>();
                            while (reader.Read())
                            {
                                var routeHistoryData = new Dictionary<string, object>
                                {
                                    { "RouteHistoryID", reader.GetInt64(0) },
                                    { "VehicleID", reader.GetInt64(1) },
                                    { "VehicleDirection", reader.GetInt32(2) },
                                    { "Status", reader.GetChar(3) },
                                    { "VehicleSpeed", reader.GetString(4) },
                                    { "Epoch", reader.GetInt64(5) },
                                    { "Address", reader.GetString(6) },
                                    { "Latitude", reader.GetDouble(7) },
                                    { "Longitude", reader.GetDouble(8) }
                                };
                                routeHistories.Add(routeHistoryData);
                            }

                            var routeHistoriesConcurrentDict = new ConcurrentDictionary<string, string>();
                            int count = 0;
                            foreach (var routeHistory in routeHistories)
                            {
                                routeHistoriesConcurrentDict.TryAdd($"RouteHistory_{count}", JsonConvert.SerializeObject(routeHistory));
                                count++;
                            }

                            gvar.DicOfDic["RouteHistories"] = routeHistoriesConcurrentDict;
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
        public IHttpActionResult AddRouteHistory([FromBody] GVAR gvar)
        {
            var vehicleDirection = Convert.ToInt32(gvar.DicOfDic["RouteHistory"]["VehicleDirection"]);
            var vehicleID = Convert.ToInt64(gvar.DicOfDic["RouteHistory"]["VehicleID"]);
            var status = Convert.ToChar(gvar.DicOfDic["RouteHistory"]["Status"]);
            var vehicleSpeed = gvar.DicOfDic["RouteHistory"]["VehicleSpeed"];
            var epoch = Convert.ToInt64(gvar.DicOfDic["RouteHistory"]["Epoch"]);
            var address = gvar.DicOfDic["RouteHistory"]["Address"];
            var latitude = Convert.ToDouble(gvar.DicOfDic["RouteHistory"]["Latitude"]);
            var longitude = Convert.ToDouble(gvar.DicOfDic["RouteHistory"]["Longitude"]);
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "INSERT INTO RouteHistory (VehicleDirection,VehicleID, Status, VehicleSpeed, Epoch, Address, Latitude, Longitude) VALUES (@VehicleDirection, @VehicleID, @Status, @VehicleSpeed, @Epoch, @Address, @Latitude, @Longitude)";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@VehicleDirection", vehicleDirection);
                    command.Parameters.AddWithValue("@VehicleID", vehicleID);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@VehicleSpeed", vehicleSpeed);
                    command.Parameters.AddWithValue("@Epoch", epoch);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Latitude", latitude);
                    command.Parameters.AddWithValue("@Longitude", longitude);
                    command.ExecuteNonQuery();
                }
            }
            var routeHistory = new RouteHistory
            {
                VehicleDirection = vehicleDirection,
                VehicleID = vehicleID,
                Status = status,
                VehicleSpeed = vehicleSpeed,
                Epoch = epoch,
                Address = address,
                Latitude = latitude,
                Longitude = longitude
            };
            var message = JsonConvert.SerializeObject(routeHistory);

            return Ok();
        }

        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateRouteHistory([FromBody] GVAR gvar)
        {
            var id = Convert.ToInt64(gvar.DicOfDic["RouteHistory"]["RouteHistoryID"]);
            var vehicleDirection = Convert.ToInt32(gvar.DicOfDic["RouteHistory"]["VehicleDirection"]);
            var vehicleID = Convert.ToInt64(gvar.DicOfDic["RouteHistory"]["VehicleID"]);
            var status = gvar.DicOfDic["RouteHistory"]["Status"];
            var vehicleSpeed = gvar.DicOfDic["RouteHistory"]["VehicleSpeed"];
            var epoch = Convert.ToInt64(gvar.DicOfDic["RouteHistory"]["Epoch"]);
            var address = gvar.DicOfDic["RouteHistory"]["Address"];
            var latitude = Convert.ToDouble(gvar.DicOfDic["RouteHistory"]["Latitude"]);
            var longitude = Convert.ToDouble(gvar.DicOfDic["RouteHistory"]["Longitude"]);
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "UPDATE RouteHistory SET VehicleID = @VehicleID, VehicleDirection = @VehicleDirection, Status = @Status, VehicleSpeed = @VehicleSpeed, Epoch = @Epoch, Address = @Address, Latitude = @Latitude, Longitude = @Longitude WHERE RouteHistoryID = @RouteHistoryID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@RouteHistoryID", id);
                    command.Parameters.AddWithValue("@VehicleDirection", vehicleDirection);
                    command.Parameters.AddWithValue("@VehicleID", vehicleID);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@VehicleSpeed", vehicleSpeed);
                    command.Parameters.AddWithValue("@Epoch", epoch);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Latitude", latitude);
                    command.Parameters.AddWithValue("@Longitude", longitude);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("delete")]
        public IHttpActionResult DeleteRouteHistory([FromBody] GVAR gvar)
        {
            var id = Convert.ToInt64(gvar.DicOfDic["RouteHistory"]["RouteHistoryID"]);
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM RouteHistory WHERE RouteHistoryID = @RouteHistoryID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@RouteHistoryID", id);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }
    }
}
