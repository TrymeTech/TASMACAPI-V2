using Microsoft.AspNetCore.Mvc;
using WebApplication1.SQLConnection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {
        [HttpPut("{id}")]
        public bool Put(PsswordUpdate update)
        {
            ManageSQLConnection manageSQLConnection = new ManageSQLConnection();
            string Querystring = string.Empty;
            Querystring = "UPDATE profiles SET userpwd = '" + update.Pswd + "' WHERE userid = '" + update.UserId + "';";
            return manageSQLConnection.UpdateValuesByQuery(Querystring);
        }

    }
    public class PsswordUpdate
    {
        public string UserId { get; set; }
        public string Pswd { get; set; }
        public string OldPswd { get; set; }
    }
}