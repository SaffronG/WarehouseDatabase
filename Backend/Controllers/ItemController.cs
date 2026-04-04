using Microsoft.AspNetCore.Mvc;
using Shared.Records;
using Backend.Service;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemController : Controller {
    private readonly ItemServices _itemService = new();

    [HttpPost("/create_purchase_order")]
    public async Task<IActionResult> CreatePurchaseOrder([FromBody] PurchaseOrderRecord purchaseOrder)
    {
        try
        {
            var itemService = new ItemServices();    
            await _itemService.CreatePurchaseOrderAsync(purchaseOrder.ItemId, purchaseOrder.Quantity);
        } 
        catch (Exception ex)
        {

        }
        return Ok();
    }
    [HttpPost("RecieveShipment")]
    public async Task<IActionResult> RecieveShipment([FromBody] ShipmentRecord shipment)
    {
        try
        {
            var itemService = new ItemServices();    
            await _itemService.RecieveShipmentAsync(shipment.ItemId, shipment.Quantity);
        } 
        catch (Exception ex)
        {
            
        }
        return Ok();
    }
}