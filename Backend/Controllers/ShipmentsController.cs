using Backend.Contracts;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShipmentsController : ControllerBase
{
    private readonly WarehouseWorkflowService _workflowService;

    public ShipmentsController(WarehouseWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ShipmentCreateRequest request)
    {
        try
        {
            var result = await _workflowService.CreateShipmentAsync(request);
            return Created($"/api/shipments/{result.Id}", result);
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

    [HttpPost("{id}/receive")]
    public async Task<IActionResult> Receive(int id, [FromBody] ShipmentReceiveRequest request)
    {
        try
        {
            var result = await _workflowService.ReceiveShipmentAsync(id, request);
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
