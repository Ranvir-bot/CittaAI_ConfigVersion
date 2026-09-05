namespace ConfigurationVersioning.Api.Models
{
    public class ConfigurationVersion
    {
        public int Id { get; set; }

        public int ConfigurationId { get; set; } //foreign key to Configuration

        public int VersionNumber { get; set; }

        public string ConfigurationJson { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string Author { get; set; } = string.Empty;

        public string? Comment { get; set; }

        public int? PreviousVersionId { get; set; }

        public Configuration Configuration { get; set; } = null!;
    }
}
