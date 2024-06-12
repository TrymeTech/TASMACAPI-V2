using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string username)
        {
            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@username", username));
                ds = sqlConnection.GetDataSetValues("GetLoginDetails", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
                //return "Dulasi success";
            }
            finally
            {
                ds.Dispose();
            }
        }

    }
}