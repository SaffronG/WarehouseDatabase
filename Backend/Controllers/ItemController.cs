using Microsoft.AspNetCore.Mvc;
using Shared.Records;
using Backend.Services;
using Backend.Data;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemController : Controller {
    private readonly ItemServices _itemService;

    public ItemController(AppDbContext context)
    {
        _itemService = new ItemServices(context);
    }

    [HttpPost("/create_purchase_order")]
    public async Task<IActionResult> CreatePurchaseOrder([FromBody] PurchaseOrderRecord purchaseOrder)
    {
        try
        {   
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
            await _itemService.RecieveShipmentAsync(shipment.ItemId, shipment.Quantity);
        } 
        catch (Exception ex)
        {
            
        }
        return Ok();
    }
}