using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TakNotify;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly INotification _notification;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            INotification notification,
            IConfiguration configuration)
        {
            _logger = logger;
            _notification = notification;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();
            var items = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            await SendSmsWithTwilio(items);

            return items;
        }

        private async Task SendSmsWithTwilio(WeatherForecast[] items)
        {
            var message = new SMSMessage
            {
                FromNumber = _configuration["DefaultFromNumber"],
                ToNumber = _configuration["TestNumber"],
                Content = $"Weather Forcast: {JsonConvert.SerializeObject(items)}"
            };
            var result = await _notification.SendSmsWithTwilio(message);

            if (result.IsSuccess)
                _logger.LogDebug("SMS notification was sent to {toAddresses}", message.ToNumber);
            else
                _logger.LogWarning("Failed to send notification to {toAddresses}. Error: {error}", message.ToNumber, result.Errors);
        }
    }
}
