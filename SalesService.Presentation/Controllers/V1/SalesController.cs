using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Common;

namespace SalesService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/sales")]
public class SalesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(ApiResponse<string>.Ok("Sales Service is operational"));
    }
}
