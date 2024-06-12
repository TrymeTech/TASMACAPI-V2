using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Mail;
using WebApplication1.Models;
using WebApplication1.SQLConnection;

namespace WebApplication1.Mail
{
    public class MailSending
    {
        public bool Send(MailEntity mailEntity)
        {
            bool isSend = false;
            string CCMailId = string.Empty;
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(mailEntity.FromMailid);
                //Add mutiple too
                string[] ToMuliId = mailEntity.ToMailid.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string ToEMailId in ToMuliId)
                {
                    message.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                }
                if (!string.IsNullOrEmpty(mailEntity.ToCC) && mailEntity.ToCC != null)
                {
                    string[] ToMuliCC = mailEntity.ToCC.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string CCEMailId in ToMuliCC)
                    {
                        if (!string.IsNullOrEmpty(CCEMailId) && CCEMailId != null && CCEMailId.Contains("@"))
                        {
                            message.CC.Add(new MailAddress(CCEMailId)); //adding multiple CC Email Id
                        }
                    }
                }

                CCMailId = GetToCCMailid();
                if (!string.IsNullOrEmpty(CCMailId) && CCMailId != null)
                {
                    string[] ToMuliCC = CCMailId.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string CCEMailId in ToMuliCC)
                    {
                        message.CC.Add(new MailAddress(CCEMailId)); //adding multiple CC Email Id
                    }
                }
                message.Subject = mailEntity.Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = mailEntity.BodyMessage;
                smtp.Port = mailEntity.Port;
                smtp.Host = mailEntity.SMTP; //for gmail host  
                smtp.UseDefaultCredentials = false;
                smtp.Timeout = 200000;
                smtp.Credentials = new NetworkCredential(mailEntity.FromMailid, mailEntity.FromPassword);
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                isSend = true;
            }
            catch (Exception ex)
            {
                isSend = false;
                AuditLog.WriteError(ex.Message);
            }
            return isSend;
        }

        public string GetToCCMailid()
        {
            string toMailid = string.Empty;
            try
            {

                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@mailtypes", "5"));
                ManageSQLConnection sqlConnection = new ManageSQLConnection();
                DataSet ds = new DataSet();
                ds = sqlConnection.GetDataSetValues("Getmailsettings", sqlParameters);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        toMailid = Convert.ToString(ds.Tables[0].Rows[0]["ccmailid"]);
                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError("GetToCCMailid " + ex.Message);
            }


            return toMailid;
        }

        public string BodyMessage(BodyMessageEntity bodyMessageEntity, int Id, string Assignee)
        {
            string messageBody = string.Empty, htmlTableStart = string.Empty, htmlTableEnd = string.Empty,
               htmlHeadertdStart = string.Empty, htmlHeadertdEnd = string.Empty, htmlTrStart = string.Empty,
               htmlTrEnd = string.Empty, htmlTdStart = string.Empty, htmlTdEnd = string.Empty;
            try
            {
                messageBody = "<font>Hi Team, </font><br><br>";
                htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
                htmlTableEnd = "</table>";
                htmlHeadertdStart = "<td style=\"background-color:#6FA1D2; color:#ffffff;\">";
                htmlHeadertdEnd = "</td>";
                htmlTrStart = "<tr style=\"color:#555555;\">";
                htmlTrEnd = "</tr>";
                htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">";
                htmlTdEnd = "</td>";
                messageBody += "Please go through the ticket details <br><br>";
                messageBody += htmlTableStart;
                messageBody += htmlTrStart + htmlHeadertdStart + "Ticket Id " + htmlHeadertdEnd
                    + htmlTdStart + ":" + htmlTdEnd
                    + htmlTdStart + Convert.ToString(Id) + htmlTdEnd + htmlTrEnd;

                messageBody += htmlTrStart + htmlHeadertdStart + "Location Name" + htmlHeadertdEnd
                    + htmlTdStart + ":" + htmlTdEnd
                    + htmlTdStart + bodyMessageEntity.Location + htmlTdEnd + htmlTrEnd;

                messageBody += htmlTrStart + htmlHeadertdStart + "Regional Office" + htmlHeadertdEnd
                    + htmlTdStart + ":" + htmlTdEnd
                    + htmlTdStart + bodyMessageEntity.RegionalOffice + htmlTdEnd + htmlTrEnd;

                messageBody += htmlTrStart + htmlHeadertdStart + "District Office" + htmlHeadertdEnd
                    + htmlTdStart + ":" + htmlTdEnd
                    + htmlTdStart + bodyMessageEntity.DistrictOffice + htmlTdEnd + htmlTrEnd;

                messageBody += htmlTrStart + htmlHeadertdStart + "Shop Code" + htmlHeadertdEnd
                    + htmlTdStart + ":" + htmlTdEnd
                    + htmlTdStart + bodyMessageEntity.ShopCode + htmlTdEnd + htmlTrEnd;

                messageBody += htmlTrStart + htmlHeadertdStart + "Component Name" + htmlHeadertdEnd
                    + htmlTdStart + ":" + htmlTdEnd
                    + htmlTdStart + bodyMessageEntity.Component + htmlTdEnd + htmlTrEnd;

                messageBody += htmlTrStart + htmlHeadertdStart + "Component Description" + htmlHeadertdEnd
                    + htmlTdStart + ":" + htmlTdEnd
                    + htmlTdStart + bodyMessageEntity.ComponentDescription + htmlTdEnd + htmlTrEnd;

                messageBody += htmlTrStart + htmlHeadertdStart + "Assignee" + htmlHeadertdEnd
                    + htmlTdStart + ":" + htmlTdEnd
                    + htmlTdStart + Assignee + htmlTdEnd + htmlTrEnd;

                messageBody += htmlTrStart + htmlHeadertdStart + "Status" + htmlHeadertdEnd
                    + htmlTdStart + ":" + htmlTdEnd
                    + htmlTdStart + bodyMessageEntity.Status + htmlTdEnd + htmlTrEnd;

                messageBody += htmlTrStart + htmlHeadertdStart + "Description" + htmlHeadertdEnd
                    + htmlTdStart + ":" + htmlTdEnd
                    + htmlTdStart + bodyMessageEntity.TicketDescription + htmlTdEnd + htmlTrEnd;
                messageBody += htmlTableEnd + "<br><br>";

                messageBody += "Thanks & Regads <br>";
                messageBody += " SI Support Team";
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
            }
            finally
            {
                htmlTableStart = string.Empty;
                htmlTableEnd = string.Empty;
                htmlHeadertdStart = string.Empty;
                htmlHeadertdEnd = string.Empty;
                htmlTrStart = string.Empty;
                htmlTrEnd = string.Empty;
                htmlTdStart = string.Empty;
                htmlTdEnd = string.Empty;
            }
            return messageBody;
        }

        public bool SendForAll(CommonEntity mailEntity)
        {
            bool isSend = false;
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(GlobalVariables.FromMailid);
                //Add mutiple too
                string[] ToMuliId = mailEntity.ToMailid.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string ToEMailId in ToMuliId)
                {
                    message.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                }
                //message.To.Add(new MailAddress("dulasimca@gmail.com"));

                if (!string.IsNullOrEmpty(mailEntity.ToCC) && mailEntity.ToCC != null)
                {
                    string[] ToMuliCC = mailEntity.ToCC.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string CCEMailId in ToMuliCC)
                    {
                        message.CC.Add(new MailAddress(CCEMailId)); //adding multiple CC Email Id
                    }
                }

                //Add multiple CC
                message.Subject = mailEntity.Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = mailEntity.BodyMessage;
                smtp.Port = GlobalVariables.Port;
                smtp.Host = GlobalVariables.Host; //for gmail host  
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(GlobalVariables.FromMailid, GlobalVariables.Password);
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                isSend = true;
            }
            catch (Exception ex)
            {
                isSend = false;
                AuditLog.WriteError(ex.Message);
            }
            return isSend;
        }

        public void SendMailForIncident(TheftEntity entity, int type = 0)
        {
            try
            {
                if (type != 0)
                {

                    //Get the mailid from tables.
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@mailtypes", type.ToString()));
                    ManageSQLConnection sqlConnection = new ManageSQLConnection();
                    DataSet ds = new DataSet();
                    ds = sqlConnection.GetDataSetValues("Getmailsettings", sqlParameters);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            CommonEntity common = new CommonEntity
                            {

                                ToMailid = Convert.ToString(ds.Tables[0].Rows[0]["tomailid"]),
                                ToCC = Convert.ToString(ds.Tables[0].Rows[0]["ccmailid"]),
                                Subject = Convert.ToString(ds.Tables[0].Rows[0]["subjects"]),
                                BodyMessage = "Hi Rajaram, <br/> Reason :" + entity.Reason + " <br/>"
                           + "Address = " + entity.Address + "<br/>"
                           + "Issues Type = " + entity.IssueType + "<br/>"
                           + "Shop Code = " + entity.ShopCode + "<br/>"
                           + "Incident Date = " + entity.DocDate + "<br/><br/>"
                           + "Regards" + "<br/>"
                           + "SI Team"

                            };
                            SendForAll(common);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
            }

        }


        public void SendMailForQuotation(QuotationEntity entity, int type = 0)
        {
            try
            {
                if (type != 0)
                {

                    //Get the mailid from tables.
                    List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                    sqlParameters.Add(new KeyValuePair<string, string>("@mailtypes", type.ToString()));
                    ManageSQLConnection sqlConnection = new ManageSQLConnection();
                    DataSet ds = new DataSet();
                    ds = sqlConnection.GetDataSetValues("Getmailsettings", sqlParameters);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            CommonEntity common = new CommonEntity
                            {

                                ToMailid = Convert.ToString(ds.Tables[0].Rows[0]["tomailid"]),
                                ToCC = Convert.ToString(ds.Tables[0].Rows[0]["ccmailid"]),
                                Subject = Convert.ToString(ds.Tables[0].Rows[0]["subjects"]),
                                BodyMessage = "Hi Rajaram, <br/> Reason :" + entity.Remarks + " <br/>"
                           + "Address = " + entity.Address + "<br/>"
                           + "Issues Type = " + entity.ComponentId + "<br/>"
                           + "Shop Code = " + entity.ShopNumber + "<br/>"
                           + "Quotation Request Date = " + DateTime.Now + "<br/><br/>"
                           + "Regards" + "<br/>"
                           + "SI Team"

                            };
                            SendForAll(common);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AuditLog.WriteError(ex.Message);
            }


        }
    }

    public class MailEntity
    {
        public string FromMailid { get; set; }
        public string FromPassword { get; set; }
        public string ToMailid { get; set; }
        public string ToCC { get; set; }
        public int Port { get; set; }
        public string Subject { get; set; }
        public string BodyMessage { get; set; }
        public string SMTP { get; set; }
    }

    public class CommonEntity
    {
        public string ToMailid { get; set; }
        public string ToCC { get; set; }
        public string Subject { get; set; }
        public string BodyMessage { get; set; }
    }

    public class BodyMessageEntity
    {
        public string TicketId { get; set; }
        public string Location { get; set; }
        public string RegionalOffice { get; set; }
        public string DistrictOffice { get; set; }
        public string ShopCode { get; set; }
        public string Component { get; set; }
        public string ComponentDescription { get; set; }
        public string Assignee { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public string TicketDescription { get; set; }
        public string TOCC { get; set; }
    }
}
