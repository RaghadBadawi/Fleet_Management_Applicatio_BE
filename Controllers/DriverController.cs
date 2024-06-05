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
    [RoutePrefix("api/driver")]
    public class DriverController : ApiController
    {
        

        GVAR gvar = new GVAR();
        string connectionString = "Host = localhost; Port = 5432; Database = RaghadBadawi_FMS; Username = postgres; Password = NEETisLIFE";
        public DriverController()
        {
            gvar.DicOfDic["Driver"] = new ConcurrentDictionary<string, string>();
        }



        [HttpPost]
        [Route("getAll")]
        public IHttpActionResult GetAllDrivers([FromBody] GVAR gvar)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = "SELECT DriverID, DriverName, PhoneNumber FROM Driver";
                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var drivers = new List<Dictionary<string, object>>();
                            while (reader.Read())
                            {
                                var driverData = new Dictionary<string, object>
                        {
                            { "DriverID", reader.GetInt32(0) },
                            { "DriverName", reader.GetString(1) },
                            { "PhoneNumber", reader.GetInt64(2) }
                        };
                                drivers.Add(driverData);
                            }

                            var driversConcurrentDict = new ConcurrentDictionary<string, string>();
                            int count = 0;
                            foreach (var driver in drivers)
                            {
                                driversConcurrentDict.TryAdd($"Driver_{count}", Newtonsoft.Json.JsonConvert.SerializeObject(driver));
                                count++;
                            }

                            gvar.DicOfDic["Drivers"] = driversConcurrentDict;
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
        public IHttpActionResult AddDriver([FromBody] GVAR gvar)
        {
            var driverName = gvar.DicOfDic["Driver"]["DriverName"];
            var phoneNumber = Convert.ToInt64(gvar.DicOfDic["Driver"]["PhoneNumber"]);
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "INSERT INTO Driver (DriverName, PhoneNumber) VALUES (@DriverName, @PhoneNumber)";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DriverName", driverName);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }



        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateDriver([FromBody] GVAR gvar)
        {
            var driverID =Convert.ToInt64( gvar.DicOfDic["Driver"]["DriverID"]);
            var driverName = gvar.DicOfDic["Driver"]["DriverName"];
            var phoneNumber = Convert.ToInt64(gvar.DicOfDic["Driver"]["PhoneNumber"]);
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "UPDATE Driver SET DriverName = @DriverName, PhoneNumber = @PhoneNumber WHERE DriverID = @DriverID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    command.Parameters.AddWithValue("@DriverName", driverName);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("delete")]
        public IHttpActionResult DeleteDriver([FromBody] GVAR gvar)
        {
            var driverID = Convert.ToInt64( gvar.DicOfDic["Driver"]["DriverID"]);
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM Driver WHERE DriverID = @DriverID";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }
    }
}
