using Business.Libraries.ServiceResult.Interfaces;
using Inventory.InventoryBusinessLogic.Interfaces;

namespace Inventory.InventoryBusinessLogic
{
    public class InventoryBusinessLogic : ICatalogueItemBusinessLogic
    {
        private readonly IConfiguration _config;
        private readonly IServiceResultFactory _resultFact;

        public InventoryBusinessLogic(IConfiguration config, IServiceResultFactory resultFact)
        {
            _config = config;
            _resultFact = resultFact;
        }










    }
}
