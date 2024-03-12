

namespace Business.appsettings
{
    public struct RemoteService
    {
        public string Name { get; set; }
        public string BaseURL { get; set; }
        public string Type { get; set; }
        public IEnumerable<Subs> Paths { get; set; }


        public struct Subs
        {
            public string Name { get; set; }
            public string Route { get; set; }
        }
    }
}
