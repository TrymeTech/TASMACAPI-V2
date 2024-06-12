using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;
using WebApplication1.SQLConnection;

[Route("api/[controller]")]
[ApiController]
public class regionmasterentry : Controller
{

    [HttpPost("{id}")]
    public Tuple<bool, string> Post(regionmasterEntry entity)
    {
        try
        {
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();

            //sqlParameters.Add(new KeyValuePair<string, string>("@rno", entity.rno));

            string Querystring = string.Empty;
            //Querystring = "select max(regncode) from region_master";

            //sqlParameters.Add(new KeyValuePair<string, string>("@regncode", Convert.ToString(Querystring)));
            sqlParameters.Add(new KeyValuePair<string, string>("@regncode", entity.regncode));
            sqlParameters.Add(new KeyValuePair<string, string>("@regnname", entity.regnname));
            sqlParameters.Add(new KeyValuePair<string, string>("@Address", entity.Address));
            
            var result = manageSQL.InsertData("Insertregionmaster", sqlParameters);
            return new Tuple<bool, string>(result, "Saved Successfully");
            


        }
        catch (Exception ex)
        {
            return new Tuple<bool, string>(false, "Please Contact Administrator");

        }


    } 
    
    /*
    [HttpGet]
    public string Get()
    {
        // SQLConnection sqlConnection = new SQLConnection();
        ManageSQLConnection sqlConnection = new ManageSQLConnection();
        DataSet ds = new DataSet();
        try
        {
            ds = sqlConnection.GetDataSetValues("GetRegionsdetails");
            return JsonConvert.SerializeObject(ds.Tables[0]);
        }
        finally
        {
            ds.Dispose();
        }
    }
    */
   public class regionmasterEntry
    {
        public string regncode { get; set; }
        public string regnname { get; set; }
        public string Address { get; set; }

   

    }
}
