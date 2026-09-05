namespace ConfigurationVersioning.Api.DTOs
{
    public class SaveConfigurationResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public ConfigurationVersionDto? Version { get; set; }
    }
}
