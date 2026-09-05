using ConfigurationVersioning.Api.Data;
using ConfigurationVersioning.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationVersioning.Api.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly AppDbContext _context;

        public ConfigurationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Configuration?> GetConfigurationByIdAsync(int configurationId)
        {
            return await _context.Configurations.FirstOrDefaultAsync(x => x.Id == configurationId);
        }

        public async Task<ConfigurationVersion?> GetLatestVersionAsync(int configurationId)
        {
            return await _context.ConfigurationVersions.Where(x => x.ConfigurationId == configurationId)
                .OrderByDescending(x => x.VersionNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<ConfigurationVersion?> GetVersionByIdAsync(int versionId)
        {
            return await _context.ConfigurationVersions.FirstOrDefaultAsync(x => x.Id == versionId);
        }

        public async Task<List<ConfigurationVersion>> GetVersionsAsync()
        {
            return await _context.ConfigurationVersions.OrderBy(x => x.ConfigurationId)
                .ThenBy(x => x.VersionNumber)
                .ToListAsync();
        }

        public async Task AddConfigurationAsync(Configuration configuration)
        {
            await _context.Configurations.AddAsync(configuration);
        }

        public async Task AddVersionAsync(ConfigurationVersion version)
        {
            await _context.ConfigurationVersions.AddAsync(version);
        }

        

        public async Task SaveVersionAsync(Configuration configuration, ConfigurationVersion version)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                if (configuration.Id == 0)
                {
                    await _context.Configurations.AddAsync(configuration);

                    await _context.SaveChangesAsync();

                    version.ConfigurationId = configuration.Id;
                }

                await _context.ConfigurationVersions.AddAsync(version);

                await _context.SaveChangesAsync();

                configuration.CurrentVersionId = version.Id;
                configuration.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}