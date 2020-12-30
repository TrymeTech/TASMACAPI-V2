using System;
using System.Net;
using System.Net.Mail;

namespace WebApplication1.Mail
{
    public class MailSending
    {
        public bool Send(MailEntity mailEntity)
        {
            bool isSend = false;
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
                string[] ToMuliCC = mailEntity.ToCC.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string CCEMailId in ToMuliCC)
                {
                    message.CC.Add(new MailAddress(CCEMailId)); //adding multiple CC Email Id
                }
                //Add multiple CC
                message.Subject = mailEntity.Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = mailEntity.BodyMessage;
                smtp.Port = mailEntity.Port;
                smtp.Host = mailEntity.SMTP; //for gmail host  
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(mailEntity.FromMailid, mailEntity.FromPassword);
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
        public string BodyMessage(BodyMessageEntity bodyMessageEntity)
        {
            string messageBody = string.Empty, htmlTableStart=string.Empty, htmlTableEnd=string.Empty,
               htmlHeadertdStart=string.Empty, htmlHeadertdEnd=string.Empty, htmlTrStart=string.Empty,
               htmlTrEnd=string.Empty, htmlTdStart=string.Empty, htmlTdEnd=string.Empty;
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
                messageBody += htmlTrStart + htmlHeadertdStart  + "Ticket Id "  + htmlHeadertdEnd
                    + htmlTdStart + ":" + htmlTdEnd
                    + htmlTdStart + bodyMessageEntity.TicketId + htmlTdEnd+htmlTrEnd;

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
                    + htmlTdStart + bodyMessageEntity.Assignee + htmlTdEnd + htmlTrEnd;

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
                messageBody = string.Empty;
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

    }
}
