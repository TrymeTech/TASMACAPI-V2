using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class RelocationEntity
    {
        public string Rcode { get; set; }
        public string Dcode { get; set; }
        public string ShopCode { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string DocDate { get; set; }
        public string CompletedDate { get; set; }
    }

    public class TheftEntity
    {
        public string Rcode { get; set; }
        public string Dcode { get; set; }
        public string ShopCode { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string IssueType { get; set; }
        public string DocDate { get; set; }
        public string CompletedDate { get; set; }
        public string URL { get; set; }
    }
}
