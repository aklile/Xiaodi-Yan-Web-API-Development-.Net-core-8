using Microsoft.AspNetCore.Mvc;

namespace MyFirstApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        // Get the "X-Correlation-Id" header from the request
        var correlationId = Request.Headers["X-Correlation-Id"].FirstOrDefault();
        // Log the correlation ID
        _logger.LogInformation("Handling the request. CorrelationId:{ CorrelationId}", correlationId);
        // Call another service with the same "X-Correlation-Id" header
        //when you set up the HttpClient
        //var httpContent = new StringContent("Hello world!");
        //httpContent.Headers.Add("X-Correlation-Id", correlationId);
        _logger.Log(LogLevel.Information, "This is a logging message.");
        _logger.LogTrace("This is a trace message");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
    [HttpGet]
    [Route("structured-logging")]
    public ActionResult StructuredLoggingSample()
    {
        _logger.LogInformation("This is a logging message with args: Today  is { Week }. It is { Time }.",
            DateTime.Now.DayOfWeek, DateTime.Now.ToLongTimeString());


        _logger.LogInformation($"This is a logging message with string concatenation: Today is {DateTime.Now.DayOfWeek}. It is {DateTime.Now.ToLongTimeString()}.");

        return Ok("This is to test the difference between structuredlogging and string concatenation.");
    }
}
