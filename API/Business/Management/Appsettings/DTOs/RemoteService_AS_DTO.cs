namespace Business.Management.Appsettings.DTOs
{
    public class RemoteService_AS_DTO
    {


        public string Name { get; set; }
        public IEnumerable<ServiceType_DTO> Type { get; set; }
        public bool IsHTTPClient { get; set; }



        public class ServiceType_DTO
        {

            public string Name { get; set; }
            public SchemeHostPort_DTO BaseURL { get; set; }
            public IEnumerable<URLPath_DTO> Paths { get; set; }


            public class SchemeHostPort_DTO
            {
                public string Dev { get; set; } = "";
                public string Prod { get; set; } = "";
            }

            public class URLPath_DTO
            {
                public string Name { get; set; } = "";
                public string Route { get; set; } = "";
            }


        }

    }
}
