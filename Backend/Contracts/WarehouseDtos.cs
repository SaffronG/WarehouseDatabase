namespace Backend.Contracts;

public record PurchaseOrderLine(int ItemId, int Quantity, decimal? UnitPrice = null);

public record PurchaseOrderCreateRequest(
    List<PurchaseOrderLine> Items
);

public record ShipmentItemCreateRequest(int ItemId, int Quantity);

public record ShipmentCreateRequest(
    int PurchaseOrderId,
    List<ShipmentItemCreateRequest> Items
);

public record ShipmentReceiveBinRequest(int ItemId, int BinId);

public record ShipmentReceiveRequest(List<ShipmentReceiveBinRequest> InventoryBins);

public record InventoryStoreRequest(int ItemId, int BinId, int Quantity);

public record OrderItemCreateRequest(int ItemId, int Quantity);

public record OrderCreateRequest(List<OrderItemCreateRequest> Items);

public record AuthRequest(string Email, string Password);

public record AuthResponse(string Token, string TokenType = "Bearer");

public record PurchaseOrderLineResponse(int ItemId, int Quantity, decimal? UnitPrice);

public record PurchaseOrderResponse(int Id, DateTime DateOrdered, IReadOnlyList<PurchaseOrderLineResponse> Items);

public record ShipmentLineResponse(int ItemId, int Quantity);

public record ShipmentResponse(int Id, int PurchaseOrderId, DateTime? DateReceived, IReadOnlyList<ShipmentLineResponse> Items);

public record InventoryResponse(int BinId, int ItemId, int Quantity);

public record OrderLineResponse(int ItemId, int Quantity);

public record OrderResponse(
    int Id, 
    DateTime DateOrdered, 
    string Status, 
    IReadOnlyList<OrderLineResponse> Items,
    DateTime? DatePicked = null,
    DateTime? DatePacked = null,
    DateTime? DateShipped = null);

public record InventoryItemResponse(
    int ItemId, 
    int TotalQuantity);

public record InventoryByBinResponse(
    int BinId, 
    int ItemId, 
    int Quantity);
