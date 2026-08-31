using Microsoft.AspNetCore.Mvc;
using TelemetryDeviceV1.Services;

namespace TelemetryDeviceV1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TDController : ControllerBase
    {
        private readonly TDRuntimeService _runtime;

        public TDController(TDRuntimeService runtime)
        {
            _runtime = runtime;
        }

        [HttpPost("start")]
        public async Task<IActionResult> Start()
        {
            try
            {
                await _runtime.StartAsync();
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status503ServiceUnavailable);
            }
        }

        [HttpPost("stop")]
        public async Task<IActionResult> Stop()
        {
            try
            {
                await _runtime.StopAsync();
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new { isRunning = _runtime.IsRunning });
        }
    }
}
