using Business.Management.Enums;
using static Business.Management.Appsettings.Models.Service_Model_AS.ServiceType;



namespace Business.Management.Appsettings.Models
{
    public class Service_Model_AS
    {


        public string Name { get; set; }
        public ICollection<ServiceType> Type { get; set; } = new List<ServiceType>();



        public class ServiceType
        {

            private string _name;


            public string Name 
            {
                get { return _name; }
                set 
                {
                    foreach (var st in Enum.GetValues(typeof(TypeOfService)))
                    {
                        if (value == st.ToString())
                        { 
                            _name = value;
                            break;
                        }
                        else {
                            _name = TypeOfService.Undefined.ToString();
                        }
                    }
                }
            }
            public SchemeHostPort BaseURL { get; set; }
            public ICollection<URLPath> Paths { get; set; } = new List<URLPath>();




            public class SchemeHostPort
            {
                public string Dev { get; set; }
                public string Prod { get; set; }
            }

            public class URLPath
            {
                public string Name { get; set; }
                public string Route { get; set; }
            }


        }




        public string GetBaseUrl(TypeOfService type, bool isProdEnv)
        {
            var url = Type.FirstOrDefault(t => t.Name == type.ToString()).BaseURL;

            return isProdEnv ? url.Prod : url.Dev;
        }


        public string GetPathByName(TypeOfService type, string name)
        {
            return Type.FirstOrDefault(t => t.Name == type.ToString()).Paths.FirstOrDefault(p => p.Name == name).Route;
        }


        public ICollection<URLPath> GetPaths(TypeOfService type)
        { 
            return Type.Where(t => t.Name == type.ToString()).SelectMany(s => s.Paths).ToList();
        }


        public string GetUrlWithPath(TypeOfService type, string pathName, bool isProdEnv)
        {
            var url = GetBaseUrl(type, isProdEnv);
            var path = GetPathByName(TypeOfService.REST, pathName);

            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(path))
                return "";

            return url += "/" + path;
        }

    }
}
