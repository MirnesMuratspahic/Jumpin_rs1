using System.ComponentModel.DataAnnotations;

namespace Jumpin.Models.DTO
{
    public class dtoRequest
    {
        [Required]
        public string PassengerEmail { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;
        [Required]
        public int ObjectId { get; set; }
        [Required]
        public string ObjectType { get; set; } = string.Empty;
    }
}

