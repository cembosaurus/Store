using AutoMapper;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data;



namespace Business.Management.Appsettings
{
    public class Persistence_REPO : IPersistence_REPO
    {
        private Config_Global_DB _db;
        private IMapper _mapper;



        public Persistence_REPO(Config_Global_DB db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }




        public Persistence_AS_MODEL Data => _db.Data.Persistence;

        public int DefaultPageNumber => _db.Data.Persistence.Pagination.DefaultPageNumber;

        public int DefaultPageSize => _db.Data.Persistence.Pagination.DefaultPageSize;



        public void Initialize(Persistence_AS_MODEL persistence) => _db.Data.Persistence = _mapper.Map<Persistence_AS_MODEL>(persistence);




        // To Do:
        //
        // Update
        // Create
        // Delete
    }
}
