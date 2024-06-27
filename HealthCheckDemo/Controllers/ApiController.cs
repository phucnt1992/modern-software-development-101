using Microsoft.AspNetCore.Mvc;

namespace HealthCheckDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        public ApiController()
        {
        }

        [HttpGet("_healthz/liveness")]
        public IActionResult Liveness()
        {
            return Ok("OK");
        }

        [HttpGet("_healthz/readiness")]
        public IActionResult Readiness()
        {
            return Ok("OK");
        }
    }
}