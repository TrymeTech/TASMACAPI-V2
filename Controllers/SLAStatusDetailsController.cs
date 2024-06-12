﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SLAStatusDetailsController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(string FDate, string TDate, int type)
        {
            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            var procedure = (type == 1) ? "GetSLAByTicketStatus" : "GetSlaStatusDetails";
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@FDate", FDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@TDate", TDate));
                ds = sqlConnection.GetDataSetValues(procedure, sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }

    }
}