using Newtonsoft.Json;



namespace Business.Management.Appsettings.Models
{
    public class Persistence_AS_MODEL
    {
        public Pagination_AS_MODEL Pagination { get; set; } = new ();


        public class Pagination_AS_MODEL
        {
            [JsonProperty("DefaultPageNumber")]
            public int DefaultPageNumber { get; set; }
            [JsonProperty("DefaultPageSize")]
            public int DefaultPageSize { get; set; }

        }
    }
}
