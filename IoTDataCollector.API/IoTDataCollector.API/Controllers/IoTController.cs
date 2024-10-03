using IoTDataCollector.API.Helpers;
using IoTDataCollector.API.Models;
using IoTDataCollector.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoTDataApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IoTDataController : ControllerBase
    {
        private readonly IoTDataService _dataService;
        private readonly HttpClientHelper _httpClientHelper;

        public IoTDataController(IoTDataService dataService, HttpClientHelper httpClientHelper)
        {
            _dataService = dataService;
            _httpClientHelper = httpClientHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IoTData data)
        {
            await _dataService.SaveDataAsync(data);
            return Ok("Data saved.");
        }

        [HttpGet("report")]
        public async Task<ActionResult<List<IoTData>>> GetReport()
        {
            var data = await _dataService.GetAllDataAsync();
            return Ok(data);
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartProduction()
        {
            var response = await _httpClientHelper.PostAsync("https://localhost:7150/start");
            if (response.IsSuccessStatusCode)
            {
                return Ok("Production started.");
            }
            return StatusCode((int)response.StatusCode, "Failed to start production.");
        }

        [HttpPost("stop")]
        public async Task<IActionResult> StopProduction()
        {
            var response = await _httpClientHelper.PostAsync("https://localhost:7150/stop");
            if (response.IsSuccessStatusCode)
            {
                return Ok("Production stopped.");
            }
            return StatusCode((int)response.StatusCode, "Failed to stop production.");
        }
    }
}
