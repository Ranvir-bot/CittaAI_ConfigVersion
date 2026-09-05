using ConfigurationVersioning.Api.Models;

namespace ConfigurationVersioning.Api.Repositories
{
    public interface IConfigurationRepository
    {
        Task<Configuration?> GetConfigurationByIdAsync(int configurationId);

        Task<ConfigurationVersion?> GetLatestVersionAsync(
            int configurationId);

        Task<ConfigurationVersion?> GetVersionByIdAsync(int versionId);

        Task<List<ConfigurationVersion>> GetVersionsAsync();

        Task AddConfigurationAsync(Configuration configuration);

        Task AddVersionAsync(ConfigurationVersion version);

        Task SaveVersionAsync(
            Configuration configuration,
            ConfigurationVersion version);
    }
}
