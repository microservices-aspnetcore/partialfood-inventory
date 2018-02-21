using System;
using PartialFoods.Services.InventoryServer.Entities;

namespace PartialFoods.Services.InventoryServer
{
    public class InventoryReleasedEventProcessor
    {
        private IInventoryRepository repository;

        public InventoryReleasedEventProcessor(IInventoryRepository repository)
        {
            this.repository = repository;
        }

        public bool HandleInventoryReleasedEvent(InventoryReleasedEvent evt)
        {
            Console.WriteLine($"Handling inventory released event - {evt.EventID}");
            ProductActivity activity = new ProductActivity
            {
                OrderID = evt.OrderID,
                SKU = evt.SKU,
                Quantity = (int)evt.Quantity,
                ActivityID = evt.EventID,
                CreatedOn = DateTime.UtcNow.Ticks,
                ActivityType = PartialFoods.Services.InventoryServer.Entities.ActivityType.Released
            };
            var result = repository.PutActivity(activity);

            return (result != null);
        }
    }
}