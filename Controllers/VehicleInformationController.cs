using FPro;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace TProject.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/vehicleInfo")]
    public class VehicleInformationController : ApiController
    {
       
        GVAR gvar = new GVAR();
        string connectionString = "Host = localhost; Port = 5432; Database = RaghadBadawi_FMS; Username = postgres; Password = NEETisLIFE";

        public VehicleInformationController()
        {
          
            gvar.DicOfDic["VehicleInfo"] = new ConcurrentDictionary<string, string>();
        }

        [HttpPost]
        [Route("getAll")]
        public IHttpActionResult GetAllVehicleInformation([FromBody] GVAR gvar)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = @"SELECT ID, VehicleID, DriverID, VehicleMake, VehicleModel, PurchaseDate FROM VehiclesInformations";
                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var vehicleInfos = new List<Dictionary<string, object>>();
                            while (reader.Read())
                            {
                                var vehicleInfoData = new Dictionary<string, object>
                        {
                            { "ID", reader.GetInt64(0) },
                            { "VehicleID", reader.GetInt64(1) },
                            { "DriverID", reader.GetInt64(2) },
                            { "VehicleMake", reader.GetString(3) },
                            { "VehicleModel", reader.GetString(4) },
                            { "PurchaseDate", reader.GetInt64(5) }
                        };
                                vehicleInfos.Add(vehicleInfoData);
                            }

                            var vehicleInfoConcurrentDict = new ConcurrentDictionary<string, string>();
                            int count = 0;
                            foreach (var vehicleInfo in vehicleInfos)
                            {
                                vehicleInfoConcurrentDict.TryAdd($"VehicleInfo_{count}", Newtonsoft.Json.JsonConvert.SerializeObject(vehicleInfo));
                                count++;
                            }

                            gvar.DicOfDic["VehicleInformations"] = vehicleInfoConcurrentDict;
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
        [Route("getById")]
        public IHttpActionResult GetVehicleInfoByVehicleId([FromBody] GVAR gvar)
        {
            try
            {
                
                var vehicleID = Convert.ToInt64(gvar.DicOfDic["VehicleInfo"]["VehicleID"]);
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = "SELECT ID, VehicleID, DriverID, VehicleMake, VehicleModel, PurchaseDate FROM VehicleInformation WHERE VehicleID = @VehicleID";
                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@VehicleID", vehicleID);

                        using (var reader = command.ExecuteReader())
                        {
                            var vehicleInfos = new List<Dictionary<string, object>>();
                            while (reader.Read())
                            {
                                var vehicleInfoData = new Dictionary<string, object>
                                {
                                    { "ID", reader.GetInt64(0) },
                                    { "VehicleID", reader.GetInt64(1) },
                                    { "DriverID", reader.GetInt64(2) },
                                    { "VehicleMake", reader.GetString(3) },
                                    { "VehicleModel", reader.GetString(4) },
                                    { "PurchaseDate", reader.GetInt64(5) }
                                };
                                vehicleInfos.Add(vehicleInfoData);
                            }

                            var vehicleInfosConcurrentDict = new ConcurrentDictionary<string, string>();
                            int count = 0;
                            foreach (var vehicleInfo in vehicleInfos)
                            {
                                vehicleInfosConcurrentDict.TryAdd($"VehicleInfo_{count}", JsonConvert.SerializeObject(vehicleInfo));
                                count++;
                            }

                            gvar.DicOfDic["VehicleInfos"] = vehicleInfosConcurrentDict;
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
        public IHttpActionResult AddVehicleInfo([FromBody] GVAR gvar)
        {
            var vehicleID = Convert.ToInt64(gvar.DicOfDic["VehicleInfo"]["VehicleID"]);
            var driverID = Convert.ToInt64(gvar.DicOfDic["VehicleInfo"]["DriverID"]);
            var vehicleMake = gvar.DicOfDic["VehicleInfo"]["VehicleMake"];
            var vehicleModel = gvar.DicOfDic["VehicleInfo"]["VehicleModel"];
            var purchaseDateString = gvar.DicOfDic["VehicleInfo"]["PurchaseDate"];

            DateTime purchaseDate;
            if (!DateTime.TryParse(purchaseDateString, out purchaseDate))
            {
                return BadRequest("Invalid purchase date format");
            }

            
            var purchaseDateEpoch = new DateTimeOffset(purchaseDate).ToUnixTimeSeconds();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "INSERT INTO VehiclesInformations (VehicleID, DriverID, VehicleMake, VehicleModel, PurchaseDate) VALUES (@VehicleID, @DriverID, @VehicleMake, @VehicleModel, @PurchaseDate)";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@VehicleID", vehicleID);
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    command.Parameters.AddWithValue("@VehicleMake", vehicleMake);
                    command.Parameters.AddWithValue("@VehicleModel", vehicleModel);
                    command.Parameters.AddWithValue("@PurchaseDate", purchaseDateEpoch);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }



        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateVehicleInfo([FromBody] GVAR gvar)
        {
            var id = Convert.ToInt64(gvar.DicOfDic["VehicleInfo"]["ID"]);
            var vehicleID = Convert.ToInt64(gvar.DicOfDic["VehicleInfo"]["VehicleID"]);
            var driverID = Convert.ToInt64(gvar.DicOfDic["VehicleInfo"]["DriverID"]);
            var vehicleMake = gvar.DicOfDic["VehicleInfo"]["VehicleMake"];
            var vehicleModel = gvar.DicOfDic["VehicleInfo"]["VehicleModel"];
            var purchaseDate = Convert.ToInt64(gvar.DicOfDic["VehicleInfo"]["PurchaseDate"]);
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "UPDATE VehiclesInformations SET VehicleID = @VehicleID, DriverID = @DriverID, VehicleMake = @VehicleMake, VehicleModel = @VehicleModel, PurchaseDate = @PurchaseDate WHERE ID = @ID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@VehicleID", vehicleID);
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    command.Parameters.AddWithValue("@VehicleMake", vehicleMake);
                    command.Parameters.AddWithValue("@VehicleModel", vehicleModel);
                    command.Parameters.AddWithValue("@PurchaseDate", purchaseDate);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("delete")]
        public IHttpActionResult DeleteVehicleInfo([FromBody] GVAR gvar)
        {
            var id = Convert.ToInt64(gvar.DicOfDic["VehicleInfo"]["ID"]);
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM VehiclesInformations WHERE ID = @ID";
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
