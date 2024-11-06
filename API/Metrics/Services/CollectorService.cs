using Business.Libraries.ServiceResult.Interfaces;
using Metrics.Data.Repositories.Interfaces;
using Metrics.Services.Interfaces;
using Business.Metrics.DTOs;
using Metrics.Services.Tools.Interfaces;

namespace Metrics.Services
{

    public class CollectorService : ICollectorService
    {
        private readonly IMetricsDataHandler _processor;
        private readonly IMetricsRepository _repo;
        private readonly IServiceResultFactory _resultFact;



        public CollectorService(IMetricsDataHandler processor, IMetricsRepository repo, IServiceResultFactory resultFact)
        {
            _processor = processor;
            _repo = repo;
            _resultFact = resultFact;
        }





        public async Task<IServiceResult<bool>> AddHttpTransactionRecord(MetricsCreateDTO metricsData)
        {
            if (metricsData.Data != null)
            {
                var processorRersult = _processor.Inspect(metricsData.Data);
            }

            //if (await _repo.ExistsByName(itemCreateDTO.Name))
            //    return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{itemCreateDTO.Name}' already EXISTS !");

            //var item = _mapper.Map<Item>(itemCreateDTO);

            //var resultState = await _repo.AddItem(item);


            //if (resultState != EntityState.Added || _repo.SaveChanges() < 1)
            //    return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{itemCreateDTO.Name}' was NOT created");

            //return _resultFact.Result(result, true, message);

            return null;
        }

    }
}
