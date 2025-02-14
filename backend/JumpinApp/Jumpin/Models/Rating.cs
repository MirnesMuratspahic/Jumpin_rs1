using System.ComponentModel.DataAnnotations;

namespace Jumpin.Models
{
    public class Rating
    {
        [Required]
        [Key]public int Id { get; set; }
        [Required]
        public float Review { get; set; }
        [Required]
        public string Comment { get; set; } = string.Empty;
        [Required]
        public DateTime DateTime { get; set; }

    }
}
