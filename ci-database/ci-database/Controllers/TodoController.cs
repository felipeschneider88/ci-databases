using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Newtonsoft.Json.Linq;
using System.Data;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ci_database.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly IConfiguration _config;

        public TodoController(ILogger<TodoController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        //[HttpGet]
        [HttpGet("{id?}")]
        public async Task<JToken> get(int? id)
        {
            using (var conn = new NpgsqlConnection(_config.GetConnectionString("Postgres")))
            {
                string query = " select * from get_todos(@id)";
                NpgsqlCommand selectCommand = new NpgsqlCommand(query);
                if (id.HasValue)
                {
                    selectCommand.Parameters.Add(
                        new NpgsqlParameter()
                        {
                            ParameterName = "@id",
                            DbType = System.Data.DbType.Int32,
                            Value = id
                        }
                        );
                }
                else
                {
                    selectCommand.Parameters.Add(
                    new NpgsqlParameter()
                    {
                        ParameterName = "@id",
                        DbType = System.Data.DbType.Int32,
                        //set 0 with is no value on the SP
                        Value = 0
                    }
                    );
                }
                selectCommand.Connection = conn;
                conn.Open();
                NpgsqlDataReader reader = selectCommand.ExecuteReader(); 
                var datatable = new DataTable();
                datatable.Load(reader);
                string JsonResponse = string.Empty;
                JsonResponse = JsonConvert.SerializeObject(datatable);
                reader.Close();
                conn.Close();
                return JToken.Parse(JsonResponse);
            }
        }

        // PUT api/<TodoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TodoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
