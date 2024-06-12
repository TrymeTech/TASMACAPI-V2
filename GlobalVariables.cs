namespace WebApplication1
{
    public class GlobalVariables
    {
        //Dev setting
        //public const string ConnectionString = "Server=127.0.0.1;Database=tncscbug;Uid=root;Pwd=54321;";

        //Live old Uid Settings 127.0.0.1 125.17.108.118
        // public const string ConnectionString = "Server=127.0.0.1;Database=tasmac;Uid=tasmacbug;Pwd=Test1234$;";
        //public const string ConnectionString = "Server=127.0.0.1;Database=tasmac;Uid=tasmacbugnew;Pwd=Test1234$;";

        //Live Settings 127.0.0.1 125.17.108.118
        //public const string ConnectionString = "Server=localhost;Database=tasmac;Uid=tasMac;Pwd=tasMac1234$;";

        //Localhost
        public const string ConnectionString = "Server=localhost;Database=tasmac;Uid=root;Pwd=admin123;";




        //Mail settings
        //Mail settings
        public const string FromMailid = "alerts@bontonsoftwares.com"; //"si.bontonsoftwares@gmail.com";
        //public const string ToMailid = "shenbagamoorthyr@gmail.com";
        //public const string Password = "BonTon@12345%";
        public const string Password = "Bonton@1451";
        public const int Port = 587;
        //public const string Host = "webmail.bontonsoftwares.com";
        public const string Host = "smtp.gmail.com";
    }
}


//  FromMailid = "alerts@bontonsoftwares.com",  
//   ToMailid = "shenbagamoorthyr@gmail.com",
//  FromPassword = "BonTon@12345%",
//   ToCC = "",
//   SMTP = "webmail.bontonsoftwares.com",
//   Port = 587,
