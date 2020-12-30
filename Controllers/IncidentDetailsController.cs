using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using WebApplication1.Models;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentDetailsController : ControllerBase
    {
        MySqlCommand cmd = new MySqlCommand();

        [HttpPost("{id}")]
        public Tuple<bool, string> Post(IncidentEntity entity)
        {
            //  ManageSQLConnection sqlConnection = new ManageSQLConnection();
            MySqlConnection sqlConnection = new MySqlConnection();
            MySqlTransaction objTrans = null;
            string connectionString = GlobalVariables.ConnectionString;
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                DataSet ds = new DataSet();

                cmd = new MySqlCommand();
                try
                {
                    if (sqlConnection.State == 0)
                    {
                        sqlConnection.Open();
                    }
                    objTrans = sqlConnection.BeginTransaction();
                    cmd.Transaction = objTrans;
                    cmd.Connection = sqlConnection;
                    cmd.CommandText = "InsertIncidentDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@rcode", entity.RCode);
                    cmd.Parameters.AddWithValue("@dcode", entity.DCode);
                    cmd.Parameters.AddWithValue("@shopcode", entity.ShopCode);
                    cmd.Parameters.AddWithValue("@doc_date", entity.DocDate);
                    cmd.Parameters.AddWithValue("@reason", entity.Reason);
                    cmd.Parameters.AddWithValue("@url_path", entity.URL);
                    cmd.Parameters.AddWithValue("@remarks", entity.Remarks);
                    cmd.ExecuteNonQuery();
                    objTrans.Commit();
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    return new Tuple<bool, string>(true, JsonConvert.SerializeObject(ds));
                }
                catch (Exception ex)
                {
                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                    objTrans.Rollback();
                    return new Tuple<bool, string>(false, "Exception occured");
                }
                finally
                {
                    sqlConnection.Close();
                    cmd.Dispose();
                    ds.Dispose();
                }
            }
        }

        [HttpGet("{id}")]
        public string Get(string FDate, string TDate, string RCode, string DCode)
        {
            // SQLConnection sqlConnection = new SQLConnection();
            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@FDate", FDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@TDate", TDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
                sqlParameters.Add(new KeyValuePair<string, string>("@DCode", DCode));
                ds = sqlConnection.GetDataSetValues("GetIncidentDetails", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }

    }
}
