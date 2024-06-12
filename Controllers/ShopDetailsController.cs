using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopDetailsController : ControllerBase
    {
        [HttpGet]
        public string Get(int type)
        {
            var call_procedure = (type == 1) ? "ShopInstallationStatus" : "Shops";
            // SQLConnection sqlConnection = new SQLConnection();
            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = sqlConnection.GetDataSetValues(call_procedure);
                var table = (type == 1) ? JsonConvert.SerializeObject(ds) :
                    JsonConvert.SerializeObject(ds.Tables[0]);
                return table;
            }
            finally
            {
                ds.Dispose();
            }
        }

    }
}