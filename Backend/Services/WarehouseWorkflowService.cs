using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Contracts;
using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class WarehouseWorkflowService
{
    private readonly AppDbContext _context;

    public WarehouseWorkflowService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseOrderResponse> CreatePurchaseOrderAsync(PurchaseOrderCreateRequest request)
    {
        if (request.Items is null || request.Items.Count == 0)
            throw new ArgumentException("Purchase order must contain one or more items.");

        var invalid = request.Items.Where(i => i.Quantity <= 0).ToList();
        if (invalid.Any())
            throw new ArgumentException("All purchase order item quantities must be greater than zero.");

        var itemIds = request.Items.Select(i => i.ItemId).Distinct().ToList();
        var existingItemIds = await _context.Items.Where(i => itemIds.Contains(i.Id)).Select(i => i.Id).ToListAsync();
        var missingItem = itemIds.Except(existingItemIds).FirstOrDefault();
        if (missingItem != default)
            throw new InvalidOperationException($"Item {missingItem} does not exist.");

        var purchaseOrder = new PurchaseOrder
        {
            DateOrdered = DateTime.UtcNow
        };

        foreach (var line in request.Items)
        {
            purchaseOrder.OrderItems.Add(new OrderItem
            {
                ItemId = line.ItemId,
                Quantity = line.Quantity,
                UnitPrice = line.UnitPrice
            });
        }

        _context.PurchaseOrders.Add(purchaseOrder);
        await _context.SaveChangesAsync();

        return new PurchaseOrderResponse(
            purchaseOrder.Id,
            purchaseOrder.DateOrdered ?? DateTime.UtcNow,
            purchaseOrder.OrderItems.Select(i => new PurchaseOrderLineResponse(i.ItemId ?? 0, i.Quantity ?? 0, i.UnitPrice)).ToList());
    }

    public async Task<ShipmentResponse> CreateShipmentAsync(ShipmentCreateRequest request)
    {
        if (request.Items is null || request.Items.Count == 0)
            throw new ArgumentException("Shipment must contain one or more items.");

        var purchaseOrder = await _context.PurchaseOrders
            .Include(po => po.OrderItems)
            .FirstOrDefaultAsync(po => po.Id == request.PurchaseOrderId);

        if (purchaseOrder is null)
            throw new KeyNotFoundException("Purchase order not found.");

        var orderItemIds = purchaseOrder.OrderItems.Select(i => i.ItemId ?? 0).ToHashSet();
        if (request.Items.Any(item => !orderItemIds.Contains(item.ItemId)))
            throw new InvalidOperationException("Shipment items must belong to the linked purchase order.");

        var shipment = new Shipment
        {
            OrderId = purchaseOrder.Id
        };

        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0)
                throw new ArgumentException("All shipment item quantities must be greater than zero.");

            shipment.ItemShipments.Add(new ItemShipment
            {
                ItemId = item.ItemId,
                Quantity = item.Quantity
            });
        }

        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();

        return new ShipmentResponse(shipment.Id, shipment.OrderId ?? 0, shipment.DateReceived, shipment.ItemShipments.Select(i => new ShipmentLineResponse(i.ItemId ?? 0, i.Quantity ?? 0)).ToList());
    }

    public async Task<ShipmentResponse> ReceiveShipmentAsync(int shipmentId, ShipmentReceiveRequest request)
    {
        if (request.InventoryBins is null || request.InventoryBins.Count == 0)
            throw new ArgumentException("Receive shipment request must include at least one bin mapping.");

        var shipment = await _context.Shipments
            .Include(s => s.ItemShipments)
            .FirstOrDefaultAsync(s => s.Id == shipmentId);

        if (shipment is null)
            throw new KeyNotFoundException("Shipment not found.");

        if (shipment.DateReceived.HasValue)
        {
            return new ShipmentResponse(shipment.Id, shipment.OrderId ?? 0, shipment.DateReceived, shipment.ItemShipments.Select(i => new ShipmentLineResponse(i.ItemId ?? 0 , i.Quantity ?? 0)).ToList());
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        foreach (var mapping in request.InventoryBins)
        {
            var line = shipment.ItemShipments.FirstOrDefault(i => i.ItemId == mapping.ItemId);
            if (line is null)
                throw new InvalidOperationException($"Shipment does not contain item {mapping.ItemId}.");

            var bin = await _context.Bins.FirstOrDefaultAsync(b => b.Id == mapping.BinId);
            if (bin is null)
                throw new KeyNotFoundException($"Bin {mapping.BinId} not found.");

            if (bin.ItemId is not null && bin.ItemId != mapping.ItemId)
                throw new InvalidOperationException("A bin may only store a single product.");

            if (line.Quantity <= 0)
                throw new InvalidOperationException("Shipment quantities must be greater than zero.");

            bin.ItemId = mapping.ItemId;
            bin.Quantity = (bin.Quantity ?? 0) + line.Quantity;
        }

        shipment.DateReceived = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return new ShipmentResponse(shipment.Id, shipment.OrderId ?? 0, shipment.DateReceived, shipment.ItemShipments.Select(i => new ShipmentLineResponse(i.ItemId ?? 0, i.Quantity ?? 0)).ToList());
    }

    public async Task<InventoryResponse> StoreInventoryAsync(InventoryStoreRequest request)
    {
        if (request.Quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == request.ItemId);
        if (item is null)
            throw new KeyNotFoundException("Item not found.");

        var bin = await _context.Bins.FirstOrDefaultAsync(b => b.Id == request.BinId);
        if (bin is null)
            throw new KeyNotFoundException("Bin not found.");

        if (bin.ItemId is not null && bin.ItemId != request.ItemId)
            throw new InvalidOperationException("A bin may only store a single product.");

        bin.ItemId = request.ItemId;
        bin.Quantity = (bin.Quantity ?? 0) + request.Quantity;
        if (bin.Quantity < 0)
            throw new InvalidOperationException("Inventory cannot be negative.");

        await _context.SaveChangesAsync();
        return new InventoryResponse(bin.Id, bin.ItemId!.Value, bin.Quantity.Value);
    }

    public async Task<OrderResponse> CreateOrderAsync(OrderCreateRequest request)
    {
        if (request.Items is null || request.Items.Count == 0)
            throw new ArgumentException("Order must contain one or more items.");

        var invalid = request.Items.Where(i => i.Quantity <= 0).ToList();
        if (invalid.Any())
            throw new ArgumentException("All order item quantities must be greater than zero.");

        var itemIds = request.Items.Select(i => i.ItemId).Distinct().ToList();
        var existingItemIds = await _context.Items.Where(i => itemIds.Contains(i.Id)).Select(i => i.Id).ToListAsync();
        var missingItem = itemIds.Except(existingItemIds).FirstOrDefault();
        if (missingItem != default)
            throw new InvalidOperationException($"Item {missingItem} does not exist.");

        var order = new Order
        {
            DateOrdered = DateTime.UtcNow,
            Status = OrderStatus.Created.ToString().ToUpperInvariant()
        };

        foreach (var line in request.Items)
        {
            order.SalesOrderItems.Add(new SalesOrderItem
            {
                ItemId = line.ItemId,
                Quantity = line.Quantity,
                UnitPrice = 0
            });
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return new OrderResponse(order.Id, order.DateOrdered ?? DateTime.UtcNow, order.Status, order.SalesOrderItems.Select(i => new OrderLineResponse(i.ItemId, i.Quantity)).ToList());
    }

    public async Task<OrderResponse> PickOrderAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.SalesOrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        if (order.Status == OrderStatus.Picked.ToString().ToUpperInvariant())
            return BuildOrderResponse(order);

        if (order.Status != OrderStatus.Created.ToString().ToUpperInvariant())
            throw new InvalidOperationException("Order cannot be picked in its current status.");

        foreach (var line in order.SalesOrderItems)
        {
            var bin = await _context.Bins
                .Where(b => b.ItemId == line.ItemId && (b.Quantity ?? 0) >= line.Quantity)
                .OrderBy(b => b.Id)
                .FirstOrDefaultAsync();

            if (bin is null)
                throw new InvalidOperationException($"Insufficient inventory for item {line.ItemId}.");

            bin.Quantity = (bin.Quantity ?? 0) - line.Quantity;
            if (bin.Quantity < 0)
                throw new InvalidOperationException("Inventory cannot be negative.");
        }

        order.Status = OrderStatus.Picked.ToString().ToUpperInvariant();
        order.DatePicked = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return BuildOrderResponse(order);
    }

    public async Task<OrderResponse> PackOrderAsync(int orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        if (order.Status == OrderStatus.Packed.ToString().ToUpperInvariant())
            return BuildOrderResponse(order);

        if (order.Status != OrderStatus.Picked.ToString().ToUpperInvariant())
            throw new InvalidOperationException("Order must be picked before it can be packed.");

        order.Status = OrderStatus.Packed.ToString().ToUpperInvariant();
        order.DatePacked = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return BuildOrderResponse(order);
    }

    public async Task<OrderResponse> ShipOrderAsync(int orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        if (order.Status == OrderStatus.Shipped.ToString().ToUpperInvariant())
            return BuildOrderResponse(order);

        if (order.Status != OrderStatus.Packed.ToString().ToUpperInvariant())
            throw new InvalidOperationException("Order must be packed before it can be shipped.");

        order.Status = OrderStatus.Shipped.ToString().ToUpperInvariant();
        order.DateShipped = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return BuildOrderResponse(order);
    }

    private static OrderResponse BuildOrderResponse(Order order)
    {
        return new OrderResponse(
            order.Id,
            order.DateOrdered ?? DateTime.UtcNow,
            order.Status,
            order.SalesOrderItems.Select(i => new OrderLineResponse(i.ItemId, i.Quantity)).ToList());
    }
}
