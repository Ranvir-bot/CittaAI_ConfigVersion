namespace ConfigurationVersioning.Api.DTOs
{
    public class SaveConfigurationRequest
    {
        public int? ConfigurationId { get; set; }

        public string Data { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;
    }
}
