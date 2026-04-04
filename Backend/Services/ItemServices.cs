namespace Backend.Services;

using Backend.Data;
using Backend.Entities;

public class ItemServices
{
    private readonly AppDbContext _ctxt = new();
    async public Task CreatePurchaseOrderAsync(int itemId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero!");
        }

        var purchaseOrder = new PurchaseOrder
        {
            DateOrdered = DateTime.UtcNow
        };

        var orderItem = new OrderItem
        {
            ItemId = itemId,
            Quantity = quantity
        };

        purchaseOrder.OrderItems.Add(orderItem);
        _ctxt.PurchaseOrders.Add(purchaseOrder);
        await _ctxt.SaveChangesAsync();
    }
    async public Task RecieveShipmentAsync(int itemId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero!");
        }

        var shipment = new Shipment
        {
            DateReceived = DateTime.UtcNow
        };

        var itemShipment = new ItemShipment
        {
            ItemId = itemId,
            Quantity = quantity
        };

        shipment.ItemShipments.Add(itemShipment);
        _ctxt.Shipments.Add(shipment);
        await _ctxt.SaveChangesAsync();
    }
}