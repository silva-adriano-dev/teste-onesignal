using Microsoft.AspNetCore.Mvc;

namespace TrackingRealtime.Controllers;

[ApiController]
[Route("tracking")]
public class TrackingController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}