using Business.Management.Appsettings.Models;



namespace Business.Management.Data
{
    // singleton

    public class Config_Global_DB
    {

        public Config_Global_DB()
        {
            Data = new Config_Global_AS_MODEL();
        }


        public Config_Global_AS_MODEL Data;

    }
}
