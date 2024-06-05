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
    [RoutePrefix("api/vehicle")]
    public class VehicleController : ApiController
    {
        GVAR gvar = new GVAR();
        string connectionString = "Host=localhost;Port=5432;Database=RaghadBadawi_FMS;Username=postgres;Password=NEETisLIFE";

        public VehicleController()
        {
            gvar.DicOfDic["Vehicle"] = new ConcurrentDictionary<string, string>();
        }

        [HttpPost]
        [Route("getAll")]
        public IHttpActionResult GetAllVehicles([FromBody] GVAR gvar)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = @"SELECT v.VehicleID, v.VehicleNumber, v.VehicleType, r.Status, r.VehicleDirection AS LastDirection, 
                        r.Address AS LastAddress, r.Latitude AS LastLatitude, r.Longitude AS LastLongitude 
                        FROM Vehicles v
                        LEFT JOIN RouteHistory r ON v.VehicleID = r.VehicleID
                        ORDER BY r.Epoch DESC";
                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var vehicles = new List<Dictionary<string, object>>();
                            while (reader.Read())
                            {
                                var vehicleData = new Dictionary<string, object>
                        {
                            { "VehicleID", reader.GetInt64(0) },
                            { "VehicleNumber", reader.GetInt64(1) },
                            { "VehicleType", reader.GetString(2) },
                            { "Status", reader.IsDBNull(3) ? (char?)null : reader.GetChar(3) },
                            { "LastDirection", reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4) },
                            { "LastAddress", reader.IsDBNull(5) ? null : reader.GetString(5) },
                            { "LastLatitude", reader.IsDBNull(6) ? (double?)null : reader.GetDouble(6) },
                            { "LastLongitude", reader.IsDBNull(7) ? (double?)null : reader.GetDouble(7) }
                        };
                                vehicles.Add(vehicleData);
                            }

                            var vehiclesConcurrentDict = new ConcurrentDictionary<string, string>();
                            int count = 0;
                            foreach (var vehicle in vehicles)
                            {
                                vehiclesConcurrentDict.TryAdd($"Vehicle_{count}", Newtonsoft.Json.JsonConvert.SerializeObject(vehicle));
                                count++;
                            }

                            gvar.DicOfDic["Vehicles"] = vehiclesConcurrentDict;
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
        public IHttpActionResult AddVehicle([FromBody] GVAR gvar)
        {
            var vehicleNumber = Convert.ToInt64(gvar.DicOfDic["Vehicle"]["VehicleNumber"]);
            var vehicleType = gvar.DicOfDic["Vehicle"]["VehicleType"];
            long vehicleID;

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "INSERT INTO Vehicles (VehicleNumber, VehicleType) VALUES (@VehicleNumber, @VehicleType) RETURNING VehicleID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);
                    command.Parameters.AddWithValue("@VehicleType", vehicleType);
                    vehicleID = (long)command.ExecuteScalar(); 
                }
            }
            return Ok(new { VehicleID = vehicleID }); 
        }


        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateVehicle([FromBody] GVAR gvar)
        {
            var vehicleID = Convert.ToInt64(gvar.DicOfDic["Vehicle"]["VehicleID"]);
            var vehicleNumber = Convert.ToInt64(gvar.DicOfDic["Vehicle"]["VehicleNumber"]);
            var vehicleType = gvar.DicOfDic["Vehicle"]["VehicleType"];
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "UPDATE Vehicles SET VehicleNumber = @VehicleNumber, VehicleType = @VehicleType WHERE VehicleID = @VehicleID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@VehicleID", vehicleID);
                    command.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);
                    command.Parameters.AddWithValue("@VehicleType", vehicleType);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("delete")]
        public IHttpActionResult DeleteVehicle([FromBody] GVAR gvar)
        {
            var vehicleID = Convert.ToInt64(gvar.DicOfDic["Vehicle"]["VehicleID"]);
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM Vehicles WHERE VehicleID = @VehicleID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@VehicleID", vehicleID);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }
    }
}