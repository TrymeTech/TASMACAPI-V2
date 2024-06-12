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
    public class QuotationDetailsController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(QuotationEntity entity)
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
                    cmd.CommandText = "InsertQuotationDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", entity.Id);
                    cmd.Parameters.AddWithValue("@ProductId", entity.LocationId);
                    cmd.Parameters.AddWithValue("@Rcode", entity.RCode);
                    cmd.Parameters.AddWithValue("@Dcode", entity.DCode);
                    cmd.Parameters.AddWithValue("@StatusId", entity.StatusCode);
                    cmd.Parameters.AddWithValue("@ShopNumber", entity.ShopNumber);
                    cmd.Parameters.AddWithValue("@Remarks", entity.Remarks);
                    cmd.Parameters.AddWithValue("@ComponentId", entity.ComponentId);
                    cmd.Parameters.AddWithValue("@Address", entity.Address);
                    cmd.ExecuteNonQuery();
                    objTrans.Commit();
                    cmd.Parameters.Clear();
                    cmd.Dispose();

                    //Mail sending
                    MailSending mail = new MailSending();
                    mail.SendMailForQuotation(entity, 2);
                    return new Tuple<bool, string>(true, JsonConvert.SerializeObject(ds));
                }
                catch (Exception ex)
                {
                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                    objTrans.Rollback();
                    return new Tuple<bool, string>(false, "Please Contact Administrator!");
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
                ds = manageSqlConnection.GetDataSetValues("GetQuotationDetails", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }

    }
}