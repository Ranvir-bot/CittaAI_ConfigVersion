namespace ConfigurationVersioning.Api.Models
{
    public class Configuration
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int? CurrentVersionId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<ConfigurationVersion> Versions { get; set; }
            = new List<ConfigurationVersion>();

    }
}
