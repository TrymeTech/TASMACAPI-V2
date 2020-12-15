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
                ds = sqlConnection.GetDataSetValues("GetTicketsCount", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}