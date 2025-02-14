using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Jumpin.Models
{
    public abstract class Request
    {
        [Required]
        [Key] public int Id { get; set; }
        [Required]
        public string PassengerEmail { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
