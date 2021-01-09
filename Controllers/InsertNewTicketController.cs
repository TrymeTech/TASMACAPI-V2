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
    public class InsertNewTicketController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string, string> Post(NewTicketEntity ticketEntity)
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
                    //  cmd.Parameters.AddWithValue("@URL", ticketEntity.URL);
                    cmd.Parameters.AddWithValue("@CC", ticketEntity.CC);
                    cmd.Parameters.AddWithValue("@everconfirmed", ticketEntity.everconfirmed);
                    cmd.Parameters.AddWithValue("@reporter_accessible", ticketEntity.reporter_accessible);
                    cmd.Parameters.AddWithValue("@cclist_accessible", ticketEntity.cclist_accessible);
                    //cmd.Parameters.AddWithValue("@ticket_id", ticketEntity.ticket_id).Direction = ParameterDirection.Output;
                    //string id = cmd.Parameters["@ticket_id"].Value.ToString();
                    //LblMessage.Text = "Record inserted successfully. ID = " + id;
                    int insertedID = Convert.ToInt32(cmd.ExecuteScalar());
                    string insertID = insertedID.ToString();
                    objTrans.Commit();
                    cmd.Parameters.Clear();
                    cmd.Dispose();

                    MailSending mailSending = new MailSending();
                    //Mail sending
                    MailEntity mailEntity = new MailEntity
                    {
                        FromMailid = GlobalVariables.FromMailid,
                        FromPassword = GlobalVariables.Password,
                        ToMailid = ticketEntity.assingedTo,
                        ToCC = ticketEntity.CC,
                        Port = GlobalVariables.Port,
                        Subject = ticketEntity.short_desc,
                        BodyMessage = mailSending.BodyMessage(ticketEntity.bodyMessage),
                        SMTP = GlobalVariables.Host
                    };
                    mailSending.Send(mailEntity);
                    return new Tuple<bool, string, string>(true, JsonConvert.SerializeObject(ds), insertID);
                }
                catch (Exception ex)
                {
                    AuditLog.WriteError(ex.Message + " : " + ex.StackTrace);
                    objTrans.Rollback();
                    return new Tuple<bool, string, string>(false, "Exception occured", "No value return");
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
        public string Get(string FDate, string TDate, string RCode, string DCode,
            string Product, string Component, string Shops, int type)
        {
            // SQLConnection sqlConnection = new SQLConnection();
            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            string procedureName = string.Empty;
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@fromdate", FDate));
                sqlParameters.Add(new KeyValuePair<string, string>("@todate", TDate));
                if (type == 1)
                {
                    procedureName = "GetTicketsByDate";
                }
                else
                {
                    sqlParameters.Add(new KeyValuePair<string, string>("@RCode", RCode));
                    sqlParameters.Add(new KeyValuePair<string, string>("@DCode", DCode));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Product", Product));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Component", Component));
                    sqlParameters.Add(new KeyValuePair<string, string>("@Shops", Shops));
                    procedureName = "GetTickets";
                }
                ds = sqlConnection.GetDataSetValues(procedureName, sqlParameters);
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
            sqlParameters.Add(new KeyValuePair<string, string>("@TicketID", (entity.ticket_id).ToString()));
            sqlParameters.Add(new KeyValuePair<string, string>("@assingedTo", (entity.assingedTo).ToString()));
            sqlParameters.Add(new KeyValuePair<string, string>("@Ticketstatus", entity.Ticketstatus));
            sqlParameters.Add(new KeyValuePair<string, string>("@short_desc", entity.short_desc));
            sqlParameters.Add(new KeyValuePair<string, string>("@URL", entity.URL));
            sqlParameters.Add(new KeyValuePair<string, string>("@CC", entity.CC));
           var result=  manageSQLConnection.UpdateValues("UpdateTickets", sqlParameters);

            MailSending mailSending = new MailSending();
            //Mail sending
            MailEntity mailEntity = new MailEntity
            {
                FromMailid = GlobalVariables.FromMailid,
                FromPassword = GlobalVariables.Password,
                ToMailid = entity.assingedTo,
                ToCC = entity.CC,
                Port = GlobalVariables.Port,
                Subject = entity.short_desc,
                BodyMessage = mailSending.BodyMessage(entity.bodyMessage),
                SMTP = GlobalVariables.Host
            };
            mailSending.Send(mailEntity);

            return result;
        }

    }
}