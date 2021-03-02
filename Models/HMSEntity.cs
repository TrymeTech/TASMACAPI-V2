using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class RelocationEntity
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public string Rcode { get; set; }
        public string Dcode { get; set; }
        public string ShopCode { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string DocDate { get; set; }
        public string CompletedDate { get; set; }
        public string NewShopNo { get; set; }
    }

    public class TheftEntity
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public string Rcode { get; set; }
        public string Dcode { get; set; }
        public string ShopCode { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string IssueType { get; set; }
        public string DocDate { get; set; }
        public string CompletedDate { get; set; }
        public string VideoURL { get; set; }
        public string ImageURL { get; set; }
    }

    public class CameraStatusEntity
    {
        public string Id { get; set; }
        public string RCode { get; set; }
        public string DCode { get; set; }
        public string ShopNo { get; set; }
        public string Remarks { get; set; }
        public string Hours { get; set; }
        public string isActive { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CreatedDate { get; set; }
    }
}
