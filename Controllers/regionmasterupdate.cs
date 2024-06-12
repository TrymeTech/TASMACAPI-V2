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
public class regionmasterupdate : Controller
{
    
    [HttpPut("{id}")]
    public Tuple<bool, string> Put(RegionmasterUpdate entity)

    {
        try
        {
            ManageSQLConnection manageSQL = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();

            
            sqlParameters.Add(new KeyValuePair<string, string>("@regncode", entity.regncode));
            sqlParameters.Add(new KeyValuePair<string, string>("@regnname", entity.regnname));
            sqlParameters.Add(new KeyValuePair<string, string>("@Address",  entity.Address));

            var result1 = manageSQL.InsertData("updateregionmaster", sqlParameters);
            return new Tuple<bool, string>(result1, "Updated Successfully");



        }
        catch (Exception ex)
        {
            return new Tuple<bool, string>(false, "Please Contact Administrator");

        }


    }

    
    public class RegionmasterUpdate    {
        public string regncode { get; set; }
        public string regnname { get; set; }
       public string Address { get; set; }
    }  
}
