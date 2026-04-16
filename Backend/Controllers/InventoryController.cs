using Backend.Contracts;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly WarehouseWorkflowService _workflowService;

    public InventoryController(WarehouseWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    [HttpPost("store")]
    public async Task<IActionResult> Store([FromBody] InventoryStoreRequest request)
    {
        try
        {
            var result = await _workflowService.StoreInventoryAsync(request);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
