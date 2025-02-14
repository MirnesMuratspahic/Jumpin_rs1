using System.ComponentModel.DataAnnotations;

namespace AlarmSystem.Models.DTO
{
    public class dtoUserCode
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string CodeType { get; set; } = string.Empty;
    }
}
