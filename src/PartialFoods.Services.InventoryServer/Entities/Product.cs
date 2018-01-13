namespace PartialFoods.Services.InventoryServer.Entities
{
    public class Product
    {
        public string SKU { get; set; }
        public int OriginalQuantity { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}