using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
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
