using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace AlarmSystem.Models
{
    public class UserCode
    {
        [Required]
        [Key]public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public string CodeType { get; set; } = "Email";

    }
}
