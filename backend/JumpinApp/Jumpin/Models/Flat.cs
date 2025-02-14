using System.ComponentModel.DataAnnotations;

namespace Jumpin.Models
{
    public class Flat
    {
        [Key] public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime DateAndTime { get; set; }
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
