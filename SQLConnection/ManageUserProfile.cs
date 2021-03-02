using System.Data;

namespace WebApplication1.SQLConnection
{
    public class ManageUserProfile
    {

        public bool CheckData(DataSet ds)
        {
            bool isAvailable = false;
            if (ds.Tables.Count > 0)
            {
                isAvailable = true;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    isAvailable = true;
                }
                else
                {
                    isAvailable = false;
                }
            }
            return isAvailable;
        }

    }
}
