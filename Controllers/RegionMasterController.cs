using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionMasterController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            // SQLConnection sqlConnection = new SQLConnection();
            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = sqlConnection.GetDataSetValues("GetRegions");
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
       
    }
    public class RegionMasterEntity
    {
        public string regioncode { get; set; }
        public string regionname { get; set; }
        public string address { get; set; }

    }
}