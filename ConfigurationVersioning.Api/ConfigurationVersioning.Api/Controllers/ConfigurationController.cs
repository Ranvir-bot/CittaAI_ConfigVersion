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
            if (!response.Success)
            {
                if (response.Message.Contains("not found"))
                {
                    return NotFound(response);
                }
                if (response.Message.Contains("stale"))
                {
                    return Conflict(response);
                }

                return BadRequest(response);
            }
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
            if (from == to)
            {
                return BadRequest("From and To versions must be different.");
            }

            var fromJson = await _configurationService.GetVersionJsonByIdAsync(from);

            if (fromJson == null)
            {
                return NotFound($"Version {from} not found.");
            }

            var toJson = await _configurationService.GetVersionJsonByIdAsync(to);

            if (toJson == null)
            {
                return NotFound($"Version {to} not found.");
            }

            var diff = _jsonDiffService.Compare(fromJson, toJson);

            return Ok(diff);
        }

    }
}