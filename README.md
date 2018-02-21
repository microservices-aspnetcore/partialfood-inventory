# Inventory
Inventory service for the "Partial Foods" sample demonstrating event sourcing and gRPC services in .NET Core.

Once the server is running, you can query it with `grpcurl`:

```
$ grpcurl -k ls localhost:8082 PartialFoods.Services.InventoryManagement
PartialFoods.Services.InventoryManagement.GetEffectiveQuantity
PartialFoods.Services.InventoryManagement.GetActivity
```

Here's an example query of activity belonging to the SKU **ABC123**:

```
$ echo '{"SKU": "ABC123"}' | grpcurl -k call localhost:8082 PartialFoods.Services.InventoryManagement.GetActivity
{"Activities":[{"SKU":"ABC123","Timestamp":1,"Quantity":10,"ActivityType":"RESERVED","ActivityID":"6b082670-1a4e-43f5-8a67-3c71a4c1feef","OrderID":"DEMO"},{"SKU":"ABC123","Timestamp":2,"Quantity":10,"ActivityType":"RELEASED","ActivityID":"9f68ab19-cef3-4e57-a1cd-c8b21cc060fd","OrderID":"DEMO"}]}
```