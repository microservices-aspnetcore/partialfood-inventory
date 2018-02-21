using System;
using System.Collections.Generic;
using System.Linq;

namespace PartialFoods.Services.InventoryServer.Entities
{
    public class InventoryRepository : IInventoryRepository
    {
        private InventoryContext context;

        public InventoryRepository(InventoryContext context)
        {
            this.context = context;
        }

        public int GetCurrentQuantity(string sku)
        {
            try
            {
                var product = context.Products.FirstOrDefault(p => p.SKU == sku);
                if (product != null)
                {
                    var productActivities = (from activity in context.Activities
                                             where activity.SKU == sku
                                             orderby activity.CreatedOn
                                             select activity).ToArray();

                    // Reserve decreases quantity, Ship has no effect (already decreased), Released increases
                    var quantity = product.OriginalQuantity;
                    foreach (var activity in productActivities)
                    {
                        if (activity.ActivityType == ActivityType.Released)
                        {
                            quantity += activity.Quantity;
                        }
                        else if (activity.ActivityType == ActivityType.Reserved)
                        {
                            quantity -= activity.Quantity;
                        }
                    }
                    return quantity;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine($"Failed to query current quantity: {ex.ToString()}");
            }
            return 0;
        }

        public Product GetProduct(string sku)
        {
            try
            {
                var product = context.Products.FirstOrDefault(p => p.SKU == sku);
                return product;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine($"Failed to query product - {sku}");
            }
            return null;
        }

        public IList<ProductActivity> GetActivity(string sku)
        {
            var activities = (from activity in context.Activities
                              where activity.SKU == sku
                              orderby activity.CreatedOn ascending
                              select activity).ToList();
            return activities;
        }

        public ProductActivity PutActivity(ProductActivity activity)
        {
            Console.WriteLine($"Attempting to put activity {activity.ActivityID}, type {activity.ActivityType.ToString()}");

            try
            {
                var existing = context.Activities.FirstOrDefault(a => a.ActivityID == activity.ActivityID);
                if (existing != null)
                {
                    Console.WriteLine($"Bypassing add for order activity {activity.ActivityID} - already exists.");
                    return activity;
                }
                context.Add(activity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to store activity {ex.ToString()}");
                Console.WriteLine(ex.StackTrace);
                return null;
            }
            return activity;
        }
    }
}