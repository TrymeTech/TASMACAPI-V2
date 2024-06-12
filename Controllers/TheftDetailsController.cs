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
    public class TheftDetailsController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(TheftEntity entity)
        {
            MySqlCommand cmd = new MySqlCommand();
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
                    cmd.CommandText = "InsertTheftDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", entity.Id);
                    cmd.Parameters.AddWithValue("@Location", entity.Location);
                    cmd.Parameters.AddWithValue("@Rcode", entity.Rcode);
                    cmd.Parameters.AddWithValue("@Dcode", entity.Dcode);
                    cmd.Parameters.AddWithValue("@Status", entity.Status);
                    cmd.Parameters.AddWithValue("@Shopcode", entity.ShopCode);
                    cmd.Parameters.AddWithValue("@Reason", entity.Reason);
                    cmd.Parameters.AddWithValue("@IssueType", entity.IssueType);
                    cmd.Parameters.AddWithValue("@Address", entity.Address);
                    cmd.Parameters.AddWithValue("@URL", entity.VideoURL);
                    cmd.Parameters.AddWithValue("@ImageURL", entity.ImageURL);
                    cmd.Parameters.AddWithValue("@DocDate", entity.DocDate);
                    cmd.Parameters.AddWithValue("@CompletedDate", entity.CompletedDate);
                    cmd.ExecuteNonQuery();
                    objTrans.Commit();
                    cmd.Parameters.Clear();
                    cmd.Dispose();

                    //Mail sending
                    MailSending mail = new MailSending();
                    mail.SendMailForIncident(entity, 2);
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
        public string Get(string FDate, string TDate)
        {
            ManageSQLConnection manageSqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@FDate", FDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@TDate", TDate));
                ds = manageSqlConnection.GetDataSetValues("GetTheftDetails", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }

    }
}