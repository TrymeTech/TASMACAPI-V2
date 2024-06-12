using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugzillaDataController : ControllerBase
    {
        //[HttpGet]
        //public string Get()
        //{
        //    return "value1, value2" ;
        //}
        [HttpGet("{id}")]
        public string Get(int value)
        {
            var call_procedure = (value == 1) ? "GetBugsCount" : "GetBugzillaData";
            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = sqlConnection.GetDataSetValues(call_procedure);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}