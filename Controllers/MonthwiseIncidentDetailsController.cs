using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthwiseIncidentDetailsController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = sqlConnection.GetDataSetValues("GetMonthWiseIncidentDetails");
                return JsonConvert.SerializeObject(ds);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}