﻿using System.ComponentModel.DataAnnotations;

namespace Jumpin.Models.DTO
{
    public class dtoUserRegistration
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber {  get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
