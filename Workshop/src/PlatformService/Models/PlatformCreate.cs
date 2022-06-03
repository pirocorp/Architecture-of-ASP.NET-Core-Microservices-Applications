namespace PlatformService.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PlatformCreate
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Publisher { get; set; } = string.Empty;

        [Required]
        public string Cost { get; set; } = string.Empty;
    }
}
