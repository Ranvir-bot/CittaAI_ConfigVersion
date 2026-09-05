using ConfigurationVersioning.Api.DTOs;
using ConfigurationVersioning.Api.Models;
using ConfigurationVersioning.Api.Repositories;
using Newtonsoft.Json.Linq;

namespace ConfigurationVersioning.Api.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRepository _repository;

        public ConfigurationService(IConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<SaveConfigurationResponse> CreateVersionAsync(SaveConfigurationRequest request)
        {
            try
            {
                JToken.Parse(request.Data);
            }
            catch
            {
                return new SaveConfigurationResponse
                {
                    Success = false,
                    Message = "Invalid JSON configuration."
                };
            }

            var configuration = request.ConfigurationId.HasValue ? await _repository.GetConfigurationByIdAsync(request.ConfigurationId.Value)
                : null;

            if (request.ConfigurationId.HasValue && configuration == null)
            {
                return new SaveConfigurationResponse
                {
                    Success = false,
                    Message = $"Configuration {request.ConfigurationId.Value} not found."
                };
            }

            if (configuration == null)
            {
                configuration = new Configuration
                {
                    Name = "New Configuration",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            }

            var lastVersion = await _repository.GetLatestVersionAsync(configuration.Id);
            if (request.BaseVersionId.HasValue && lastVersion != null && request.BaseVersionId.Value != lastVersion.Id)
            {
                return new SaveConfigurationResponse
                {
                    Success = false,
                    Message = "This version is stale. A newer version already exists."
                };
            }

            var newVersionNumber = lastVersion == null ? 1 : lastVersion.VersionNumber + 1;

            var newConfigurationVersion = new ConfigurationVersion
            {
                ConfigurationId = configuration.Id,
                VersionNumber = newVersionNumber,
                ConfigurationJson = request.Data,
                Author = request.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                PreviousVersionId = lastVersion?.Id
            };

            await _repository.SaveVersionAsync(configuration, newConfigurationVersion);

            return new SaveConfigurationResponse
            {
                Success = true,
                Message = $"Version {newConfigurationVersion.VersionNumber} saved successfully.",
                Version = new ConfigurationVersionDto
                {
                    Id = newConfigurationVersion.Id,
                    ConfigurationId = newConfigurationVersion.ConfigurationId,
                    VersionNumber = newConfigurationVersion.VersionNumber,
                    ConfigurationJson = newConfigurationVersion.ConfigurationJson,
                    CreatedAt = newConfigurationVersion.CreatedAt,
                    Author = newConfigurationVersion.Author,
                    Comment = newConfigurationVersion.Comment,
                    PreviousVersionId = newConfigurationVersion.PreviousVersionId
                }
            };
        }

        public async Task<List<ConfigurationVersionDto>> GetVersionsAsync()
        {
            var versions = await _repository.GetVersionsAsync();

            return versions.Select(x => new ConfigurationVersionDto
            {
                Id = x.Id,
                ConfigurationId = x.ConfigurationId,
                VersionNumber = x.VersionNumber,
                ConfigurationJson = x.ConfigurationJson,
                CreatedAt = x.CreatedAt,
                Author = x.Author,
                Comment = x.Comment,
                PreviousVersionId = x.PreviousVersionId
            }).ToList();
        }

        public async Task<ConfigurationVersionDto?> GetVersionByIdAsync(int versionId)
        {
            var version = await _repository.GetVersionByIdAsync(versionId);

            if (version == null)
            {
                return null;
            }

            return new ConfigurationVersionDto
            {
                Id = version.Id,
                ConfigurationId = version.ConfigurationId,
                VersionNumber = version.VersionNumber,
                ConfigurationJson = version.ConfigurationJson,
                CreatedAt = version.CreatedAt,
                Author = version.Author,
                Comment = version.Comment,
                PreviousVersionId = version.PreviousVersionId
            };
        }

        public async Task<string?> GetVersionJsonByIdAsync(int versionId)
        {
            var version = await _repository.GetVersionByIdAsync(versionId);

            return version?.ConfigurationJson;
        }
    }
}