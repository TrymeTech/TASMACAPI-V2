using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using WebApplication1.Mail;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        [HttpGet("{id}")]
        public bool Get(string EMailId)
        {

            ManageSQLConnection sqlConnection = new ManageSQLConnection();
            ManageUserProfile manageUser = new ManageUserProfile();
            DataSet ds = new DataSet();
            List<KeyValuePair<string, string>> sqlParameters = new List<KeyValuePair<string, string>>();
            sqlParameters.Add(new KeyValuePair<string, string>("@username", EMailId));
            ds = sqlConnection.GetDataSetValues("GetLoginDetails", sqlParameters);
            if (manageUser.CheckData(ds))
            {
                MailSending mail = new MailSending();
                //
                CommonEntity commonEntity = new CommonEntity
                {
                    Subject = "Forgot Password From HMS",
                    BodyMessage = "Hi, <br/> User Password is : " + ds.Tables[0].Rows[0]["userpwd"].ToString(),
                    ToMailid = EMailId,
                    ToCC = "",
                };
                mail.SendForAll(commonEntity);
                return true;
            }
            //Check Data
            return false;
        }
    }
}