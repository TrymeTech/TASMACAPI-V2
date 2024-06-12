﻿using Microsoft.AspNetCore.Mvc;
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
    public class TicketDescriptionController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(TicketDescription descriptionEntity)
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
                    cmd.CommandText = "TicketDescription";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ticketID", descriptionEntity.ticketID);
                    cmd.Parameters.AddWithValue("@reporter", descriptionEntity.reporter);
                    cmd.Parameters.AddWithValue("@ticketdescription", descriptionEntity.ticketdescription);
                    cmd.Parameters.AddWithValue("@Status", descriptionEntity.Status);
                    cmd.Parameters.AddWithValue("@StatusCode", descriptionEntity.StatusCode);
                    cmd.Parameters.AddWithValue("@UserId", descriptionEntity.UserId);
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
        public string Get(string TicketID, int UserId)
        {
            // SQLConnection sqlConnection = new SQLConnection();
            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                if (TicketID == "A")
                {
                    sqlParameters.Add(new KeyValuePair<string, string>("@UserId", UserId.ToString()));
                    ds = sqlConnection.GetDataSetValues("GetMyTicket", sqlParameters);
                }
                else if (TicketID == "TD")
                {
                    sqlParameters.Add(new KeyValuePair<string, string>("@UserId", UserId.ToString()));
                    ds = sqlConnection.GetDataSetValues("GetTicketDescription", sqlParameters);
                }
                else
                {
                    sqlParameters.Add(new KeyValuePair<string, string>("@TicketID", TicketID));
                    ds = sqlConnection.GetDataSetValues("ticketbyid", sqlParameters);
                }
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
}
