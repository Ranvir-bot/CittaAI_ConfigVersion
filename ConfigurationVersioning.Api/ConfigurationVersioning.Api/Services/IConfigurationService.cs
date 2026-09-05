using ConfigurationVersioning.Api.DTOs;

namespace ConfigurationVersioning.Api.Services
{
    public interface IConfigurationService
    {
        Task<ConfigurationVersionDto> CreateVersionAsync(SaveConfigurationRequest request);

        Task<List<ConfigurationVersionDto>> GetVersionsAsync();

        Task<ConfigurationVersionDto?> GetVersionByIdAsync(int versionId);
        Task<string?> GetVersionJsonByIdAsync(int versionId);
    }
}
