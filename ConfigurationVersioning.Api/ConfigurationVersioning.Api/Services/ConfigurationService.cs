using ConfigurationVersioning.Api.Data;
using ConfigurationVersioning.Api.DTOs;
using ConfigurationVersioning.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationVersioning.Api.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly AppDbContext _context;
        public ConfigurationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ConfigurationVersionDto> CreateVersionAsync(SaveConfigurationRequest request)
        {
            // 1. Check whether configuration exists
            var configuration = request.ConfigurationId.HasValue ? await _context.Configurations
                    .FirstOrDefaultAsync(x => x.Id == request.ConfigurationId.Value) : null;

            // 2. Create new configuration if it does not exist
            if (configuration == null)
            {
                configuration = new Configuration
                {
                    Name = "New Configuration",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Configurations.Add(configuration);

                await _context.SaveChangesAsync();
            }

            // 3. Find the latest version
            var lastVersion = await _context.ConfigurationVersions
                .Where(x => x.ConfigurationId == configuration.Id).OrderByDescending(x => x.VersionNumber)
                .FirstOrDefaultAsync();

            // 4. Calculate next version number
            var newVersionNumber = lastVersion == null ? 1 : lastVersion.VersionNumber + 1;

            // 5. Create new version
            var newConfigurationVersion = new ConfigurationVersion
            {
                ConfigurationId = configuration.Id,
                VersionNumber = newVersionNumber,
                ConfigurationJson = request.Data,
                Author = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            _context.ConfigurationVersions.Add(newConfigurationVersion);

            await _context.SaveChangesAsync();

            // 6. Update current version
            configuration.CurrentVersionId =
                newConfigurationVersion.Id;

            configuration.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // 7. Return DTO
            return new ConfigurationVersionDto
            {
                Id = newConfigurationVersion.Id,
                ConfigurationId = newConfigurationVersion.ConfigurationId,
                VersionNumber = newConfigurationVersion.VersionNumber,
                ConfigurationJson = newConfigurationVersion.ConfigurationJson,
                CreatedAt = newConfigurationVersion.CreatedAt,
                Author = newConfigurationVersion.Author,
                Comment = newConfigurationVersion.Comment,
                PreviousVersionId = newConfigurationVersion.PreviousVersionId
            };
        }

        public async Task<List<ConfigurationVersionDto>> GetVersionsAsync()
        {
            return await _context.ConfigurationVersions.OrderBy(x => x.ConfigurationId).ThenBy(x => x.VersionNumber)
                .Select(x => new ConfigurationVersionDto
                {
                    Id = x.Id,
                    ConfigurationId = x.ConfigurationId,
                    VersionNumber = x.VersionNumber,
                    ConfigurationJson = x.ConfigurationJson,
                    CreatedAt = x.CreatedAt,
                    Author = x.Author,
                    Comment = x.Comment,
                    PreviousVersionId = x.PreviousVersionId
                }).ToListAsync();
        }

        public async Task<ConfigurationVersionDto?> GetVersionByIdAsync(int versionId)
        {
            var version = await _context.ConfigurationVersions.FirstOrDefaultAsync(x => x.Id == versionId);

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
            var version = await _context.ConfigurationVersions.FirstOrDefaultAsync(x => x.Id == versionId);

            return version?.ConfigurationJson;
        }


    }
}