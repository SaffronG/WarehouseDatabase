using Backend.Contracts;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseOrdersController : ControllerBase
{
    private readonly WarehouseWorkflowService _workflowService;

    public PurchaseOrdersController(WarehouseWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PurchaseOrderCreateRequest request)
    {
        try
        {
            var result = await _workflowService.CreatePurchaseOrderAsync(request);
            return Created($"/api/purchase-orders/{result.Id}", result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
