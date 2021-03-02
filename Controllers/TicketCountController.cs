using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardTicketCountController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string userId)
        {
            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@userId", userId));
                string query = "SELECT  COUNT(ticket_id) AS total_bugs  FROM tickets ;" +
                                " SELECT COUNT(ticket_id) AS user_bugs FROM tickets WHERE userid = '"+userId+"';"+
                                " SELECT product_id, COUNT(ticket_id) AS product_bugs FROM tickets GROUP BY product_id;";

                ds = sqlConnection.GetDataSetValuesFromQuery(query);
                return JsonConvert.SerializeObject(ds.Tables);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}