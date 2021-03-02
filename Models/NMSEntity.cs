using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Mail;

namespace WebApplication1.Models
{
    public class NewTicketEntity
    {
        public string Region { get; set; }
        public string District { get; set; }
        public string Shops { get; set; }
        public string assingedTo { get; set; }
        public string Ticketseverity { get; set; }
        public string Ticketstatus { get; set; }
        public string short_desc { get; set; }
        public int product { get; set; }
        public string UserId { get; set; }
        public string reporter { get; set; }
        public int component_id { get; set; }
        public bool everconfirmed { get; set; }
        public bool reporter_accessible { get; set; }
        public bool cclist_accessible { get; set; }
        public string URL { get; set; }
        public string CC { get; set; }
        public int ticket_id { get; set; }
        public int StatusCode { get; set; }
        public BodyMessageEntity bodyMessage { get; set; }

    }

    public class TicketDescription
    {
        public string ticketID { get; set; }
        public string reporter { get; set; }
        public string ticketdescription { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public int UserId { get; set; }
    }

    public class NMSEntity
    {
        public string RCode { get; set; }
        public string BugId { get; set; }
        public string Location { get; set; }
        public string Component { get; set; }
        public string DCode { get; set; }
        public int SLAType { get; set; }
        public string ShopNumber { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ClosedDate { get; set; }
        public string Remarks { get; set; }
        public int Reason { get; set; }
        public int ID { get; set; }
    }

    public class IncidentEntity
    {
        public string RCode { get; set; }
        public string DCode { get; set; }
        public int ShopCode { get; set; }
        public string DocDate { get; set; }
        public string ToDate { get; set; }
        public string URL { get; set; }
        public string Remarks { get; set; }
        public string Reason { get; set; }
    }
}
