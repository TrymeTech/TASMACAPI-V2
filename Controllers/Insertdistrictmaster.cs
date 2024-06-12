using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;
using WebApplication1.SQLConnection;
//namespace WebApplication1.Controllers { }

    [Route("api/[controller]")]
    [ApiController]
    
    public class Insertdistrictmaster : Controller
    {

    [HttpPost("{id}")]
    public Tuple<bool, string> Post(districtmaster entity)
    {
        try
        {
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();

            //sqlParameters.Add(new KeyValuePair<string, string>("@rno", entity.rno));

            string Querystring = string.Empty;
            //Querystring = "select max(regncode) from region_master";

            //sqlParameters.Add(new KeyValuePair<string, string>("@regncode", Convert.ToString(Querystring)));
            sqlParameters.Add(new KeyValuePair<string, string>("@Dcode", entity.Dcode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Dname", entity.Dname));
            sqlParameters.Add(new KeyValuePair<string, string>("@Rcode", entity.Rcode));
            sqlParameters.Add(new KeyValuePair<string, string>("@Address", entity.Address));

            var result = manageSQL.InsertData("Insertdistrictmaster", sqlParameters);
            return new Tuple<bool, string>(result, "Saved Successfully");

        }
        catch (Exception ex)
        {
            return new Tuple<bool, string>(false, "Please Contact Administrator");

        }
    
    }

    public class districtmaster
    {
        public string Dcode { get; set; }
        public string Dname { get; set; }
        public string Rcode { get; set; }
        public string Address { get; set; }

    }
   }

