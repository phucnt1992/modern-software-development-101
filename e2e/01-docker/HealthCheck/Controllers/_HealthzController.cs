using Microsoft.AspNetCore.Mvc;

namespace HealthCheck.Controllers;

[ApiController]
[Route("api/_healthz")]
public class HealthCheckController : ControllerBase
{
  public HealthCheckController()
  {
  }

  [HttpGet]
  [Route("liveness")]
  public IActionResult Liveness()
  {
    return StatusCode(StatusCodes.Status200OK, "OK");
  }

  [HttpGet]
  [Route("readiness")]
  public IActionResult Readiness()
  {
    return StatusCode(StatusCodes.Status200OK, "OK");
  }
}
