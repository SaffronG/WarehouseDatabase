using Backend.Contracts;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly WarehouseWorkflowService _workflowService;

    public OrdersController(WarehouseWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderCreateRequest request)
    {
        try
        {
            var result = await _workflowService.CreateOrderAsync(request);
            return Created($"/api/orders/{result.Id}", result);
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

    [HttpPost("{id}/pick")]
    public async Task<IActionResult> Pick(int id)
    {
        try
        {
            var result = await _workflowService.PickOrderAsync(id);
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

    [HttpPost("{id}/pack")]
    public async Task<IActionResult> Pack(int id)
    {
        try
        {
            var result = await _workflowService.PackOrderAsync(id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPost("{id}/ship")]
    public async Task<IActionResult> Ship(int id)
    {
        try
        {
            var result = await _workflowService.ShipOrderAsync(id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
