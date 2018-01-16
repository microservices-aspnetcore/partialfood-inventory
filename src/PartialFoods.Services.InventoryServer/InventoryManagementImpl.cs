using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PartialFoods.Services;
using PartialFoods.Services.InventoryServer.Entities;

namespace PartialFoods.Services.InventoryServer
{
    public class InventoryManagementImpl : InventoryManagement.InventoryManagementBase
    {
        private IInventoryRepository repository;

        private ILogger logger;

        public InventoryManagementImpl(IInventoryRepository repository,
            ILogger<InventoryManagementImpl> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public override Task<GetQuantityResponse> GetEffectiveQuantity(GetQuantityRequest request, Grpc.Core.ServerCallContext context)
        {
            logger.LogInformation($"Received query for effective quantity of SKU {request.SKU}");
            int quantity = repository.GetCurrentQuantity(request.SKU);
            return Task.FromResult(new GetQuantityResponse { Quantity = (uint)quantity });
        }
    }
}