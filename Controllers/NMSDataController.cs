using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using WebApplication1.Models;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NMSDataController : ControllerBase
    {
        MySqlCommand cmd = new MySqlCommand();

        [HttpPost("{id}")]
        public Tuple<bool, string> Post(NMSEntity nmsEntity)
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
                    cmd.CommandText = "InsertNMSData";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@rcode", nmsEntity.RCode);
                    cmd.Parameters.AddWithValue("@dcode", nmsEntity.DCode);
                    cmd.Parameters.AddWithValue("@bug_id", nmsEntity.BugId);
                    cmd.Parameters.AddWithValue("@location", nmsEntity.Location);
                    cmd.Parameters.AddWithValue("@component_id", nmsEntity.Component);
                    cmd.Parameters.AddWithValue("@shop_number", nmsEntity.ShopNumber);
                    cmd.Parameters.AddWithValue("@sla_type", nmsEntity.SLAType);
                    cmd.Parameters.AddWithValue("@from_date", nmsEntity.FromDate);
                    cmd.Parameters.AddWithValue("@to_date", nmsEntity.ToDate);
                    cmd.Parameters.AddWithValue("@closed_date", nmsEntity.ClosedDate);
                    cmd.Parameters.AddWithValue("@reason_cd", nmsEntity.Reason);
                    cmd.Parameters.AddWithValue("@remarks", nmsEntity.Remarks);
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
        public string Get(string FDate, string TDate)
        {
            // SQLConnection sqlConnection = new SQLConnection();
            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@fromdate", FDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@todate", TDate));
                ds = sqlConnection.GetDataSetValues("GetNMSData", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }

        [HttpPut("{id}")]
        public bool Put(NMSEntity entity)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@ID", (entity.ID).ToString()));
            sqlParameters.Add(new KeyValuePair<string, string>("@ClosedDate", entity.ClosedDate));
            return manageSQLConnection.UpdateValues("UpdateNMSData", sqlParameters);
        }

    }
}