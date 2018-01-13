using System.Threading.Tasks;
using PartialFoods.Services;
using PartialFoods.Services.InventoryServer.Entities;

namespace PartialFoods.Services.InventoryServer
{
    public class InventoryManagementImpl : InventoryManagement.InventoryManagementBase
    {
        private IInventoryRepository repository;

        public InventoryManagementImpl(IInventoryRepository repository)
        {
            this.repository = repository;
        }

        public override Task<GetQuantityResponse> GetEffectiveQuantity(GetQuantityRequest request, Grpc.Core.ServerCallContext context)
        {
            int quantity = repository.GetCurrentQuantity(request.SKU);
            return Task.FromResult(new GetQuantityResponse { Quantity = (uint)quantity });
        }
    }
}