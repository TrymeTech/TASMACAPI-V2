﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Autosrlno : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                ds = sqlConnection.GetDataSetValues("newregioncoderegionmaster");
                return JsonConvert.SerializeObject(ds.Tables[0]);
                
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
    
}

    