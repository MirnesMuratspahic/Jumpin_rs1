using System.ComponentModel.DataAnnotations;

namespace Jumpin.Models.DTO
{
    public class dtoUserCar
    {
        [Required]
        public string Email { get; set; }
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
    }
}
