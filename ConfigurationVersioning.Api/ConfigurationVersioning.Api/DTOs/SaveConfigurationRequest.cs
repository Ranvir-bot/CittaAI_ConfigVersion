using System.ComponentModel.DataAnnotations;

namespace ConfigurationVersioning.Api.DTOs
{
    public class SaveConfigurationRequest
    {
        public int? ConfigurationId { get; set; }

        public int? BaseVersionId { get; set; }

        [Required]
        public string Data { get; set; } = string.Empty;

        [Required]
        public string CreatedBy { get; set; } = string.Empty;
    }
}
