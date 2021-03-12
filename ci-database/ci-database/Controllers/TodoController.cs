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
        [HttpGet]
        public async Task<JToken> get()
        {
            using (var conn = new NpgsqlConnection(_config.GetConnectionString("Postgres")))
            {
                string query = " select * from todos";
                conn.Open();
                NpgsqlCommand selectCommand = new NpgsqlCommand(query, conn);
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

        // GET api/<TodoController>/5
        [HttpGet("{id?}")]
        public string Get(int? id)
        {
            return "value";
        }

        // POST api/<TodoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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

        //private JToken EnrichResult(string source)
        //{
        //    var jr = JToken.Parse(source ?? "[]");

        //    var baseUrl = (HttpContext != null) ? HttpContext.Request.Scheme + "://" + HttpContext.Request.Host : string.Empty;

        //    var AddUrl = new Action<JObject>(o =>
        //    {
        //        if (o == null) return;
        //        var todoUrl = $"{baseUrl}/todo/{o["id"]}";
        //        o["url"] = todoUrl;
        //    });

        //    if (jr is JArray ja)
        //        ja.ToList().ForEach(e => AddUrl(e as JObject));
        //    else if (jr is JObject jo)
        //        AddUrl(jo);
        //    else
        //        throw new ArgumentException($"{nameof(source)} is not an array or an object");

        //    return jr;
        //}
    }
}
