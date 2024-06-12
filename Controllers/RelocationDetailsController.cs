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
    public class RelocationDetailsController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(RelocationEntity entity)
        {
            MySqlCommand cmd = new MySqlCommand();
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
                    cmd.CommandText = "InsertRelocationDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", entity.Id);
                    cmd.Parameters.AddWithValue("@Location", entity.Location);
                    cmd.Parameters.AddWithValue("@Rcode", entity.Rcode);
                    cmd.Parameters.AddWithValue("@Dcode", entity.Dcode);
                    cmd.Parameters.AddWithValue("@Status", entity.Status);
                    cmd.Parameters.AddWithValue("@ShopCode", entity.ShopCode);
                    cmd.Parameters.AddWithValue("@Reason", entity.Reason);
                    cmd.Parameters.AddWithValue("@FromAddress", entity.FromAddress);
                    cmd.Parameters.AddWithValue("@ToAddress", entity.ToAddress);
                    cmd.Parameters.AddWithValue("@DocDate", entity.DocDate);
                    cmd.Parameters.AddWithValue("@CompletedDate", entity.CompletedDate);
                    cmd.Parameters.AddWithValue("@NewShopNo", entity.NewShopNo);
                    cmd.ExecuteNonQuery();
                    objTrans.Commit();
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    //Mail sending
                    MailSending mail = new MailSending();
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@mailtypes", "1"));
                    ManageSQLConnection managesqlConnection = new ManageSQLConnection();
                    DataSet ds1 = new DataSet();
                    ds1 = managesqlConnection.GetDataSetValues("Getmailsettings", sqlParameters);
                    if (ds1.Tables.Count > 0)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            CommonEntity common = new CommonEntity
                            {
                                ToMailid = Convert.ToString(ds1.Tables[0].Rows[0]["tomailid"]),
                                ToCC = Convert.ToString(ds1.Tables[0].Rows[0]["ccmailid"]),
                                Subject = Convert.ToString(ds1.Tables[0].Rows[0]["subjects"]),
                                BodyMessage = "Hi Rajaram, <br/> Reason :" + entity.Reason + " <br/>"
                                    + "Region Name = " + entity.RegionName + "<br/>"
                                     + "Shop code = " + entity.ShopCode + "<br/>"
                                      + "New Shop Code = " + entity.NewShopNo + "<br/>"
                                    + "District Name  = " + entity.DistrictName + "<br/>"
                                      + "From Address = " + entity.FromAddress + "<br/>"
                                    + "To Address = " + entity.ToAddress + "<br/>"
                                    + "Relocation Date = " + entity.DocDate + "<br/><br/>"
                                    + "Regards" + "<br/>"
                                    + "SI Team"

                            };
                            mail.SendForAll(common);
                        }
                    }

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
                ds = manageSqlConnection.GetDataSetValues("GetRelocationDetails", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}