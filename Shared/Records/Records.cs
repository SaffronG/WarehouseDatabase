namespace Shared.Records {
    public record ItemRecord(int Id, string Name, int Quantity);
    public record PurchaseOrderRecord(int Id, int ItemId, int Quantity, DateTime OrderDate);
    public record ShipmentRecord(int Id, int ItemId, int Quantity, DateTime ShipmentDate);
}