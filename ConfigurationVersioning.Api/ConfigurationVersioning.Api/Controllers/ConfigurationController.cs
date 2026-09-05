using ConfigurationVersioning.Api.Data;
using ConfigurationVersioning.Api.Diff;
using ConfigurationVersioning.Api.DTOs;
using ConfigurationVersioning.Api.Models;
using ConfigurationVersioning.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationVersioning.Api.Controllers
{
    [Route("config")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IJsonDiffService _jsonDiffService;
        public ConfigurationController(IConfigurationService configurationService, IJsonDiffService jsonDiffService)
        {
            _configurationService = configurationService;
            _jsonDiffService = jsonDiffService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save(SaveConfigurationRequest request)
        {
            var response =await _configurationService.CreateVersionAsync(request);

            return Ok(response);
        }

        [HttpGet("versions")]
        public async Task<IActionResult> GetVersions()
        {
            var versions =await _configurationService.GetVersionsAsync();

            return Ok(versions);
        }

        [HttpGet("versions/{versionId}")]
        public async Task<IActionResult> GetVersion(int versionId)
        {
            var version = await _configurationService.GetVersionByIdAsync(versionId);

            if (version == null)
            {
                return NotFound("Version not found.");
            }

            return Ok(version);
        }


        [HttpGet("diff")]
        public async Task<IActionResult> GetDiff([FromQuery] int from, [FromQuery] int to)
        {
            // 1. Get "from" version JSON
            var fromJson = await _configurationService.GetVersionJsonByIdAsync(from);

            if (fromJson == null)
            {
                return NotFound($"Version {from} not found.");
            }

            // 2. Get "to" version JSON
            var toJson = await _configurationService.GetVersionJsonByIdAsync(to);

            if (toJson == null)
            {
                return NotFound($"Version {to} not found.");
            }

            // 3. Compare the two JSON configurations
            var diff = _jsonDiffService.Compare(fromJson, toJson);

            // 4. Return the diff
            return Ok(diff);
        }

    }
}