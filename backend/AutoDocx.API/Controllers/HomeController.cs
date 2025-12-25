using Microsoft.AspNetCore.Mvc;

namespace AutoDocx.API.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok(new
        {
            message = "Welcome to AutoDocx API",
            version = "1.0.0",
            status = "Running",
            endpoints = new
            {
                swagger = "/swagger",
                templates = "/api/templates",
                documents = "/api/documents"
            },
            documentation = "Visit /swagger for API documentation"
        });
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow
        });
    }
}
