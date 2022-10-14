
using IRFestival.Api.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using System.Net;

namespace IRFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly AppSettingsOptions _options;
        private readonly IFeatureManagerSnapshot _featureManager;
        public SettingsController(IOptions<AppSettingsOptions> options, IFeatureManagerSnapshot featureManager)
        {
            _options = options.Value;
            _featureManager = featureManager;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AppSettingsOptions))]
        public IActionResult Get()
        {
            return Ok(_options);
        }

        [HttpGet("Features")]
        public async Task<ActionResult> Features()
        {
            string message = await _featureManager.IsEnabledAsync("BuyTickets")
                ? "the ticket sale started"
                : "you cannopt";

            return Ok(message);
        }
    }
}
