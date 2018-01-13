namespace PartialFoods.Services.InventoryServer.Entities
{
    public interface IInventoryRepository
    {
        ProductActivity PutActivity(ProductActivity activity);
        Product GetProduct(string sku);
        int GetCurrentQuantity(string sku);
    }
}