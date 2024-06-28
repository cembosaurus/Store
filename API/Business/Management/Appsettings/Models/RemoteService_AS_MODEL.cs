﻿using Business.Management.Enums;
using Microsoft.IdentityModel.Tokens;
using static Business.Management.Appsettings.Models.RemoteService_AS_MODEL.ServiceType;



namespace Business.Management.Appsettings.Models
{
    public class RemoteService_AS_MODEL
    {


        public string Name { get; set; } = "";
        public List<ServiceType> Type { get; set; } = new List<ServiceType>();



        public class ServiceType
        {

            private string _name = TypeOfService.Undefined.ToString();


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
            public SchemeHostPort BaseURL { get; set; } = new SchemeHostPort();
            public ICollection<URLPath> Paths { get; set; } = new List<URLPath>();




            public class SchemeHostPort
            {
                public string Dev { get; set; } = "";
                public string Prod { get; set; } = "";
            }

            public class URLPath
            {
                public string Name { get; set; } = "";
                public string Route { get; set; } = "";
            }


        }




        public string GetBaseUrl(TypeOfService typeName, bool isProdEnv)
        {
            var url = Type.FirstOrDefault(t => t.Name == typeName.ToString())?.BaseURL;

            return url == null ? 
                "" : 
                isProdEnv ? url.Prod : url.Dev;
        }


        public string GetPathByName(TypeOfService typeName, string name)
        {
            var path = Type.FirstOrDefault(t => t.Name == typeName.ToString())?.Paths.FirstOrDefault(p => p.Name == name)?.Route;

            return path ?? "";
        }


        public ICollection<URLPath> GetPaths(TypeOfService typeName)
        { 
            var paths = Type.Where(t => t.Name == typeName.ToString())?.SelectMany(s => s.Paths)?.ToList();

            return paths ?? new List<URLPath>();
        }


        public string GetUrlWithPath(TypeOfService typeName, string pathName, bool isProdEnv)
        {
            if (string.IsNullOrWhiteSpace(pathName) && GetPaths(typeName).IsNullOrEmpty())
                return "";

                var url = GetBaseUrl(typeName, isProdEnv);
                var path = GetPathByName(TypeOfService.REST, pathName);

            return string.IsNullOrWhiteSpace(url) && string.IsNullOrWhiteSpace(path) 
                ? "" 
                : url += "/" + path;
        }

    }
}
