using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Jumpin.Models
{
    public class Car
    {
        [Key] public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string DateAndTime { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
