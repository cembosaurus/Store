using Business.Management.Enums;
using Microsoft.AspNetCore.Http;
using static Business.Management.Appsettings.Models.ServiceURL.ServiceType;

namespace Business.Management.Appsettings.Models
{
    public class ServiceURL
    {


        public string Name { get; set; }
        public IEnumerable<ServiceType> Type { get; set; }



        public class ServiceType
        {
            private string _name;

            public string Name 
            {
                get { return _name; }
                set 
                {
                    //foreach (var st in Enum.GetValues(typeof(TypeOfService)))
                    //{
                    //    _name = value == st.ToString() ? value : TypeOfService.Undefined.ToString();
                    //}
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
            public IEnumerable<URLPath> Paths { get; set; }




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


        public string GetUrl(TypeOfService type, bool isProdEnv)
        {
            var url = Type.FirstOrDefault(t => t.Name == type.ToString()).BaseURL;

            return isProdEnv ? url.Prod : url.Dev;
        }


        public string GetPathByName(string name, TypeOfService type)
        {
            return Type.FirstOrDefault(t => t.Name == type.ToString()).Paths.FirstOrDefault(p => p.Name == name).Route;
        }


        public IEnumerable<URLPath> GetPaths(TypeOfService type)
        { 
            return Type.Where(t => t.Name == type.ToString()).SelectMany(s => s.Paths);
        }

    }
}
