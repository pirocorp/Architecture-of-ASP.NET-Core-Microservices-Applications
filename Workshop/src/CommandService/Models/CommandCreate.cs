namespace CommandService.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CommandCreate
    {
        [Required]
        public string HowTo { get; set; } = string.Empty;

        [Required]
        public string CommandLine { get; set; } = string.Empty;
    }
}
