using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        [HttpPost("{id}")]
        public Tuple<bool, string> Post(UesrProfileEntity entity)
        {
            try
            {
                ManageSQLConnection manageSQL = new ManageSQLConnection();
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@UserId", entity.UserId));
                sqlParameters.Add(new KeyValuePair<string, string>("@LocationURL", entity.LocationURL));
                sqlParameters.Add(new KeyValuePair<string, string>("@Dcode", entity.Dcode));
                sqlParameters.Add(new KeyValuePair<string, string>("@Rcode", entity.Rcode));
                sqlParameters.Add(new KeyValuePair<string, string>("@Name", entity.Name));
                sqlParameters.Add(new KeyValuePair<string, string>("@Address", entity.Address));
                sqlParameters.Add(new KeyValuePair<string, string>("@MailId", entity.MailId));
                sqlParameters.Add(new KeyValuePair<string, string>("@Phone", entity.Phone));
                sqlParameters.Add(new KeyValuePair<string, string>("@FileName", entity.FileName));
                sqlParameters.Add(new KeyValuePair<string, string>("@RMName", entity.RMName));
                sqlParameters.Add(new KeyValuePair<string, string>("@RMPhone", entity.RMPhone));
                sqlParameters.Add(new KeyValuePair<string, string>("@RMDistrict", entity.RMDistrict));
                sqlParameters.Add(new KeyValuePair<string, string>("@RMEmailId", entity.RMEmailId));
                var result= manageSQL.InsertData("InsertUserProfile", sqlParameters);
                return new Tuple<bool, string>(result, "Saved Successfully");

            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "Please Contact Administrator");

            }
        }

        [HttpGet("{id}")]
        public string Get(string Id)
        {
            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            DataSet ds = new DataSet();
            try
            {
                List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
                sqlParameters.Add(new KeyValuePair<string, string>("@UserId", Id.ToString()));
                ds = sqlConnection.GetDataSetValues("GetUserProfileDetails", sqlParameters);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            finally
            {
                ds.Dispose();
            }
        }

    }

    public class UesrProfileEntity
    {
        public string UserId { get; set; }
        public string LocationURL { get; set; }
        public string Dcode { get; set; }
        public string Rcode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string MailId { get; set; }
        public string Phone { get; set; }
        public string FileName { get; set; }
        public string RMName { get; set; }
        public string RMDistrict { get; set; }
        public string RMPhone { get; set; }
        public string RMEmailId { get; set; }
    }
}