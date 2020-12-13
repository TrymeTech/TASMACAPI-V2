using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using TNCSCAPI;
using WebApplication1.Models;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsertNewTicketController : ControllerBase
    {
            MySqlCommand cmd = new MySqlCommand();

            [HttpPost("{id}")]
            public Tuple<bool, string> Post(NewTicketEntity ticketEntity)
            {
                //  ManageSQLConnection sqlConnection = new ManageSQLConnection();
                MySqlConnection sqlConnection = new MySqlConnection();
                MySqlTransaction objTrans = null;
                string connectionString = "Server=127.0.0.1;Database=tncscbug;Uid=root;Pwd=54321;";
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
                        cmd.CommandText = "InsertNewTicket";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Region", ticketEntity.Region);
                        cmd.Parameters.AddWithValue("@District", ticketEntity.District);
                        cmd.Parameters.AddWithValue("@Shops", ticketEntity.Shops);
                        cmd.Parameters.AddWithValue("@assingedTo", ticketEntity.assingedTo);
                        cmd.Parameters.AddWithValue("@Ticketseverity", ticketEntity.Ticketseverity);
                        cmd.Parameters.AddWithValue("@Ticketstatus", ticketEntity.Ticketstatus);
                        cmd.Parameters.AddWithValue("@short_desc", ticketEntity.short_desc);
                        cmd.Parameters.AddWithValue("@product", ticketEntity.product);
                        cmd.Parameters.AddWithValue("@reporter", ticketEntity.reporter);
                        cmd.Parameters.AddWithValue("@component_id", ticketEntity.component_id);
                        cmd.Parameters.AddWithValue("@everconfirmed", ticketEntity.everconfirmed);
                        cmd.Parameters.AddWithValue("@reporter_accessible", ticketEntity.reporter_accessible);
                        cmd.Parameters.AddWithValue("@cclist_accessible", ticketEntity.cclist_accessible);
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
            public string Get(string FDate, string TDate, string RCode, string DCode, string Product, string Comopnent)
            {
                // SQLConnection sqlConnection = new SQLConnection();
                ManageSQLConnection sqlConnection = new ManageSQLConnection();
                DataSet ds = new DataSet();
                try
                {
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
                    sqlParameters.Add(new KeyValuePair<string, string>("@DCode", DCode));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Product", Product));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Comopnent", Comopnent));
                    sqlParameters.Add(new KeyValuePair<string, string>("@fromdate", FDate));
                    sqlParameters.Add(new KeyValuePair<string, string>("@todate", TDate));
                    ds = sqlConnection.GetDataSetValues("GetTickets", sqlParameters);
                    return JsonConvert.SerializeObject(ds.Tables[0]);
                }
                finally
                {
                    ds.Dispose();
                }
            }

            [HttpPut("{id}")]
            public bool Put(NewTicketEntity entity)
            {
                ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@ID", (entity.product).ToString()));
                sqlParameters.Add(new KeyValuePair<string, string>("@ClosedDate", entity.Region));
                return manageSQLConnection.UpdateValues("GetMyTickets", sqlParameters);
            }

        }
    }