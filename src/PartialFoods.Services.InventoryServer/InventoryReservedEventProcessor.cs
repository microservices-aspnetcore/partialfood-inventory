using System;
using PartialFoods.Services.InventoryServer.Entities;

namespace PartialFoods.Services.InventoryServer
{
    public class InventoryReservedEventProcessor
    {
        private IInventoryRepository repository;

        public InventoryReservedEventProcessor(IInventoryRepository repository)
        {
            this.repository = repository;
        }

        public bool HandleInventoryReservedEvent(InventoryReservedEvent evt)
        {
            Console.WriteLine($"Handling inventory reserved event - {evt.EventID}");
            ProductActivity activity = new ProductActivity
            {
                OrderID = evt.OrderID,
                SKU = evt.SKU,
                Quantity = (int)evt.Quantity,
                ActivityID = evt.EventID,
                CreatedOn = DateTime.UtcNow.Ticks,
                ActivityType = ActivityType.Reserved
            };
            var result = repository.PutActivity(activity);

            return (result != null);
        }
    }
}