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
    public class ChangePasswordController : ControllerBase
    { 
        [HttpPut("{id}")]
        public bool Put(string UserName, string Pswd)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@UserName", UserName));
            sqlParameters.Add(new KeyValuePair<string, string>("@Pswd", Pswd));
            return manageSQLConnection.UpdateValues("UpdatePassword", sqlParameters);
        }

    }
}