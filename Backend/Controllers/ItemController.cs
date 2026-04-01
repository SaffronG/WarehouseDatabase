using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemController : Controller {
    [HttpGet("/test")]
    public IActionResult TestEntpoint() {
        return Ok(new Test("TEST"));
    }
}

public record Test(string text);
