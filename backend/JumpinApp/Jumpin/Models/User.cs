using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Jumpin.Models
{
    public class User
    {
        [JsonIgnore][Key] public int Id { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        [JsonIgnore]
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber {  get; set; } = string.Empty;
        public bool PhoneConfirmed { get; set; } = false;
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;
        [JsonIgnore]
        [Required]
        public string Role { get; set; } = string.Empty;
        [Required]
        public string AccountType { get; set; } = string.Empty;   

    }
}
