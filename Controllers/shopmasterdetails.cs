using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using WebApplication1.Mail;
using WebApplication1.Models;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopmasterdetailsController : ControllerBase
    {

        [HttpPost("{id}")]
        public Tuple<bool, string> Post(shopmasters entity)
        {
            try
            {
                ManageSQLConnection manageSQL = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();

                //int sno = "SELECT  max(slno) + 1,dcode from shops_master ;"

                //sqlParameters.Add(new KeyValuePair<string, string>("@SLNO", Convert.ToString(entity.SLNO)));
                //sqlParameters.Add(new KeyValuePair<string, string>("@SLNO", Convert.ToString(0)));

                sqlParameters.Add(new KeyValuePair<string, string>("@DISTRICT", entity.DISTRICT));
                sqlParameters.Add(new KeyValuePair<string, string>("@SHOPNO", Convert.ToString(entity.SHOPNO)));
                sqlParameters.Add(new KeyValuePair<string, string>("@SHOPADDRESS", entity.SHOPADDRESS));
                sqlParameters.Add(new KeyValuePair<string, string>("@SHOPINCHARGE", entity.SHOPINCHARGE));
                //sqlParameters.Add(new KeyValuePair<string, string>("@CONTACTNO", Convert.ToString(entity.CONTACTNO)));
                sqlParameters.Add(new KeyValuePair<string, string>("@CONTACTNO",entity.CONTACTNO));
                sqlParameters.Add(new KeyValuePair<string, string>("@DCODE", entity.DCODE));

        //        sqlParameters.Add(new KeyValuePair<string, string>("@RCODE", entity.RCODE));

                sqlParameters.Add(new KeyValuePair<string, string>("@installation_status", Convert.ToString(entity.installation_status)));
                sqlParameters.Add(new KeyValuePair<string, string>("@isActive", Convert.ToString(entity.isActive)));
                
                //string snno = "SELECT  max(slno) + 1 from shops_master where dcode=entity.DCODE;";
                //sqlParameters.Add(new KeyValuePair<string, string>("@SLNO", Convert.ToString(snno)));


                var result = manageSQL.InsertData("InsertShopDetails", sqlParameters);
                return new Tuple<bool, string>(result, "Saved Successfully");

            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "Please Contact Administrator");

            }
        }


        [HttpGet("{id}")]
       
        public string Get(string rcode,string dcode)
        {
            ManageSQLConnection manageSqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                //sqlParameters.Add(new KeyValuePair<string, string>("@rcode", "R02"));
                sqlParameters.Add(new KeyValuePair<string, string>("@rcode",rcode));
                sqlParameters.Add(new KeyValuePair<string, string>("@dcode",dcode));
                //sqlParameters.Add(new KeyValuePair<string, string>("@installation_status", installation_status));

                
                ds = manageSqlConnection.GetDataSetValues("Getshopdetails", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
    public class shopmasters
    {

        public string isActive { get; set; }
        public string installation_status { get; set; }

      //  public string RCODE { get; set; }
        
        public string DCODE { get; set; }
        public string DISTRICT { get; set; }
        public int SHOPNO { get; set; }
        public string SHOPADDRESS { get; set; }
        public string  CONTACTNO { get; set; }
        //public string installation_status { get; internal set; }
        public string SHOPINCHARGE { get; set; }
        public int SLNO { get; set; }
    }
}