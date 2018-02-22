using System;
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

        public override Task<GetQuantityResponse> GetEffectiveQuantity(GetProductRequest request, Grpc.Core.ServerCallContext context)
        {
            logger.LogInformation($"Received query for effective quantity of SKU {request.SKU}");
            int quantity = repository.GetCurrentQuantity(request.SKU);
            return Task.FromResult(new GetQuantityResponse { Quantity = (uint)quantity });
        }

        public override Task<ActivityResponse> GetActivity(GetProductRequest request, Grpc.Core.ServerCallContext context)
        {
            logger.LogInformation($"Received query for product activity for SKU {request.SKU}");
            var response = new ActivityResponse();
            try
            {
                var activities = this.repository.GetActivity(request.SKU);
                foreach (var activity in activities)
                {
                    response.Activities.Add(new PartialFoods.Services.Activity
                    {
                        SKU = activity.SKU,
                        ActivityID = activity.ActivityID,
                        Timestamp = (ulong)activity.CreatedOn,
                        OrderID = activity.OrderID,
                        Quantity = (uint)activity.Quantity,
                        ActivityType = ToProtoActivityType(activity.ActivityType)
                    });
                }
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve activity for SKU : {}", request.SKU);
                return (Task<ActivityResponse>)Task.FromException(ex);
            }
        }

        private PartialFoods.Services.ActivityType ToProtoActivityType(
            PartialFoods.Services.InventoryServer.Entities.ActivityType at)
        {
            if (at == PartialFoods.Services.InventoryServer.Entities.ActivityType.Released)
            {
                return PartialFoods.Services.ActivityType.Released;
            }
            else if (at == PartialFoods.Services.InventoryServer.Entities.ActivityType.Shipped)
            {
                return PartialFoods.Services.ActivityType.Shipped;
            }
            else if (at == PartialFoods.Services.InventoryServer.Entities.ActivityType.Reserved)
            {
                return PartialFoods.Services.ActivityType.Reserved;
            }
            else if (at == PartialFoods.Services.InventoryServer.Entities.ActivityType.StockAdd)
            {
                return PartialFoods.Services.ActivityType.Stockadd;
            }
            else
            {
                return PartialFoods.Services.ActivityType.Unknown;
            }
        }
    }
}