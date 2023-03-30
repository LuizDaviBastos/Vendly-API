using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASM.Api.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingsService settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        [HttpPost("SetSettings")]
        public IActionResult SetSettings([FromBody] AsmAppSettings settings)
        {
            settingsService.SetAppSettings(settings);
            return Ok();
        }

        [HttpGet("GetSettings")]
        public IActionResult GetSettings()
        {
            return Ok(settingsService.GetAppSettings());
        }
    }
}
